using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TestCombo : MonoBehaviour
{
    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject explosionPrefab; // Prefab de la explosi�n (GIF o sprite)
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
        if (multiCombo > auxpoint)
        {
            scoreManager.AddCombo(multiCombo); // Agrega el combo al ScoreManager
            auxpoint = multiCombo; // Actualiza el �ltimo combo procesado
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
            

            // Opcional: Generar explosi�n o texto flotante
            //SpawnExplosion();
            //SpawnFloatingText(deltaPoints);
            isComboEnded = false;
        }
        //SpawnExplosion(); // Generar explosi�n
        //SpawnFloatingText(points); // Generar el texto flotante
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
