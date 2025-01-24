using UnityEngine;

public class Explosion : MonoBehaviour
{
    private string affectedBombTag;
    private static float explosionScaleMultiplier = 1f;  // Multiplicador para el tama�o de las explosiones
    private static int consecutiveExplosions = 0;       // Cantidad de explosiones consecutivas
    private static float timeSinceLastExplosion = 0f;   // Tiempo desde la �ltima explosi�n
    private const int maxConsecutiveExplosions = 7;     // M�ximo n�mero de explosiones consecutivas antes de limitar el tama�o
    private const float explosionResetTime = 3f;        // Tiempo para reiniciar el tama�o de las explosiones

    // Establece el tag de las bombas afectadas por esta explosi�n
    public void SetAffectedBombTag(string bombTag)
    {
        affectedBombTag = bombTag;
    }

    // Establece el tama�o (escala) de la explosi�n
    public void SetExplosionScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void Start()
    {
        // Destruye la explosi�n despu�s de 1 segundo
        Destroy(gameObject, 1f);

        // Resetea el temporizador porque ocurri� una explosi�n
        timeSinceLastExplosion = 0f;

        // Incrementa las explosiones consecutivas y ajusta el tama�o
        if (consecutiveExplosions < maxConsecutiveExplosions)
        {
            explosionScaleMultiplier += 0.3f;
        }
        consecutiveExplosions++;
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

    // Para obtener el multiplicador de escala en cualquier momento
    public static float GetExplosionScaleMultiplier()
    {
        return explosionScaleMultiplier;
    }

    // Reinicia el tama�o de las explosiones y el contador
    public static void ResetExplosionScale()
    {
        explosionScaleMultiplier = 1f;    // Reinicia el tama�o base de las explosiones
        consecutiveExplosions = 0;       // Reinicia el contador de explosiones consecutivas
        timeSinceLastExplosion = 0f;     // Reinicia el temporizador
    }
}
