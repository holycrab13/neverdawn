using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Fighter : FrameComponent {

    [SerializeField]
    private bool isAggressive;

    [SerializeField]
    private float aggroRange;

    [SerializeField]
    private float supportRange;

    [SerializeField]
    private string _faction;

    private bool isFighting;

    public bool Call(Fighter supporter)
    {
        return supporter._faction == _faction && Vector3.Distance(supporter.transform.position, transform.position) < supportRange;
    }

    void Update()
    {
        if (isAggressive && !isFighting)
        {
            List<Character> charactersInRange = CharacterUtils.GetCharactersInRange(GameController.instance.party.activeCharacters, 
                transform.position, aggroRange);

            if (charactersInRange != null && charactersInRange.Count > 0)
            {
                isFighting = true;
                GameController.instance.JoinCombat(GetComponent<Character>());
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, supportRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    public string faction
    {
        get { return _faction; }
    }
}
