using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Puzzle1UI : MonoBehaviour
{
    public GameObject panel;
    public TMP_InputField inputField;
    public Button submitButton;

    private Puzzle1Console activeConsole;
    public string correctAnswer = "12:12:12";

    public void Setup(Puzzle1Console console)
    {
        activeConsole = console;
        panel.SetActive(true);
        inputField.text = "";
    }

    void Start()
    {
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void CheckAnswer()
    {
        if (inputField.text.Trim() == correctAnswer)
        {
            Debug.Log("정답!");
            activeConsole.UnlockDoor();
            panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EventBus.PublishVoid("EnablePlayerInput");
        }
        else
        {
            Debug.Log("오답.");
        }
    }
    public void ExitPuzzle()
    {
        Debug.Log("퍼즐 UI 종료");
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventBus.PublishVoid("EnablePlayerInput");
    }
}
