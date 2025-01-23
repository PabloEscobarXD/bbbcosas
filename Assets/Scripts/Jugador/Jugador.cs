using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public GameObject player, hook;
    public float playerSpeed;
    public bool isShooting;
    private HookBehaviour HookBehaviourScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.transform.position = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        player.transform.Translate(new Vector2(horizontalInput, verticalInput)* playerSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Mouse0) && !isShooting)
        {
            isShooting = true;
            Instantiate(hook);
        }
    }
    void Shoot()
    {

    }
}
