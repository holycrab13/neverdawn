using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ChatMessageIconMode
{
    Left,
    Right,
    None
}

public class ChatMessage
{
    public string message { get; private set; }

    public Sprite icon { get; private set; }

    public bool discarded { get; private set; }

    public ChatMessageIconMode mode { get; private set; }

    public ChatMessage(Sprite icon, string message, ChatMessageIconMode mode)
    {
        this.icon = icon;
        this.message = message;
        this.mode = mode;
        this.discarded = false;
    }

    public void Discard()
    {
        discarded = true;
    }

}
