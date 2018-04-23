using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIQuickMenuPage : UIBehaviour {

    public UIQuickMenu menu { get; set; }

    [SerializeField]
    private bool _includeHeader;

    protected AvatarController avatarController
    {
        get { return menu.avatarController; }
    }

    public bool includeHeader
    {
        get { return _includeHeader; }
    }

    public IEnumerator enablePage()
    {
        yield return null;

        OnQuickMenuPageEnabled();
    }

    protected virtual void OnQuickMenuPageEnabled()
    {

    }
}
