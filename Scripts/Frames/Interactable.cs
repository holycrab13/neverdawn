using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Identity))]
public class Interactable : FrameComponent, IQuickMenuInteractionCollection
{
    [SerializeField]
    private ComponentInteraction[] _interactions;

    private Identity _identity;
    private bool _selected;

    public ComponentInteraction[] interactions
    {
        get { return _interactions; }
    }

    void Start()
    {
        _interactions = validate<ComponentInteraction>(interactions);
    }

    private T[] validate<T>(T[] interactions) where T : ComponentInteraction
    {
        List<T> validInteractions = new List<T>();

        foreach (T interaction in interactions)
        {
            T instance = Instantiate(interaction);

            if (instance.Initialize(gameObject))
            {
                validInteractions.Add(instance);
            }
        }

        return validInteractions.ToArray();
    }

    QuickMenuInteraction[] IQuickMenuInteractionCollection.interactions
    {
        get
        {
            return interactions.OrderBy(a => a.GetPriority()).ToArray();
        }
    }


    public string description
    {
        get { return identity.description; }
    }

    public int revision
    {
        get { return 0; }
    }

    internal void Deselect()
    {
        if (_selected)
        {
            Highlighting.Unhighlight(this);
            _selected = false;
        }
    }

    internal void Select()
    {
        if (!_selected)
        {
            Highlighting.Highlight(this);
            _selected = true;
        }
    }

    public bool IsSelected
    {
        get { return _selected; }
    }

    public Identity identity
    {
        get
        {
            if (_identity == null)
            {
                _identity = GetComponent<Identity>();
            }

            return _identity;
        }
    }

    public Sprite icon
    {
        get { return identity.icon; }
    }

    public string label
    {
        get { return identity.label; }
    }
}
