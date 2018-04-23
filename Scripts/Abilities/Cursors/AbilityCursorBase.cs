using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public abstract class AbilityCursorBase : MonoBehaviour
{
    public AvatarController controller { get; private set; }

    public bool isActive { get; private set; }

    public Character character { get; private set; }

    public Color color { get; private set; }

    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public bool Confirm()
    {
        if (OnConfirmCursor())
        {
            hide();
            return true;
        }

        return false;
    }

    internal bool Cancel()
    {
        if (OnCancelCursor())
        {
            hide();
            return true;
        }

        return false;
    }

    private void hide()
    {
        isActive = false;
        OnHideCursor();
    }

    public void Initialize(AvatarController controller)
    {
        this.controller = controller;
        this.isActive = true;
        this.color = controller.color;
        this.character = controller.character;

        OnInitializeCursor();

        gameObject.SetActive(true);
    }

    public void Initialize(Color color, Character character)
    {
        this.isActive = true;
        this.color = color;
        this.character = character;

        OnInitializeCursor();

        gameObject.SetActive(true);
    }

    protected abstract void OnInitializeCursor();

    protected abstract bool OnConfirmCursor();

    protected abstract void OnHideCursor();

    protected abstract bool OnCancelCursor();

    public virtual void Next()
    {

    }

    public virtual void Previous()
    {

    }

    public virtual void UpdateValues(Vector3 direction, float intensity)
    {

    }

}
