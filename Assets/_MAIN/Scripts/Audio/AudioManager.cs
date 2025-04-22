using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

[System.Serializable]
public class AudioManager : MonoBehaviour
{   
    private const string SFX_NAME = "SFX - [{0}]";

    private static AudioManager instance;
    [SerializeField] public GameObject music;
    [SerializeField] public GameObject soundEffect;

    private List<AudioTrack> tracks = new List<AudioTrack>();


    private void Awake() {
        if (instance == null) {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            instance =  this;
        }
        else  {
            DestroyImmediate(gameObject);
            return;
        }
    }

    public static AudioManager GetInstance() {
        return instance;
    }

    // Play sound effect in filePath
    public AudioSource PlaySoundEffect(string filePath, float volume = 1, bool loop = false) {
        
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if (clip == null) {
            Debug.LogError("Could not load sfx from filepath: " + filePath);
            return null;
        }

        return PlaySoundEffect(clip, volume, loop);
    }

    // Play sound effect from an audio clip
    public AudioSource PlaySoundEffect(AudioClip clip, float volume = 0.7f, bool loop = false) {
        GameObject sfxObject = new GameObject(string.Format(SFX_NAME, clip.name));
        sfxObject.transform.SetParent(soundEffect.transform);
        AudioSource source = sfxObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.Play();

        // Destroy object after it finished
        if (!loop) {
            Destroy(sfxObject.gameObject, clip.length + 1);
        }

        return source;
    }


    public void StopSoundEffect(AudioClip clip) {
        StopSoundEffect(clip.name);
    }  

    // Stop a sound effect in every sound effect running
    public void StopSoundEffect(string soundName) {
        soundName = soundName.ToLower();
        AudioSource[] sources = soundEffect.GetComponentsInChildren<AudioSource>(); // Get all the sources from sfx game object

        foreach(var source in sources) {
            if (source.clip.name.ToLower() == soundName)  {
                Destroy(source.gameObject);
                return;
            }
        }
    }

    // Stop all sound effect
    public void StopAllSoundEffect() {
        AudioSource[] sources = soundEffect.GetComponentsInChildren<AudioSource>(); // Get all the sources from sfx game object

        foreach(var source in sources) {
            Destroy(source.gameObject);
        }
    }

    // Play a music track
    public AudioTrack PlayTrack(string filePath, bool loop = true, float startingVolume = 1f, float volumeCap = 1f) {
        
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if (clip == null) {
            Debug.LogError("Could not load music from filepath: " + filePath);
            return null;
        }

        return PlayTrack(clip, loop, startingVolume, volumeCap, filePath);
    }

    public AudioTrack PlayTrack(AudioClip clip, bool loop = true, float startingVolume = 1f, float volumeCap = 1f, string filePath = "") {
        Debug.Log(clip.name);
        
        // Check if the track is currently playing
        foreach (AudioTrack track in tracks) {
             Debug.Log(clip.name);
            if (track.source.clip.name == clip.name) {
                return track;
            }
        }

        // Create a new AudioTrack object
        AudioTrack newTrack = new AudioTrack(clip, loop, startingVolume, volumeCap);

        tracks.Add(newTrack);
        newTrack.Play();

        return newTrack;
    }

    // Stop a music track
    public void StopTrack(string clipName) {
        foreach (AudioTrack track in tracks) {
            Debug.Log("Stop: " + clipName);
            if (track.source.clip.name == clipName) {
                tracks.Remove(track);
                Destroy(track.root); 
                break;
            }
        }
    }

    // Stop all music tracks
    public void StopAllTrack() {
        foreach (AudioTrack track in tracks) {
            Destroy(track.root); 
        }

        tracks.Clear();
    }
}
