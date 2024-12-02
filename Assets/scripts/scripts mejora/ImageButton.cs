using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    public SpriteData spriteData;
    public Button button;

    public delegate void ImageSelected(GameObject prefab);
    public static event ImageSelected OnImageSelected;

    private void Start()
    {
        button.onClick.AddListener(() => OnImageSelected?.Invoke(spriteData.prefab));
    }
}
