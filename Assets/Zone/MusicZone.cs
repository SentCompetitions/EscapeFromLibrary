using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MusicZone : Zone
{
    public AudioClip audioClip;

    public override void OnZoneEnter(Collider other)
    {
        MusicManager.Clip = audioClip;
    }
}
