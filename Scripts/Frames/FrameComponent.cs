using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Frame))]
public abstract class FrameComponent : MonoBehaviour, ITransmittable {

    private Frame _frame;

    public Frame frame
    {
        get
        {
            if(_frame == null)
            {
                _frame = GetComponent<Frame>();
            }

            return _frame;
        }
    }

    public string id
    {
        get
        {
            return frame.id;
        }
    }

    protected virtual void readData(IMessageReader reader)
    {

    }

    protected virtual void writeData(IMessageWriter writer)
    {
        
    }

    public void ReadData(IMessageReader reader)
    {
        readData(reader);
    }

    public void WriteData(IMessageWriter writer)
    {
        writeData(writer);
    }

    protected static T FindFrameComponentOfType<T>(string pickableId) where T : FrameComponent
    {
        throw new System.NotImplementedException();
    }
}
