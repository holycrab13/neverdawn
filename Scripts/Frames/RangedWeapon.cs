using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RangedWeapon : Weapon {

    public Vector3 projectileSpawn;

    public int range;

    public float estimatedRange { get { return (range * range) / GameSettings.gravity; } }
}
