using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

[SelectionBase]
[ExecuteInEditMode]
public class Frame : MonoBehaviour {

    private static List<Frame> _frames;

    [SerializeField]
    private string _id;

    private bool selected;

    private Frame _parent;

    [SerializeField]
    [HideInInspector]
    private string _prefab;

    private bool _destroyedByPlayer;

    private Transform _cachedTransform;

    private Transform frameTransform
    {
        get
        {
            if (!_cachedTransform)
                _cachedTransform = transform;

            return _cachedTransform;
        }
    }

    private string parentId
    {
        get { return _parent != null ? _parent.id : string.Empty; }
    }

    private HexTile _currentTile;

    public HexTile currentTile
    {
        get
        {
            return _currentTile;
        }
        set
        {
            if (_currentTile)
            {
                _currentTile.currentFrame = null;
            }

            _currentTile = value;

            if (_currentTile)
            {
                _currentTile.currentFrame = this;
            }
        }
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        gameObject.layer = 10;

        if (string.IsNullOrEmpty(_id))
        {
            _id = Guid.NewGuid().ToString();
        }

        if (_frames != null)
        {
            if (!_frames.Contains(this))
            {
                _frames.Add(this);
            }
        }

        if(HexTerrain.exists)
            UpdateTile();
    }

    public void UpdateTile()
    {
        if (HexTerrain.exists)
        {
            currentTile = HexTerrain.GetClosestTile(transform.position);
            lastPosition = transform.position;
        }
    } 

    void Update()
    {
        //if (!gameObject.isStatic && HexTerrain.exists)
        //{
        //    if (Vector3.Distance(lastPosition, transform.position) > HexTerrain.tileSize * 0.1f)
        //    {
        //        updateTile();
        //    }
        //}
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (PrefabUtility.GetPrefabParent(gameObject) != null)
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
    }
#endif

    void OnDestroy()
    {
        if (destroyed)
            SerializeTo(GameController.instance.currentGame);

        if (_frames == null)
            return;

        _frames.Remove(this);
    }


    internal SerializedFrame SerializeTo(SerializableGame data)
    {
        if (data == null)
            return null;

        data[id] = new SerializedFrame()
        {
            id = id,
            prefab = prefab,
            scene = SceneManager.GetActiveScene().buildIndex,
            destroyed = destroyed,
            position = SerializableVector3.FromVector3(transform.position),
            rotation = SerializableVector3.FromVector3(transform.eulerAngles),
            componentData = SerializeComponents()
        };

        return data[id];
    }

    internal static void FlushMemory()
    {
        
    }


    /// <summary>
    /// Deserializes a set of serialized frames into the scene
    /// </summary>
    /// <param name="frames"></param>
    internal static void DeserializeFrames(SerializableGame gameData, int sceneBuildIndex)
    {
        SerializableGame data = GameController.instance.currentGame;

        // Create a dictionarry with ALL saved frames
        foreach (SerializedFrame serializedFrame in gameData.frames)
        {
            data[serializedFrame.id] = serializedFrame;
        }

        _frames = new List<Frame>();
        _frames.AddRange(FindObjectsOfType<Frame>());

        // Check the current scene of all stationary frames
        foreach(Frame frame in _frames)
        {
            if (data.ContainsKey(frame.id))
            {
                SerializedFrame serializedFrame = data[frame.id];

                // stationary is not in the scene..
                if (serializedFrame.scene != sceneBuildIndex)
                {
                    Destroy(frame.gameObject);
                }
            }
        }

        SerializedFrame[] sceneFrames = gameData.GetFramesBySceneBuildIndex(sceneBuildIndex);

        // Create all the frames needed in the scene
        foreach (SerializedFrame serializedFrame in sceneFrames)
        {
            Frame frame = FindFrameById(serializedFrame.id);

            // If the frame doesn't exist in the level yet, create it!
            if (frame == null)
            {
                // If the frame has been destroyed in the last session, we won't need it ever again!
                if(serializedFrame.destroyed)
                {
                    data.Remove(serializedFrame.id);
                    continue;
                }

                frame = Instantiate(NeverdawnDatabase.GetPrefab(serializedFrame.prefab));
            }

            if(serializedFrame.destroyed)
            {
                Destroy(frame.gameObject);
                continue;
            }

            frame.id = serializedFrame.id;
            frame.prefab = serializedFrame.prefab;
            frame.transform.position = serializedFrame.position.ToVector3();
            frame.transform.eulerAngles = serializedFrame.rotation.ToVector3();

            frame.Init();
        }

        // Load the frame components
        foreach (SerializedFrame serializedFrame in sceneFrames)
        {
            Frame frame = FindFrameById(serializedFrame.id);
            frame.DeserializeComponents(serializedFrame.componentData);            
        }
    }

    /// <summary>
    /// Deserializes all components from a string
    /// </summary>
    /// <param name="p"></param>
    private void DeserializeComponents(string p)
    {
        StringReader reader = new StringReader(p);
        int length = reader.ReadInt();

        for (int i = 0; i < length; i++)
        {
            string typeString = reader.ReadString();
            Type type = Type.GetType(typeString);

            FrameComponent component = (FrameComponent)GetComponent(type);
            component.ReadData(reader);
        }
    }

    /// <summary>
    /// Serializes the components of the frame into a string
    /// </summary>
    /// <returns></returns>
    private string SerializeComponents()
    {
        FrameComponent[] components = GetComponents<FrameComponent>();
        StringWriter writer = new StringWriter();

        writer.WriteInt(components.Length);

        for (int i = 0; i < components.Length; i++)
        {
            writer.WriteString(components[i].GetType().FullName);
            writer.WriteTransmittable(components[i]);
        }

        return writer.Text;
    }

    private void SetParent(Frame parentFrame)
    {
        _parent = parentFrame;
        transform.SetParent(parentFrame.transform);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    internal static Frame FindFrameById(string p)
    {
        if (_frames != null)
        {
            return _frames.FirstOrDefault(f => f._id == p);
        }

        return null;
    }

    public string id
    {
        get { return _id; }
        private set { _id = value; }
    }


    public bool destroyed
    {
        get { return _destroyedByPlayer; }
        set { _destroyedByPlayer = value; }
    }

    public string prefab
    {
        get { return _prefab; }
        set { _prefab = value; }
    }


    public string label
    {
        get
        {
            Identity identity = GetComponent<Identity>();
            return identity != null ? identity.label : string.Empty;
        }
    }


    internal static T FindComponentById<T>(string id) where T : FrameComponent
    {
        Frame frame = FindFrameById(id);

        if(frame)
        {
            return frame.GetComponent<T>();
        }

        return null;
    }

    public Vector3 lastPosition { get; set; }

    public Vector3 position
    {
        get { return frameTransform.position; }
    }

    internal void AddBuffs(IEnumerable<BuffBase> buffs)
    {
        // throw new NotImplementedException();
    }
}
