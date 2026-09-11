using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImages : MonoBehaviour
{
    private SpriteRenderer _playerRenderer;
    private float _spawnInterval = 1f;
    [SerializeField] private AfterImage _afterImagePrefab;
    private Coroutine _afterImageCoroutine;

    void Start()
    {
        _playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    IEnumerator AfterImages()
    {
        AfterImage lastImage = Instantiate(_afterImagePrefab, transform.position, Quaternion.identity);
        SpriteRenderer lastImageRenderer = lastImage.GetComponent<SpriteRenderer>();
        lastImage.transform.rotation = transform.rotation;
        lastImageRenderer.sprite = _playerRenderer.sprite;
        lastImageRenderer.flipX = _playerRenderer.flipX;
        lastImageRenderer.material = _playerRenderer.material;
        lastImageRenderer.sortingOrder = _playerRenderer.sortingOrder;
        lastImageRenderer.color = _playerRenderer.color;

        yield return new WaitForSeconds(_spawnInterval);
    }
    public void StartAfterImages()
    {
        if (_afterImageCoroutine != null) 
            _afterImageCoroutine = StartCoroutine(AfterImages());
    }
    public void StopAfterImages()
    {
        StopCoroutine(_afterImageCoroutine);
        _afterImageCoroutine = null;
    }
}
