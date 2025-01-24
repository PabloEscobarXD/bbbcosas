using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public Color color; // Color de la bomba.
    public bool isAttachedToHook = false; // Si esta bomba está acoplada al gancho.
    private List<Bombs> connectedBombs = new List<Bombs>(); // Bombas conectadas a esta bomba.

    // Método para acoplar la bomba al gancho.
    public void AcoplarAlGancho()
    {
        if (!isAttachedToHook)
        {
            isAttachedToHook = true;
            Debug.Log("La bomba se acopló al gancho.");
            // Mueve las bombas conectadas junto con esta.
            ActualizarPosicionDeBombas();
        }
    }

    // Método para acoplar una bomba a esta bomba.
    public void AcoplarOtraBomba(Bombs otraBomba)
    {
        if (!connectedBombs.Contains(otraBomba))
        {
            connectedBombs.Add(otraBomba);
            otraBomba.AcoplarAlGancho(); // Marca la otra bomba como acoplada también.
            Debug.Log("Se acopló otra bomba.");
        }
    }

    // Método que mueve todas las bombas conectadas con esta bomba.
    private void ActualizarPosicionDeBombas()
    {
        foreach (Bombs bomba in connectedBombs)
        {
            bomba.transform.position = transform.position; // Las mueve a la posición de la bomba principal.
        }
    }

    // Método para manejar colisiones.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bombs otraBomba = collision.gameObject.GetComponent<Bombs>();
        if (otraBomba != null && otraBomba.color == color)
        {
            if (isAttachedToHook || otraBomba.isAttachedToHook)
            {
                AcoplarOtraBomba(otraBomba); // Acopla la bomba a la red.
            }
        }
    }
}
