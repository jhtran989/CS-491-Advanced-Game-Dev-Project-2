using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SoundTypesEnum
{
    CracklingFire,
    FirePutOutWater,
    PuzzleSolvedCorrect,
    PuzzleSolvedIncorrect,
    LowOxygen,
    EscapePodLaunch
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource oneShotAudioSource;
    
    // audio clips
    private static AudioClip _cracklingFire;
    
    private static AudioClip _firePutOutWater;
    private static bool _firePutOutWaterCheck;
    
    private static AudioClip _puzzleSolvedCorrect;
    private static AudioClip _puzzleSolvedIncorrect;
    private static AudioClip _lowOxygen;
    private static AudioClip _escapePodLaunch;
    
    private static AudioClip _spaceAmbienceBackground;

    private static AudioClip _mainMenuTheme;

    private List<CustomAudioClip> customAudioClipsList;
    
    private AudioSource loopingAudioSource;
    
    // TODO: use loopingAudioSource.PlayOneShot(<AudioClip>) for sound effects (one time effects -- can play multiple at once...)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        // get audio clips
        _cracklingFire = Resources
            .Load<AudioClip>("Sounds/fire-sound-efftect-21991");
        _firePutOutWater = Resources
            .Load<AudioClip>("Sounds/399548__soundslikewillem__extinguishing-fire");
        _puzzleSolvedCorrect = Resources
            .Load<AudioClip>("Sounds/correct-2-46134");
        _puzzleSolvedIncorrect = Resources
            .Load<AudioClip>("Sounds/system-error-notice-132470");
        _lowOxygen = Resources
            .Load<AudioClip>("Sounds/system_trouble-32321");
        _escapePodLaunch = Resources
            .Load<AudioClip>("Sounds/EscapePod/escape_pod");
        
        _spaceAmbienceBackground = Resources
            .Load<AudioClip>("Sounds/spaceship-ambience-with-effects-21420");
        
        _mainMenuTheme = Resources
            .Load<AudioClip>("Sounds/spacesound-7547");
        
        // create list
        customAudioClipsList = CreateCustomAudioClipList();

        loopingAudioSource = GetComponent<AudioSource>();
        loopingAudioSource.volume = 0.2f;

        _firePutOutWaterCheck = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayStartingSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static bool IsSceneLoaded(String sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }

    public void PlayStartingSound()
    {
        // play menu music
        if (IsSceneLoaded(Constants.MainMenuSceneName))
        {
            // loopingAudioSource.PlayOneShot(_mainMenuTheme);
            loopingAudioSource.clip = _mainMenuTheme;
        }
        // when the game starts up
        else if (IsSceneLoaded(Constants.MainSceneName))
        {
            loopingAudioSource.clip = _spaceAmbienceBackground;
        }
        else if (IsSceneLoaded(Constants.EndScreenSceneName))
        {
            // loopingAudioSource.PlayOneShot(_spaceAmbienceBackground);
            //loopingAudioSource.clip = _escapePodLaunch;
        }

        loopingAudioSource.loop = true;
        loopingAudioSource.Play();

        // FIXME:
        // loopingAudioSource.PlayOneShot(_firePutOutWater);
    }

    private AudioClip GetAudioClipFromSoundType(SoundTypesEnum soundTypesEnum)
    {
        AudioClip audioClip = null;

        switch (soundTypesEnum)
        {
            case SoundTypesEnum.CracklingFire:
                audioClip = _cracklingFire;
                break; 
            case SoundTypesEnum.LowOxygen:
                audioClip = _lowOxygen;
                break;
            case SoundTypesEnum.EscapePodLaunch:
                audioClip = _escapePodLaunch;
                break;
            case SoundTypesEnum.FirePutOutWater:
                audioClip = _firePutOutWater;
                break;
            case SoundTypesEnum.PuzzleSolvedCorrect:
                audioClip = _puzzleSolvedCorrect;
                break;
            case SoundTypesEnum.PuzzleSolvedIncorrect:
                audioClip = _puzzleSolvedIncorrect;
                break;
            default:
                Debug.LogError("Invalid sound type audio...");
                break;
        }

        return audioClip;
    }
    
    private CustomAudioClip GetCustomAudioClipFromSoundType(SoundTypesEnum soundTypesEnum)
    {
        var audioCheck = false;
        CustomAudioClip customAudioClip = null;

        switch (soundTypesEnum)
        {
            case SoundTypesEnum.CracklingFire:
                customAudioClip = new CustomAudioClip(_cracklingFire, audioCheck, soundTypesEnum);
                break; 
            case SoundTypesEnum.LowOxygen:
                customAudioClip = new CustomAudioClip(_lowOxygen, audioCheck, soundTypesEnum);
                break;
            case SoundTypesEnum.EscapePodLaunch:
                customAudioClip = new CustomAudioClip(_escapePodLaunch, audioCheck, soundTypesEnum);
                break;
            case SoundTypesEnum.FirePutOutWater:
                customAudioClip = new CustomAudioClip(_firePutOutWater, audioCheck, soundTypesEnum);
                break;
            case SoundTypesEnum.PuzzleSolvedCorrect:
                customAudioClip = new CustomAudioClip(_puzzleSolvedCorrect, audioCheck, soundTypesEnum);
                break;
            case SoundTypesEnum.PuzzleSolvedIncorrect:
                customAudioClip = new CustomAudioClip(_puzzleSolvedIncorrect, audioCheck, soundTypesEnum);
                break;
            default:
                Debug.LogError("Invalid sound type audio...");
                break;
        }

        return customAudioClip;
    }

    private List<CustomAudioClip> CreateCustomAudioClipList()
    {
        List<CustomAudioClip> customAudioClips = new List<CustomAudioClip>();

        foreach (var soundTypeEnum in Utilities.GetValues<SoundTypesEnum>())
        {
            customAudioClips.Add(GetCustomAudioClipFromSoundType(soundTypeEnum));
        }

        return customAudioClips;
    }

    public CustomAudioClip GetCustomAudioClip(SoundTypesEnum soundTypesEnum)
    {
        // FIXME: lazy way to do so (should use a dictionary instead)
        foreach (var customAudioClip in customAudioClipsList)
        {
            if (customAudioClip.SoundTypesEnum == soundTypesEnum)
            {
                return customAudioClip;
            }
        }

        // FIXME: should not happen
        return null;
    }

    public void PlayFireCracklingLocation(Vector3 fireLocation)
    {
        AudioSource.PlayClipAtPoint(_cracklingFire, fireLocation);
    }
    
    public void PlaySoundEffectLocation(SoundTypesEnum soundTypesEnum, Vector3 fireLocation)
    {
        AudioClip audioClip = GetAudioClipFromSoundType(soundTypesEnum);
        CustomAudioClip customAudioClip = GetCustomAudioClip(soundTypesEnum);

        if (!customAudioClip.AudioCheck && audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, fireLocation);
            customAudioClip.AudioCheck = true;
            customAudioClip.InitialPlayTime = Time.time;
        }

        if (Time.time - customAudioClip.InitialPlayTime > customAudioClip.AudioClip.length)
        {
            customAudioClip.AudioCheck = false;
        }
    }

    public void PlaySoundEffect(SoundTypesEnum soundTypesEnum)
    {
        // AudioClip audioClip = GetAudioClipFromSoundType(soundTypesEnum);
        //
        // if (audioClip != null)
        // {
        //     loopingAudioSource.PlayOneShot(audioClip);
        // }
        
        AudioClip audioClip = GetAudioClipFromSoundType(soundTypesEnum);
        CustomAudioClip customAudioClip = GetCustomAudioClip(soundTypesEnum);

        if (!customAudioClip.AudioCheck && audioClip != null)
        {
            oneShotAudioSource.PlayOneShot(customAudioClip.AudioClip);
            customAudioClip.AudioCheck = true;
            customAudioClip.InitialPlayTime = Time.time;
        }

        if (Time.time - customAudioClip.InitialPlayTime > customAudioClip.AudioClip.length)
        {
            customAudioClip.AudioCheck = false;
        }
    }
}
