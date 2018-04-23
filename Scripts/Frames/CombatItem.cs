using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum CombatItemSlotType
{
    Hip,
    Back,
    Hands,
    None
}

[RequireComponent(typeof(Identity))]
[RequireComponent(typeof(Castable))]
public class CombatItem : Equipable
{

    [Header("Visuals")]
    public CombatItemSlotType preferredSlot;

    public GameObject drawableLeftHand;

    public GameObject drawableRightHand;


    [Header("Animation Overrides")]

    [SerializeField]
    private OverrideAnimationClips _animationOverrides;

    private Identity _identity;

    public Frame interactable { get; private set; }
    
    public OverrideAnimationClips animationOverrides { get { return _animationOverrides; } }

    void Awake()
    {
        interactable = GetComponent<Frame>();
    }

    public AnimationClipMap GetOverrides(AnimatorOverrideController controller)
    {
        return animationOverrides.GetOverrides(controller);
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
}

public class AnimationClipMap : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipMap(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

[Serializable]
public struct OverrideAnimationClips
{
    public AnimationClip attack;
    public AnimationClip equip;
    public AnimationClip idle;
    public AnimationClip walk;

    public static OverrideAnimationClips empty
    {
        get
        {
            return new OverrideAnimationClips();
        }
    }



    public AnimationClipMap GetOverrides(AnimatorOverrideController controller)
    {
        AnimationClipMap map = new AnimationClipMap(controller.overridesCount);
        controller.GetOverrides(map);

        map["Default Attack"] = attack != null ? attack : map["Default Attack"];
        map["Default Equip"] = equip != null ? equip : map["Default Equip"];
        map["Default Combat Idle"] = idle != null ? idle : map["Default Combat Idle"];
        map["Default Combat Walk"] = walk != null ? walk : map["Default Combat Walk"];

        return map;
    }

    internal void Add(OverrideAnimationClips overrideAnimationClips)
    {
        attack = attack != null ? attack : overrideAnimationClips.attack;
        equip = equip != null ? equip : overrideAnimationClips.equip;
        idle = idle != null ? idle : overrideAnimationClips.idle;
        walk = walk != null ? walk : overrideAnimationClips.walk;

    }
}
