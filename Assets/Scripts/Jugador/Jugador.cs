using UnityEngine;
using System.Collections;

public class Jugador : MonoBehaviour
{
    public float playerSpeed = 5f; // Velocidad del jugador
    public GameObject burbujaPrefab; // Prefab de la burbuja
    public float intervaloBurbuja = 0.05f; // Intervalo de tiempo entre cada burbuja (más corto para amontonarlas)
    public float velocidadBurbuja = 5f; // Velocidad a la que las burbujas se disparan
    public float tiempoDesvanecer = 2f; // Tiempo para desvanecer la burbuja antes de destruirla (más largo)

    private float tiempoUltimaBurbuja = 0f;

    void Start()
    {
        transform.position = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Movimiento del jugador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movimiento = new Vector2(horizontalInput, verticalInput);

        // Mueve al jugador
        transform.Translate(movimiento * playerSpeed * Time.deltaTime);

        // Lanza burbujas en dirección contraria al movimiento
        LanzarBurbujas(movimiento);
    }

    void LanzarBurbujas(Vector2 direccion)
    {
        // Asegúrate de que la dirección no sea cero
        if (direccion.magnitude > 0 && Time.time - tiempoUltimaBurbuja >= intervaloBurbuja)
        {
            // Instancia la burbuja en la posición del jugador, pero ligeramente hacia atrás (en la dirección contraria)
            Vector2 posicionBurbuja = (Vector2)transform.position - direccion.normalized * 0.5f; // Aumenta a 0.5f o lo que necesites

            // Instancia la burbuja y orienta su dirección
            GameObject burbuja = Instantiate(burbujaPrefab, posicionBurbuja, Quaternion.identity);

            // Ajustar el tamaño de la burbuja (más pequeña)
            burbuja.transform.localScale = new Vector3(0.1f, 0.1f, 1f); // Ajusta la escala según sea necesario (más pequeño)

            // Mueve la burbuja en dirección contraria al movimiento del jugador y hace que ascienda
            Rigidbody2D rb = burbuja.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(-direccion.x, -direccion.y + 1) * velocidadBurbuja; // Asciende poco a poco
            }

            // Llama a la función para hacer que la burbuja se desvanezca después de un tiempo
            StartCoroutine(DesvanecerBurbuja(burbuja));

            // Resetea el tiempo de la última burbuja lanzada
            tiempoUltimaBurbuja = Time.time;
        }
    }

    private IEnumerator DesvanecerBurbuja(GameObject burbuja)
    {
        SpriteRenderer spriteRenderer = burbuja.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color colorInicial = spriteRenderer.color;

            // Comienza a desvanecer la burbuja
            float tiempoTranscurrido = 0f;
            while (tiempoTranscurrido < tiempoDesvanecer)
            {
                float alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / tiempoDesvanecer);
                spriteRenderer.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
                tiempoTranscurrido += Time.deltaTime;
                yield return null;
            }

            // Destruye la burbuja después de que se desvanezca
            Destroy(burbuja);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLISION");
    }
}
