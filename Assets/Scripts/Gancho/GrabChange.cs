using UnityEngine;

public class GrabChange : MonoBehaviour
{
    public Sprite normalSprite;  // Sprite predeterminado (cuando no est� agarrando).
    public Sprite grabbedSprite; // Sprite cuando est� agarrando algo.

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del hijo.
    private Gancho gancho; // Referencia al script Gancho en el padre.

    void Start()
    {
        // Obt�n el SpriteRenderer del propio objeto (el hijo).
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No se encontr� un SpriteRenderer en el objeto hijo.");
            return;
        }

        // Busca el script Gancho en el objeto padre.
        gancho = GetComponentInParent<Gancho>();

        if (gancho == null)
        {
            Debug.LogError("No se encontr� el script Gancho en el objeto padre.");
        }
    }

    void Update()
    {
        if (gancho != null && spriteRenderer != null)
        {
            // Cambia el sprite seg�n el estado del gancho (padre).
            if (gancho.isAttached)
            {
                spriteRenderer.sprite = grabbedSprite; // Cambia al sprite de agarrado.
            }
            else
            {
                spriteRenderer.sprite = normalSprite; // Cambia al sprite predeterminado.
            }
        }
    }
}
