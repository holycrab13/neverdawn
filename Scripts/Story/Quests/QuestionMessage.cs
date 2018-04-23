using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class QuestionMessage
{
    public TopicQuestion[] questions { get; private set; }

    public string message { get; private set; }

    public Sprite icon { get; private set; }

    public bool discarded { get; private set; }

    public QuestionMessage(Sprite icon, string message, TopicQuestion[] responses)
    {
        this.icon = icon;
        this.message = message;
        this.questions = responses;
        this.discarded = false;
    }

    public void Discard()
    {
        discarded = true;
    }
}


