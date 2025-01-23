using UnityEngine;

public class Retícula : MonoBehaviour
{
    public Transform player; // Referencia al jugador.
    public float radius = 3f; // Radio del círculo.

    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main; // Obtén la cámara principal.
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        // 1. Obtén la posición del mouse en el mundo 2D.
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // 2. Calcula el vector dirección desde el jugador hacia el mouse.
        Vector2 direction = (Vector2)(worldMousePosition - player.position);

        // 3. Restringe la posición al borde del círculo.
        direction = direction.normalized * radius; // Normaliza y ajusta al radio.
        Vector2 reticlePosition = (Vector2)player.position + direction;

        // 4. Actualiza la posición de la mira.
        transform.position = new Vector3(reticlePosition.x, reticlePosition.y, transform.position.z);
    }
}
