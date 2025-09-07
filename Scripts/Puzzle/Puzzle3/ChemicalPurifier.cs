using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChemicalPurifier : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI entryCodeText;
    public TextMeshProUGUI selectedText1;
    public TextMeshProUGUI selectedText2;
    public Button submitButton;

    [Header("시약 슬롯 (프리팹 위치할 부모)")]
    public Transform flaskSlot1;
    public Transform flaskSlot2;

    [Header("정답 시약 설정")]
    public string correct1 = "A";
    public string correct2 = "F";

    [Header("플라스크 프리팹 (A~F 순서)")]
    public GameObject[] flaskPrefabs;

    [Header("오염 지역 제어")]
    public ToxicZone toxicZone;

    private List<string> selectedChemicals = new List<string>();
    private GameObject currentFlask1;
    private GameObject currentFlask2;

    public void Start()
    {
        submitButton.onClick.AddListener(CheckCombination);
        ResetPuzzle();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventBus.PublishVoid("DisablePlayerInput");
    }

    public void SelectChemicalButton(Button btn)
    {
        string chemical = btn.name; // "A", "B", ...

        if (selectedChemicals.Count >= 2 || selectedChemicals.Contains(chemical)) return;

        selectedChemicals.Add(chemical);
        UpdateSelectedUI();

        GameObject prefab = GetFlaskPrefab(chemical);
        if (selectedChemicals.Count == 1)
            AddFlaskToSlot(flaskSlot1, prefab, 1);
        else if (selectedChemicals.Count == 2)
            AddFlaskToSlot(flaskSlot2, prefab, 2);
    }

    void AddFlaskToSlot(Transform slot, GameObject prefab, int index)
    {
        if (index == 1 && currentFlask1 != null) Destroy(currentFlask1);
        if (index == 2 && currentFlask2 != null) Destroy(currentFlask2);

        GameObject go = Instantiate(prefab, slot);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;

        if (index == 1) currentFlask1 = go;
        else currentFlask2 = go;
    }

    void UpdateSelectedUI()
    {
        // Entry Code: A + B 형식 출력
        if (selectedChemicals.Count == 0)
            entryCodeText.text = "Entry Code: -";
        else if (selectedChemicals.Count == 1)
            entryCodeText.text = $"Entry Code: {selectedChemicals[0]}";
        else
            entryCodeText.text = $"Entry Code: {selectedChemicals[0]} + {selectedChemicals[1]}";

        selectedText1.text = selectedChemicals.Count > 0 ? $" Flask 1: {selectedChemicals[0]}" : " Flask 1: -";
        selectedText2.text = selectedChemicals.Count > 1 ? $" Flask 2: {selectedChemicals[1]}" : " Flask 2: -";
    }

    void CheckCombination()
    {
        if (selectedChemicals.Contains(correct1) && selectedChemicals.Contains(correct2))
        {
            Debug.Log("정답! 오염 제거 실행");
            EventBus.Publish<int>("QuestClear", 8);
            toxicZone.Cleanse();
            puzzlePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EventBus.PublishVoid("EnablePlayerInput");
        }
        else
        {
            Debug.Log("잘못된 조합입니다.");
        }

        ResetPuzzle();
    }

    public void ResetPuzzle()
    {
        selectedChemicals.Clear();
        entryCodeText.text = "Entry Code: -";
        selectedText1.text = "Flask 1: -";
        selectedText2.text = "Flask 2: -";

        if (currentFlask1 != null) Destroy(currentFlask1);
        if (currentFlask2 != null) Destroy(currentFlask2);
    }

    GameObject GetFlaskPrefab(string chemical)
    {
        int index = chemical[0] - 'A';
        return (index >= 0 && index < flaskPrefabs.Length) ? flaskPrefabs[index] : null;
    }

    // Exit 버튼에 연결할 함수
    public void ExitPuzzle()
    {
        Debug.Log("퍼즐 UI 종료");
        puzzlePanel.SetActive(false);
        ResetPuzzle();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventBus.PublishVoid("EnablePlayerInput");
    }
}