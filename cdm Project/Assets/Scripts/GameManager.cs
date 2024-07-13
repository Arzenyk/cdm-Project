using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Ball ball {  get; private set; }
    public Paddle paddle { get; private set; }
    public Brick[] bricks { get; private set; }

    public int level = 1;
    public int score = 0;
    public int lives = 3;

    public TMP_Text Scoretxt;
    public TMP_Text Livestxt;
    public TMP_Text Leveltxt;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        this.score = 0;
        this.lives = 3;

        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > 10)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            SceneManager.LoadScene("level" + level);
        }
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();

        Scoretxt = GameObject.Find("Scoretxt").GetComponent<TMP_Text>();
        Livestxt = GameObject.Find("Livestxt").GetComponent<TMP_Text>();
        Leveltxt = GameObject.Find("Leveltxt").GetComponent<TMP_Text>();

        UpdateScoreText();
        UpdateLivesText();
        UpdateLevelText();
    }

    private void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();

        /*
        for (int i = 0; i < this.bricks.Length; i++)
        {
            this.bricks[i].ResetBricks();
        }
        */
    }

    private void GameOver()
    {
        // SceneManager.LoadScene("GameOver");

        NewGame();
    }

    public void Miss()
    {
        this.lives--;

        if (this.lives > 0)
        {
            ResetLevel();
            UpdateLivesText();
        }
        else
        {
            GameOver();
        }
    }

    public void Hit(Brick brick)
    {
        this.score += brick.points;
        UpdateScoreText();

        if (Cleared())
        {
            LoadLevel(this.level + 1);
            lives = 3;
            UpdateLivesText();
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < this.bricks.Length; i++)
        {
            if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateScoreText()
    {
        if (Scoretxt != null)
        {
            Scoretxt.text = "Score: " + score;
        }
    }

    private void UpdateLivesText()
    {
        if (Livestxt != null)
        {
            Livestxt.text = "Lives: " + lives;
        }
    }

    private void UpdateLevelText()
    {
        if(Leveltxt != null)
        {
            Leveltxt.text = "Level: " + level;
        }
    }
}
