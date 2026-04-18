using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float spawnInterval = 1.0f;
    private float timer;
    private float difficultyMultiplier = 1f;

    void Start()
    {
        timer = spawnInterval;
        InvokeRepeating("IncreaseDifficulty", 0f, 10f);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnCloud();
            timer = spawnInterval / difficultyMultiplier;
        }
    }

    void SpawnCloud()
    {
        // Aparece desde el borde DERECHO
        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0, 0));

        // Posición Y aleatoria dentro de la pantalla
        float yPos = Random.Range(-4f, 4f);
        spawnPos.y = yPos;

        GameObject newCloud = Instantiate(cloudPrefab, spawnPos, Quaternion.identity);

        // Tamaño aleatorio
        float randomScale = Random.Range(0.7f, 1.3f);
        newCloud.transform.localScale = new Vector3(randomScale, randomScale, 1);
    }

    void IncreaseDifficulty()
    {
        difficultyMultiplier += 0.1f;
        difficultyMultiplier = Mathf.Min(difficultyMultiplier, 2.5f);

        CloudMovement[] clouds = FindObjectsByType<CloudMovement>(FindObjectsSortMode.None);
        foreach (CloudMovement c in clouds)
        {
            c.speed += 0.2f;
        }
    }
}