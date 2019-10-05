using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] GameObject audioIsMuted;

    [SerializeField] List<AudioClip> wormClips = new List<AudioClip>();

    bool isMuted;
    float bgmDefault;
    float sfxDefault;
    void Start()
    {
        bgmDefault = bgmSource.volume;
        sfxDefault = sfxSource.volume;
    }

    public void PlayWormThrow()
    {
        sfxSource.PlayOneShot(wormClips[0]);
    }
    public void PlayWormHit()
    {
        sfxSource.PlayOneShot(wormClips[1]);
    }
    public void PlayWormSuccessful()
    {
        sfxSource.PlayOneShot(wormClips[2]);
    }

    public void MuteAllAudio()
    {
        if(!isMuted)
        {
            bgmSource.volume = 0;
            sfxSource.volume = 0;
            isMuted = true;
            audioIsMuted.SetActive(true);
        }
        else
        {
            isMuted = false;
            bgmSource.volume = bgmDefault;
            sfxSource.volume = sfxDefault;
            audioIsMuted.SetActive(false);
        }
    }
}
