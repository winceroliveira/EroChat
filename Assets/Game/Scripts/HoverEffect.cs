using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private bool isHovering = false;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateImageColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateImageColor();
    }

    private void UpdateImageColor()
    {
        if (isHovering)
        {
            // Define a cor da imagem como colorida
            image.color = Color.white;
        }
        else
        {
            // Define a cor da imagem como preto e branco
            image.color = Color.gray;
        }
    }
}