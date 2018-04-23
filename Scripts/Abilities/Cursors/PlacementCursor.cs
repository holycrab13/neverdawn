using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlacementCursor : AbilityCursorBase
{
    private Vector3 targetDirection;

    private float turnSpeed;

    private HexTile startTile;

    private HexTile _targetTile;
    
    private Vector3 _targetDirection;

    private Character _characterToPlace;
    private GameObject _selector;
    private Transform _selectorTransform;


    public HexTile targetTile
    {
        get { return _targetTile; }
    }

    protected override void OnInitializeCursor()
    {
        startTile = character.currentTile;

        _selector = HexTerrain.CreateTileSelector(controller.color, 1.5f, 0.9f);

        AlphaPingPong anim = _selector.AddComponent<AlphaPingPong>();
        anim.intensity = 0.5f;

        _selectorTransform = _selector.transform;
        _selectorTransform.position = Vector3.forward * 10000.0f;
    }

    void OnDestroy()
    {

    }

    protected override bool OnConfirmCursor()
    {
        if (_targetTile != null)
        {
            _selector.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    public override void UpdateValues(Vector3 direction, float intensity)
    {
        if (isActive)
        {


            // find adjacent tile with min angle to direction!
            float minAngle = float.MaxValue;
            _targetTile = null;

            if (direction == Vector3.zero)
            {
                _targetTile = null;
            }
            else
            {
                foreach (HexPath path in startTile.neighbors)
                {
                    targetDirection = (path.target.position - startTile.position).normalized;

                    float angle = Vector3.Angle(direction, targetDirection);

                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        _targetTile = path.target;
                        _targetDirection = (path.target.position - startTile.position).normalized;
                    }
                }
            }

            if (_targetTile != null)
            {
                float angle = Vector3.SignedAngle(_targetDirection, Vector3.forward, Vector3.up);
                _selectorTransform.position = _targetTile.position;
            }
            else
            {
                _selectorTransform.position = Vector3.forward * 10000.0f;
            }
            
        }
    }

    protected override void OnHideCursor()
    {
        
    }

    protected override bool OnCancelCursor()
    {
        character.remainingSteps = 0;
        return true;
    }


    public Character characterToPlace
    {
        get { return _characterToPlace; }
        set { _characterToPlace = value; }
    }
}
