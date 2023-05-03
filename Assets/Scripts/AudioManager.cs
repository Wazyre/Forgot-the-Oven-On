using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // public static Dictionary<string, AudioMixerGroup> SoundTypes = new Dictionary<string, AudioMixerGroup>() { { Statics.AmbienceMixerGroupName, null }, { Statics.SFXMixerGroupName, null } };

    public static AudioManager Singleton = null;
    public static Queue<AudioSource> sourcePool = new Queue<AudioSource>();
    [Range (0, 1)]
    public static float AmbienceVolume = 1f;
    [Range (0, 1)]
    public static float SFXVolume = 1f;

    AudioMixer mixer;

    void Awake() {
        if (!Singleton) {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        // ambienceVolume = PlayerPrefs.GetFloat(Statics.AmbienceVolumePlayerPrefName, 1);
        // SFXVolume = PlayerPrefs.GetFloat(Statics.SFXVolumePlayerPrefName, 1);

        // mixer = Resources.Load(Statics.MasterMixerName) as AudioMixer;
        // SoundTypes[Statics.AmbienceMixerGroupName] = mixer.FindMatchingGroups(Statics.AmbienceMixerGroupName)[0];
        // SoundTypes[Statics.SFXMixerGroupName] = mixer.FindMatchingGroups(Statics.SFXMixerGroupName)[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
