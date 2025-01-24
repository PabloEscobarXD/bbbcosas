    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MusicPlayer : MonoBehaviour
    {
        private static MusicPlayer instance;

        private AudioSource audioSource;

        void Awake()
        {
            
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
        if (SceneManager.GetActiveScene().name == "Juego")
        {
            StopMusic();
        }
    }

       
        public void StopMusic()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

       
        public bool IsMusicPlaying()
        {
            return audioSource.isPlaying;
        }
    }
