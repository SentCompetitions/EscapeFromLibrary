using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MusicZone : MonoBehaviour
{
    public AudioClip audioClip;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        MusicManager.Clip = audioClip;
    }
}
