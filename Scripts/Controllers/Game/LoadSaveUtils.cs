using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the input devices, checks if any device "Start" is pressed and creates a new avatar controller
/// </summary>
public class LoadSaveUtils : MonoBehaviour
{
    private static string PLAYER_DATA_FILE_ENDING = ".dat";

    public static void Save(SerializableGame data, string saveGameName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream files = File.Open(Application.persistentDataPath + "/" + saveGameName + PLAYER_DATA_FILE_ENDING, FileMode.OpenOrCreate);

        bf.Serialize(files, data);
        files.Close();
    }

    public static bool Load(string saveGameName, out SerializableGame data)
    {
        if (File.Exists(Application.persistentDataPath + "/" + saveGameName + PLAYER_DATA_FILE_ENDING))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGameName + PLAYER_DATA_FILE_ENDING, FileMode.Open);
            data = (SerializableGame)bf.Deserialize(file);
            file.Close();
            return true;
        }

        data = new SerializableGame();
        return false;
    }

  
}

[Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static SerializableVector3 FromVector3(Vector3 v)
    {
        return new SerializableVector3(v.x, v.y, v.z);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

/// <summary>
/// Frame, that is decoupled from the unity lifecycle system
/// </summary>
[Serializable]
public class SerializedFrame
{
    public string id;
    public string prefab;
    public int scene;
    public bool destroyed;
    public SerializableVector3 position;
    public SerializableVector3 rotation;
    public string componentData;
}

[Serializable]
public class SerializableGame
{
    public int currentScene;
    public SerializableVector3 currentPosition;
    public SerializableVector3 currentRotation;

    public IEnumerable<SerializedFrame> frames
    {
        get
        {
            return _frameMap != null ? _frameMap.Values.ToList() : null;
        }
    }

    private Dictionary<string, SerializedFrame> _frameMap;

    public SerializedFrame this[string name]
    {
        get 
        {
            if (_frameMap == null)
                return null;

            return _frameMap[name]; 
        }
        set
        {
            if (_frameMap == null)
                _frameMap = new Dictionary<string, SerializedFrame>();

            _frameMap[name] = value;
        }
    }

    internal static SerializableGame NewGame(int playerCount)
    {
        SerializableGame result = new SerializableGame();
        //{
        result.currentPosition = new SerializableVector3(0, 0, 0);
        result.currentRotation = new SerializableVector3(0, 0, 0);
        result.currentScene = 1;

        GameObject configObject = GameObject.FindGameObjectWithTag("New Game");

        NewGameConfig config = configObject.GetComponent<NewGameConfig>();
        config.InstantiateGame(playerCount);

        List<Frame> frames = config.InstantiateGame(playerCount);

        List<SerializedFrame> serializedFrames = new List<SerializedFrame>();


        foreach(Frame frame in frames)
        {
            serializedFrames.Add(frame.SerializeTo(result));
        }

        foreach(SerializedFrame frame in serializedFrames)
        {
            frame.scene = 1;
        }

        return result;
    }

    internal SerializedFrame[] GetFramesBySceneBuildIndex(int sceneIndex)
    {
        return frames.Where(f => f.scene == sceneIndex).ToArray();
    }

    internal SerializedFrame GetFrameById(string p)
    {
        return frames.FirstOrDefault(f => f.id == p);
    }

    internal bool ContainsKey(string p)
    {
        return _frameMap.ContainsKey(p);
    }

    internal void Remove(string p)
    {
        _frameMap.Remove(p);
    }
}

[Serializable]
public class SerializableItem
{
    public string prefab;
    public float durability;
}

[Serializable]
public class SerializableJournal
{
    public string[] discoveries;
    public string[] topics;
}

