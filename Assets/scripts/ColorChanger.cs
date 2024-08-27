using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public enum ColorOption { Red, LightRed, Blue, LightBlue, Green, LightGreen }
    public ColorOption selectedColor;

    void Update()
    {
        switch (selectedColor)
        {
            case ColorOption.Red:
                spriteRenderer.color = Color.red;
                break;
            case ColorOption.LightRed:
                spriteRenderer.color = new Color(1f, 0.5f, 0.5f); // Rojo claro
                break;
            case ColorOption.Blue:
                spriteRenderer.color = Color.blue;
                break;
            case ColorOption.LightBlue:
                spriteRenderer.color = new Color(0.5f, 0.5f, 1f); // Azul claro
                break;
            case ColorOption.Green:
                spriteRenderer.color = Color.green;
                break;
            case ColorOption.LightGreen:
                spriteRenderer.color = new Color(0.5f, 1f, 0.5f); // verde claro
                break;
        }
    }
}
