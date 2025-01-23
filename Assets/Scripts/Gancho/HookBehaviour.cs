using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    public Transform mira;
    public GameObject player;
    public float hookSpeed;
    private Jugador playerScript;
    private Rigidbody rb;

    void Update()
    {
        if (playerScript.isShooting)
        {
            shootHook();
        }
    }
    public void shootHook()
    {
        transform.position = player.transform.position;
        Vector2 tempCrosshairPos = mira.transform.position;
        transform.position = tempCrosshairPos * hookSpeed * Time.deltaTime;
    }
}
