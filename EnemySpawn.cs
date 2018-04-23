using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : FrameComponent {

    [SerializeField]
    private int _count;

    [SerializeField]
    private float _radius;

    [SerializeField]
    private float _respawnDelayInHours;

    [SerializeField]
    private Character[] _enemyPrefabs;



    public float radius
    {
        get { return _radius; }
        set { _radius = value; }
    }
}
