using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct LootTableEntry
{
    public float probability;
    public Pickable prefab;
}

[CreateAssetMenu(fileName = "New Loot Table", menuName = "Neverdawn/Settings/Loot Table", order = 1)]
public class LootTable : ScriptableObject
{
    [SerializeField]
    private LootTableEntry[] _entries;

    public Pickable GenerateLoot()
    {
        normalizeProbabilities();

        float rand = UnityEngine.Random.value;

        float sum = 0.0f;

        foreach(LootTableEntry entry in _entries)
        {
            sum += entry.probability;

            if(rand <= sum)
            {
                return Instantiate(entry.prefab);
            }
        }

        return null;
    }

    private void normalizeProbabilities()
    {
        float sum = 0.0f;
        foreach(LootTableEntry entry in _entries)
        {
            sum += entry.probability;
        }

        for(int i = 0; i < _entries.Length; i++)
        {
            _entries[i].probability /= sum;
        }
    }
}
