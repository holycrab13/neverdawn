using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexCameraMask
{
    private float fadeSpeed = 20.0f;

    public Character character { get; set; }
    public float radius { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float range { get; set; }

    public Vector4 maskVector
    {
        get { return new Vector4(x, y, radius); }
    }

    internal bool Fade()
    {
        float prev = radius;
        radius = Mathf.MoveTowards(radius, range, Time.deltaTime * fadeSpeed);
        return prev != radius;
    }
}

[ExecuteInEditMode]
public class HexCamera : MonoBehaviour {

    [SerializeField]
    private float _size = 25;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Projector projector;

    [SerializeField]
    private Color _gridTint;

    [SerializeField]
    private int textureSize = 1024;

    private float lerp;

    private float fadeTime;

    [SerializeField]
    private bool gridVisible = true;

    [SerializeField]
    private bool useMask;
    
    private static HexCamera instance;

    private List<HexCameraMask> masks;

    private bool materialDirty;

    private float opacity;
    private float targetOpacity;

    [SerializeField]
    private float minOpacity = 0.5f;

    private Vector4[] maskArray = new Vector4[10];

    public float size
    {
        get { return _size; }
        set
        {
            _size = value;
            updateSize();
        }
    }

    public Color gridTint
    {
        get { return _gridTint; }
        set
        {
            _gridTint = value;
            updateMaterial();
        }
    }

    internal static void ShowMask(Character frame, int maxRange)
    {
        HexCameraMask mask = instance.masks.FirstOrDefault(m => m.character == frame);
        HexTile tile = frame.currentTile;

        if (mask == null)
        {
            mask = new HexCameraMask()
            {
                radius = 0.0f,
                x = tile.position.x,
                y = tile.position.z,
                range = HexTerrain.tileSize * maxRange,
                character = frame
            };

            instance.masks.Add(mask);
        }
    }

    internal static void HideMask(Character frame)
    {
        HexCameraMask mask = instance.masks.FirstOrDefault(m => m.character == frame);

        if (mask != null)
        {
            mask.range = 0.0f;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        updateMaterial();
    }

    private void updateMaterial()
    {
        if (masks == null)
            masks = new List<HexCameraMask>();

        Vector4[] maskPositions = masks.Select(m => m.maskVector).ToArray();

        for (int i = 0; i < maskPositions.Length; i++)
        {
            maskArray[i] = maskPositions[i];
        }

        projector.material.SetFloat("_Opacity", opacity);
        projector.material.SetInt("_NumMaskPositions", maskPositions.Length);
        projector.material.SetVectorArray("_MaskPosition", maskArray);
    }

    private void updateSize()
    {
        if(camera)
            camera.orthographicSize = _size;

        if(projector)
            projector.orthographicSize = _size;
    }

	void OnValidate()
    {
        updateSize();
        updateMaterial();
    }

    void Update()
    {
        if (lerp < fadeTime)
        {
            lerp += Time.deltaTime;
            opacity = Mathf.MoveTowards(opacity, targetOpacity, Time.deltaTime / fadeTime);
            materialDirty = true;
        }

        foreach (HexCameraMask mask in masks)
        {
            if (mask.Fade())
            {
                materialDirty = true;
            }
        }

        masks.RemoveAll(m => m.radius == 0.0f);

        if (materialDirty)
        {
            updateMaterial();
            materialDirty = false;
        }
    }

    public void ShowGrid(float fadeTime)
    {
        
        this.fadeTime = fadeTime;
        this.targetOpacity = 1.0f;
        this.lerp = 0.0f;

        if (fadeTime == 0.0f)
        {
            this.opacity = 1.0f;
            materialDirty = true;
        }
    }

    public void HideGrid(float fadeTime)
    {
        this.fadeTime = fadeTime;
        this.targetOpacity = minOpacity;
        this.lerp = 0.0f;

        if (fadeTime == 0.0f)
        {
            this.opacity = minOpacity;
            materialDirty = true;
        }
    }

}
