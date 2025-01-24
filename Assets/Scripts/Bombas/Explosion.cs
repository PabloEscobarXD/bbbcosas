using UnityEngine;

public class Explosion : MonoBehaviour
{
    private string affectedBombTag;
    private static float explosionScaleMultiplier = 1f;  // Multiplier to scale the explosion size

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

        // Crece la escala de la explosi�n para la siguiente en la cadena
        explosionScaleMultiplier += 0.3f;
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
}
