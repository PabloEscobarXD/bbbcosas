using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class audioManager : MonoBehaviour
{
    public static audioManager Instance { get; private set; }
    private AudioSource audioSource;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Evitar múltiples instancias
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void reproducir(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

}
