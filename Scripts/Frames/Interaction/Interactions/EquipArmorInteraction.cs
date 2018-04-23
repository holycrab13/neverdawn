using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interaction", menuName = "Neverdawn/Interaction/Equip Armor", order = 1)]
public class EquipArmorInteraction : ComponentInteraction
{
    private Equipable equipable;

    public override void Interact(NeverdawnCharacterController controller)
    {
        Mannequin mannequin = controller.character.GetComponent<Mannequin>();

        if(mannequin != null)
        {
            mannequin.Equip(equipable);
        }
    }

    public override bool IsAvailable(Character character)
    {
        return !equipable.isEquipped && character.mannequin && character.mannequin.CanEquip(equipable);
    }

    public override bool Initialize(GameObject gameObject)
    {
        equipable = gameObject.GetComponent<Equipable>();

        return equipable != null;
    }
}