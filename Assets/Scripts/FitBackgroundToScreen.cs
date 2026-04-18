using UnityEngine;

public class FitBackgroundToScreen : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null) return;

        // Obtener tamaño de la pantalla en unidades
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Escalar el fondo para que cubra la pantalla
        Vector3 scale = transform.localScale;
        scale.x = screenWidth / sr.sprite.bounds.size.x;
        scale.y = screenHeight / sr.sprite.bounds.size.y;
        transform.localScale = scale;
    }
}