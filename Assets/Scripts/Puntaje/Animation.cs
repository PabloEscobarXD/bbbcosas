using UnityEngine;

public class ExplosionAnimationController : MonoBehaviour
{
    private Animator animator;
    private float animationDuration;

    private void Start()
    {
        // Obtener el Animator del GameObject (el prefab de explosión)
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Obtener la duración de la animación (asumiendo que solo hay una animación en el Animator)
            animationDuration = animator.runtimeAnimatorController.animationClips[0].length;

            // Iniciar la animación con el Trigger "Explode"
            animator.SetTrigger("Explode");

            // Destruir el GameObject después de que termine la animación
            Destroy(gameObject, animationDuration);
        }
        else
        {
            Debug.LogWarning("El prefab de explosión no tiene un Animator.");
            // Si no tiene un Animator, destruimos el GameObject después de 2 segundos como fallback
            Destroy(gameObject, 2f);
        }
    }
}
