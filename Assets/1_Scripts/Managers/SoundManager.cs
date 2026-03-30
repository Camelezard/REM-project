using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Musics
{
    CalmBoard,
    CalmDance,
    CalmLove,
    CalmPeps,
    Energic,
    LoveTunnel,
    TVMusic
}

public enum SFX
{
    None
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("SFX Lists")]
    [SerializeField] private List<AudioResource> _MusicResources = new List<AudioResource>();
    [SerializeField] private List<AudioResource> _SFXResources = new List<AudioResource>();

    [Header("AudioSources")]
    [SerializeField] private AudioSource _MusicAudioSource;
    [SerializeField] private AudioSource _SFXAudioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    public void PlayMusic(Musics pMusic)
    {
        _MusicAudioSource.resource = _MusicResources[(int)pMusic];
        _MusicAudioSource.Play();
    }

    public void PlaySFX(SFX pSFX)
    {
        _SFXAudioSource.resource = _SFXResources[(int)pSFX];
        _SFXAudioSource.Play();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
