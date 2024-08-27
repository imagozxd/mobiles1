using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TouchController : MonoBehaviour
{
    public Rect restrictedArea; // Define la zona restringida en coordenadas del mundo
    public Color gizmoColor = Color.red; // Color para dibujar el área restringida en la vista de escena

    public GameObject image1Prefab;
    public GameObject image2Prefab;
    public GameObject image3Prefab;

    private GameObject selectedImagePrefab;


    public Button button1; // Botón para seleccionar la imagen 1
    public Button button2; // Botón para seleccionar la imagen 2
    public Button button3; // Botón para seleccionar la imagen 3

    public Button colorButtonRed;
    public Button colorButtonGreen;
    public Button colorButtonBlue;

    private Color selectedColor = Color.white; // Color por defecto

    public float doubleTapTime = 0.3f; // Tiempo máximo entre dos toques para considerarlo un doble tap
    private float lastTapTime = -1f; // Tiempo del último toque
    private bool firstTapOnPoke = false; // Para verificar si el primer tap fue en un objeto con tag 'poke'
    private GameObject draggedObject = null; // Objeto que se está arrastrando

    // Variables para swipe
    public GameObject trailPrefab; // Prefab del TrailRenderer
    private GameObject currentTrail; // Instancia actual del TrailRenderer
    private TrailRenderer currentTrailRenderer; // Componente TrailRenderer del TrailRenderer
    private Vector2 swipeStartPos; // Posición de inicio del swipe
    private bool isSwiping = false;
    public float swipeThreshold = 0.1f; // Umbral para considerar un swipe
    public float swipeDistanceThreshold = 1.0f; // Distancia mínima para eliminar imágenes instanciadas

    // Lista para guardar las imágenes instanciadas
    private List<GameObject> instantiatedImages = new List<GameObject>();

    void Start()
    {
        button1.onClick.AddListener(() => SelectImage(image1Prefab));
        button2.onClick.AddListener(() => SelectImage(image2Prefab));
        button3.onClick.AddListener(() => SelectImage(image3Prefab));

        colorButtonRed.onClick.AddListener(() => SetColor(Color.red));
        colorButtonGreen.onClick.AddListener(() => SetColor(Color.green));
        colorButtonBlue.onClick.AddListener(() => SetColor(Color.blue));
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                float currentTime = Time.time;

                // Realizar un raycast para detectar colisiones con objetos
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (lastTapTime > 0 && currentTime - lastTapTime <= doubleTapTime)
                {
                    // Detectar el doble tap
                    Debug.Log("Doble tap detectado.");

                    if (firstTapOnPoke)
                    {
                        if (hit.collider != null && hit.collider.CompareTag("poke"))
                        {
                            // Eliminar el objeto con el tag 'poke'
                            Destroy(hit.collider.gameObject);
                            Debug.Log("Objeto con tag 'poke' eliminado.");
                        }
                    }
                }
                else
                {
                    // Primer tap
                    firstTapOnPoke = false;
                    draggedObject = null; // Resetear el objeto arrastrado

                    if (hit.collider != null && hit.collider.CompareTag("poke"))
                    {
                        Debug.Log("Raycast tocó un objeto con el tag 'poke'. No se instanciará ninguna imagen.");
                        firstTapOnPoke = true;
                        draggedObject = hit.collider.gameObject; // Asignar el objeto para arrastrarlo
                    }
                    else if (selectedImagePrefab != null && IsInRestrictedArea(touchPosition))
                    {
                        Debug.Log("Touch dentro del área restringida.");
                        GameObject instantiatedImage = Instantiate(selectedImagePrefab, touchPosition, Quaternion.identity);
                        SpriteRenderer instantiatedRenderer = instantiatedImage.GetComponent<SpriteRenderer>();
                        if (instantiatedRenderer != null)
                        {
                            instantiatedRenderer.color = selectedColor;
                        }
                        // Añadir la imagen instanciada a la lista
                        instantiatedImages.Add(instantiatedImage);
                    }

                    // Configurar inicio del swipe
                    swipeStartPos = touchPosition;
                    isSwiping = false;

                    // Crear y posicionar el TrailRenderer
                    if (trailPrefab != null)
                    {
                        currentTrail = Instantiate(trailPrefab, touchPosition, Quaternion.identity);
                        currentTrail.transform.position = touchPosition;
                        currentTrailRenderer = currentTrail.GetComponent<TrailRenderer>(); // Obtener el TrailRenderer
                        if (currentTrailRenderer != null)
                        {
                            // Cambiar el color del TrailRenderer al color seleccionado
                            currentTrailRenderer.startColor = selectedColor;
                            currentTrailRenderer.endColor = selectedColor;
                        }
                    }
                }

                lastTapTime = currentTime; // Actualizar el tiempo del último toque
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (draggedObject != null)
                {
                    // Si hay un objeto siendo arrastrado, moverlo con el toque
                    draggedObject.transform.position = touchPosition;
                }

                if (currentTrail != null)
                {
                    // Mover el TrailRenderer con el swipe
                    currentTrail.transform.position = touchPosition;

                    // Detectar si el swipe ha comenzado
                    float distance = Vector2.Distance(swipeStartPos, touchPosition);
                    if (distance > swipeThreshold)
                    {
                        isSwiping = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (draggedObject != null)
                {
                    // Finalizar el drag
                    draggedObject = null; // Limpiar el objeto arrastrado
                }

                if (isSwiping)
                {
                    // Detectar el fin del swipe
                    Debug.Log("Swipe detectado. Fin del swipe.");

                    // Verificar si el swipe supera la distancia mínima
                    float swipeDistance = Vector2.Distance(swipeStartPos, touchPosition);
                    if (swipeDistance > swipeDistanceThreshold)
                    {
                        // Eliminar todas las imágenes instanciadas
                        foreach (GameObject image in instantiatedImages)
                        {
                            Destroy(image);
                        }
                        instantiatedImages.Clear(); // Limpiar la lista
                        Debug.Log("Todas las imágenes instanciadas han sido eliminadas.");
                    }
                }

                // Eliminar el TrailRenderer si existe
                if (currentTrail != null)
                {
                    Destroy(currentTrail);
                }
            }
        }
    }

    bool IsInRestrictedArea(Vector2 position)
    {
        return restrictedArea.Contains(position);
    }

    void SelectImage(GameObject imagePrefab)
    {
        selectedImagePrefab = imagePrefab;
    }

    void SetColor(Color color)
    {
        selectedColor = color;

        // Actualizar el color del TrailRenderer si está activo
        if (currentTrailRenderer != null)
        {
            currentTrailRenderer.startColor = selectedColor;
            currentTrailRenderer.endColor = selectedColor;
        }
    }

    void OnDrawGizmos()
    {
        // Dibuja un rectángulo en la vista de escena para mostrar la zona restringida
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(new Vector3(restrictedArea.x + restrictedArea.width / 2, restrictedArea.y + restrictedArea.height / 2, 0), new Vector3(restrictedArea.width, restrictedArea.height, 0));
    }
}
