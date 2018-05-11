using UnityEngine.Audio;
using UnityEngine;

//code adapted from: 
//https://www.youtube.com/watch?v=6OT43pvUyfY&t=2s

/* ****************************************
 * Class: Sound()
 * ****************************************
 * Gives the AudioManager the parameters to 
 * store and edit each audioclip
 * ****************************************
 */
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
