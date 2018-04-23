using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Solid : FrameComponent {

    [SerializeField]
    private GameObject _prefab;

    [HideInInspector]
    public Renderer[] renderers;
 
    [HideInInspector]
    public Collider[] colliders;

    private bool _isHidden;

    private GameObject _instance;

    void Awake()
    {
        createInstance();
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        createInstance();
    }
    
    void OnValidate()
    {
        _instance = null;
    }
#endif

    private void createInstance()
    {
        
#if UNITY_EDITOR
        if (PrefabUtility.GetPrefabType(gameObject) != PrefabType.Prefab)
        {
#endif
            if (_instance == null && _prefab != null)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Frame>() == null)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }

                _instance = Instantiate(_prefab);
            }

            if (_instance != null)
            {
                _instance.transform.SetParent(transform);
                _instance.name = getInstanceName();
                _instance.transform.localPosition = Vector3.zero;
                _instance.transform.localEulerAngles = Vector3.zero;

                
#if UNITY_EDITOR
                setNotEditableRecursive(_instance.transform);
#endif
            }
            
#if UNITY_EDITOR
        }
#endif
    }

    private void setNotEditableRecursive(Transform t)
    {
        t.gameObject.hideFlags = HideFlags.NotEditable | HideFlags.HideAndDontSave;

        foreach(Transform child in t)
        {
            setNotEditableRecursive(child);
        }
    }

    private string getInstanceName()
    {
        return string.Format("{0}_{1}", frame.prefab, "Solid");
    }

    public void Hide(bool visualsOnly = false)
    {
        _isHidden = true;

        foreach(Renderer renderer in _instance.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        foreach (Collider collider in _instance.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void Show(bool visualsOnly = false)
    {
        _isHidden = false;

        foreach (Renderer renderer in _instance.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }

        foreach (Collider collider in _instance.GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
    }

    public bool IsHidden  
    {
        get { return _isHidden; }
    }
}
