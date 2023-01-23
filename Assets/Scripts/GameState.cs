using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    // Singleton reference to Game State
    public static GameState Singleton { get; private set; }
    
    // The Game State properties
    public float defaultGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float GameSpeed { get; private set; }
    public float defaultGameTime = 60f * 2.5f; // Defaults to 2.5min 
    private float _timeRemaining;

    private Player1 _player1;
    private Player2 _player2;
    private ObstacleSpawner _obstacleSpawner;

    // UI Control based on GameState
    public TextMeshProUGUI countdown;
    
    public GameObject startGamePanel;
    
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameResultText;
    public Button gameOverRetryButton;


    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _player1 = FindObjectOfType<Player1>();
        _player2 = FindObjectOfType<Player2>();
        
        _obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        
        PrepareGame();
    }

    // Any Game State dependent updates
    private void Update()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining = Mathf.Max(_timeRemaining - Time.deltaTime, 0f);
            UpdateCountdown();
        }
        else
        {
            GameOver();
        }
        
        // Gradually increase game speed
        GameSpeed += gameSpeedIncrease * Time.deltaTime;
        
        // The UI Updates are performed from the player's update event
        _player1.AddScore();
        _player2.AddScore();
    }

    private void OnDestroy()
    {
        if (Singleton == this)
        {
            Singleton = null;
        }
    }

    /// <summary>
    /// Prepare the game BEFORE playing
    /// </summary>
    private void PrepareGame()
    {  
        GameSpeed = 0f;
        this.enabled = false;
        
        DeactivatePlayers();
        DeactivateObstacleSpawner();
        
        HideGameOverScreen();
        ShowStartGameScreen();
    }

    /// <summary>
    /// Start the play-gameover loop
    /// </summary>
    public void BeginPlaying()
    {
        HideStartGameScreen();
        StartNewGame();
    }

    /// <summary>
    /// Reset to defaults and start a new game
    /// </summary>
    public void StartNewGame()
    {
        ClearAllObstacles();
        ResetPlayerPositions();
        ResetPlayerScores();
        
        HideGameOverScreen();
        
        GameSpeed = defaultGameSpeed;
        this.enabled = true;

        ActivatePlayers();
        ActivateObstacleSpawner();
        
        _timeRemaining = defaultGameTime;
        countdown.color = Color.white;
    }

    /// <summary>
    /// Terminate current game and determine a winner
    /// </summary>
    public void GameOver()
    {
        GameSpeed = 0f;
        this.enabled = false;
        
        DeactivatePlayers();
        DeactivateObstacleSpawner();

        DetermineWinner();
        ShowGameOverScreen();
    }

    private void UpdateCountdown()
    {
        float minutes = Mathf.FloorToInt(_timeRemaining / 60);
        float seconds = Mathf.FloorToInt(_timeRemaining % 60);

        if (_timeRemaining < 10f)
        {
            countdown.color = Color.red;
        }
        countdown.text = string.Format("Time Remaining:\n{0:00}:{1:00}", minutes, seconds);
    }

    private void ShowStartGameScreen()
    {
        startGamePanel.gameObject.SetActive(true);
    }

    private void HideStartGameScreen()
    {
        startGamePanel.gameObject.SetActive(false);
    }

    private void HideGameOverScreen()
    {
        gameOverPanel.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        gameResultText.gameObject.SetActive(false);
        gameOverRetryButton.gameObject.SetActive(false);
    }

    private void ShowGameOverScreen()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        gameResultText.gameObject.SetActive(true);
        gameOverRetryButton.gameObject.SetActive(true);
    }

    private void DetermineWinner()
    {
        int p1Score = Mathf.FloorToInt(_player1.Score);
        int p2Score = Mathf.FloorToInt(_player2.Score);
        if (p1Score == p2Score)
        {
            gameResultText.color = Color.yellow;
            gameResultText.text = "IT'S A TIE!";
        }
        else if (p1Score > p2Score)
        {
            gameResultText.color = Color.green;
            gameResultText.text = "P1 WINS!";
        }
        else
        {
            gameResultText.color = Color.red;
            gameResultText.text = "P2 WINS!";
        }
    }

    private void ResetPlayerPositions()
    {
        _player1.ResetPosition();
        _player2.ResetPosition();
    }

    private void ResetPlayerScores()
    {
        _player1.ResetScore();
        _player2.ResetScore();
    }

    private void ActivatePlayers()
    {
        _player1.gameObject.SetActive(true);
        _player2.gameObject.SetActive(true);
    }

    private void DeactivatePlayers()
    {
        _player1.gameObject.SetActive(false);
        _player2.gameObject.SetActive(false);
    }

    private void ActivateObstacleSpawner()
    {
        _obstacleSpawner.gameObject.SetActive(true);
    }

    private void DeactivateObstacleSpawner()
    {
        _obstacleSpawner.gameObject.SetActive(false);
    }

    private void ClearAllObstacles()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
    }
}
