using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Topic 
{
    public bool isCommon { get; private set; }

    public string label { get; set; }

    public List<string> questions { get; private set; }

    public NeverdawnEventBase defaultEvent { get; set; }

    public Topic(string id, bool isCommon)
    {
        this.questions = new List<string>();
        this.id = id;
        this.isCommon = isCommon;
    }

    public string id { get; private set; }


    internal string questionMessage
    {
        get { return questions[Random.Range(0, questions.Count)]; }
    }
}
