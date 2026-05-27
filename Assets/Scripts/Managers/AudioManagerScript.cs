using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField]private AudioSource musicSource;
    [SerializeField]List<AudioClip> allBGM = new List<AudioClip>();
    [SerializeField]List<AudioClip> allSFX = new List<AudioClip>();
    public AudioSource sourcePrefab;
    public static AudioManagerScript instance;


    public void Awake()
    {
        if (!instance){instance = this;}
        foreach (AudioClip song in allBGM)
        {
            song.LoadAudioData();
        }
        foreach (AudioClip sfx in allSFX)
        {
            sfx.LoadAudioData();
        }
    }

    public void ChangeMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfx)
    {
        AudioSource audioSource = Instantiate(sourcePrefab, this.transform.position, Quaternion.identity);
        audioSource.clip = sfx;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
