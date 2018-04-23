using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class PlayerParty : FrameComponent, IEnumerable<Character> {


    [SerializeField]
    private List<Character> _characters;

    [SerializeField]
    private List<Character> _activeCharacters;

    [SerializeField]
    private List<Character> _hiddenCharacters;


    public List<Character> characters
    {
        get { return _characters.ToList(); }
    }

    protected override void readData(IMessageReader reader)
    {
        int size = reader.ReadInt();

        for(int i = 0; i < size; i++)
        {
            string frameId = reader.ReadString();
            Character character = Frame.FindComponentById<Character>(frameId);

            _characters.Add(character);
            PutCharacter(character);
        }
    }

    protected override void writeData(IMessageWriter writer)
    {
        writer.WriteInt(_characters.Count);
        foreach(Character character in _characters)
        {
            writer.WriteString(character.frame.id);
        }
    }

    internal void PullCharacter(Character character, Vector3 position)
    {
        character.position = position;
        character.solid.Show();
        character.GetComponentInChildren<NavMeshAnimator>().Reset();

        _activeCharacters.Add(character);
        _hiddenCharacters.Remove(character);
    }

    internal void PutCharacter(Character character)
    {
        character.transform.SetParent(transform, true);
        character.transform.localPosition = Vector3.zero;
        character.transform.localRotation = Quaternion.identity;
        character.solid.Hide();

        _activeCharacters.Remove(character);
        _hiddenCharacters.Add(character);
    }

    internal Character GetNextCharacter(AvatarController controller = null)
    {
        if (Count == 0)
        {
            return null;
        }
        else
        {
            int index = 0;

            Vector3 position = GameController.playerSpawnPosition;
            Quaternion rotation = Quaternion.identity;
            Vector3 forward = Vector3.right;

            if (controller != null && controller.character != null)
            {
                index = characters.IndexOf(controller.character) + 1;

                position = controller.character.transform.position;
                rotation = controller.character.transform.rotation;
                forward = controller.character.transform.forward;
                PutCharacter(controller.character);
            }

            for (int i = 0; i < characters.Count; i++)
            {
                int currentIndex = NeverdawnUtility.RepeatIndex(index + i, characters.Count);

                Character character = characters[currentIndex];

                if (!character.controller)
                {
                    character.solid.Show();
                    character.transform.position = position;
                    character.transform.forward = forward;
                    character.GetComponentInChildren<NavMeshAnimator>().Reset();


                    _activeCharacters.Add(character);
                    _hiddenCharacters.Remove(character);
                    return character;   
                }
            }
        }

        return null;
    }

    internal string[] GetNames()
    {
        return characters.Select(c => c.identity.label).ToArray();
    }

    internal GameObject[] GetObjects()
    {
        return characters.Select(c => c.gameObject).ToArray();
    }

    internal Character[] GetAllCharacters()
    {
        return characters.ToArray();
    }

    internal void StoreUnused()
    {
        foreach(Character character in characters)
        {
            if(!character.controller)
            {
                PutCharacter(character);
            }
        }
    }

    internal void SetCharacterPositions(Vector3 targetPosition, float p)
    {
       foreach(Character character in _characters)
       {
           if(IsActive(character))
           {
               character.position = targetPosition;
           }
       }
    }

   

    internal bool IsCharacterInParty(Character character)
    {
        return characters.Contains(character);
    }

    internal bool IsCharacterInParty(Frame frame)
    {
        return frame.GetComponent<Character>() && characters.Contains(frame.GetComponent<Character>());
    }

    internal void Clear()
    {
        _characters.Clear();
    }

    public IEnumerator<Character> GetEnumerator()
    {
        return characters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return characters.GetEnumerator();
    }

    public Character this[int index]
    {
        get { return characters[index]; }
    }

    public Character this[string id]
    {
        get { return characters.FirstOrDefault(c => c.id == id); }
    }

    public int Count
    {
        get { return characters.Count; }
    }

    public void SetCharacters(List<Character> characters)
    {
        _characters = characters;
    }

    public bool IsHidden(Character character)
    {
        return _hiddenCharacters.Contains(character);
    }

    public bool IsActive(Character character)
    {
        return _activeCharacters.Contains(character);
    }

    public List<Character> hiddenCharacters
    {
        get { return _hiddenCharacters; }
    }

    public List<Character> activeCharacters
    {
        get { return _activeCharacters; }
    }

}
