using UnityEngine;
using TMPro;

public class Explosion : MonoBehaviour
{
    private string affectedBombTag;
    private static float explosionScaleMultiplier = 1f; // Multiplicador para el tama�o de las explosiones
    private static int consecutiveExplosions = 0;      // Cantidad de explosiones consecutivas
    private static float timeSinceLastExplosion = 0f;  // Tiempo desde la �ltima explosi�n
    private const int maxConsecutiveExplosions = 7;    // M�ximo n�mero de explosiones consecutivas antes de limitar el tama�o
    private const float explosionResetTime = 1.5f;       // Tiempo para reiniciar el tama�o de las explosiones
    public GameObject explosionPrefab; // Prefab de la explosi�n (GIF o sprite)

    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject floatingTextPrefab; // Prefab del texto flotante

    private int pointsPerExplosion = 10; // Puntos por explosi�n

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
        // Destruye la explosi�n despu�s de 1 segundo
        Destroy(gameObject, 0.5f);

        // Resetea el temporizador porque ocurri� una explosi�n
        timeSinceLastExplosion = 0f;

        // Incrementa las explosiones consecutivas y ajusta el tama�o
        if (consecutiveExplosions < maxConsecutiveExplosions)
        {
            TestCombo.multiCombo++;
            explosionScaleMultiplier += 0.3f;
        }
        consecutiveExplosions++;

        // A�adir puntos al puntaje
        AddPoints(pointsPerExplosion);

        // Mostrar texto flotante
        SpawnFloatingText(pointsPerExplosion);
    }

    private void Update()
    {
        // Actualiza el tiempo desde la �ltima explosi�n
        timeSinceLastExplosion += Time.deltaTime;

        // Si pasan 3 segundos sin explosiones, reinicia el tama�o y el contador
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
                bomba.Explode(null); // Reutiliza la funci�n Explode en la bomba
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
            Debug.LogError("ScoreManager no est� asignado en Explosion.");
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

            Destroy(floatingTextInstance, 2f); // Destruir el texto despu�s de un tiempo fijo
        }
        else
        {
            Debug.LogError("FloatingTextPrefab no est� asignado en Explosion.");
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
