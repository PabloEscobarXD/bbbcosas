using UnityEngine;

public class bomb_fast : MonoBehaviour
{
    //referencias
    public Transform circle; //asi se llama el obj del jugador


    //velocidad
    public float speed = 1f;

    //tiempo de persecucion
    public float pursuitTime = 5f; //final
    public float passedTime = 0f; //incial

    public bool isPursuing = true; //para saber si está en modo perseguir o no
    Vector3 direction; //direccion del jugador


    void Update()
    {
        if (circle != null)
        {

            //calcular direccion
            if (isPursuing)
            {
                //perseguir
                direction = (circle.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                passedTime += Time.deltaTime; //timer

                //end
                if (passedTime >= pursuitTime)
                {
                    isPursuing = false;
                }
            }
            else
            {
                //moverse
                transform.position += direction * speed * Time.deltaTime;
            }

            
        }
    }
}