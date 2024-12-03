using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClientMusicPlayer : Singleton<ClientMusicPlayer>
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _nomAudioClip;

    public override void Awake() {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip() {
        _audioSource.clip = _nomAudioClip;
        _audioSource.Play();
    }
}
