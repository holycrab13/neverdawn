using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Castable))]
public class CombatCharacter : FrameComponent
{
    [SerializeField]
    private OverrideAnimationClips _animationOverrides;
   
    public Frame interactable { get; private set; }

    public bool isEquipped { get; set; }

    public OverrideAnimationClips animationOverrides { get { return _animationOverrides; } }

    void Awake()
    {
        interactable = GetComponent<Frame>();
    }

    public AnimationClipMap GetOverrides(AnimatorOverrideController controller)
    {
        return animationOverrides.GetOverrides(controller);
    }
}
