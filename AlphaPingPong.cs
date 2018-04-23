using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshRenderer))]
public class AlphaPingPong : MonoBehaviour
{
    private float _time;
    private Material _material;
    private Color _color;
    private float _intensity;

    public float intensity
    {
        set { _intensity = value; }
    }
   
    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        _material = renderer.material;
        _color = _material.color;
    }

    void Update()
    {
        _time += Time.deltaTime / 2.0f;
        _color.a = _intensity + Mathf.PingPong(_time, (1.0f - _intensity));

        _material.color = _color;
    }
}
