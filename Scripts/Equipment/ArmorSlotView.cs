using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArmorSlotView : MonoBehaviour {

    public Armor drawable { get; set; }

    [SerializeField]
    private ArmorType _type;

    public ArmorType type
    {
        get { return _type; }
    }
    
    public abstract void SetMaterial(Material material);

    public abstract void SetMesh(Mesh mesh);

    public void SetArmor(Armor drawable)
    {
        this.drawable = drawable;

        if (drawable != null)
        {
            SetMaterial(drawable.armorMaterial);
            SetMesh(drawable.armorMesh);
        }
        else
        {
            SetMaterial(null);
            SetMesh(null);
        }
    }
}
