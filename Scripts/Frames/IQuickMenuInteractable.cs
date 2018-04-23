using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IQuickMenuInteractionCollection 
{
    Sprite icon { get; }

    string label { get; }

    string description { get; }

    QuickMenuInteraction[] interactions { get; }

    int revision { get; }
}

