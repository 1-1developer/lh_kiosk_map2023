using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static string SfxGroup = "SFX";


    [Header("UI Sounds")]
    [Tooltip("General button click.")]
    [SerializeField] AudioClip m_DefaultButtonSound;


    static AudioSource local_audioSource;
    // Start is called before the first frame update
    public static void PlayDefaultButtonSound()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
            return;

        PlayOneSFX(audioManager.m_DefaultButtonSound, Vector3.zero);
    }

    public static void PlayOneSFX(AudioClip clip, Vector3 sfxPosition)
    {
        if (clip == null)
            return;

        GameObject sfxInstance = GameObject.Find(clip.name);
        if(sfxInstance != null)
            return;
        sfxInstance = new GameObject(clip.name);
        sfxInstance.transform.position = sfxPosition;

        AudioSource source = sfxInstance.AddComponent<AudioSource>();
        source.volume = .3f;
        source.clip = clip;
        source.Play();

        // destroy after clip length
        Destroy(sfxInstance, clip.length);
    }

    public static void Playloop(AudioClip clip,AudioManager a)
    {
        if (clip == null)
            return;

        local_audioSource = a.GetComponent<AudioSource>();
        local_audioSource.clip = clip;
        // destroy after clip length
    }
    public static void  PauseSound()
    {
        local_audioSource.Pause();
    }
    public static void StopSound()
    {
        if (!local_audioSource)
        {
            return;
        }
        local_audioSource.Stop();
    }
    public static void PlaySound()
    {
        local_audioSource.Play();
    }
}
