using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private readonly bool isOff = false;

    public GameObject LoginUI;
    public GameObject SettingUI;

    public GameObject AppUI;
    public GameObject ProfileUI;

    public void OpenSettings()
    {
        LoginUI.SetActive(isOff);
        SettingUI.SetActive(!isOff);

    }
    public void CloseSettings()
    {
        LoginUI.SetActive(!isOff);
        SettingUI.SetActive(isOff);
    }

    public void OpenProfile()
    {
        AppUI.SetActive(isOff);
        ProfileUI.SetActive(!isOff);

    }
    public void CloseProfile()
    {
        AppUI.SetActive(!isOff);
        ProfileUI.SetActive(isOff);
    }

    public void ReturnToMain() {
        SceneManager.LoadSceneAsync(1);
    }
    public void OpenAugmantedReality() {
        SceneManager.LoadSceneAsync(2);
    }
    public void OpenStationControl() {
        SceneManager.LoadSceneAsync(3);
    }
    public void OpenStationExplorer() {
        SceneManager.LoadSceneAsync(4);
    }
}
