using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioClip 
{
    private AudioClip _audioClip;
    private bool _audioCheck;
    private SoundTypesEnum _soundTypesEnum;
    private float initialPlayTime;

    public AudioClip AudioClip
    {
        get => _audioClip;
        set => _audioClip = value;
    }
    public bool AudioCheck
    {
        get => _audioCheck;
        set => _audioCheck = value;
    }
    public SoundTypesEnum SoundTypesEnum => _soundTypesEnum;
    public float InitialPlayTime
    {
        get => initialPlayTime;
        set => initialPlayTime = value;
    }

    public CustomAudioClip(AudioClip audioClip, bool audioCheck, SoundTypesEnum soundTypesEnum)
    {
        _audioClip = audioClip;
        _audioCheck = audioCheck;
        _soundTypesEnum = soundTypesEnum;
    }
}
