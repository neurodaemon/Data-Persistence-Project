using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private string playerName;
    private HiScore currentScore;
    private List<HiScore> hiScores;

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = $"{Application.persistentDataPath}/scoresfile.json";

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHiScores();
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public string GetHiScore()
    {
        return $"{hiScores[0].score} {hiScores[0].name}";
    }

    public string GetHiScores()
    {
        string scores = string.Empty;

        foreach(HiScore score in hiScores)
        {
            scores += $"{score.score} {score.name}\n";
        }

        return scores;
    }

    public void UpdatePlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            playerName = "???";
        }
        else
        {
            playerName = name;
        }
        Debug.Log($"player name updated: {playerName}");
    }

    public void UpdateHiScore(int score)
    {
        Debug.Log($"updating score: {score} {playerName}");

        currentScore = new HiScore(playerName, score);
        UpdateHiScores();
    }

    private void UpdateHiScores()
    {
        // sort hi scores with current score included
        hiScores.Add(currentScore);
        List<HiScore> sortedScores = (from p in hiScores
                  orderby p.score descending
                  select p).ToList();

        // we only want the top 10 scores
        List<HiScore> newScores = new List<HiScore>();
        for (int i = 0; i < 10; i++)
        {
            newScores.Add(sortedScores[i]);
        }

        hiScores = newScores;

        SaveHiScores();
    }

    [System.Serializable]
    class HiScore
    {
        public string name;
        public int score;
        // could add a datetime value to allow proper sorting of high scores
        // this would allow to push older scores out and keep newer scores when score values are the same

        public HiScore(string nameIn, int scoreIn)
        {
            name = nameIn;
            score = scoreIn;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public List<HiScore> hiScores;

        public SaveData()
        {
            hiScores = new List<HiScore>();
            hiScores.Add(new HiScore("AAA", 20));
            hiScores.Add(new HiScore("BBB", 19));
            hiScores.Add(new HiScore("CCC", 18));
            hiScores.Add(new HiScore("DDD", 17));
            hiScores.Add(new HiScore("EEE", 16));
            hiScores.Add(new HiScore("FFF", 15));
            hiScores.Add(new HiScore("GGG", 14));
            hiScores.Add(new HiScore("HHH", 13));
            hiScores.Add(new HiScore("III", 12));
            hiScores.Add(new HiScore("JJJ", 11));
        }

        public SaveData(List<HiScore> hiScoresIn)
        {
            hiScores = hiScoresIn;
        }
    }

    private void SaveHiScores()
    {
        Debug.Log($"saving hi scores, path: {saveFilePath}");
        SaveData data = new SaveData(hiScores);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(saveFilePath, json);
    }

    private void LoadHiScores()
    {
        Debug.Log($"saveFilePath: {saveFilePath}");
        SaveData data;
        if (File.Exists(saveFilePath))
        {
            // load existing scores if the file exists
            string json = File.ReadAllText(saveFilePath);
            data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("loaded hi scores");
        }
        else
        {
            // generate default list of scores if no file exists
            data = new SaveData();
            Debug.Log("generated hi scores");
        }

        hiScores = data.hiScores;
    }
}
