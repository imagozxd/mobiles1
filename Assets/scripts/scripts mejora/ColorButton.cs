using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private Button button;

    public delegate void ColorSelected(Color color);
    public static event ColorSelected OnColorSelected;

    private void Start()
    {
        button.onClick.AddListener(() => OnColorSelected?.Invoke(colorData.colorValue));
    }
}
