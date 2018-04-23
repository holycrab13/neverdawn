using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootSteps : MonoBehaviour
{

    public float pitchRange;

    public AudioClip[] sounds;

    private int stepIndex;
    private AudioSource source;
    private Animator anim;
    private float lastFootStep;

    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float footStep = anim.GetFloat("FootStep");

        if (footStep >= 0.0f && lastFootStep < 0.0f)
        {
            playFootSound();
            nextStep();
        }

        if (footStep <= 0.0f && lastFootStep > 0.0f)
        {
            playFootSound();
            nextStep();
        }

        lastFootStep = footStep;
    }

    private void nextStep()
    {
        stepIndex = Random.Range(0, sounds.Length);
    }


    private void playFootSound()
    {
        source.clip = sounds[stepIndex];
        source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
        source.Play();
    }
}
