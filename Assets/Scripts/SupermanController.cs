using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SupermanController : MonoBehaviour
{
    // Movimiento
    public float speed = 5f;
    private float minX, maxX, minY, maxY;
    public ScoreManager scoreManager;

    // Vidas
    public int maxLives = 3;
    private int currentLives;
    public Image[] heartImages;

    // Game Over
    public GameObject gameOverPanel;

    // Invulnerabilidad
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 2f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Audio
    public AudioClip hitSound;
    private AudioSource audioSource;
    public AudioSource windSound;

    void Start()
    {
        // Calcular límites de la pantalla
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float halfHeight = cam.orthographicSize;

        minX = -halfWidth + 0.5f;
        maxX = halfWidth - 0.5f;
        minY = -halfHeight + 0.5f;
        maxY = halfHeight - 0.5f;

        // Inicializar vidas
        currentLives = maxLives;
        UpdateHeartsUI();

        // Ocultar Game Over al inicio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        if (windSound != null)
        {
            windSound.loop = true;
            windSound.Play();
        }
    }

    void Update()
    {
        if (currentLives <= 0) return;

        // Movimiento WASD
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.D)) moveX = 1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;
        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;

        // Normalizar para movimiento diagonal
        Vector2 move = new Vector2(moveX, moveY).normalized;

        // Aplicar movimiento
        Vector3 newPos = transform.position;
        newPos.x += move.x * speed * Time.deltaTime;
        newPos.y += move.y * speed * Time.deltaTime;

        // Aplicar límites
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;

        if (Random.Range(0f, 1f) < 0.1f)
        {
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cloud") && !isInvulnerable && currentLives > 0)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentLives--;
        UpdateHeartsUI();

        // Reproducir sonido de golpe
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        float elapsed = 0;

        // Parpadeo durante la invulnerabilidad
        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
                heartImages[i].enabled = true;   // Corazón visible
            else
                heartImages[i].enabled = false;  // Corazón oculto
        }
    }

    public void OnCloudEscaped()
    {
        if (scoreManager != null && currentLives > 0)
        {
            scoreManager.AddScore(10);
        }
    }

    void GameOver()
    {
        if (windSound != null)
        {
            windSound.Stop();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Reanudar el tiempo (por si estaba pausado)
        Time.timeScale = 1f;

        // Recargar la escena actual
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}