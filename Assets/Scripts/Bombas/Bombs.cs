using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bombs : MonoBehaviour
{
    private float explosionRadius = 3f;
    public bool isAttachedToHook;
    public GameObject explosionPrefab, hook;
    public AudioClip explosionAudio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Contains("player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene("gameover");
        }
        // Verifica si esta bomba está siendo transportada por el gancho.
        if (isAttachedToHook)
        {
            Bombs otraBomba = collision.gameObject.GetComponent<Bombs>();
            if (otraBomba != null && collision.gameObject.CompareTag(gameObject.tag))
            {
                Explode(collision);
            }
        }
    }

    public void Explode(Collision2D collision)
    {
        // Crear la explosión en la posición actual.
        GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Obtener el multiplicador de escala de la explosión
        float scaleMultiplier = Explosion.GetExplosionScaleMultiplier();

        // Notificar al gancho que debe regresar si está transportando esta bomba.
        if (isAttachedToHook && hook != null)
        {
            Debug.Log("Regresando Gancho");
            Gancho ganchoScript = hook.GetComponent<Gancho>();
            if (ganchoScript != null)
            {
                ganchoScript.ReleaseBomb(); // Nuevo método en el gancho.
            }
        }

        // Configurar el tamaño de la explosión
        Explosion explosionScript = explosionInstance.GetComponent<Explosion>();
        if (explosionScript != null)
        {
            explosionScript.SetAffectedBombTag(gameObject.tag);
            explosionScript.SetExplosionScale(scaleMultiplier);  // Establecer la escala de la explosión
        }

        // Destruir la bomba actual.
        Destroy(gameObject);
        TestCombo.points += 10;
        Debug.Log("a: " + TestCombo.points);
        audioManager.Instance.reproducir(explosionAudio);

        // Si la colisión proviene de otra bomba, crea una explosión adicional en la posición de la bomba colisionada.
        if (collision != null)
        {
            GameObject otherExplosionInstance = Instantiate(explosionPrefab, collision.gameObject.transform.position, Quaternion.identity);
            Explosion otherExplosionScript = otherExplosionInstance.GetComponent<Explosion>();
            if (otherExplosionScript != null)
            {
                otherExplosionScript.SetAffectedBombTag(collision.gameObject.tag);
                otherExplosionScript.SetExplosionScale(scaleMultiplier);  // Establecer la escala de la explosión
                //TestCombo.points++;
                Debug.Log("a: " + TestCombo.points);
                //audioManager.Instance.reproducir(explosionAudio);
            }

            Destroy(collision.gameObject);
        }
    }
}
