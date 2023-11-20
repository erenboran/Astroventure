using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject SettingsPanel;

    [SerializeField] Image soundButton;
    [SerializeField] Sprite soundOn, soundOff;
    [SerializeField] AudioSource audioSource;

    bool isSoundOn = true;

    void Awake()
    {

    }

    void Update()
    {

    }
    public void OpenGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);


    }
    public void SettingScreen()
    {
        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(!SettingsPanel.activeSelf);
        }


    }
    public void SoundOnOf()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            audioSource.Play();
            soundButton.sprite = soundOn;
        }

        else
        {
            audioSource.Stop();
            soundButton.sprite = soundOff;
        }


    }
}
