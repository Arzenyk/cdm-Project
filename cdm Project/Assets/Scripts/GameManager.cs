using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Ball ball {  get; private set; }
    public Paddle paddle { get; private set; }
    public Brick[] bricks { get; private set; }

    public int level = 1;
    public int score = 0;
    public int lives = 3;

    private TMP_Text Scoretxt;
    private TMP_Text Livestxt;
    private TMP_Text Leveltxt;
    private TMP_Text HighScoretxt;

    public bool gameOver = false;
    public bool levelWon = false;

    private GameObject gameOverPanel;
    private GameObject HighScoreInPanel;
    private GameObject LevelCompletePanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate GameManager instances
            return;
        }

        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void Start()
    {

    }
    public void NewGame()
    {
        this.score = 0;
        this.lives = 3;
        this.gameOver = false;

        LoadLevel(1);
    }

    public void LoadLevel(int level)
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

        GameObject canvas = GameObject.Find("Canvas");
        HighScoretxt = canvas.transform.Find("GameOverPanel/HighScoretxt").GetComponent<TMP_Text>();
        
        gameOver = false;
        levelWon = false;
        LevelCompletePanel = canvas.transform.Find("LevelCompletePanel").gameObject;
        LevelCompletePanel.SetActive(false);
        UpdateScoreText();
        UpdateLivesText();
        UpdateLevelText();
        UpdateHighscoreText();
    }

    public void ResetLevel()
    {
        ResetBallgm();
        ResetPaddlegm();
        gameOver = false;
        this.lives = 3;
        this.score = 0;
        SceneManager.LoadScene("level" + 1);
    }

    private void ResetBallgm()
    {
        this.ball.ResetBall();
    }

    private void ResetPaddlegm()
    {
        this.paddle.ResetPaddle();
    }

    private void GameOver()
    {
        gameOver = true;
        GameObject canvas = GameObject.Find("Canvas");
        gameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;
        gameOverPanel.SetActive(true);
        Debug.Log("GameOverPanel set to active.");

        HighScoreInPanel = GameObject.Find("Canvas/GameOverPanel/HighScoretxt");


        int highScore = PlayerPrefs.GetInt("HIGHSCORE");
        if (this.score > highScore)
        {
            PlayerPrefs.SetInt("HIGHSCORE", this.score);

            HighScoretxt.text = "New High Score: " + this.score;
        }
        else
        {
            HighScoretxt.text = "High Score " + highScore + "\n" + "Can you beat it?";
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Miss()
    {
        this.lives--;

        if (this.lives > 0)
        {
            ResetBallgm();
            ResetPaddlegm();
            UpdateLivesText();
        }
        else
        {
            UpdateLivesText();
            GameOver();
        }
    }

    public void Hit(Brick brick)
    {
        this.score += brick.points;
        UpdateScoreText();
        UpdateHighscoreText();

        if (Cleared())
        {
            GameObject canvas = GameObject.Find("Canvas");
            LevelCompletePanel = canvas.transform.Find("LevelCompletePanel").gameObject;
            LevelCompletePanel.SetActive(true);
            levelWon = true;
        }
    }

    public bool Cleared()
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

    private void UpdateHighscoreText()
    {
        int highScore = PlayerPrefs.GetInt("HIGHSCORE");
        HighScoretxt.text = "High Score: " + highScore;
    }

    public void AddLife()
    {
        this.lives++;
        UpdateLivesText();
    }

    public void LoadNextLevel()
    {
        LoadLevel(this.level + 1);
    }

    public void forceLevelWon()
    {
        levelWon = true;
        GameObject canvas = GameObject.Find("Canvas");
        LevelCompletePanel = canvas.transform.Find("LevelCompletePanel").gameObject;
        LevelCompletePanel.SetActive(true);
        UpdateHighscoreText();
    }
}
