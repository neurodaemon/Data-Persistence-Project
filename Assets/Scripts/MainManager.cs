using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public int lineCount = 6;
    public Rigidbody ball;

    public Text scoreText;
    public Text hiScoreText;
    public GameObject gameOverText;
    
    private bool isStarted = false;
    private int points;
    private int brickCount = 0;
    
    private bool isGameOver = false;

    private bool isScore = false;
    private readonly string hiScoreLabel = "HI-SCORE";
    private string hiScore;
    private readonly float repeatRate = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                Brick brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.pointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brickCount++;
            }
        }

        // load hi-score if it exists
        hiScore = DataManager.Instance.GetHiScore();

        // trigger hi-score text toggle
        InvokeRepeating("ToggleHiScore", repeatRate, repeatRate);
    }

    private void Update()
    {
        if (!isStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene("scores");
            }
        }

        // toggle HiScore text based on boolean
        if (isScore)
        {
            hiScoreText.text = $"{hiScore}";
        }
        else
        {
            hiScoreText.text = hiScoreLabel;
        }

        // force game over on win
        if (!isGameOver && brickCount <= 0)
        {
            GameOver();
        }
    }

    void AddPoint(int point)
    {
        points += point;
        scoreText.text = $"Score : {points}";
        brickCount--;
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverText.SetActive(true);
            DataManager.Instance.UpdateHiScore(points);
        }
    }

    private void ToggleHiScore()
    {
        isScore = !isScore;
    }
}
