using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentInteraction : QuickMenuInteraction
{
    public abstract bool Initialize(GameObject gameObject);
}