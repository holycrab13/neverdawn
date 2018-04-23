using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Castable : FrameComponent
{
    [SerializeField]
    private AbilityBase[] _abilities;

    public AbilityBase[] abilities
    {
        get { return validate(_abilities); }
    }

    private AbilityBase[] validate(AbilityBase[] abilities)
    {
        List<AbilityBase> validAbilities = new List<AbilityBase>();

        foreach (AbilityBase interaction in abilities)
        {
            if (interaction == null)
            {
                Debug.LogWarning("Ability is null on " + gameObject.name);
                continue;
            }

            AbilityBase instance = Instantiate(interaction) as AbilityBase;

            if (instance.Initialize(gameObject))
            {
                validAbilities.Add(instance);
            }
            else
            {
                Destroy(instance);
                Debug.LogWarning("Failed to initialize " + interaction.ToString() + " on " + gameObject.name);
            }
        }

        return validAbilities.ToArray();
    }
}
