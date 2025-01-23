using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float playerSpeed = 5f; // Velocidad del jugador
    void Start()
    {
        transform.position = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Movimiento del jugador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector2(horizontalInput, verticalInput) * playerSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLISION");
    }
}