using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineSpawners : MonoBehaviour
{
    public List<GameObject> minesPrefab; // Prefab de las minas
    public float spawnInterval = 1f; // Tiempo entre minas
    public float offscreenDistance = 2f; // Distancia fuera de la pantalla
    public Camera maincamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Iniciar el ciclo de generación de minas
        InvokeRepeating(nameof(SpawnMine), 0f, spawnInterval);
    }

    void SpawnMine()
    {
        // Obtener límites de la cámara en el mundo
        float minX = maincamera.ViewportToWorldPoint(new Vector3(0f, 0f, maincamera.nearClipPlane)).x;
        float maxX = maincamera.ViewportToWorldPoint(new Vector3(1f, 0f, maincamera.nearClipPlane)).x;

        float minY = maincamera.ViewportToWorldPoint(new Vector3(0f, 0f, maincamera.nearClipPlane)).y;
        float maxY = maincamera.ViewportToWorldPoint(new Vector3(0f, 1f, maincamera.nearClipPlane)).y;

        // Generar una posición fuera de la cámara
        Vector3 spawnPosition = new Vector3();

        // Aleatoriamente decidimos si generamos la mina en un borde
        if (Random.value < 0.5f) // Generar en el eje Y (arriba o abajo)
        {
            spawnPosition = new Vector3(Random.Range(minX, maxX), (Random.value < 0.5f) ? maxY + offscreenDistance : minY - offscreenDistance, 0f);
        }
        else // Generar en el eje X (izquierda o derecha)
        {
            spawnPosition = new Vector3((Random.value < 0.5f) ? maxX + offscreenDistance : minX - offscreenDistance, Random.Range(minY, maxY), 0f);
        }

        // Seleccionar aleatoriamente un enemigo de la lista de variantes
        int randomIndex = Random.Range(0, minesPrefab.Count);
        GameObject selectedEnemy = minesPrefab[randomIndex];

        // Crear el enemigo seleccionado en la posición calculada
        GameObject enemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
        Debug.Log("Mine Spawned: " + selectedEnemy);

        // Configurar movimiento (puedes adaptarlo a tus necesidades)
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Direccionar la mina hacia el centro de la pantalla
            Vector2 direction = (maincamera.transform.position - enemy.transform.position).normalized;
            rb.linearVelocity = direction * 2f; // Velocidad de la mina
        }

        Destroy(enemy, 10f); // Destruir la mina después de 10 segundos
    }
}
