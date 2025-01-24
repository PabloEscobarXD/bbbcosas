using UnityEngine;

public class Explosion : MonoBehaviour
{
    private string affectedBombTag;
    private static float explosionScaleMultiplier = 1f;  // Multiplicador para el tamaño de las explosiones
    private static int consecutiveExplosions = 0;       // Cantidad de explosiones consecutivas
    private static float timeSinceLastExplosion = 0f;   // Tiempo desde la última explosión
    private const int maxConsecutiveExplosions = 7;     // Máximo número de explosiones consecutivas antes de limitar el tamaño
    private const float explosionResetTime = 3f;        // Tiempo para reiniciar el tamaño de las explosiones

    // Establece el tag de las bombas afectadas por esta explosión
    public void SetAffectedBombTag(string bombTag)
    {
        affectedBombTag = bombTag;
    }

    // Establece el tamaño (escala) de la explosión
    public void SetExplosionScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void Start()
    {
        // Destruye la explosión después de 1 segundo
        Destroy(gameObject, 1f);

        // Resetea el temporizador porque ocurrió una explosión
        timeSinceLastExplosion = 0f;

        // Incrementa las explosiones consecutivas y ajusta el tamaño
        if (consecutiveExplosions < maxConsecutiveExplosions)
        {
            explosionScaleMultiplier += 0.3f;
        }
        consecutiveExplosions++;
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

    // Para obtener el multiplicador de escala en cualquier momento
    public static float GetExplosionScaleMultiplier()
    {
        return explosionScaleMultiplier;
    }

    // Reinicia el tamaño de las explosiones y el contador
    public static void ResetExplosionScale()
    {
        explosionScaleMultiplier = 1f;    // Reinicia el tamaño base de las explosiones
        consecutiveExplosions = 0;       // Reinicia el contador de explosiones consecutivas
        timeSinceLastExplosion = 0f;     // Reinicia el temporizador
    }
}
