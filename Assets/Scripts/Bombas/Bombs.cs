using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bombs : MonoBehaviour
{
    private float explosionRadius = 3f;
    public bool isAttachedToHook;
    public GameObject explosionPrefab, hook;
    public GameObject explosionPrefab2; // Prefab de la explosión (GIF o sprite)
    public AudioClip explosionAudio;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Contains("player"))
        {
            Destroy(collision.gameObject);
            TestCombo.points = 0;
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
