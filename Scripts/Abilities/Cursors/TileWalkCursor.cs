using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class TileWalkCursor : AbilityCursorBase
{
    private Vector3 targetDirection;

    private float turnSpeed;

    private HexTile _targetTile;

    [SerializeField]
    private SpriteRenderer renderer;

    private Vector3 _targetDirection;

    private GameObject _selector;
    private Transform _selectorTransform;

    public HexTile targetTile
    {
        get { return _targetTile; }
    }

    protected override void OnInitializeCursor()
    {
        renderer.color = controller.color;

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
        if (_targetTile != null && character.isIdle)
        {
            _selector.SetActive(false);

            if (tileIsValidTarget(_targetTile))
            {
                if (character.remainingSteps > 0)
                {
                    character.currentTile = _targetTile;
                    character.remainingSteps--;
                    character.PushAction(new CharacterWalkToAction(_targetTile.position));
                }
            }
        }

        return character.remainingSteps == 0;
    }


    private bool tileIsValidTarget(HexTile _targetTile)
    {
        if (_targetTile.isOccluded)
            return false;

        if (_targetTile.type != HexTileType.Empty)
            return false;

        if (_targetTile.currentFrame != null)
            return false;

        return true;
    }

    public override void UpdateValues(Vector3 direction, float intensity)
    {
        if (isActive)
        {

            _selector.SetActive(character.isIdle);

            float minAngle = float.MaxValue;
            _targetTile = null;

            if (direction == Vector3.zero)
            {
                _targetTile = null;
            }
            else
            {
                foreach (HexPath path in character.currentTile.neighbors)
                {
                    targetDirection = (path.target.position - character.currentTile.position).normalized;

                    float angle = Vector3.Angle(direction, targetDirection);

                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        _targetTile = path.target;
                        _targetDirection = (path.target.position - character.currentTile.position).normalized;
                    }
                }
            }

            if (_targetTile != null)
            {
                float angle = Vector3.SignedAngle(_targetDirection, Vector3.forward, Vector3.up);
                _selectorTransform.position = _targetTile.position;

                float angleToCharacter = Vector3.SignedAngle(character.forward, targetDirection, Vector3.up);

                if (Mathf.Abs(angleToCharacter) > 10.0f)
                {
                    character.transform.Rotate(Vector3.up, Mathf.Sign(angleToCharacter) * turnSpeed * Time.deltaTime);
                }

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
        return true;
    }

}
