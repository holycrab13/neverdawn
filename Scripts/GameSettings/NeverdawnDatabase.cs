using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;
using System.Xml;

public enum SkillType
{
    Swords,
    CombatExpertise,
    CombatManeuvering,
    Archery,
    Axes,
    BluntWeapons,
    HeavyArmor,
    LightArmor,
    Sorcery,
    Restoration
}

public enum AttributeType
{
    Strength, 
    Intelligence,
    Dexterity
}

public struct SkillConfig
{
    public int Swords;
    public int CombatExpertise;
    public int CombatManeuvering;
    public int Archery;
    public int Axes;
    public int BluntWeapons;
    public int HeavyArmor;
    public int LightArmor;
    public int NoArmor;
    public int Sorcery;
    public int Restoration;
    public int Elementalism;

    public int[] ToArray()
    {
        return new int[]
        {
            Swords,
            CombatExpertise,
            CombatManeuvering,
            Archery,
            Axes,
            BluntWeapons,
            HeavyArmor,
            LightArmor,
            NoArmor,
            Sorcery,
            Restoration,
            Elementalism
        };
    }
}

[ExecuteInEditMode]
public class NeverdawnDatabase : MonoBehaviour {

    public static NeverdawnDatabase instance;

    public static IEnumerable<CharacterAttribute> attributes
    {
        get { return _attributeMap.Values; }
    }

    public static IEnumerable<CharacterSkill> skills
    {
        get { return _skillMap.Values; }
    }

    private static Dictionary<string, Frame> _frameMap;

    private static Dictionary<SkillType, CharacterSkill> _skillMap;

    private static Dictionary<AttributeType, CharacterAttribute> _attributeMap;

    private static Dictionary<string, Discovery> _discoveryMap;

    private List<DiscoveryGroup> _discoveryGroups;

    private static List<Topic> _commonTopics;

    private static Dictionary<string, Topic> _topicMap;

    [SerializeField]
    private TextAsset topicsFile;

    [SerializeField]
    private TextAsset discoveriesFile;

    private static string NODE_NAME_LABEL = "label";
    
	// Use this for initialization
    void OnEnable()
    {

        // Load Skills and Attributes
        loadSkillsAndAttributes();

        // Load Frame Prefabs
        loadFramePrefabs();

        loadTopics();

        loadDiscoveries();
    }

    private static void loadFramePrefabs()
    {
        _frameMap = new Dictionary<string, Frame>();

        Frame[] frames = Resources.LoadAll<Frame>("Frames");

        foreach (Frame frame in frames)
        {
            frame.prefab = frame.name;
            _frameMap.Add(frame.name, frame);
        }
    }

    private void loadSkillsAndAttributes()
    {
        CharacterAttribute[] attributes = Resources.LoadAll<CharacterAttribute>("Attributes");
        CharacterSkill[] skills = Resources.LoadAll<CharacterSkill>("Skills");


        _skillMap = new Dictionary<SkillType, CharacterSkill>();
        _attributeMap = new Dictionary<AttributeType, CharacterAttribute>();

        foreach (CharacterAttribute attribute in attributes)
        {
            _attributeMap.Add(attribute.type, attribute);
        }

        foreach (CharacterSkill skill in skills)
        {
            _skillMap.Add(skill.type, skill);
        }
    }

    public static CharacterAttribute GetAttribute(AttributeType characterAttributeType)
    {
        return _attributeMap[characterAttributeType];
    }

    public static CharacterSkill GetSkill(SkillType characterSkillType)
    {
        if (_skillMap.ContainsKey(characterSkillType))
        {
            return _skillMap[characterSkillType];
        }
        return null;
    }

    public static AttributeType GetBaseAttributeType(SkillType skillType)
    {
        return _skillMap[skillType].baseAttribute;
    }

    public static int GetUpgradeCost(SkillType characterSkillType, int level)
    {
        return level / 10 + 1;
    }

    public static int GetUpgradeCost(AttributeType characterAttributeType, int level)
    {
        return level / 10 + 5;
    }

    public static Frame GetPrefab(string key)
    {
        Frame prefab = _frameMap[key];

        return prefab;
    }

    public static Discovery GetDisovery(string key)
    {
        return _discoveryMap[key];
    }

    public static Topic GetTopic(string key)
    {
        return _topicMap[key];
    }


    private void loadTopics()
    {
        _topicMap = new Dictionary<string,Topic>();
        _commonTopics = new List<Topic>();

        XmlDocument doc = new XmlDocument();

        if (topicsFile)
        {
            doc.LoadXml(topicsFile.text);
        }
        else
        {
            return;
        }

        XmlNodeList list = doc.GetElementsByTagName("topics");

        if (list != null)
        {
            XmlNode topicsNode = list[0];

            if (topicsNode != null)
            {
                foreach (XmlNode node in topicsNode.ChildNodes)
                {
                    Topic topic = loadTopicNode(node);
                    _topicMap[topic.id] = topic;

                    if (topic.isCommon)
                    {
                        _commonTopics.Add(topic);
                    }
                }
            }
        }
    }

    private Topic loadTopicNode(XmlNode node)
    {
        Topic topic = new Topic(XmlUtil.Get(node, "id", string.Empty), XmlUtil.Get(node, "common", false));

        foreach (XmlNode childNode in node.ChildNodes)
        {
            if (childNode.Name == "label")
            {
                topic.label = childNode.InnerText;
            }

            if (childNode.Name == "questions")
            {
                foreach (XmlNode questionNode in childNode.ChildNodes)
                {
                    topic.questions.Add(questionNode.InnerText);
                }
            }

            if (childNode.Name == "default")
            {
                topic.defaultEvent = NeverdawnUtility.LoadEvent(childNode.FirstChild);
            }
        }

        return topic;
    }

    internal static Topic[] GetTopics(string[] topicIds, bool includeCommon = false)
    {
        HashSet<Topic> result = new HashSet<Topic>();

        if (includeCommon)
        {
            foreach (Topic topic in _commonTopics)
            {
                result.Add(topic);
            }
        }

        if (topicIds != null)
        {
            foreach (string id in topicIds)
            {
                result.Add(_topicMap[id]);
            }
        }

        return result.ToArray();
    }

    private void loadDiscoveries()
    {
        _discoveryMap = new Dictionary<string, Discovery>();
        _discoveryGroups = new List<DiscoveryGroup>();

        XmlDocument doc = new XmlDocument();

        if (discoveriesFile)
        {
            doc.LoadXml(discoveriesFile.text);
        }
        else
        {
            return;
        }

        XmlNodeList list = doc.GetElementsByTagName("discoveries");

        if (list != null)
        {
            XmlNode mainNode = list[0];

            if (mainNode != null)
            {
                foreach (XmlNode node in mainNode.ChildNodes)
                {
                    DiscoveryGroup discoveryGroup = new DiscoveryGroup();

                    XmlNode labelNode = node.ChildNodes[0];
                    discoveryGroup.label = labelNode.InnerText;

                    foreach (XmlNode discoveryNode in node.ChildNodes)
                    {
                        if (!discoveryNode.Name.Equals(NODE_NAME_LABEL))
                        {
                            Discovery discovery = loadDiscoveryFromNode(discoveryNode);
                            _discoveryMap.Add(discovery.id, discovery);
                        }
                    }

                    _discoveryGroups.Add(discoveryGroup);
                }
            }
        }
    }

    private Discovery loadDiscoveryFromNode(XmlNode node)
    {
        Discovery discovery = new Discovery();
        discovery.id = XmlUtil.Get(node, "id", string.Empty);
        discovery.text = node.InnerText;

        return discovery;
    }
}
