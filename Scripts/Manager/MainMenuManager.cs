using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsPanel;


    public void OnStartClicked()
    {
        SceneManager.LoadScene("First Floor");
    }

    public void OnSettingClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
