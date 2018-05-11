using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//code adapted from: 
//https://www.youtube.com/watch?v=6OT43pvUyfY&t=2s
public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    /* ****************************************
     * Function: Awake
     * ****************************************
     * Executes before Start() and keeps the music
     * looping between transitions. Is also able to 
     * keep a list of all Audio files used. 
     * ****************************************
     */
    void Awake () {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}

    /* ****************************************
     * Function: Play(string name)
     * ****************************************
     * Plays an audioclip according to clip name 
     * ****************************************
     */
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "notfound");
            return;
        }

        s.source.Play();
    }

}
