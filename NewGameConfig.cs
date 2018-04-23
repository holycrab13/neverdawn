using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameConfig : MonoBehaviour {

    [SerializeField]
    private Character[] characters;

    [SerializeField]
    private Frame playerFrame;

    public List<Frame> InstantiateGame(int amount)
    {
        List<Frame> result = new List<Frame>();

        result.Add(playerFrame);

        List<Character> _characters = new List<Character>();

        for(int i = 0; i < amount; i++)
        {
            Character character = characters[i];

            _characters.Add(character);
            result.AddRange(character.GetComponentsInChildren<Frame>());
        }

        playerFrame.GetComponent<PlayerParty>().SetCharacters(_characters);

        return result;
    }
}
