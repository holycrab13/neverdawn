using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Highlighting : PostEffectsBase
{

    private static Highlighting instance;

    public Shader highlightingShader;

    [HideInInspector]
    public Material mat;

    [HideInInspector]
    public Camera occlusionCamera;
    private Camera sourceCamera;
    private RenderTexture renderTexture;
    private CommandBuffer occlusionBuffer;


    public Color occludedColor;
    public Color highlightColor;

    private List<Interactable> interactables;
    private CommandBuffer sourceBuffer;
    private Outlined[] outlined;

    new void Start()
    {
        base.Start();

        instance = this;

        sourceCamera = GetComponent<Camera>();
        interactables = new List<Interactable>();

        renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);

        if (occlusionCamera == null)
        {
            GameObject cameraGameObject = new GameObject("Outline Camera");
            cameraGameObject.transform.parent = sourceCamera.transform;
            occlusionCamera = cameraGameObject.AddComponent<Camera>();
            occlusionCamera.enabled = false;
        }

        UpdateOutlineCameraFromSource();

        occlusionBuffer = new CommandBuffer();
        occlusionCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, occlusionBuffer);

        sourceBuffer = new CommandBuffer();

        sourceCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, sourceBuffer);

        outlined = FindObjectsOfType<Outlined>();


        mat = CreateMaterial(highlightingShader, mat);
    }

    public static void Highlight(Interactable frame)
    {
        if(instance != null)
            instance.interactables.Add(frame);
    }

    public static void Unhighlight(Interactable frame)
    {
        if (instance != null)
            instance.interactables.Remove(frame);
    }

    public override bool UpdateMaterial()
    {
        CheckSupport(true);

        mat = CreateMaterial(highlightingShader, mat);
        mat.SetColor("_OccludedColor", occludedColor);
        mat.SetColor("_Color", highlightColor);

        return isSupported;
    }

    public void OnPreRender()
    {
        if (UpdateMaterial() == false)
        {
            return;
        }

        UpdateOutlineCameraFromSource();

        if (occlusionBuffer == null)
            return;

        occlusionBuffer.Clear();


        foreach (AvatarController controller in GameController.activeControllers)
        {
            if (controller.character != null)
            {
                mat.SetColor("_OccludedColor", controller.color);

                Renderer[] renderers = controller.character.GetComponentsInChildren<Renderer>(false);

                if (renderers != null)
                {
                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer.gameObject.activeInHierarchy && renderer.enabled && !(renderer is ParticleSystemRenderer))
                        {
                            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                                occlusionBuffer.DrawRenderer(renderer, mat, i, 0);
                        }
                    }
                }
            }
        }

        occlusionCamera.Render();

        sourceBuffer.Clear();

        interactables.RemoveAll(i => i == null);
        List<Interactable> tmp = new List<Interactable>(interactables);

        foreach (Interactable obj in tmp)
        {
            if (obj.IsSelected)
            {
                foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
                {
                    if (renderer.enabled && !(renderer is ParticleSystemRenderer))
                    {
                        for (int i = 0; i < renderer.materials.Length; i++)
                            sourceBuffer.DrawRenderer(renderer, mat, i, 2);
                    }
                }
            }
        }
    }


    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (UpdateMaterial() == false)
        {
            Graphics.Blit(source, destination);
            return;
        }

        mat.SetTexture("_OutlineSource", renderTexture);


        Graphics.Blit(source, destination, mat, 1);
    }


    void UpdateOutlineCameraFromSource()
    {
        occlusionCamera.CopyFrom(sourceCamera);
        occlusionCamera.renderingPath = RenderingPath.Forward;
        occlusionCamera.backgroundColor = new Color(1, 1, 1, 1);
        occlusionCamera.clearFlags = CameraClearFlags.SolidColor;
        occlusionCamera.rect = new Rect(0, 0, 1, 1);
        occlusionCamera.cullingMask = 0;
        occlusionCamera.targetTexture = renderTexture;
        occlusionCamera.enabled = false;
    }
}

