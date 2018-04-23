﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour : MonoBehaviour
{
    private static SingletonBehaviour instance;

   // Use this for initialization
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
}
