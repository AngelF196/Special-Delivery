using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioSource _levelMusic; 
    [SerializeField] private AudioSource _pauseMusic;
    [SerializeField] private float _fadeDuration;
    private Coroutine currentFade;

    public void PauseTransition()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeTransition(_levelMusic, _pauseMusic));
    }

    public void GameTransition()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeTransition(_pauseMusic, _levelMusic));
    }

    public IEnumerator FadeTransition(AudioSource fadeOut, AudioSource fadeIn)
    {
        float currentTime = 0;
        float inStart = fadeIn.volume;
        float outStart = fadeOut.volume;
        while (currentTime < _fadeDuration)
        {
            currentTime += Time.unscaledDeltaTime;
            fadeIn.volume = Mathf.Lerp(inStart, 1, currentTime / _fadeDuration);
            fadeOut.volume = Mathf.Lerp(outStart, 0, currentTime / _fadeDuration);
            yield return null;
        }
        yield break;
    }
}
