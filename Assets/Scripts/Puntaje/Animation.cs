using UnityEngine;

public class ExplosionAnimationController : MonoBehaviour
{
    private Animator animator;
    private float animationDuration;

    private void Start()
    {
        // Obtener el Animator del GameObject (el prefab de explosi�n)
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Obtener la duraci�n de la animaci�n (asumiendo que solo hay una animaci�n en el Animator)
            animationDuration = animator.runtimeAnimatorController.animationClips[0].length;

            // Iniciar la animaci�n con el Trigger "Explode"
            animator.SetTrigger("Explode");

            // Destruir el GameObject despu�s de que termine la animaci�n
            Destroy(gameObject, animationDuration);
        }
        else
        {
            Debug.LogWarning("El prefab de explosi�n no tiene un Animator.");
            // Si no tiene un Animator, destruimos el GameObject despu�s de 2 segundos como fallback
            Destroy(gameObject, 2f);
        }
    }
}
