using UnityEngine;

public class bomb_slow : MonoBehaviour
{
    //referencias
    public Transform circle; //asi se llama el obj del jugador


    //velocidad
    public float speed = 0.1f;

    void Update()
    {
        if (circle != null)
        {
            //calcular direccion
            Vector3 direction = (circle.position - transform.position).normalized;
            //moverse
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}

