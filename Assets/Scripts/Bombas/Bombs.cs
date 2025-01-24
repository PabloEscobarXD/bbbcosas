using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public Color color; // Color de la bomba.
    public bool isAttachedToHook = false; // Si esta bomba est� acoplada al gancho.
    private List<Bombs> connectedBombs = new List<Bombs>(); // Bombas conectadas a esta bomba.

    // M�todo para acoplar la bomba al gancho.
    public void AcoplarAlGancho()
    {
        if (!isAttachedToHook)
        {
            isAttachedToHook = true;
            Debug.Log("La bomba se acopl� al gancho.");
            // Mueve las bombas conectadas junto con esta.
            ActualizarPosicionDeBombas();
        }
    }

    // M�todo para acoplar una bomba a esta bomba.
    public void AcoplarOtraBomba(Bombs otraBomba)
    {
        if (!connectedBombs.Contains(otraBomba))
        {
            connectedBombs.Add(otraBomba);
            otraBomba.AcoplarAlGancho(); // Marca la otra bomba como acoplada tambi�n.
            Debug.Log("Se acopl� otra bomba.");
        }
    }

    // M�todo que mueve todas las bombas conectadas con esta bomba.
    private void ActualizarPosicionDeBombas()
    {
        foreach (Bombs bomba in connectedBombs)
        {
            bomba.transform.position = transform.position; // Las mueve a la posici�n de la bomba principal.
        }
    }

    // M�todo para manejar colisiones.
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
