using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _environment;

    public static GameManager instance;

    private void Start()
    {
        instance = this;
    }

    public void SetEnvironment(GameObject environment)
    {
        if (_environment) Destroy(_environment);
        _environment = environment;
        Instantiate(_environment);

        Debug.Log("[Environment] Set " + _environment.name);
    }
}