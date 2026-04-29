using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float minSpeed = 10f;
    public float maxSpeed = 50f;
    public float acceleration = 15f;
    public float deceleration = 10f;
    public float currentSpeed;
    private float speedMultiplier = 1f;

    [Header("Lane Settings")]
    public float laneDistance = 3f;
    public float sideSpeed = 10f;

    [Header("Player Stats")]
    public int maxHp = 100;
    public int currentHp;

    [Header("UI References")]
    public Slider hpSlider;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI gameOverScoreText;

    [Header("Win/Loss Settings")]
    public int maxScore = 500;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI lastGameScoreText;

    [Header("Power-up Settings")]
    public float boostMultiplier = 2f;
    public bool isInvincible = false;

    [Header("Visual Effects")]
    public Color boostColor = Color.cyan;
    private Color originalColor;
    public MeshRenderer carRenderer;

    public int currentScore = 0;

    private bool isGameFinished = false;
    private CharacterController controller;
    private Vector3 targetPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHp = maxHp;
        currentSpeed = minSpeed;
        targetPosition = transform.position;

        if (hpSlider != null) hpSlider.maxValue = maxHp;

        carRenderer = GetComponentInChildren<MeshRenderer>();
        if (carRenderer != null)
        {
            originalColor = carRenderer.material.color;
        }

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (isGameFinished) return;
        HandleSpeed();

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLane(-1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveLane(1);

        Vector3 nextPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (nextPosition.x - transform.position.x) * sideSpeed;
        moveVector.z = currentSpeed * speedMultiplier;

        controller.Move(moveVector * Time.deltaTime);

        CheckWinLossConditions();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (hpSlider != null) hpSlider.value = currentHp;
        if (scoreText != null) scoreText.text = "Score: " + currentScore.ToString("0000");
        if (speedText != null) speedText.text = "Speed: " + Mathf.FloorToInt(currentSpeed * 10).ToString() + " km/h";
    }

    void HandleSpeed()
    {
        if (Input.GetKey(KeyCode.W))
            currentSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            currentSpeed -= deceleration * 2f * Time.deltaTime;
        else
        {
            if (currentSpeed > minSpeed)
                currentSpeed -= deceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }
    
    void GameOver()
    {
        isGameFinished = true;
        gameOverPanel.SetActive(true);
        SaveScore();
        Time.timeScale = 0;
    }
    void WinGame()
    {
        isGameFinished = true;
        winPanel.SetActive(true);
        SaveScore();
        Time.timeScale = 0;
    }
    void CheckWinLossConditions()
    {
        if (currentScore >= maxScore)
        {
            WinGame();
        }

        if (currentHp <= 0)
        {
            GameOver();
        }
    }

    void SaveScore()
    {
        PlayerPrefs.SetInt("LastScore", currentScore);
        PlayerPrefs.Save();

        if (winScoreText != null)
            winScoreText.text = "Final Score: " + currentScore.ToString();

        if (gameOverScoreText != null)
            gameOverScoreText.text = "Final Score: " + currentScore.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void MoveLane(int direction)
    {
        float newX = targetPosition.x + (direction * laneDistance);
        newX = Mathf.Clamp(newX, -laneDistance, laneDistance);
        targetPosition = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostRoutine(duration));
    }
    private void ResetSpeed() => speedMultiplier = 1f;

    public void Heal(int amount) => currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHp -= damage;
        if (currentHp <= 0) GameOver();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    private System.Collections.IEnumerator SpeedBoostRoutine(float duration)
    {
        isInvincible = true;
        speedMultiplier = boostMultiplier;

        if (carRenderer != null) carRenderer.material.color = boostColor;

        yield return new WaitForSeconds(duration);

        if (carRenderer != null) carRenderer.material.color = originalColor;

        speedMultiplier = 1f;
        isInvincible = false;
    }
}