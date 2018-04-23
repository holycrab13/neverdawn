using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPickTrigger : FrameComponent {

    [SerializeField]
    private bool _alwaysTrigger;

    [SerializeField]
    private string _trigger;

    private bool _wasTriggered;

    public void Trigger()
    {
        if (_alwaysTrigger || !_wasTriggered)
        {
            _wasTriggered = true;
            GameController.instance.TriggerEvent(_trigger);
        }
    }

    protected override void readData(IMessageReader reader)
    {
        _alwaysTrigger = reader.ReadBool();
        _wasTriggered = reader.ReadBool();
        _trigger = reader.ReadString();
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteBool(_alwaysTrigger);
        writer.WriteBool(_wasTriggered);
        writer.WriteString(_trigger);
    }
	
}
