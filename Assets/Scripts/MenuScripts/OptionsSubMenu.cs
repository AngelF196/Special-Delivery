using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsSubMenu : MonoBehaviour
{
    // Tabs
    [Header ("Tabs")]
    [SerializeField] private Button _audioTab;
    [SerializeField] private Button _videoTab;
    [SerializeField] private Button _keybindTab;

    [Header ("")]
    [SerializeField] private Button _backButton;
    private Button _selectedTab;

    // Submenus
    [Header ("Submenus")]
    [SerializeField] private GameObject _audioOptions;
    [SerializeField] private GameObject _videoOptions;
    [SerializeField] private GameObject _keybindOptions;
    private Selectable _elementUnderSelectedTab;
    private Selectable _elementAboveBackButton; 

    // When disabled, set the submenu to the audio submenu
    void OnDisable()
    {
        AudioSelected();
    }

    public void AudioValueChanged()
    {
        Debug.Log("audio value changed");
    }

    // Under the 3 methods, disable other menus first, then enable the clicked tab.
    // Afterward, disable the clicked tab and enable the others.
    public void AudioSelected()
    {
        _selectedTab = _audioTab;  // Selected tab is the Audio Tab
        _videoOptions.SetActive(false);
        _keybindOptions.SetActive(false);
        _audioOptions.SetActive(true);

        // Keep selected tab highlighted
        Color32 activeMenuColor = new Color32(0x68, 0xD5, 0xD9, 0xFF);
        ColorBlock _cb = _selectedTab.colors;
        _cb.normalColor = activeMenuColor;
        _selectedTab.colors = _cb;

        // Visually deselect all other buttons
        _cb = _videoTab.colors;
        _cb.normalColor = Color.white;
        _videoTab.colors = _keybindTab.colors = _cb;

        // Change navigation of certain UI elements
        _elementAboveBackButton = _audioOptions.transform.Find("SFXGroup").GetComponentInChildren<Selectable>();
        Navigation _nav = _backButton.navigation;
        _nav.selectOnUp = _elementAboveBackButton;
        _nav.selectOnDown = _audioTab;
        _backButton.navigation = _nav;

        _elementUnderSelectedTab = _audioOptions.transform.Find("MusicGroup").GetComponentInChildren<Selectable>();
        _nav = _selectedTab.navigation;
        _nav.selectOnDown = _elementUnderSelectedTab;
        _selectedTab.navigation = _nav;
        // Preserve left and right navigation of the other tabs
        _nav = _videoTab.navigation;
        _nav.selectOnDown = _elementUnderSelectedTab;
        _videoTab.navigation = _nav;
        _nav = _keybindTab.navigation;
        _nav.selectOnDown = _elementUnderSelectedTab;
        _keybindTab.navigation = _nav;
    }

    public void VideoSelected()
    {
        _selectedTab = _videoTab;  // Selected tab is the Video Tab
        _audioOptions.SetActive(false);
        _keybindOptions.SetActive(false);
        _videoOptions.SetActive(true);

        // Keep selected tab highlighted
        Color32 activeMenuColor = new Color32(0x68, 0xD5, 0xD9, 0xFF);
        ColorBlock _cb = _selectedTab.colors;
        _cb.normalColor = activeMenuColor;
        _selectedTab.colors = _cb;

        // Visually deselect all other buttons
        _cb = _audioTab.colors;
        _cb.normalColor = Color.white;
        _audioTab.colors = _keybindTab.colors = _cb;

        // Change navigation of certain UI elements
        _elementAboveBackButton = _videoOptions.transform.Find("VSyncGroup").GetComponentInChildren<Selectable>();
        Navigation _nav = _backButton.navigation;
        _nav.selectOnUp = _elementAboveBackButton;
        _nav.selectOnDown = _videoTab;
        _backButton.navigation = _nav;

        _elementUnderSelectedTab = _videoOptions.transform.Find("BrightnessGroup").GetComponentInChildren<Selectable>();
        _nav = _selectedTab.navigation;
        _nav.selectOnDown = _elementUnderSelectedTab;
        _selectedTab.navigation = _nav;
        // Preserve left and right navigation of the other tabs
        _nav = _audioTab.navigation;
        _nav.selectOnDown = _elementUnderSelectedTab;
        _audioTab.navigation = _nav;
        _nav = _keybindTab.navigation;
        _nav.selectOnDown = _elementUnderSelectedTab;
        _keybindTab.navigation = _nav;
    }
    
    public void KeyBindsSelected()
    {
        _selectedTab = _keybindTab;  // Selected tab is the Keybinds Tab
        _audioOptions.SetActive(false);
        _videoOptions.SetActive(false);
        _keybindOptions.SetActive(true);

        // Keep selected tab highlighted
        Color32 activeMenuColor = new Color32(0x68, 0xD5, 0xD9, 0xFF);
        ColorBlock _cb = _selectedTab.colors;
        _cb.normalColor = activeMenuColor;
        _selectedTab.colors = _cb;

        // Visually deselect all other buttons
        _cb = _audioTab.colors;
        _cb.normalColor = Color.white;
        _audioTab.colors = _videoTab.colors = _cb;;
    }
}
