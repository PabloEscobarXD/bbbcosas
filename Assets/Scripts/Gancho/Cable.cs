using UnityEngine;

public class CableController : MonoBehaviour
{
    public Transform player; // Transform del jugador (punto A).
    public Transform hook;   // Transform del gancho (punto B).
    public Vector2 hookOffset; // Offset para ajustar a la base del sprite del gancho.

    private LineRenderer lineRenderer;

    void Start()
    {
        // Añade o referencia el LineRenderer.
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("No se encontró un LineRenderer en el objeto.");
        }

        // Configuración básica del LineRenderer.
        lineRenderer.positionCount = 2; // Dos puntos: inicio y fin.
        lineRenderer.startWidth = 0.1f; // Grosor inicial de la línea.
        lineRenderer.endWidth = 0.1f;   // Grosor final de la línea.
        lineRenderer.useWorldSpace = true; // Usa las posiciones globales.
    }

    void Update()
    {
        if (lineRenderer != null && player != null && hook != null)
        {
            UpdateCable();
        }
    }

    void UpdateCable()
    {
        // Posición inicial: el jugador.
        lineRenderer.SetPosition(0, player.position);

        // Posición final: base del sprite del gancho.
        Vector3 hookBasePosition = hook.position + (Vector3)hookOffset; // Aplica el offset.
        lineRenderer.SetPosition(1, hookBasePosition);
    }
}
