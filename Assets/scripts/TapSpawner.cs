using UnityEngine;

public class TapSpawner : MonoBehaviour
{
    public GameObject imagePrefab;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Instantiate(imagePrefab, touchPosition, Quaternion.identity);
        }
    }
}
