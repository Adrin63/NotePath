using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    [SerializeField] string _volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;
    [SerializeField] private float _multiplier = 30f;
    [SerializeField] Toggle _toggle;
    bool _disableToggleEvent;


    private void Awake()
    {
        _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        _toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }
    
    private void HandleToggleValueChanged(bool enabled)
    {
        if (_disableToggleEvent)
            return;

        if (enabled)
            _slider.value = _slider.maxValue;
        else
            _slider.value = _slider.minValue;
    }

    private void HandleSliderValueChanged(float value)
    {
        _mixer.SetFloat(_volumeParameter, Mathf.Log10(value) * _multiplier);
        _disableToggleEvent = true;
        _toggle.isOn = _slider.value > _slider.minValue;
        _disableToggleEvent = false;
    }

    //-----------------

    void Start()
    {
       if(transform.gameObject.name == "VolumeSlider")
       {
           _slider.value = PlayerPrefs.GetFloat("MasterVolume");
       }
       else if (transform.gameObject.name == "MusicSlider")
       {
            _slider.value = PlayerPrefs.GetFloat("MusicVolume");
       }
       else if(transform.gameObject.name == "SFXSlider")
       {
            _slider.value = PlayerPrefs.GetFloat("SFXVolume");
       }
    }

    void Update()
    {
        if (transform.gameObject.name == "VolumeSlider")
        {
            PlayerPrefs.SetFloat("MasterVolume", _slider.value);
        }
        else if (transform.gameObject.name == "MusicSlider")
        {
            PlayerPrefs.SetFloat("MusicVolume", _slider.value);
        }
        else if (transform.gameObject.name == "SFXSlider")
        {
            PlayerPrefs.SetFloat("SFXVolume", _slider.value);
        }
    }
}
