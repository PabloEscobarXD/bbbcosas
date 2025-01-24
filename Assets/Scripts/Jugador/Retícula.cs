using UnityEngine;

public class Retícula : MonoBehaviour
{
    public Transform player; // Referencia al jugador.
    public Transform spriteTransform; // Transform del sprite que se va a rotar.
    public float radius = 3f; // Radio inicial del círculo.
    public float minRadius = 1f; // Radio mínimo del círculo.
    public float maxRadius = 3f; // Radio máximo del círculo.
    public float adjustmentSpeed = 5f; // Velocidad de ajuste del radio.

    private Camera mainCamera;
    private float currentRadius; // Radio actual de la retícula.

    void Start()
    {
        mainCamera = Camera.main;
        currentRadius = radius;
    }

    void Update()
    {
        if (player == null || spriteTransform == null) return;

        // 1. Obtén la posición del mouse en el mundo 2D.
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)(worldMousePosition - player.position);

        // 2. Limita la posición de la retícula al borde del círculo.
        direction = direction.normalized * currentRadius;
        Vector2 reticlePosition = (Vector2)player.position + direction;

        // 3. Actualiza la posición de la retícula.
        transform.position = new Vector3(reticlePosition.x, reticlePosition.y, transform.position.z);

        // 4. Suaviza el radio hacia el valor deseado.
        currentRadius = Mathf.Lerp(currentRadius, radius, adjustmentSpeed * Time.deltaTime);

        // 5. Rota el sprite hacia la retícula.
        RotateSpriteTowardsReticle(direction);
    }

    public void SetRadius(float newRadius)
    {
        radius = Mathf.Clamp(newRadius, minRadius, maxRadius);
    }

    void RotateSpriteTowardsReticle(Vector2 direction)
    {
        // Calcula el ángulo en grados desde el jugador hacia la retícula.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica la rotación al transform del sprite (en el eje Z).
        spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
