using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Container))]
public class Lootable : FrameComponent
{

    [SerializeField]
    private LootTable _table;

    [SerializeField]
    private int _maxNumberOfItems;

    [SerializeField]
    private float _refillDelayInHours;

    [SerializeField]
    private float _timeToRefillInHours;

    private Container _container;

    void Start()
    {
        _container = GetComponent<Container>();
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteFloat(_timeToRefillInHours);
        writer.WriteFloat(_refillDelayInHours);
    }

    protected override void readData(IMessageReader reader)
    {
        _timeToRefillInHours = reader.ReadFloat();
        _refillDelayInHours = reader.ReadFloat();
    }

    void Update()
    {
        if (_container.isEmpty)
        {
            if (_timeToRefillInHours <= 0.0f)
            {
                if (_container.isEmpty)
                {
                    int numberOfItems = UnityEngine.Random.Range(0, _maxNumberOfItems) + 1;

                    _timeToRefillInHours = _refillDelayInHours;

                    for (int i = 0; i < numberOfItems; i++)
                    {
                        _container.collection.AddPickable(_table.GenerateLoot());
                    }
                }
            }
            else
            {
                _timeToRefillInHours -= (PlayerTime.ScaleTime(Time.deltaTime) / 3600.0f);
            }   
        }
    }
}
