using UnityEngine;

public class BackgroundLoopFinal : MonoBehaviour
{
    public float speed = 1.5f;
    private float spriteWidth;
    private Camera mainCamera;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
        mainCamera = Camera.main;

        // Si es Fondo2, posicionarlo automáticamente a la derecha de Fondo1
        if (gameObject.name == "Fondo2")
        {
            GameObject fondo1 = GameObject.Find("Fondo1");
            if (fondo1 != null)
            {
                transform.position = new Vector3(
                    fondo1.transform.position.x + spriteWidth,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }

    void Update()
    {
        // Mover hacia la izquierda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Calcular el borde izquierdo de la cámara
        float leftEdgeOfCamera = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;

        // REPOSICIONAR ANTES DE QUE SALGA COMPLETAMENTE
        if (transform.position.x + spriteWidth < leftEdgeOfCamera + spriteWidth * 0.5f)
        {
            // Buscar el fondo que está más a la derecha
            GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");
            float maxX = -9999;
            GameObject rightmostBackground = null;

            foreach (GameObject bg in backgrounds)
            {
                if (bg != gameObject && bg.transform.position.x > maxX)
                {
                    maxX = bg.transform.position.x;
                    rightmostBackground = bg;
                }
            }

            // Reposicionar este fondo a la derecha del más lejano
            if (rightmostBackground != null)
            {
                Vector3 newPos = transform.position;
                newPos.x = rightmostBackground.transform.position.x + spriteWidth;

                // REDONDEAR a 3 decimales para que sean exactamente iguales
                newPos.x = Mathf.Round(newPos.x * 1000f) / 1000f;

                transform.position = newPos;
            }
        }
    }
}