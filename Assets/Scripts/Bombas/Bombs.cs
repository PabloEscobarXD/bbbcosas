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
        // Verifica si esta bomba est� siendo transportada por el gancho.
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
        // Crear la explosi�n en la posici�n actual.
        GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Obtener el multiplicador de escala de la explosi�n
        float scaleMultiplier = Explosion.GetExplosionScaleMultiplier();

        // Notificar al gancho que debe regresar si est� transportando esta bomba.
        if (isAttachedToHook && hook != null)
        {
            Debug.Log("Regresando Gancho");
            Gancho ganchoScript = hook.GetComponent<Gancho>();
            if (ganchoScript != null)
            {
                ganchoScript.ReleaseBomb(); // Nuevo m�todo en el gancho.
            }
        }

        // Configurar el tama�o de la explosi�n
        Explosion explosionScript = explosionInstance.GetComponent<Explosion>();
        if (explosionScript != null)
        {
            explosionScript.SetAffectedBombTag(gameObject.tag);
            explosionScript.SetExplosionScale(scaleMultiplier);  // Establecer la escala de la explosi�n
        }

        // Destruir la bomba actual.
        Destroy(gameObject);
        TestCombo.points += 10;
        Debug.Log("a: " + TestCombo.points);
        audioManager.Instance.reproducir(explosionAudio);

        // Si la colisi�n proviene de otra bomba, crea una explosi�n adicional en la posici�n de la bomba colisionada.
        if (collision != null)
        {
            GameObject otherExplosionInstance = Instantiate(explosionPrefab, collision.gameObject.transform.position, Quaternion.identity);
            Explosion otherExplosionScript = otherExplosionInstance.GetComponent<Explosion>();
            if (otherExplosionScript != null)
            {
                otherExplosionScript.SetAffectedBombTag(collision.gameObject.tag);
                otherExplosionScript.SetExplosionScale(scaleMultiplier);  // Establecer la escala de la explosi�n
                //TestCombo.points++;
                Debug.Log("a: " + TestCombo.points);
                //audioManager.Instance.reproducir(explosionAudio);
            }

            Destroy(collision.gameObject);
        }
    }
}
