using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : MonoBehaviour {

    [SerializeField]
    private CanvasGroup blackScreen;

    [SerializeField]
    private CanvasGroup loadingScreen;

    [SerializeField]
    private Image progressImage;

    private bool _fadeIn;
    private float _totalFadeTime;
    private float _fadeTime;
    private float _totalFadeTime2;
    private float _fadeProgress;

    public void Show(float p)
    {
        _fadeIn = true;
        _totalFadeTime = p;
        _fadeProgress = 0.0f;

        SetProgress(0.0f);
    }


    public void Hide(float p)
    {
        _fadeIn = false;
        _totalFadeTime = p;
        _fadeProgress = 0.0f;
    }

    void Update()
    {
        if (_fadeProgress <= 1.0f)
        {
            _fadeProgress += (Time.deltaTime / _totalFadeTime);

            if (_fadeIn)
            {
                blackScreen.alpha = Mathf.Clamp01(_fadeProgress * 2.0f);
                loadingScreen.alpha = Mathf.Clamp01(_fadeProgress * 2.0f - 1.0f);
            }
            else
            {
                blackScreen.alpha = Mathf.Clamp01((1.0f - _fadeProgress) * 2.0f);
                loadingScreen.alpha = Mathf.Clamp01((1.0f - _fadeProgress) * 2.0f - 1.0f);
            }
        }
    }

    public void SetProgress(float p)
    {
        progressImage.fillAmount = p;
    }

}
