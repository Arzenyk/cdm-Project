using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TMPro.EditorUtilities;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private CondeScript condeScript;
    private AldeanoScript aldeanoScript;
    private ColmillosScript colmillosScript;

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
    public bool mutedGame = false;
    public bool inMenu = false;

    private GameObject gameOverPanel;
    private GameObject HighScoreInPanel;
    private GameObject LevelCompletePanel;

    private VideoManager videoManager;

    private bool halfClearedActionPerformed = false;

    /*
    public AudioClip Cancion;
    public AudioClip gameOverSound;
    public AudioClip lostLifeSound;
    public AudioClip wonLevelSound;
    public AudioClip Cancion1;
    */

    //private AudioSource audioSource;
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
        /*
        audioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
        if (Cancion1 == null)
        {
            Debug.LogError("GameOverSound AudioClip is not assigned.");
        }
        */
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

        if (level < 1)
        {
            inMenu = true;
            //audioSource.clip = Cancion1;
            //audioSource.Stop();
        }
        else
        {
            inMenu = false;
            SceneManager.LoadScene("level" + level);
            condeScript = GameObject.FindGameObjectWithTag("Conde").GetComponent<CondeScript>();
            aldeanoScript = GameObject.FindGameObjectWithTag("Aldeano").GetComponent<AldeanoScript>();
            colmillosScript = GameObject.FindGameObjectWithTag("Colmillos").GetComponent<ColmillosScript>();
        }
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.level < 1)
        {
            inMenu = true;
            //InMenu();
        }
        condeScript = FindObjectOfType<CondeScript>();
        aldeanoScript = FindObjectOfType<AldeanoScript>();
        colmillosScript = FindObjectOfType<ColmillosScript>();
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();

        Scoretxt = GameObject.Find("Scoretxt").GetComponent<TMP_Text>();
        Livestxt = GameObject.Find("Livestxt").GetComponent<TMP_Text>();
        Leveltxt = GameObject.Find("Leveltxt").GetComponent<TMP_Text>();

        videoManager = GameObject.Find("VideoManager").GetComponent<VideoManager>();

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

        /*
        if (mutedGame == false)
        {
            PlayCancionSound();
        }
        else
        {
            StopCancionSound();
        }
        */
    }

    private void Update()
    {
        if (IsHalfCleared() && !halfClearedActionPerformed)
        {
            PerformHalfClearedAction();
            halfClearedActionPerformed = true;
        }

        if (IsCleared())
        {
            PerformLevelClearedAction();
        }
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
        //aldeanoScript.OnAttackEvent();
        gameOver = true;
        //PlayGameOverSound();
        //condeScript.OnDefeatedEvent();
        videoManager.OnGameSituationChanged("Conde pierde");
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
        //aldeanoScript.OnAttackEvent();

        if (this.lives > 0)
        {
            ResetBallgm();
            ResetPaddlegm();
            UpdateLivesText();
            //PlayLostLifeSound();
            //condeScript.OnDamageEvent();
            videoManager.OnGameSituationChanged("Conde lastim");
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

        /*
        if (IsCleared())
        {
            
        }
        else if (IsHalfCleared())
        {
            videoManager.OnGameSituationChanged("Conde ataca");
        }
        */
    }

    public bool IsCleared()
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

    public bool IsHalfCleared()
    {
        int totalBricks = this.bricks.Length;
        int breakableBricks = 0;
        int clearedBricks = 0;

        for (int i = 0; i < totalBricks; i++)
        {
            if (!this.bricks[i].unbreakable)
            {
                breakableBricks++;
                if (!this.bricks[i].gameObject.activeInHierarchy)
                {
                    clearedBricks++;
                }
            }
        }

        return clearedBricks >= breakableBricks / 2;
    }

    private void PerformHalfClearedAction()
    {
        videoManager.OnGameSituationChanged("Conde ataca");
        Debug.Log("Half of the breakable bricks have been cleared!");
    }

    private void PerformLevelClearedAction()
    {
        GameObject canvas = GameObject.Find("Canvas");
        LevelCompletePanel = canvas.transform.Find("LevelCompletePanel").gameObject;
        LevelCompletePanel.SetActive(true);
        levelWon = true;
        //PlayWonLevelSound();
        //condeScript.OnAttackEvent();
        //colmillosScript.OnAttackEvent();
        //aldeanoScript.OnDamageEvent();
        videoManager.OnGameSituationChanged("Conde gana");
        Debug.Log("Level cleared!");
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

    public void ForceLevelWon()
    {
        //PlayWonLevelSound();
        levelWon = true;
        GameObject canvas = GameObject.Find("Canvas");
        LevelCompletePanel = canvas.transform.Find("LevelCompletePanel").gameObject;
        LevelCompletePanel.SetActive(true);
        UpdateHighscoreText();
        videoManager.OnGameSituationChanged("Conde gana");
        //condeScript.OnAttackEvent();
        //colmillosScript.OnAttackEvent();
        Debug.Log("ahora?");
        //aldeanoScript.OnDefeatedEvent();
    }

    /*
    private void PlayGameOverSound()
    {
        
        audioSource.Stop();

        audioSource.clip = gameOverSound;
        audioSource.loop = false;
        audioSource.Play();
        
    }

    private void PlayLostLifeSound()
    {
        //StartCoroutine(PlayLostLifeSoundCoroutine());
    }

    
    private IEnumerator PlayLostLifeSoundCoroutine()
    {
        
        audioSource.Stop();

        audioSource.clip = lostLifeSound;
        audioSource.loop = false;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        PlayCancionSound();
        
    }

    private void PlayWonLevelSound()
    {
        audioSource.clip = wonLevelSound;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void PlayCancionSound()
    {
        this.audioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
        this.audioSource.clip = Cancion1;
        if (Cancion1 == null)
        {
            Debug.Log("cancion1 null");
        }
        else
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopCancionSound()
    {
        this.audioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
        audioSource.clip = Cancion1;
        audioSource.volume = 0;
        mutedGame = true;
    }

    public void InMenu()
    {
        if (inMenu)
        {
            StopCancionSound();
            Debug.Log("did it");
        }
        else
        {
            PlayCancionSound();
            Debug.Log("did not");
        }
    }
    */
}
