using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Solid))]
public class Outlined : MonoBehaviour
{
    public Renderer[] renderers
    {
        get { return GetComponentsInChildren<Renderer>(false); }
    }

}
