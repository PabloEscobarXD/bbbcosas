using Unity.VisualScripting;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    public GameObject player, mira, camara; // Referencia al jugador y a la mira (retícula).
    public float hookSpeed = 10f; // Velocidad del gancho.
    public float returnSpeed = 15f; // Velocidad al regresar al jugador.
    public float followDelay = 0.2f; // Retraso al seguir la mira.
    public bool isShooting, isAttached, isReleased;

    private Vector2 hookDirection;
    private GameObject attachedBomb; // Referencia a la bomba acoplada.
    private bool isReturning; // Si el gancho está regresando al jugador.
    private Vector3 returnTarget; // Objetivo al que regresa el gancho.
    private Vector3 hookSmoothPosition; // Posición suavizada del gancho.

    void Start()
    {
        isShooting = false; //Disparando (Evita el spameo)
        isAttached = false; //Agarrado
        isReleased = true;  //Soltado
        attachedBomb = null;
        isReturning = false; // Inicialmente, no está regresando.
        hookSmoothPosition = transform.position; // Inicializa la posición suavizada.
    }

    void Update()
    {
        // Lógica de disparo.
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isShooting && !isAttached)
        {
            isShooting = true;
            isReleased = false;
            isReturning = false;
            gameObject.SetActive(true);
            hookDirection = (mira.transform.position - transform.position).normalized;
        }

        // Lógica para soltar la bomba.
        if (Input.GetKeyDown(KeyCode.Mouse1) && isAttached)
        {
            ReleaseBomb();
        }

        // Disparo del gancho.
        if (isShooting)
        {
            ShootHook();
        }
        // Retorno al jugador.
        else if (isReturning)
        {
            ReturnToPlayer();
        }
        // Gancho se mueve con el mouse tras quedar estático.
        else if (isAttached && attachedBomb != null)
        {
            FollowMira();
        }
        // Si no interactúa, regresa al jugador.
        else if (isReleased)
        {
            transform.position = player.transform.position;
        }
    }

    void ShootHook()
    {
        transform.Translate(hookDirection * hookSpeed * Time.deltaTime);

        // Limita el alcance del gancho.
        if (Vector3.Distance(transform.position, player.transform.position) > 4.5f)
        {
            StopHook();
        }
    }

    void ReleaseBomb()
    {
        isAttached = false;
        attachedBomb = null;
        isShooting = false;
        isReturning = true;
        returnTarget = player.transform.position;
    }

    void ReturnToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, returnTarget, returnSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, returnTarget) < 0.1f)
        {
            isReturning = false;
            isReleased = true;
        }

        Retícula retícula = mira.GetComponent<Retícula>();
        if (retícula != null)
        {
            retícula.SetRadius(retícula.maxRadius); // Establece el nuevo radio máximo basado en la posición del gancho.
        }
    }

    void StopHook()
    {
        isShooting = false;
        isReleased = true;
        transform.position = player.transform.position;
    }

    void FollowMira()
    {
        // Movimiento del gancho hacia la posición de la retícula.
        hookSmoothPosition = Vector3.Lerp(hookSmoothPosition, mira.transform.position, followDelay);
        transform.position = hookSmoothPosition;

        // Movimiento de la bomba acoplada junto con el gancho.
        if (attachedBomb != null)
        {
            attachedBomb.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("dummy"))
        {
            Debug.Log("DUMMY");
            isShooting = false;
            isAttached = true; // El gancho queda acoplado.
            isReleased = false;
            attachedBomb = collision.gameObject; // La bomba queda acoplada al gancho.

            // Calcula la distancia entre el jugador y el gancho.
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // Ajusta el radio máximo de la retícula a la distancia actual.
            Retícula retícula = mira.GetComponent<Retícula>();
            if (retícula != null)
            {
                retícula.SetRadius(distance); // Establece el nuevo radio máximo basado en la posición del gancho.
            }

            MovimientoCamara movCamara = camara.GetComponent<MovimientoCamara>();
            movCamara.ApplyZoom();

            Bombs bomba = collision.gameObject.GetComponent<Bombs>();
            if (bomba != null)
            {
                Debug.Log("Gancho colisionó con una bomba.");
                bomba.AcoplarAlGancho();
            }
        }
    }
}
