using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TestCombo : MonoBehaviour
{
    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject explosionPrefab; // Prefab de la explosión (GIF o sprite)
    public GameObject floatingTextPrefab; // Prefab del texto flotante

    void Start()
    {
        if (scoreManager == null)
        {
            // Buscar automáticamente el ScoreManager en la escena si no está asignado
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("No se encontró un ScoreManager en la escena.");
            }
        }
    }

    void Update()
    {
        // Si se presiona la tecla E, sumar puntos, generar explosión y texto flotante
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (scoreManager != null)
            {
                int points = 10; // Cantidad de puntos que se suman
                scoreManager.AddScore(points); // Sumar puntos al puntaje total
                SpawnExplosion(); // Generar explosión
                SpawnFloatingText(points); // Generar el texto flotante
            }
        }
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab != null)
        {
            // Obtener la posición del objeto actual o un lugar específico
            Vector3 spawnPosition = transform.position; // Coloca donde quieras que aparezca la explosión

            // Instanciar la explosión en la posición
            GameObject explosionInstance = Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);

            // Destruir la explosión después de que termine su animación (suponiendo que tenga un Animator)
            Animator explosionAnimator = explosionInstance.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                // Esperar hasta que termine la animación de la explosión (dependiendo de la duración)
                Destroy(explosionInstance, explosionAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                // Si no tiene un Animator, puedes destruirla después de un tiempo fijo (por ejemplo, 2 segundos)
                Destroy(explosionInstance, 2f);
            }
        }
    }

    private void SpawnFloatingText(int points)
    {
        if (floatingTextPrefab != null)
        {
            // Obtener la posición del objeto actual o un lugar específico
            Vector3 spawnPosition = transform.position + Vector3.up * 1.1f; // Ajusta la posición para que esté por encima de la explosión

            // Instanciar el texto flotante en la posición
            GameObject floatingTextInstance = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

            // Obtener el componente TextMeshPro del prefab
            TextMeshPro textMesh = floatingTextInstance.GetComponent<TextMeshPro>();

            // Configurar el texto del FloatingText
            FloatingText floatingText = floatingTextInstance.GetComponent<FloatingText>();
            string scoreString = points.ToString(); // Convertir el puntaje a texto
            if (floatingText != null && textMesh != null)
            {
                floatingText.SetText(textMesh, "+" + scoreString + "pts"); // Asignar el texto de puntos
            }
        }
    }
}
