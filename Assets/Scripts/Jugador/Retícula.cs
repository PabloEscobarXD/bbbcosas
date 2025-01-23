using UnityEngine;

public class Retícula : MonoBehaviour
{
    public Transform player; // Referencia al jugador.
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
        if (player == null) return;

        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)(worldMousePosition - player.position);

        direction = direction.normalized * currentRadius;
        Vector2 reticlePosition = (Vector2)player.position + direction;

        transform.position = new Vector3(reticlePosition.x, reticlePosition.y, transform.position.z);

        currentRadius = Mathf.Lerp(currentRadius, radius, adjustmentSpeed * Time.deltaTime);
    }

    public void SetRadius(float newRadius)
    {
        radius = Mathf.Clamp(newRadius, minRadius, maxRadius);
    }
}
