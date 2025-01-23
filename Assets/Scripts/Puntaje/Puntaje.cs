using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al TextMeshPro para mostrar el puntaje
    private int score = 0; // Variable que almacena el puntaje
    private string scoreDisplay = "- - - - -"; // Mostrar los guiones iniciales

    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de que el puntaje comience en 0 al inicio
        UpdateScoreText();
    }

    // Llamar a este método cuando se gane puntos
    public void AddScore(int points)
    {
        score += points; // Aumentar el puntaje
        UpdateScoreText(); // Actualizar el texto del puntaje
    }

    // Llamar a este método cuando quieras restar puntos
    public void SubtractScore(int points)
    {
        score -= points; // Restar puntos
        UpdateScoreText(); // Actualizar el texto del puntaje
    }

    // Actualiza el texto del puntaje en el UI
    void UpdateScoreText()
    {
        string scoreString = score.ToString(); // Convertir el puntaje a texto
        char[] displayArray = scoreDisplay.Replace(" ", "").ToCharArray(); // Crear un arreglo con los guiones sin espacios

        // Rellenar los guiones de derecha a izquierda con los números
        int scoreLength = scoreString.Length;
        for (int i = 0; i < scoreLength; i++)
        {
            // Obtener la posición desde la derecha
            int position = displayArray.Length - 1 - i;
            displayArray[position] = scoreString[scoreLength - 1 - i];
        }

        // Volver a agregar los espacios para el formato final
        string finalDisplay = string.Join(" ", displayArray);
        scoreText.text = "Puntaje:\n"+ finalDisplay;
    }
}
