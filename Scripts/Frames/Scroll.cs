using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scroll : FrameComponent
{
    [SerializeField]
    private ArcaneAbility _arcaneAbility;

    void Update()
    {
        if(used)
        {
            Destroy(gameObject);
        }
    }

    public bool used { get; set; }

    public ArcaneAbility arcaneAbility
    {
        get { return _arcaneAbility; }
    }


}
