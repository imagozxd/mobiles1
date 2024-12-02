using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Rect restrictedArea;
    [SerializeField] private GameObject trailPrefab;

    private Color selectedColor = Color.white;
    private GameObject selectedImagePrefab;

    private GameObject currentTrail;
    private TrailRenderer currentTrailRenderer;

    private float lastTapTime = -1f;
    private const float doubleTapTime = 0.3f;
    private GameObject draggedObject;

    private void OnEnable()
    {
        ColorButton.OnColorSelected += UpdateColor;
        ImageButton.OnImageSelected += UpdateImage;
    }

    private void OnDisable()
    {
        ColorButton.OnColorSelected -= UpdateColor;
        ImageButton.OnImageSelected -= UpdateImage;
    }

    private void UpdateColor(Color color)
    {
        selectedColor = color;
        
    }

    private void UpdateImage(GameObject prefab)
    {
        selectedImagePrefab = prefab;
        Debug.Log($"Imagen actualizada: {prefab.name}");
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegan(touchPosition);
                    break;

                case TouchPhase.Moved:
                    HandleTouchMoved(touchPosition);
                    break;

                case TouchPhase.Ended:
                    HandleTouchEnded();
                    break;
            }
        }
    }

    private void HandleTouchBegan(Vector2 touchPosition)
    {
        if (trailPrefab != null)
        {
            currentTrail = Instantiate(trailPrefab, touchPosition, Quaternion.identity);
            currentTrailRenderer = currentTrail.GetComponent<TrailRenderer>();

            if (currentTrailRenderer != null)
            {
                Color trailColor = selectedColor;
                trailColor.a = 1f;
                currentTrailRenderer.startColor = trailColor;
                currentTrailRenderer.endColor = trailColor;
                Debug.Log("TrailRenderer configurado con alpha 1");
            }
        }


        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("poke"))
            {
                float currentTime = Time.time;

                if (lastTapTime > 0 && currentTime - lastTapTime <= doubleTapTime)
                {
                    Destroy(hit.collider.gameObject);
                    //Debug.Log("2 tap pokee.");
                }
                else
                {
                    draggedObject = hit.collider.gameObject;
                    //Debug.Log("arrastrando");
                }

                lastTapTime = currentTime;
                return;
            }
        }

        if (selectedImagePrefab != null && IsInRestrictedArea(touchPosition))
        {
            GameObject instantiatedImage = Instantiate(selectedImagePrefab, touchPosition, Quaternion.identity);
            SpriteRenderer renderer = instantiatedImage.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color colorWithAlpha = selectedColor;
                colorWithAlpha.a = 1f;  
                renderer.color = colorWithAlpha;
                Debug.Log("imagen + color con alpha 1");
            }
        }

    }

    private void HandleTouchMoved(Vector2 touchPosition)
    {
        if (draggedObject != null)
        {
            draggedObject.transform.position = touchPosition;
        }

        if (currentTrail != null)
        {
            currentTrail.transform.position = touchPosition;
        }
    }

    private void HandleTouchEnded()
    {
        if (draggedObject != null)
        {
            draggedObject = null;
        }

        if (currentTrail != null)
        {
            Destroy(currentTrail);
            currentTrail = null;
            currentTrailRenderer = null;
        }
    }

    private bool IsInRestrictedArea(Vector2 position)
    {
        return restrictedArea.Contains(position);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red; // Color del gizmo
    //    Vector3 center = new Vector3(restrictedArea.x + restrictedArea.width / 2, restrictedArea.y + restrictedArea.height / 2, 0);
    //    Vector3 size = new Vector3(restrictedArea.width, restrictedArea.height, 0);
    //    Gizmos.DrawWireCube(center, size);
    //}
}
