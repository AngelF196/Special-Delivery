using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _color;
    [SerializeField] private BaseMenu.colorSpot _targetSpot;

    private Button _button;
    [SerializeField] private Image _square;

    private void Start()
    {
        _button = GetComponent<Button>();
        _color.a += 1; // Alpha needs to be 100% for UI purposes

        ColorBlock cb = _button.colors;
        cb.normalColor = _color;       
        cb.highlightedColor = _color;  
        cb.pressedColor = _color;     
        cb.selectedColor = _color;

        _button.colors = cb;

        _square = transform.parent.GetComponentInParent<Image>();
    }
    public void Clicked()
    {
        if (BaseMenu.Instance != null)
        BaseMenu.Instance.ChangePreviewColor(_targetSpot, _color);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _square.color = Color.yellow;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _square.color = Color.black;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _square.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _square.color = Color.black;
    }
}
