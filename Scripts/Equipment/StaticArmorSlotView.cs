using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class StaticArmorSlotView : ArmorSlotView {
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private Mesh defaultMesh;
    private Material defaultMaterial;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        defaultMesh = meshFilter.sharedMesh;
        defaultMaterial = meshRenderer.sharedMaterial;
    }

    public override void SetMaterial(Material material)
    {
        meshRenderer.sharedMaterial = material ?? defaultMaterial;
    }

    public override void SetMesh(Mesh mesh)
    {
        meshFilter.sharedMesh = mesh ?? defaultMesh;
    }
}
