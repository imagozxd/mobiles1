using UnityEngine;

public class LimitedTouchArea : MonoBehaviour
{
    public Rect restrictedArea; // Define la zona restringida en coordenadas del mundo
    public Color gizmoColor = Color.red; // Color para dibujar el área restringida en la vista de escena

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (IsInRestrictedArea(touchPosition))
            {
                Debug.Log("Touch dentro del área restringida.");
            }
            else
            {
                Debug.Log("Touch fuera del área restringida.");
                // Aquí puedes colocar la lógica para manejar el touch fuera del área restringida
            }
        }
    }

    bool IsInRestrictedArea(Vector2 position)
    {
        return restrictedArea.Contains(position);
    }

    void OnDrawGizmos()
    {
        // Dibuja un rectángulo en la vista de escena para mostrar la zona restringida
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(new Vector3(restrictedArea.x + restrictedArea.width / 2, restrictedArea.y + restrictedArea.height / 2, 0), new Vector3(restrictedArea.width, restrictedArea.height, 0));
    }
}
