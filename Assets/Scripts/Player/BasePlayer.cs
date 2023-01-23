using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasePlayer : MonoBehaviour
{
    // GameObject's Components
    private CharacterController _player;
    protected SpriteRenderer _sr;

    public Color pColor;

    public Vector2 defaultPosition = new(-10, 3.5f);
    private Vector2 _playerDirection;

    // Player Controls
    public KeyCode jumpKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    // Player physics
    public float gravity = 30f;
    public float jumpHeight = 12f;

    public float movementSpeed = 200f;
    // public float maxHorizontalVelocity = 20f;
    // private bool doubleJumpAvailable = false;


    public string playerName;
    public float Score { get; set; } = 0f;
    private float _scoreMultiplier = 1f;


    // Possible Future implementation
    // public int Lives { get; set; } = 5;
    
    // Owned UI Elements
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreMultiplierAlert;

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
        _sr = GetComponent<SpriteRenderer>();

        _sr.color = this.pColor;
    }

    private void Start()
    {
        scoreMultiplierAlert.text = "";
    }

    private void OnEnable()
    {
        _playerDirection = Vector2.zero;
    }

    private void Update()
    {
        // Position Updates
        HandleGravity();
        HandleUserInput();

        _player.Move(_playerDirection * Time.deltaTime);

        // GameState UI Updates
        scoreText.text = playerName + " Score: " + Mathf.FloorToInt(Score);

        if (this._scoreMultiplier > 1f)
        {
            scoreMultiplierAlert.color = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {}

    private void HandleGravity()
    {
        // Continuously add downforce to the player direction
        _playerDirection += Vector2.down * (gravity * Time.deltaTime);
    }

    private void HandleUserInput()
    {
        HandleJump();
        HandleMovement();
        HandleExit();
    }

    private void HandleJump()
    {
        if (!_player.isGrounded) return;

        _playerDirection = Vector2.down;

        if (Input.GetKey(this.jumpKey))
        {
            _playerDirection = Vector2.up * jumpHeight; // One time transform (do not apply deltaTime)
        }
    }

    /// <summary>
    /// Handle moving left & right
    /// </summary>
    private void HandleMovement()
    {
        if (!_player.isGrounded) return;

        if (Input.GetKey(this.leftKey))
        {
            _playerDirection += Vector2.left * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(this.rightKey))
        {
            _playerDirection += Vector2.right * (movementSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// TODO Could be in GameState
    /// </summary>
    private void HandleExit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GameState.Singleton.GameOver();
        }
    }

    public virtual void AddScore()
    {
        Score += GameState.Singleton.GameSpeed * _scoreMultiplier * Time.deltaTime;
    }

    public void ResetScore()
    {
        Score = 0f;
    }

    public void SubtractScore(float toSubtract)
    {
        Score = Mathf.Max(Score - toSubtract, 0);
    }

    public void ResetPosition()
    {
        this.gameObject.transform.position = this.defaultPosition;
    }

    public void Death()
    {
        this.ResetPosition();
        this.EnterRespawnedState(3f);
        this.SubtractScore(50 + GameState.Singleton.GameSpeed *
            2.5f); // Account for the game getting quicker (punish idle players)
    }

    public virtual void EnterRespawnedState(float duration) {}

    protected virtual void LeaveRespawnedState() {}

    /// <summary>
    /// Activate 2x Score powerup for 5s
    /// </summary>
    protected void Activate2xScore()
    {
        this._scoreMultiplier = 2f;
        this.scoreMultiplierAlert.text = "2x Active";
        
        Invoke(nameof(Deactivate2xScore), 5f);
    }

    protected void Deactivate2xScore()
    {
        this._scoreMultiplier = 1f;
        this.scoreMultiplierAlert.text = "";
    }
}

