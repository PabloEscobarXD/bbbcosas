using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad de movimiento hacia arriba
    public float fadeSpeed = 2f; // Velocidad de desvanecimiento
    private TextMeshPro textMesh; // Referencia al componente TextMeshPro
    private Color textColor; // Color del texto

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>(); // Asegúrate de que el componente TextMeshPro esté correctamente asignado
        if (textMesh != null)
        {
            textColor = textMesh.color; // Obtener el color inicial
        }
    }

    void Update()
    {
        // Mover el texto hacia arriba
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Desvanecer el texto
        if (textMesh != null)
        {
            textColor.a -= fadeSpeed * Time.deltaTime; // Reducir la opacidad
            textMesh.color = textColor;

            // Destruir el objeto una vez que sea completamente transparente
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Método para asignar texto, pasando el componente TextMeshPro como parámetro
    public void SetText(TextMeshPro mesh, string text)
    {
        if (mesh != null)
        {
            mesh.text = text; // Asignar el texto de puntos
        }
    }
}
