using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMenuEquipment : UIQuickMenuPage {

    [SerializeField]
    private UISlotView slotViewPrefab;

    [SerializeField]
    private RectTransform slotViewParent;

    private UISlotView[] slotViews;

    [SerializeField]
    private UIQuickMenuPickArmor pickArmorPrefab;

    private int selectedIndex;


	// Use this for initialization
	void Start () {

        Mannequin mannequin = avatarController.character.GetComponent<Mannequin>();

        if (mannequin != null)
        {
            foreach (Transform child in slotViewParent)
            {
                Destroy(child.gameObject);
            }

            slotViews = new UISlotView[mannequin.armorSlots.Length];

            for(int i = 0; i < slotViews.Length; i++)
            {
                ArmorSlot slot = mannequin.armorSlots[i];
                int k = i;

                slotViews[i] = Instantiate(slotViewPrefab);
                slotViews[i].component = slot.Get();
                slotViews[i].slotName = slot.slotName;
                slotViews[i].defaultLabel = "None";
                slotViews[i].GetComponent<UIButton>().onClick.AddListener(() => pickItemForSlot(slot, k));

                slotViews[i].transform.SetParent(slotViewParent, false);
            }

            for (int i = 0; i < slotViews.Length; i++)
            {
                int prev = NeverdawnUtility.RepeatIndex(i - 1, slotViews.Length);
                int next = NeverdawnUtility.RepeatIndex(i + 1, slotViews.Length);

                slotViews[i].selectable.neighborBottom = slotViews[next].selectable;
                slotViews[i].selectable.neighborTop = slotViews[prev].selectable;
            }
        }
	}

    protected override void OnQuickMenuPageEnabled()
    {
        Mannequin mannequin = avatarController.character.GetComponent<Mannequin>();

        for (int i = 0; i < mannequin.armorSlots.Length; i++)
        {
            slotViews[i].component = mannequin.armorSlots[i].Get();
        }

        slotViews[selectedIndex].GetComponent<UISelectable>().Select();
    }

    private void pickItemForSlot(ArmorSlot slot, int i)
    {
        UIQuickMenuPickArmor page = Instantiate(pickArmorPrefab);
        page.slot = slot;
        selectedIndex = i;

        menu.NavigateInto(page);
    }
}
