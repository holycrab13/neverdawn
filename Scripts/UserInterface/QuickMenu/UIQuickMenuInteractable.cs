using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQuickMenuInteractable : UIQuickMenuPage {

    private int index;

    private IQuickMenuInteractionCollection collection;

    private QuickMenuInteraction[] interactions;

    [SerializeField]
    private UITextButton buttonPrefab;

    [SerializeField]
    private Transform iconParent;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TMP_Text textLabel;

    [SerializeField]
    private TMP_Text textDescription;

    private bool infiniteMode;

    private AudioSource audioSource;

    private List<UITextButton> buttons;

    // Use this for initialization
    void Start()
    {
        buttons = new List<UITextButton>();
        audioSource = GetComponentInParent<AudioSource>();

        updateView(collection);
    }

    public void SetInteractionCollection(IQuickMenuInteractionCollection collection)
    {
        this.collection = collection;
    }

    private void updateView(IQuickMenuInteractionCollection collection)
    {
        interactions = collection.interactions;

        clearIcons();

        
     
        if (buttons == null)
            buttons = new List<UITextButton>();

        foreach (QuickMenuInteraction interaction in interactions)
        {
            UITextButton quickMenuIcon = createQuickMenuItem(interaction);
            quickMenuIcon.transform.SetParent(iconParent, false);
            buttons.Add(quickMenuIcon);

        }

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].neighborTop = buttons[NeverdawnUtility.RepeatIndex(i - 1, buttons.Count)];
            buttons[i].neighborBottom = buttons[NeverdawnUtility.RepeatIndex(i + 1, buttons.Count)];
        }

    

        icon.sprite = collection.icon;
        textLabel.text = collection.label;
        textDescription.text = collection.description;
    }

    protected override void OnQuickMenuPageEnabled()
    {
        if(buttons.Count > 0)
        {
            buttons[0].Select();
        }
    }

    //private void createAngles()
    //{
    //    itemSize = iconPrefab.GetComponent<RectTransform>().sizeDelta.y / 2.0f;

    //    angles = new float[infiniteModeTreshold + 1];
    //    angles[0] = angleOffset;

    //    for (int i = 1; i < infiniteModeTreshold; i++)
    //    {
    //        float scalePrev = Mathf.Clamp(1f - 0.1f * (i - 1), 0.5f, 1f);
    //        float scaleCurr = Mathf.Clamp(1f - 0.1f * i, 0.5f, 1f);

    //        float anglePrev = ((scalePrev * itemSize) / (2.0f * Mathf.PI * (radius + itemSize * scalePrev))) * 360.0f;
    //        float angleCurr = ((scaleCurr * itemSize) / (2.0f * Mathf.PI * (radius + itemSize * scaleCurr))) * 360.0f;

    //        angles[i] = angles[i - 1] + anglePrev + angleCurr + angleGap;
    //    }

    //    angles[0] -= 2;
    //}

    private UITextButton createQuickMenuItem(QuickMenuInteraction interaction)
    {
        UITextButton button = Instantiate(buttonPrefab);

        button.text = interaction.label;
        // button.SetIconSprite(interaction.GetIcon());
        //iconButton.angles = angles;
        //iconButton.radius = radius;
        //iconButton.size = itemSize;

        button.onClick.AddListener(() => interactionClicked(interaction));
        button.onSelect.AddListener(() => updateSelection(interaction));

       

        return button;
    }

   

    private void interactionClicked(QuickMenuInteraction interaction)
    {
        audioSource.PlayOneShot(interaction.sound);

        interaction.Interact(avatarController);

        menu = GetComponentInParent<UIQuickMenu>();

        switch (interaction.quickMenuBehaviour)
        {
            case QuickMenuBehaviourType.Close:
                menu.Close();
                break;
            case QuickMenuBehaviourType.NavigateUp:
                menu.GoBack();
                break;
            case QuickMenuBehaviourType.Refresh:
                updateView(collection);
                break;
        }
    }

    private void clearIcons()
    {
        foreach (Transform icon in iconParent)
        {
            Destroy(icon.gameObject);
        }

        if (buttons != null)
        {
            buttons.Clear();
        }
    }

    //private void updateIconPositions()
    //{
    //    if (!infiniteMode)
    //    {
    //        for (int i = 0; i < icons.Count; i++)
    //        {
    //            icons[i].SetPosition(Mathf.Repeat(i - index, interactions.Length));
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < icons.Count; i++)
    //        {
    //            icons[i].SetPosition(specialClamp(i - index, infiniteModeTreshold - 1));
    //        }
    //    }
    //}

    //private float specialClamp(int p1, int max)
    //{
    //    return Mathf.Repeat(p1, max);
    //}

    private void updateSelection(QuickMenuInteraction interaction)
    {
        // updateText(interaction);
    }

   
}
