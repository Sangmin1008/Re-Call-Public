using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    public Image buttonImage;
    private bool isPointerOver = false;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        if (buttonImage != null && hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        if (buttonImage != null && defaultSprite != null)
        {
            buttonImage.sprite = defaultSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonImage != null && clickSprite != null)
        {
            buttonImage.sprite = clickSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage == null) return;

        // 마우스를 떼었을 때 위치에 따라 hover 또는 default로 복귀
        if (isPointerOver && hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
        }
        else if (!isPointerOver && defaultSprite != null)
        {
            buttonImage.sprite = defaultSprite;
        }
    }
}
