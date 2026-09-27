using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteMask))]
public class PlayerSpriteMask : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpriteMask _mask;
    private Sprite _lastSprite;
    private RectTransform _rectTransform;

    void Start()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        _mask = GetComponent<SpriteMask>();
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _lastSprite = _spriteRenderer.sprite;
        if (_lastSprite != _mask.sprite) _mask.sprite = _lastSprite;

        if (_spriteRenderer.flipX) _rectTransform.localScale = new Vector3 (-1, 1, 1);
        else _rectTransform.localScale = new Vector3(1, 1, 1);

    }
}
