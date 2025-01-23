using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // Prefab de la burbuja
    public float spawnInterval = 1f; // Tiempo entre burbujas
    public float spawnY = -6f; // Punto de generación (fuera de pantalla)
    public float bubbleSpeed = 2f; // Velocidad de subida

    public Camera maincamera;
    public float moveVariation = 0.5f; // Movimiento zigzag
    public float sizeVariantion = 0.1f; // Cambio de forma leve

    void Start()
    {
        // Iniciar el ciclo de generación de burbujas
        InvokeRepeating(nameof(SpawnBubble), 0f, spawnInterval);
    }

    void SpawnBubble()
    {
        // Generar una posición aleatoria dentro del rango horizontal
        float minX = maincamera.ViewportToWorldPoint(new Vector3(0f, 0f, maincamera.nearClipPlane)).x;
        float maxX = maincamera.ViewportToWorldPoint(new Vector3(1f, 0f, maincamera.nearClipPlane)).x;

        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        // Crear la burbuja en la posición calculada
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Configurar movimiento hacia arriba
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, bubbleSpeed);
        }

        bubble.AddComponent<BubbleMovement>().moveVariation = moveVariation;
        bubble.AddComponent<BubbleMovement>().speed = bubbleSpeed;
        bubble.AddComponent<BubbleMovement>().sizeVariation = sizeVariantion;


        // Destruir la burbuja automáticamente al salir de la pantalla
        StartCoroutine(DestroyBubbleWhenOutOfView(bubble));
    }

    private IEnumerator DestroyBubbleWhenOutOfView(GameObject bubble)
    {
        while (bubble != null)
        {
            Vector3 viewportPos = maincamera.WorldToViewportPoint(bubble.transform.position);
            if (viewportPos.y > 1 || viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < -5)
            {
                Destroy(bubble);
                yield break; // Finaliza la corrutina al destruir el objeto
            }
            yield return null; // Esperar hasta el próximo frame
        }
    }
}


public class BubbleMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveVariation = 0.5f;
    public float sizeVariation = 0.2f;
    private float time;

    void Update()
    {
        // Movimiento hacia arriba
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Movimiento zigzag (leve) en el eje X
        time += Time.deltaTime;
        float zigzag = Mathf.Sin(time * Mathf.PI) * moveVariation;
        transform.Translate(Vector3.right * zigzag * Time.deltaTime);

        // Movimiento expansivo (leve)
        time += Time.deltaTime;
        float randomScale = Random.Range(0.1f, sizeVariation); // Tamaño aleatorio
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}