using UnityEngine;

public class SwipeTrail : MonoBehaviour
{
    private bool isDragging = false;

    void Update()
    {
        // Detectar si el toque ha comenzado (primer toque)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Verificar si el raycast toca un objeto con el tag "poke"
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("poke"))
            {
                isDragging = true;
                Debug.Log("Swipe iniciado"); // Imprimir cuando el swipe comienza
            }
        }

        // Mientras el toque está en progreso (moviéndose)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (isDragging)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                transform.position = new Vector3(touchPosition.x, touchPosition.y, transform.position.z); // Mover el objeto vacío con el dedo
            }
        }

        // Si el toque termina (dedo levantado)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (isDragging)
            {
                Debug.Log("Swipe terminado"); // Imprimir cuando el swipe termina
                isDragging = false;
            }
        }
    }
}
