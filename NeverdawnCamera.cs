using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class NeverdawnCamera : MonoBehaviour {

    /// <summary>
    /// Global instance of the camera
    /// </summary>
    private static NeverdawnCamera instance;

    /// <summary>
    /// The unity camera component
    /// </summary>
    [SerializeField]
    private Camera camera;

    /// <summary>
    /// The camera rendering the hexagon grid
    /// </summary>
    [SerializeField]
    private HexCamera hexCamera;

    /// <summary>
    /// Tint for the hexagon grid
    /// </summary>
    [SerializeField]
    private Color hexGridTint;

    /// <summary>
    /// Size of the hex grid orthogonal camera;
    /// </summary>
    [SerializeField]
    private float hexGridSize;

    /// <summary>
    /// y-axis offset of the camera lookat
    /// </summary>
    [SerializeField]
    private float targetOffset;

    /// <summary>
    /// camera offset from the base point
    /// </summary>
    [SerializeField]
    private Vector3 cameraOffset;

    /// <summary>
    /// Camera lerp speed
    /// </summary>
    [SerializeField]
    private float lerpSpeed;

    private Vector3 lookAtOffset;

    private Transform target;

    private float lerp;

    private List<Transform> targets;

    private Vector3 targetPosition;

    private Transform _cameraTransform;

    private Transform _hexCameraTransform;

    public Transform cameraTransform
    {
        get
        {
            if (!_cameraTransform)
            {
                _cameraTransform = camera.transform;
            }

            return _cameraTransform;
        }
    }

    public Transform hexCameraTransform
    {
        get
        {
            if (!_hexCameraTransform)
            {
                _hexCameraTransform = hexCamera.transform;
            }

            return _hexCameraTransform;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        //Sets this to not be destroyed when reloading scene
        if (Application.isEditor && EditorApplication.isPlaying)
        {
#endif
            DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        }
#endif

        targets = new List<Transform>();
        updateCameras();
    }

    private void updateCameras()
    {
        lookAtOffset = new Vector3(0.0f, targetOffset, 0.0f);

        cameraTransform.SetParent(transform);
        cameraTransform.localPosition = cameraOffset;
        cameraTransform.forward = (lookAtOffset - cameraOffset).normalized;

        hexCameraTransform.localPosition = cameraTransform.localPosition + new Vector3(0.0f, 0.0f, hexCamera.size / 2.0f);
        hexCamera.gridTint = hexGridTint;
        hexCamera.size = hexGridSize;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + lookAtOffset, 0.1f);

        Gizmos.DrawLine(transform.position + cameraOffset, transform.position + lookAtOffset);

        Gizmos.DrawLine(cameraTransform.position, hexCameraTransform.position);
    }

    void OnValidate()
    {
        updateCameras();
    }

    public static bool exists
    {
        get { return FindObjectOfType<NeverdawnCamera>() != null; }
    }

    internal static void Destroy()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
    }

    public static Vector3 position
    {
        get { return instance.targetPosition; }
        set
        {
            instance.targetPosition = value;
            instance.transform.position = value;
        }
    }

    public static void AddTargetLerped(Transform target)
    {
        instance.lerp = 0.0f;
        instance.targets.Add(target);
    }

    public static void RemoveTargetLerped(Transform target)
    {
        instance.lerp = 0.0f;
        instance.targets.Remove(target);
    }

    public static void SetTargetLerped(Transform transform)
    {
        instance.targets.Clear();
        instance.targets.Add(transform);
        instance.lerp = 0.0f;
    }

    public static void SetTarget(Transform transform)
    {
        Clear();
        AddTargetLerped(transform);
    }

    public static void Clear()
    {
        instance.targets.Clear();
    }

    public static void ShowGrid(float fadeTime = 0.0f)
    {
        instance.hexCamera.ShowGrid(fadeTime);
    }

    public static void HideGrid(float fadeTime = 0.0f)
    {
        instance.hexCamera.HideGrid(fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0 && Camera.main != null)
        {
            targetPosition = Vector3.zero;

            targets.RemoveAll(t => t == null);

            if (targets.Count == 0)
                return;

            foreach (Transform transform in targets)
            {
                targetPosition += transform.position;
            }

            targetPosition /= targets.Count;
            targetPosition += lookAtOffset;

            Vector3 cameraPosition = cameraOffset + Vector3.up * getTargetSpread();
            Vector3 cameraForward = (lookAtOffset - cameraPosition).normalized;

            if (lerp < 1.0f)
            {
                Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, cameraPosition, lerp);
                Camera.main.transform.forward = Vector3.Lerp(Camera.main.transform.forward, cameraForward, lerp);

                transform.position = Vector3.Lerp(transform.position, targetPosition, lerp);
                lerp += (Time.deltaTime / lerpSpeed);
            }
            else
            {
                Camera.main.transform.forward = cameraForward;
                Camera.main.transform.localPosition = cameraPosition;
                transform.position = targetPosition;
            }
        }
    }

    private float getTargetSpread()
    {
        float spread = 0.0f;

        foreach (Transform transform in targets)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > spread)
                spread = distance;
        }

        return spread;
    }
}
