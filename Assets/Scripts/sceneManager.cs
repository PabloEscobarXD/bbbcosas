using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void cambiarScene(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void salir()
    {
        Application.Quit();
    }
}
