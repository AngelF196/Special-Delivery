using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsSubMenu : MonoBehaviour
{
    // Tabs
    [SerializeField] private Button _audioTab;
    [SerializeField] private Button _videoTab;
    [SerializeField] private Button _keybindTab;


    [SerializeField] private Button _backButton;

    // Submenus
    [SerializeField] private GameObject _audioOptions;
    [SerializeField] private GameObject _videoOptions;
    [SerializeField] private GameObject _keybindOptions;

    // When enabled, set the submenu to the audio submenu
    void OnDisable()
    {
        AudioSelected();
        // EventSystem.
    }

    // Under the 3 methods, disable other menus first, then enable the clicked tab.
    // Afterward, disable the clicked tab and enable the others.
    public void AudioSelected()
    {
        _videoOptions.SetActive(false);
        _keybindOptions.SetActive(false);
        _audioOptions.SetActive(true);

        _audioTab.interactable = false;
        _videoTab.interactable = true;
        _keybindTab.interactable = true;
    }

    public void VideoSelected()
    {
        _audioOptions.SetActive(false);
        _keybindOptions.SetActive(false);
        _videoOptions.SetActive(true);

        _videoTab.interactable = false;
        _audioTab.interactable = true;
        _keybindTab.interactable = true;
    }
    
    public void KeyBindsSelected()
    {
        _audioOptions.SetActive(false);
        _videoOptions.SetActive(false);
        _keybindOptions.SetActive(true);

        _keybindTab.interactable = false;
        _audioTab.interactable = true;
        _videoTab.interactable = true;
    }
}
