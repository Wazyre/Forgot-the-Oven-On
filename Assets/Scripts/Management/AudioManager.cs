using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // public static Dictionary<string, AudioMixerGroup> SoundTypes = new Dictionary<string, AudioMixerGroup>() { { Statics.AmbienceMixerGroupName, null }, { Statics.SFXMixerGroupName, null } };

    public static AudioManager Singleton = null;
    // public static Queue<AudioSource> sourcePool = new Queue<AudioSource>();
    // [Range (0, 1)]
    // public static float AmbienceVolume = 1f;
    // [Range (0, 1)]
    // public static float SFXVolume = 1f;

    // AudioMixer mixer;

    [Header("Audio Sources")]
    [SerializeField] AudioSource[] audiosMain;
    [SerializeField] AudioSource[] audios1;
    [SerializeField] AudioSource[] audios2;
    [SerializeField] AudioSource[] audios3;
    [SerializeField] AudioSource swingAudio;
    [SerializeField] AudioSource zoomAudio;

    void Awake() {
        if (!Singleton) {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

    //     // ambienceVolume = PlayerPrefs.GetFloat(Statics.AmbienceVolumePlayerPrefName, 1);
    //     // SFXVolume = PlayerPrefs.GetFloat(Statics.SFXVolumePlayerPrefName, 1);

    //     // mixer = Resources.Load(Statics.MasterMixerName) as AudioMixer;
    //     // SoundTypes[Statics.AmbienceMixerGroupName] = mixer.FindMatchingGroups(Statics.AmbienceMixerGroupName)[0];
    //     // SoundTypes[Statics.SFXMixerGroupName] = mixer.FindMatchingGroups(Statics.SFXMixerGroupName)[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudioSwitch(int level) {
        switch (level) {
            case 0: 
                foreach (AudioSource audio in audiosMain) {
                    audio.Play();
                }
                foreach (AudioSource audio in audios1) {
                    audio.Stop();
                }
                foreach (AudioSource audio in audios2) {
                    audio.Stop();
                }
                foreach (AudioSource audio in audios3) {
                    audio.Stop();
                }
                // swingAudio.Stop();
                break;
            case 1:
                foreach (AudioSource audio in audiosMain) {
                    audio.Stop();
                }
                foreach (AudioSource audio in audios1) {
                    audio.Play();
                }
                // swingAudio.Play();
                // foreach (AudioSource audio in audios2) {
                //     audio.Stop();
                // }
                // foreach (AudioSource audio in audios3) {
                //     audio.Stop();
                // }
                break;
            case 2:
                foreach (AudioSource audio in audiosMain) {
                    audio.Stop();
                }
                foreach (AudioSource audio in audios1) {
                    audio.Stop();
                }
                foreach (AudioSource audio in audios2) {
                    audio.Play();
                }
                // swingAudio.Play();
                // foreach (AudioSource audio in audios3) {
                //     audio.Stop();
                // }
                break;
            case 3:
                foreach (AudioSource audio in audiosMain) {
                    audio.Stop();
                }
                // foreach (AudioSource audio in audios1) {
                //     audio.Stop();
                // }
                foreach (AudioSource audio in audios2) {
                    audio.Stop();
                }
                foreach (AudioSource audio in audios3) {
                    audio.Play();
                }
                // swingAudio.Play();
                break;
        }       
    }

    public void PlaySwing() {
        swingAudio.Play();
    }

    public void PlayZoom() {
        zoomAudio.Play();
    }

    public void StopZoom() {
        zoomAudio.Stop();
    }

    // Useful foreach things like UI Sliders in a settings menu
    // public void SetSFXVolume(float volume) {
    //     SFXVolume = volume;
    // }
    // public void SetAmbienceVolume(float volume) {
    //     AmbienceVolume = volume;
    // }

    public void SetVolume(float volume) {
        foreach (AudioSource audio in audiosMain) {
            audio.volume = volume / 10f;
        }
        foreach (AudioSource audio in audios1) {
            audio.volume = volume / 10f;
        }
        foreach (AudioSource audio in audios2) {
            audio.volume = volume / 10f;
        }
        foreach (AudioSource audio in audios3) {
            audio.volume = volume / 10f;
        }
        swingAudio.volume = volume / 2f;
        zoomAudio.volume = volume;
    }
}
