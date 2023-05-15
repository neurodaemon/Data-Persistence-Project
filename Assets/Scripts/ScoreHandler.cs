using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreHandler : MonoBehaviour
{
    public TextMeshProUGUI hiScoresText;

    // Start is called before the first frame update
    void Start()
    {
        hiScoresText.text = DataManager.Instance.GetHiScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("start");
    }
}
