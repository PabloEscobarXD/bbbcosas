using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineSpawners : MonoBehaviour
{
    public List<GameObject> minesPrefab; // Prefab de las minas
    public float spawnInterval = 1f; // Tiempo entre minas
    public float offscreenDistance = 2f; // Distancia fuera de la pantalla
    public Camera maincamera;
    public float minDistanceBetweenMines = 2f; // Distancia mínima entre las minas
    private float elapsedTime;

    private List<GameObject> spawnedMines = new List<GameObject>(); // Lista de minas generadas

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Iniciar el ciclo de generación de minas
        elapsedTime = 0;
        InvokeRepeating(nameof(SpawnMine), 0f, spawnInterval);
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 2f)
        {
            spawnInterval+=5f;
        }
    }

    void SpawnMine()
    {
        Vector3 spawnPosition = GetSpawnPosition();

        // Generar la mina en la posición calculada
        GameObject enemy = Instantiate(minesPrefab[Random.Range(0, minesPrefab.Count)], spawnPosition, Quaternion.identity);
        spawnedMines.Add(enemy); // Añadir la mina recién generada a la lista de minas

        // Debug.Log("Mine Spawned: " + enemy);

        // Configurar el Rigidbody2D y BoxCollider2D
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Direccionar la mina hacia el centro de la pantalla
            Vector2 direction = (maincamera.transform.position - enemy.transform.position).normalized;
            rb.linearVelocity = direction * 2f; // Velocidad de la mina
        }

        // Comenzar a monitorear si la mina está fuera de la pantalla
        StartCoroutine(DestroyMineWhenOutOfView(enemy));
    }

    // Método para calcular la posición de la mina fuera de la pantalla
    Vector3 GetSpawnPosition()
    {
        float minX = maincamera.ViewportToWorldPoint(new Vector3(0f, 0f, maincamera.nearClipPlane)).x;
        float maxX = maincamera.ViewportToWorldPoint(new Vector3(1f, 0f, maincamera.nearClipPlane)).x;

        float minY = maincamera.ViewportToWorldPoint(new Vector3(0f, 0f, maincamera.nearClipPlane)).y;
        float maxY = maincamera.ViewportToWorldPoint(new Vector3(0f, 1f, maincamera.nearClipPlane)).y;

        // Generar una posición fuera de la cámara (al azar)
        Vector3 spawnPosition = new Vector3();

        if (Random.value < 0.5f) // Generar en el eje Y (arriba o abajo)
        {
            spawnPosition = new Vector3(Random.Range(minX, maxX), (Random.value < 0.5f) ? maxY + offscreenDistance : minY - offscreenDistance, 0f);
        }
        else // Generar en el eje X (izquierda o derecha)
        {
            spawnPosition = new Vector3((Random.value < 0.5f) ? maxX + offscreenDistance : minX - offscreenDistance, Random.Range(minY, maxY), 0f);
        }

        return spawnPosition;
    }

    // Coroutine para destruir la mina después de 10 segundos fuera de la pantalla
    IEnumerator DestroyMineWhenOutOfView(GameObject mine)
    {
        // Tiempo que la mina ha estado fuera de la pantalla
        float timeOutside = 0f;

        // Comprobar si la mina está fuera de la pantalla
        while (true)
        {
            if (IsOutsideOfScreen(mine))
            {
                timeOutside += Time.deltaTime; // Aumentar el tiempo fuera de la pantalla
            }
            else
            {
                timeOutside = 0f; // Resetear el contador si la mina está dentro de la pantalla
            }

            if (timeOutside >= 10f) // Si lleva más de 10 segundos fuera de la pantalla
            {
                spawnedMines.Remove(mine); // Eliminar la mina de la lista
                Destroy(mine); // Destruir la mina
                yield break; // Salir del ciclo
            }

            yield return null; // Esperar al siguiente frame
        }
    }

    // Método para verificar si un objeto está fuera de la vista de la cámara
    bool IsOutsideOfScreen(GameObject mine)
    {
        // Obtener los límites de la cámara en el mundo
        Vector3 screenPosition = maincamera.WorldToViewportPoint(mine.transform.position);  

        // Verificar si está fuera de la pantalla (fuera del rango [0, 1] en X o Y)
        return screenPosition.x < 0f || screenPosition.x > 1f || screenPosition.y < 0f || screenPosition.y > 1f;
    }
}
