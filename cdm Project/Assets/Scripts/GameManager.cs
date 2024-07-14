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

    public bool gameOver = false;

    private GameObject gameOverPanel;

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
        NewGame();
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

        if (canvas != null)
        {
            gameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false); // Ensure the panel is hidden at the start
            }
            else
            {
                Debug.LogWarning("GameOverPanel not found in the Canvas.");
            }
        }
        else
        {
            Debug.LogWarning("Canvas not found in the scene.");
        }

        UpdateScoreText();
        UpdateLivesText();
        UpdateLevelText();
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

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("GameOverPanel set to active.");
        }
        else
        {
            Debug.LogWarning("GameOverPanel not found in the Canvas at game over.");
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

    public void AddLife()
    {
        this.lives++;
        UpdateLivesText();
    }
}
