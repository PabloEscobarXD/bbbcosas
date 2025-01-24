using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al TextMeshPro para mostrar el puntaje
    private int score = 0; // Variable que almacena el puntaje
    private string scoreDisplay = "- - - - -"; // Mostrar los guiones iniciales
    public float scoreSpeed = 1f; // Velocidad de incremento del puntaje (mayor valor = más rápido)
    public TextMeshProUGUI comboText;
    private int combo = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de que el puntaje comience en 0 al inicio
        UpdateScoreText();
    }

    // Llamar a este método cuando se gane puntos
    public void AddScore(int points)
    {
        StartCoroutine(AnimateScoreIncrease(score, score + points)); // Inicia la animación de aumento de puntaje
    }

    public void AddCombo(int points)
    {
        //comboText.text = "Combo:\n" + "X " +points;
    }

    // Llamar a este método cuando quieras restar puntos
    public void SubtractScore(int points)
    {
        StartCoroutine(AnimateScoreIncrease(score, score - points)); // Inicia la animación de disminución de puntaje
    }

    // Corrutina para animar el aumento de puntaje
    private IEnumerator AnimateScoreIncrease(int startValue, int targetValue)
    {
        float duration = Mathf.Abs(targetValue - startValue) / scoreSpeed; // Duración basada en la diferencia y la velocidad
        float elapsedTime = 0f; // Tiempo transcurrido

        while (elapsedTime < duration)
        {
            score = (int)Mathf.Lerp(startValue, targetValue, elapsedTime / duration); // Interpolación entre los valores
            UpdateScoreText(); // Actualiza el texto con el puntaje actual
            elapsedTime += Time.deltaTime; // Incrementa el tiempo transcurrido
            yield return null; // Espera el siguiente frame
        }

        score = targetValue; // Asegúrate de que el puntaje final sea el objetivo
        UpdateScoreText(); // Actualiza el texto una última vez con el valor final
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
        scoreText.text = "Puntaje:\n" + finalDisplay;
    }
}
