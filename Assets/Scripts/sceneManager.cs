using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public AudioClip clickSonido;

    public void cambiarScene(string nombre)
    {
        audioManager.Instance.reproducir(clickSonido);
        SceneManager.LoadScene(nombre);
    }

    public void salir()
    {
        Application.Quit();
    }
}