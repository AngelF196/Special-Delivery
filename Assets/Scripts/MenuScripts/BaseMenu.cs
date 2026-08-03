using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseMenu : MonoBehaviour
{
    private PlayerInfo _player;
    private string _sceneToLoad;

    // Color stuff
    public enum colorSpot { hat, star, shoe, shirt, skin, pants }
    private Color _hatColor;
    private Color _starColor;
    private Color _shoeColor;
    private Color _shirtColor;
    private Color _skinColor;
    private Color _pantsColor;

    [Header("UI Hookups")]
    [SerializeField] private GameObject _topMenu;
    [SerializeField] private GameObject _colorMenu;
    [SerializeField] private SpriteRenderer _playerPreview;

    public static BaseMenu Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _player = FindFirstObjectByType<PlayerInfo>();

        if (_player != null)
        {
            _player.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            _sceneToLoad = _player.lastLevelScene;
        }
    }
    public void LoadTop()
    {
        _topMenu.SetActive(true);
    }

    public void LoadColorMenu()
    {
        _colorMenu.SetActive(true);
    }

    public void ChangePreviewColor(colorSpot targetSpot, Color color)
    {
        switch (targetSpot) {
            case colorSpot.hat:
                _hatColor = color;
                _playerPreview.sharedMaterial.SetColor("_Hat", _hatColor);
                break;
            case colorSpot.star:
                _starColor = color;
                _playerPreview.sharedMaterial.SetColor("_Star", _starColor);
                break;
            case colorSpot.shoe:
                _shirtColor = color;
                _playerPreview.sharedMaterial.SetColor("_Shoe", _shirtColor);
                break;
            case colorSpot.skin:
                _skinColor = color;
                _playerPreview.sharedMaterial.SetColor("_Skin", _skinColor);
                break;
            case colorSpot.pants:
                _pantsColor = color;
                _playerPreview.sharedMaterial.SetColor("_Pants", _pantsColor);
                break;
            case colorSpot.shirt:
                _shirtColor = color;
                _playerPreview.sharedMaterial.SetColor("_Shirt", _shirtColor);
                break;
        }
    }

    public void Leave()
    {
        _player.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        SceneManager.LoadScene(_sceneToLoad);
    }
}
