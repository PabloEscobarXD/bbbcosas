using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{

    public Transform playerTransform; // Referencia a la c�mara
    public float followSpeed = 2f; // Velocidad del seguimiento
    public Vector3 offset; // Offset opcional para ajustar la posici�n del fondo

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Calcula la posici�n objetivo del fondo
            Vector3 targetPosition = playerTransform.position + offset;

            // Interpola suavemente hacia la posici�n objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
