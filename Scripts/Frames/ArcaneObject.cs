using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcaneObject : FrameComponent
{
    [SerializeField]
    private float _arcanePower;

    public float arcanePower
    {
        get { return _arcanePower; }
    }

    protected override void readData(IMessageReader reader)
    {
        _arcanePower = reader.ReadFloat();
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteFloat(arcanePower);
    }
}
