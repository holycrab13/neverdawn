using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedArmorSlotView : ArmorSlotView {

    private SkinnedMeshRenderer meshRenderer;

    private Mesh defaultMesh;

    private Material defaultMaterial;

    void Awake()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();

        defaultMesh = meshRenderer.sharedMesh;
        defaultMaterial = meshRenderer.sharedMaterial;
    }

    public override void SetMaterial(Material material)
    {
        meshRenderer.sharedMaterial = material ?? defaultMaterial;
    }

    public override void SetMesh(Mesh mesh)
    {
        meshRenderer.sharedMesh = mesh ?? defaultMesh;
    }
}
