using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Range(0f, 10f)] public float musicDelay = 2f;
    public AudioClip startUpMusic;
    
    private static AudioClip _clip;
    public static AudioClip Clip
    {
        get => _clip;
        set
        {
            if (_clip != value)
            {
                _clip = value;
                instance._source.Stop();
                instance._source.clip = _clip;
                instance._source.PlayDelayed(instance.musicDelay);
            }
        }
    }

    private AudioSource _source;

    void Start()
    {
        instance = this;

        _source = GetComponent<AudioSource>();

        Clip = startUpMusic;
    }
}
