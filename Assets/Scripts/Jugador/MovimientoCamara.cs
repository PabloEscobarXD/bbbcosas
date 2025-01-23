using Unity.VisualScripting;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    public GameObject player;
    public Jugador scriptJugador;
    private Vector2 velocity = Vector2.zero;  
    public float smoothTime;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector2.SmoothDamp(gameObject.transform.position, player.transform.position, ref velocity, smoothTime);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
    }
}
