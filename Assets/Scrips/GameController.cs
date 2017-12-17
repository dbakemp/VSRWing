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

    public int Score = 0;

    public float GameTime = 0;
    public int KillScore = 0;

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
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            CallMenuScene();
        }
    }

    public void AddKill()
    {
        KillScore += 100;
    }
    
    public void CallMenuScene()
    {
        Application.LoadLevel("Menu");
    }

    public void GameOver()
    {
        GameOverBool = true;
        GameOverText.SetActive(true);
    }
}