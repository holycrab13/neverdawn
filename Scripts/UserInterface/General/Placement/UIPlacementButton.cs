using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlacementButton : MonoBehaviour {

    [SerializeField]
    private UIPlacementSelector selectorPrefab;

    [SerializeField]
    private RectTransform selectorParent;

    [SerializeField]
    private Image iconImage;

    [HideInInspector]
    public List<AvatarController> controllers;

    [HideInInspector]
    public int index;

    public Character character { get; set; }

    public Sprite icon { get; set; }

    private Dictionary<AvatarController, UIPlacementSelector> selectors;

	void Start()
    {
        iconImage.sprite = icon;

        selectors = new Dictionary<AvatarController, UIPlacementSelector>();

        foreach (Transform child in selectorParent)
        {
            Destroy(child.gameObject);
        }

        foreach (AvatarController controller in controllers)
        {
            UIPlacementSelector selector = Instantiate(selectorPrefab);
            selector.color = controller.color;
            selector.transform.SetParent(selectorParent, false);
            selector.gameObject.SetActive(false);

            selectors.Add(controller, selector);
        }
    }

    void Update()
    {
        foreach (AvatarController controller in controllers)
        {
            selectors[controller].gameObject.SetActive(controller.placementSelectionIndex == index);
        }
    }


}
