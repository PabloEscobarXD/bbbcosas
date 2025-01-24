using Unity.VisualScripting;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    public GameObject player, mira, camara; // Referencia al jugador y a la mira (retícula).
    public float hookSpeed = 10f; // Velocidad del gancho.
    public float returnSpeed = 15f; // Velocidad al regresar al jugador.
    public float followDelay = 0.2f; // Retraso al seguir la mira.
    public bool isShooting, isAttached, isReleased;
    public AudioClip ganchoGo;
    public AudioClip ganchoBack;
    public AudioClip acopleAudio;

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
            audioManager.Instance.reproducir(ganchoGo);
            isShooting = true;
            isReleased = false;
            isReturning = false;
            gameObject.SetActive(true);
            hookDirection = (mira.transform.position - transform.position).normalized;
        }

        // Lógica para soltar la bomba.
        if (Input.GetKeyDown(KeyCode.Mouse1) && isAttached)
        {
            audioManager.Instance.reproducir(ganchoBack);
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

    public void ReleaseBomb()
    {
        if (attachedBomb != null)
        {
            Bombs bombScript = attachedBomb.GetComponent<Bombs>();
            if (bombScript != null)
            {
                bombScript.isAttachedToHook = false; // La bomba ya no está en el gancho.
            }
        }

        isAttached = false;
        attachedBomb = null;
        isShooting = false;
        isReturning = true;
        returnTarget = player.transform.position;
    }


    public void ReturnToPlayer()
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
        if (isReturning)
            return;

        if (isAttached)
            return;

        if (isShooting && (collision.gameObject.CompareTag("red") || collision.gameObject.CompareTag("blue") || collision.gameObject.CompareTag("green") || collision.gameObject.CompareTag("yellow")))
        {
            audioManager.Instance.reproducir(acopleAudio);
            isShooting = false;
            isAttached = true; // El gancho queda acoplado.
            isReleased = false;
            attachedBomb = collision.gameObject; // La bomba queda acoplada al gancho.

            // Marca la bomba como agarrada por el gancho.
            Bombs bombScript = attachedBomb.GetComponent<Bombs>();
            if (bombScript != null)
            {
                bombScript.isAttachedToHook = true;
                bombScript.hook = gameObject; // Asigna este gancho a la bomba.
            }

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
        }
    }

}
