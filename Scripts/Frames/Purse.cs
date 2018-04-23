using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Frame))]
public class Purse : FrameComponent
{

    [SerializeField]
    private int _gold;


    public int gold
    {
        get { return _gold; }
    }

    protected override void readData(IMessageReader reader)
    {
        _gold = reader.ReadInt();
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteInt(_gold);
    }

    /// <summary>
    /// Add gold to the purse
    /// </summary>
    /// <param name="amount"></param>
    public void AddGold(int amount)
    {
        _gold += amount;
    }

    /// <summary>
    /// Removes gold from the purse
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int RemoveGold(int amount)
    {
        int goldAmount = Mathf.Min(amount, _gold);

        _gold -= goldAmount;
        return goldAmount;
    }
}
