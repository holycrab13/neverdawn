using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Caster : FrameComponent
{
    [SerializeField]
    private int _manaPoints;

    [SerializeField]
    private int _maxManaPoints;

    public bool isOom
    {
        get { return _manaPoints <= 0; }
    }

    public int manaPoints
    {
        get
        {
            return _manaPoints;
        }
        set
        {
            _manaPoints = value;
            _manaPoints = Mathf.Clamp(_manaPoints, 0, _maxManaPoints);
        }
    }

    public int maxManaPoints
    {
        get
        {
            return _maxManaPoints;
        }
        set
        {
            _maxManaPoints = value;
            _manaPoints = Mathf.Clamp(_manaPoints, 0, _maxManaPoints);
        }
    }


    public float manaPercentage
    {
        get { return (float)_manaPoints / (float)_maxManaPoints; } 
    }
}
