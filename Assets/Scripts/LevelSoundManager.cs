using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource _sfx;

    [SerializeField]
    private AudioSource _music;


    [Header("Music")]
    [SerializeField]
    private AudioClip _level;

    [SerializeField]
    private AudioClip _win;

    [SerializeField]
    private AudioClip _lose;

    [Header("SFX")]
    [SerializeField]
    private AudioClip _reloading;

    [SerializeField]
    private AudioClip[] _kick;



    public void Awake()
    {
        _music.loop = true;

        _music.playOnAwake = true;

        _sfx.loop = false;

        _sfx.playOnAwake = false;
    }



    public void PlaySfx(AudioClip sfx)
    {
        _sfx.PlayOneShot(sfx);
    }

    public void PlaySfx(ESFX sfx)
    {
        switch (sfx)
        {
            case ESFX.reloading:
                _sfx.PlayOneShot(_reloading);
                break;

            case ESFX.kick:
                _sfx.PlayOneShot(_kick[UnityEngine.Random.Range(0, _kick.Length)]);
                break;
        }
    }

    public void PlayMusic(EMusic music)
    {
        switch (music)
        {
            case EMusic.level:
                _music.clip = _level;
                break;

            case EMusic.win:
                _music.clip = _win;
                break;

            case EMusic.lose:
                _music.clip = _lose;
                break;
        }

        _music.Play();
    }



    public void SetUpVolume(float sfxVolume, float musicVolume)
    {
        _sfx.volume = sfxVolume;

        _music.volume = musicVolume;
    }



    public enum EMusic
    {
        level, win, lose,
    }

    public enum ESFX
    {
        reloading, kick,
    }
}