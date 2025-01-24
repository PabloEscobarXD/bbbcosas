using UnityEngine;
using TMPro;

public class Explosion : MonoBehaviour
{
    private string affectedBombTag;
    private static float explosionScaleMultiplier = 1f; // Multiplicador para el tamaño de las explosiones
    private static int consecutiveExplosions = 0;      // Cantidad de explosiones consecutivas
    private static float timeSinceLastExplosion = 0f;  // Tiempo desde la última explosión
    private const int maxConsecutiveExplosions = 7;    // Máximo número de explosiones consecutivas antes de limitar el tamaño
    private const float explosionResetTime = 1.5f;       // Tiempo para reiniciar el tamaño de las explosiones
    public GameObject explosionPrefab; // Prefab de la explosión (GIF o sprite)

    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject floatingTextPrefab; // Prefab del texto flotante

    private int pointsPerExplosion = 10; // Puntos por explosión

    public void SetAffectedBombTag(string bombTag)
    {
        affectedBombTag = bombTag;
    }

    public void SetExplosionScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void Start()
    {
        // Destruye la explosión después de 1 segundo
        Destroy(gameObject, 0.5f);

        // Resetea el temporizador porque ocurrió una explosión
        timeSinceLastExplosion = 0f;

        // Incrementa las explosiones consecutivas y ajusta el tamaño
        if (consecutiveExplosions < maxConsecutiveExplosions)
        {
            TestCombo.multiCombo++;
            explosionScaleMultiplier += 0.3f;
        }
        consecutiveExplosions++;

        // Añadir puntos al puntaje
        AddPoints(pointsPerExplosion);

        // Mostrar texto flotante
        SpawnFloatingText(pointsPerExplosion);
    }

    private void Update()
    {
        // Actualiza el tiempo desde la última explosión
        timeSinceLastExplosion += Time.deltaTime;

        // Si pasan 3 segundos sin explosiones, reinicia el tamaño y el contador
        if (timeSinceLastExplosion >= explosionResetTime)
        {
            ResetExplosionScale();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(affectedBombTag))
        {
            Bombs bomba = collision.gameObject.GetComponent<Bombs>();
            if (bomba != null)
            {
                bomba.Explode(null); // Reutiliza la función Explode en la bomba
            }
        }
    }

    private void AddPoints(int points)
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(points);
        }
        else
        {
            Debug.LogError("ScoreManager no está asignado en Explosion.");
        }
    }

    private void SpawnFloatingText(int points)
    {
        if (floatingTextPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 1.1f;

            GameObject floatingTextInstance = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

            TextMeshPro textMesh = floatingTextInstance.GetComponent<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = "+" + points + "pts";
            }

            Destroy(floatingTextInstance, 2f); // Destruir el texto después de un tiempo fijo
        }
        else
        {
            Debug.LogError("FloatingTextPrefab no está asignado en Explosion.");
        }
    }

    public static float GetExplosionScaleMultiplier()
    {
        return explosionScaleMultiplier;
    }

    public static void ResetExplosionScale()
    {
        explosionScaleMultiplier = 1f;
        consecutiveExplosions = 0;
        timeSinceLastExplosion = 0f;
        TestCombo.multiCombo = 1;
    }
}
