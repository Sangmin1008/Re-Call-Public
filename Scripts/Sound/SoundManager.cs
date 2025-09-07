using System.Collections.Generic;
using UnityEngine;

public enum SFXType {Jump, Land, Hit, Die, TakeDamage, Footstep, RunFootstep, SelectItem, PickUpItem, MonsterHit, MonsterDie, DoorOpen, DoorClose}
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip defaultBGMClip;

    //중복되는 사운드
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip selectItemClip;
    [SerializeField] private AudioClip pickUpItemClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorCloseClip;
    [SerializeField] private List<AudioClip> footstepClips;
    [SerializeField] private List<AudioClip> runFootstepClips;


    public AudioClip DefaultBGMClip => defaultBGMClip;

    private void Start()
    {
        bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        PlayBGMSource(defaultBGMClip); //배경음 자동실행
    }
    public void PlayBGMSource(AudioClip audioClip)  //배경음악 교체시
    {
        if(audioClip==null) return;

        bgmSource.clip=audioClip;
        bgmSource.loop = true;

        bgmSource.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    //사운드만 갈경우
    public void PlaySFX(AudioClip audioClip) //효과음 교체시
    {
        if(audioClip==null) return;

        sfxSource.PlayOneShot(audioClip);
    }

    //중복되는 사운드 사용할 경우
    public void PlaySFX(SFXType type)
    {
        switch(type)
        {
            case SFXType.Land: sfxSource.PlayOneShot(landClip); break;
            case SFXType.Jump: sfxSource.PlayOneShot(jumpClip); break;
            case SFXType.Hit:sfxSource.PlayOneShot(hitClip); break;
            case SFXType.Die:sfxSource.PlayOneShot(dieClip); break;
            case SFXType.Footstep:sfxSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Count)]); break;
            case SFXType.RunFootstep:sfxSource.PlayOneShot(runFootstepClips[Random.Range(0, runFootstepClips.Count)]); break;
            case SFXType.SelectItem: sfxSource.PlayOneShot(selectItemClip); break;
            case SFXType.PickUpItem: sfxSource.PlayOneShot(pickUpItemClip); break;
            case SFXType.TakeDamage: sfxSource.PlayOneShot(takeDamageClip); break;
            case SFXType.DoorOpen: sfxSource.PlayOneShot(doorOpenClip); break;
            case SFXType.DoorClose: sfxSource.PlayOneShot(doorCloseClip); break;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
