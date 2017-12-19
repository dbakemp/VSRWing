using System.Collections;
using TerrainGenerator;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int Radius = 1;

    private Vector2i PreviousPlayerChunkPosition;

    public Transform Player;

    public TerrainChunkGenerator Generator;

    public bool GameOverBool = false;

    public GameObject GameOverText;
    public GameObject ScoreText;
    public GameObject LevelText;
    public GameObject PowerupText;

    public int Score = 0;

    public float GameTime = 0;
    public int KillScore = 0;
    public int KillsToLevel = 0;
    public int Level = 1;

    private float PowerupTimer = 0f;
    private float PowerupTime = 30f;
    public bool PowerupReady = false;

    void Start()
    {
        Cursor.visible = false;
    }

    public void Update()
    {
        if (!GameOverBool)
        {
            GameTime += Time.deltaTime;
            Score = (int)Mathf.Floor(GameTime) * 10 + KillScore;
            ScoreText.GetComponent<Text>().text = "Score: " + Score.ToString();
            LevelText.GetComponent<Text>().text = "Level: " + Level.ToString();

            if (!PowerupReady)
            {
                PowerupText.SetActive(false);
                PowerupTimer += Time.deltaTime;
                if (PowerupTimer >= PowerupTime)
                {
                    PowerupReady = true;
                    PowerupTimer = 0;
                    PowerupText.SetActive(true);
                }
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            CallMenuScene();
        }
    }

    public void AddKill()
    {
        KillScore += 100;
        KillsToLevel++;
        if(KillsToLevel >= 10)
        {
            Level++;
            KillsToLevel = 0;
        }
    }
    
    public void CallMenuScene()
    {
        Application.LoadLevel("Menu");
    }

    public void GameOver()
    {
        GameOverBool = true;
        GameOverText.SetActive(true);
        PowerupText.SetActive(false);
    }
}