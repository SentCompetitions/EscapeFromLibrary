using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnvironmentZone : Zone
{
    public GameObject environment;

    public override void OnZoneEnter(Collider other)
    {
        GameManager.instance.SetEnvironment(environment);
    }
}
