using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Drinkable,
    Edible,
    Magic
}

public class Consumable : FrameComponent {

    public ConsumableType type;

    public BuffBase[] buffs;
}
