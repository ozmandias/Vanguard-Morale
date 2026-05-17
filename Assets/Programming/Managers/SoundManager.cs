using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioSource mainAudio;
    Dictionary<SoundType, AudioClip> soundDictionary = new Dictionary<SoundType, AudioClip>();
    public SoundKeyValue []soundKeyValues;

    void Start() {
        // sound clips into sound dictionary
        foreach(var soundKeyValue in soundKeyValues) {
            soundDictionary.Add(soundKeyValue.key, soundKeyValue.value);
        }
    }

    void Update() {

    }

    public void Play(SoundType soundType) {
        if(soundDictionary.ContainsKey(soundType)) {
            mainAudio.Stop();
            mainAudio.clip = soundDictionary[soundType];
            mainAudio.Play();
        }
    }
}