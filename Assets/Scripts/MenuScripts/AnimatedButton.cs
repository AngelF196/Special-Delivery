using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [Header("SFXs")]
    [SerializeField] private AudioClip _selectAudio;
    [Tooltip("Audio that plays when pressing any confirm action that isn't a mouse click."), SerializeField] private AudioClip _confirmAudio;
    private Animator _animator;
    private AudioSource _menuSoundPlayer;
    private Button _lastSelectedButton;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _menuSoundPlayer = GetComponent<AudioSource>();
    }

    // Pointer refers to the mouse
    public void OnPointerEnter(PointerEventData data)
    {
        if (data.selectedObject == this.gameObject)  // Don't grow any other button that isn't selected by gamepad or keyboard 
            _animator.SetBool("buttonSelected", true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (data.selectedObject != this.gameObject)
            _animator.SetBool("buttonSelected", false);
    }

    public void OnPointerClick(PointerEventData data)
    {
        _menuSoundPlayer.clip = _confirmAudio;
        _menuSoundPlayer.Play();
    }

    // Gamepad controls and keyboard navigation
    public void OnSelect(BaseEventData data)
    {
        _menuSoundPlayer.clip = _selectAudio;
        _menuSoundPlayer.Play();
        _animator.SetBool("buttonSelected", true);
        _lastSelectedButton = data.selectedObject.GetComponent<Button>();
    }

    public void OnDeselect(BaseEventData data)
    {
        _animator.SetBool("buttonSelected", false);
    }

    public void OnSubmit(BaseEventData data)
    {
        _menuSoundPlayer.clip = _confirmAudio;
        _menuSoundPlayer.Play();
    }
}
