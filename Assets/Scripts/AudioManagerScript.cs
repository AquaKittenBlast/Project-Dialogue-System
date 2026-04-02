using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField]private AudioSource musicSource;
    [SerializeField]private AudioClip venom;
    [SerializeField]private AudioClip artorias;

    public void Awake()
    {
        venom.LoadAudioData();
        artorias.LoadAudioData();
    }

    public void ChangeMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }
}
