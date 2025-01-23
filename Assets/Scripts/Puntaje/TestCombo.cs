using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TestCombo : MonoBehaviour
{
    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject explosionPrefab; // Prefab de la explosi�n (GIF o sprite)
    public GameObject floatingTextPrefab; // Prefab del texto flotante

    void Start()
    {
        if (scoreManager == null)
        {
            // Buscar autom�ticamente el ScoreManager en la escena si no est� asignado
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("No se encontr� un ScoreManager en la escena.");
            }
        }
    }

    void Update()
    {
        // Si se presiona la tecla E, sumar puntos, generar explosi�n y texto flotante
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (scoreManager != null)
            {
                int points = 10; // Cantidad de puntos que se suman
                scoreManager.AddScore(points); // Sumar puntos al puntaje total
                SpawnExplosion(); // Generar explosi�n
                SpawnFloatingText(points); // Generar el texto flotante
            }
        }
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab != null)
        {
            // Obtener la posici�n del objeto actual o un lugar espec�fico
            Vector3 spawnPosition = transform.position; // Coloca donde quieras que aparezca la explosi�n

            // Instanciar la explosi�n en la posici�n
            GameObject explosionInstance = Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);

            // Destruir la explosi�n despu�s de que termine su animaci�n (suponiendo que tenga un Animator)
            Animator explosionAnimator = explosionInstance.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                // Esperar hasta que termine la animaci�n de la explosi�n (dependiendo de la duraci�n)
                Destroy(explosionInstance, explosionAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                // Si no tiene un Animator, puedes destruirla despu�s de un tiempo fijo (por ejemplo, 2 segundos)
                Destroy(explosionInstance, 2f);
            }
        }
    }

    private void SpawnFloatingText(int points)
    {
        if (floatingTextPrefab != null)
        {
            // Obtener la posici�n del objeto actual o un lugar espec�fico
            Vector3 spawnPosition = transform.position + Vector3.up * 1.1f; // Ajusta la posici�n para que est� por encima de la explosi�n

            // Instanciar el texto flotante en la posici�n
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
