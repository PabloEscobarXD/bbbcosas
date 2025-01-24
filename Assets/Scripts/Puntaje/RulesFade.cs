using UnityEngine;
using System.Collections;

public class RulesFade : MonoBehaviour
{
    public float displayDuration = 5f;  // Tiempo que las reglas estar�n visibles.
    public float fadeDuration = 2f;     // Duraci�n del desvanecimiento.

    private CanvasGroup canvasGroup;

    void Start()
    {
        // Intenta obtener el CanvasGroup desde este objeto.
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError("No se encontr� un CanvasGroup en este objeto.");
            return;
        }

        // Empieza la secuencia de desvanecimiento
        StartCoroutine(FadeOutRules());
    }

    private IEnumerator FadeOutRules()
    {
        // Espera el tiempo que el usuario estableci� para mostrar las reglas
        yield return new WaitForSeconds(displayDuration);

        // Tiempo transcurrido para el efecto de desvanecimiento
        float elapsedTime = 0f;

        // Desvanecimiento gradual de la opacidad
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Lerp la opacidad de 1 (visible) a 0 (invisible)
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Asegurarse de que la opacidad llegue exactamente a 0
        canvasGroup.alpha = 0f;

        // Destruir el objeto con el CanvasGroup despu�s de desaparecer
        Destroy(gameObject);
    }
}
