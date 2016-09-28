using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml.Schema;

public class FadeSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _audioBank;

    [SerializeField]
    private float _fadeTime = 10.0f;
    [SerializeField]
    private float _maxVolume = 1f; // 0 - 1f
    [SerializeField]
    private bool _startOnPlay = false;

    private float _currentLerpOutTime = 0f;
    private float _currentLerpInTime = 0f;
    
    private AudioSource _fadeIn = null;
    private AudioSource _fadeOut = null;
    private AudioSource _currentlyPlaying = null;

    [HideInInspector] public bool _playingSound = false;
    private bool _fading = false;

    

    void Awake()
    {
        _audioBank = GetComponents<AudioSource>();
        foreach (var i in _audioBank)
        {
            i.volume = 0f;
            if (i.isPlaying)
            {
                i.Pause();
                i.time = 0.0f;
            }
        }
        if (_audioBank.Length > 0)
        {
            _currentlyPlaying = _audioBank[0];
        }
        if (_startOnPlay)
        {
            SwitchSound();
        }
    }

    void Update()
    {
        //Debug.Log("Clip Length: " + _currentlyPlaying.clip.length + ", Time: " + _currentlyPlaying.time);
        if (!_fading)
        {
            if (_currentlyPlaying != null && (_currentlyPlaying.time >= (_currentlyPlaying.clip.length - _fadeTime)))
            {
                SwitchSound();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                SwitchSound();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                StopSound();
            }
        }
        if (_fading)
        {
            if (_fadeIn != null)
            {
                FadeIn(_fadeIn);
            }
            if (_fadeOut != null)
                FadeOut(_fadeOut);
        }
    }

    private void SetFadeAudio(bool fadeSomethingIn, bool fadeSomethingOut)
    {
        foreach (var i in _audioBank)
        {
            if (i.volume == 0f && _fadeIn == null && fadeSomethingIn)
            {
                i.time = 0f;
                _fadeIn = i;
                _currentlyPlaying = _fadeIn;
            }
            else if (i.volume == _maxVolume && _fadeOut == null && fadeSomethingOut)
            {
                _fadeOut = i;
            }
        }
    }

    private void Fade() 
    {
        _fading = true;
    }

    public void SwitchSound()
    {
        SetFadeAudio(true, true);
        _playingSound = true;
        Fade();
    }

    public void StopSound()
    {
        SetFadeAudio(false, true);
        _playingSound = false;
        Fade();
    }

    /*public void StartSound()
    {
        SetFadeAudio(true, false);
        Fade();
    }*/

    void FadeIn(AudioSource audio) // RAISE VOLUME
    {
        if(!audio.isPlaying)
            audio.Play();
        _currentLerpInTime += Time.deltaTime;
        float percent = _currentLerpInTime/_fadeTime;
        //audio.volume = Mathf.Lerp(_fromFadeIn, _toFadeIn, percent);
        audio.volume = Mathf.Lerp(0, _maxVolume, percent);

        if (percent >= 1f)
        {
            _currentLerpInTime = 0f;
        }
        if (audio.volume >= _maxVolume)
        {
            _fading = false;
            _currentlyPlaying = _fadeIn;
            _fadeIn = null;
        }
    }

    void FadeOut(AudioSource audio) // LOWER VOLUME
    {
        if (!audio.isPlaying)
            audio.Play();
        _currentLerpOutTime += Time.deltaTime; //lasketaan aikaa
        float percent = _currentLerpOutTime/_fadeTime;
        audio.volume = Mathf.Lerp(_maxVolume, 0, percent);
        if (percent >= 1f)
        {
            _currentLerpOutTime = 0f;
        }
        if (_fadeOut.volume <= 0f)
        {
            _fading = false;
            audio.Pause();
            _fadeOut = null;
        }
    }

}
