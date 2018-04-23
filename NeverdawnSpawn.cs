using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class NeverdawnSpawn : MonoBehaviour
{
    [SerializeField]
    private int _index;

    public int index
    {
        get { return _index; }
    }

    public Vector3 position
    {
        get { return transform.position; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }

    internal static NeverdawnSpawn FindSpawnByIndex(int spawnIndex)
    {
        return FindObjectsOfType<NeverdawnSpawn>().FirstOrDefault(s => s.index == spawnIndex);
    }
}
