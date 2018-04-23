using UnityEngine;

[CreateAssetMenu(fileName = "New Text", menuName = "Neverdawn/Story/Text", order = 1)]
public class TextResource : ScriptableObject
{
    [SerializeField]
    private string _valueEN;

    [SerializeField]
    private string _valueDE;

    public string value
    {
        get { return _valueEN; }
    }
}