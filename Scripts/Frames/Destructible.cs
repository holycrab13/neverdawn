using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("FrameComponent/Destructible")]
public class Destructible : FrameComponent
{
    [SerializeField]
    private int _healthPoints;

    [SerializeField]
    private int _maxHealthPoints;

    public void Heal(int amount)
    {
        _healthPoints += amount;
        _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealthPoints);
    }

    public void Heal(float amount)
    {
        Heal((int)amount);
    }

    public void Damage(int points)
    {
        _healthPoints -= points;

        _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealthPoints);

        Character character = GetComponent<Character>();


        FloatingTextFactory.Create(points.ToString(), transform.position + Vector3.up, new Color(1.0f, 0.2f, 0.2f));

        if (character != null)
        {
            if (isDead)
            {
                character.Kill();
            }
            else if (points > 0)
            {
                if(GameController.state == GameState.Exploration)
                    GameController.instance.StartCombat(character);

                character.PushAction(new CharacterHurtAction());
            }
        }
        
    }

    private void killIfExists<T>() where T : Behaviour
    {
        T toKill = GetComponent<T>();

        if(toKill != null)
        {
            Destroy(toKill);
        }
    }

    public bool isDead
    {
        get { return _healthPoints <= 0; }
    }

    public int healthPoints
    {
        get
        {
            return _healthPoints;
        }
        set
        {
            _healthPoints = value;
            _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealthPoints);
        }
    }

    public int maxHealthPoints
    {
        get
        {
            return _maxHealthPoints;
        }
        set
        {
            _maxHealthPoints = value;
            _healthPoints = Mathf.Clamp(_healthPoints, 0, _maxHealthPoints);
        }
    }

    protected override void readData(IMessageReader reader)
    {
        _healthPoints = reader.ReadInt();
        _maxHealthPoints = reader.ReadInt();

        if(isDead)
        {
            Character character = GetComponent<Character>();

            if (character != null)
                character.Kill();
        }
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteInt(healthPoints);
        writer.WriteInt(maxHealthPoints);
    }
}
