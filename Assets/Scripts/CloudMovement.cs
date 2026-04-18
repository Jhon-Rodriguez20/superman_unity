using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 3f;
    private bool hasBeenCounted = false;
    private SupermanController supermanController;

    void Start()
    {
        supermanController = FindObjectOfType<SupermanController>();
    }

    void Update()
    {
        // Moverse hacia la izquierda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Destruir al salir por la izquierda y sumar punto
        if (transform.position.x < Camera.main.ViewportToWorldPoint(Vector3.zero).x - 1f)
        {
            // Si no ha sido contada aún, sumar punto
            if (!hasBeenCounted && supermanController != null)
            {
                hasBeenCounted = true;
                supermanController.OnCloudEscaped();
            }

            Destroy(gameObject);
        }
    }
}