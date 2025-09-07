using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Button backButton;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(HandleVolumeChange);
        backButton.onClick.AddListener(CloseSettings);

        // ESC 키 처리
        settingsPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }

    void HandleVolumeChange(float value)
    {
        AudioListener.volume = value;
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
