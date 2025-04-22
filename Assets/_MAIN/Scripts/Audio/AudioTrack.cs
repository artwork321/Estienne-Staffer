using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioTrack
{
    private const string TRACK_NAME_FORMAT = "Track - [{0}]";

    public string name {get; private set;}

    public GameObject root => source.gameObject;

    public AudioSource source {get; private set;}

    public bool loop => source.loop;

    public float volumeCap {get; private set;}

    public float volume {get {return source.volume;} set {source.volume = value;} }

    public bool isPlaying => source.isPlaying;

    public AudioTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap) {
        this.name = clip.name;
        this.volumeCap = volumeCap;
        
        source = CreateSource();
        source.clip = clip;
        source.loop = loop;
        source.volume = startingVolume;
    }

    private AudioSource CreateSource() {
        GameObject trackObject = new GameObject(string.Format(TRACK_NAME_FORMAT, name));
        trackObject.transform.SetParent(AudioManager.GetInstance().music.transform);
        AudioSource source = trackObject.AddComponent<AudioSource>();

        return source;
    }

    public void Play() {
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }
}
