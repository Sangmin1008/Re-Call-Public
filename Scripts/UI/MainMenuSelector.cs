using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuSelector : MonoBehaviour
{
    public RectTransform selector;
    public Button[] menuButtons;

    private int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            int index = i;
            // 마우스 올렸을 때 화살표 이동
            EventTrigger trigger = menuButtons[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entryEnter.callback.AddListener((_) => OnHover(index));
            trigger.triggers.Add(entryEnter);

            // 마우스 클릭도 가능
            menuButtons[i].onClick.AddListener(() => OnClick(index));
        }

        UpdateSelectorPosition();
    }

    void Update()
    {
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length;
            UpdateSelectorPosition();
        }
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateSelectorPosition();
        }
        else if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    void UpdateSelectorPosition()
    {
        RectTransform target = menuButtons[currentIndex].GetComponent<RectTransform>();
        Vector3 newPosition = target.position;
        newPosition.x -= 387f;
        selector.position = newPosition;

        // 포커스도 함께 이동
        menuButtons[currentIndex].Select();
    }

    public void OnHover(int index)
    {
        currentIndex = index;
        UpdateSelectorPosition();
    }

    public void OnClick(int index)
    {
        currentIndex = index;
        UpdateSelectorPosition();
    }
}