using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TestCombo : MonoBehaviour
{
    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject explosionPrefab; // Prefab de la explosión (GIF o sprite)
    public GameObject floatingTextPrefab; // Prefab del texto flotante
    static public int points = 0; // Cantidad de puntos que se suman
    static public int multiCombo = 1;
    private int auxpoint = 0;
    private int auxpoint2 = 0;
    private float timer = 0f; // Temporizador
    public float resetTime = 5f; // Tiempo para reiniciar el combo (en segundos)
    static public bool isComboEnded;
    

    void Start()
    {
        isComboEnded = false;
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
        if (multiCombo > auxpoint)
        {
            scoreManager.AddCombo(multiCombo); // Agrega el combo al ScoreManager
            auxpoint = multiCombo; // Actualiza el último combo procesado
            timer = 0f; // Reinicia el temporizador
        }
        else
        {
            // Incrementa el temporizador si no hay cambios
            timer += Time.deltaTime;

            // Si han pasado 5 segundos, reinicia el combo
            if (timer >= resetTime)
            {
                multiCombo = 0;
                auxpoint = 0;
                timer = 0f; // Reinicia el temporizador
            }
        }


        if (points > auxpoint)
        {
            int deltaPoints = points - auxpoint; // Calcula la diferencia de puntos
            deltaPoints = deltaPoints * multiCombo;

            scoreManager.AddScore(deltaPoints); // Suma solo la diferencia al puntaje
            
            auxpoint = points; // Actualiza `auxpoint` al nuevo valor de `points`
            

            // Opcional: Generar explosión o texto flotante
            //SpawnExplosion();
            //SpawnFloatingText(deltaPoints);
            isComboEnded = false;
        }
        //SpawnExplosion(); // Generar explosión
        //SpawnFloatingText(points); // Generar el texto flotante
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
