using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeverdawnGate : MonoBehaviour {

    [SerializeField]
    private float radius;

    [SerializeField]
    private int sceneBuildIndex;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private int spawnIndex;

    private Vector3 position;

    void Start()
    {
        position = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        cooldown -= Time.deltaTime;


        if (cooldown <= 0.0f && CharacterUtils.GetCharactersInRange(GameController.instance.party.activeCharacters, position, radius).Count > 0)
        {
            GameController.instance.ChangeScene(sceneBuildIndex, spawnIndex);
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
