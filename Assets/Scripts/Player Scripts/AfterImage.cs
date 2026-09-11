using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImage : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] float _opacityStep = 0.01f;
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color newColor = new Vector4 (_sr.color.r, _sr.color.g, _sr.color.b, _sr.color.a - _opacityStep);
        _sr.color = newColor;

        if (_sr.color.a <= 0) Destroy(this.gameObject);
    }
}
