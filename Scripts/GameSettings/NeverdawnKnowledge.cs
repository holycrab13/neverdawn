using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;


public class NeverdawnKnowledge : MonoBehaviour
{

    private static NeverdawnKnowledge instance;

    private static Dictionary<string, bool> knowledgeMap;

 
	// Use this for initialization
	void Start () {

        knowledgeMap = new Dictionary<string, bool>();
	}




    internal static bool HasKnowledge(string condition)
    {
        return true;
    }
}
