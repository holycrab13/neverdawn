using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NeverdawnScene : MonoBehaviour {

    [SerializeField]
    private UIRoot uiPrefab;

    [SerializeField]
    private GameController gameControllerPrefab;

    [SerializeField]
    private NeverdawnCamera cameraPrefab;

    [SerializeField]
    private TextAsset events;

	// Use this for initialization
	void Awake () {

        if (!NeverdawnCamera.exists)
        {
            Instantiate(cameraPrefab);
        }
	}

    public static XmlDocument sceneEventsXml
    {
        get
        {
            NeverdawnScene scene = FindObjectOfType<NeverdawnScene>();

       
            XmlDocument doc = new XmlDocument();

            if (scene && scene.events != null)
            {
                doc.LoadXml(scene.events.text);
            }

            return doc;
        }
    }

}
