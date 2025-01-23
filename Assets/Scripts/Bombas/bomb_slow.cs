using UnityEngine;

public class bomb_slow : MonoBehaviour
{
    //referencias
    public Transform circle; //asi se llama el obj del jugador


    //velocidad
    public float speed = 0.5f;

    public bool initState = false; //para calcular la posicion al espawnear
    Vector3 direction; //direccion del jugador


    void Update()
    {
        if (circle != null)
        {
            
            //calcular direccion
            if (!initState)
            {
                direction = (circle.position - transform.position).normalized;
                initState = true;
            }
            
            //moverse
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}

