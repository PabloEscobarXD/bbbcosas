using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bombs : MonoBehaviour
{
    private float explosionRadius = 3f;
    public bool isAttachedToHook;
    public GameObject explosionPrefab, hook;
    public GameObject explosionPrefab2; // Prefab de la explosi�n (GIF o sprite)
    public AudioClip explosionAudio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Contains("player"))
        {
            Destroy(collision.gameObject);
            TestCombo.points = 0;
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
