using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMenuCombatItems : UIQuickMenuPage {

    [SerializeField]
    private UISlotView slotViewPrefab;

    [SerializeField]
    private RectTransform slotViewParent;

    private UISlotView[] slotViews;

    [SerializeField]
    private UIQuickMenuPickCombatItem pickCombatItemPrefab;

    private int selectedIndex;

	// Use this for initialization
	void Start () {

        updateView();

        slotViews[selectedIndex].GetComponent<UISelectable>().Select();
	}

    private void updateView()
    {
        foreach (Transform child in slotViewParent)
        {
            Destroy(child.gameObject);
        }

        Mannequin mannequin = avatarController.character.GetComponent<Mannequin>();

        slotViews = new UISlotView[mannequin.combatItemSlots.Length];

        for (int i = 0; i < mannequin.combatItemSlots.Length; i++)
        {
            CombatItemSlot slot = mannequin.combatItemSlots[i];

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

    protected override void OnQuickMenuPageEnabled()
    {
        Mannequin mannequin = avatarController.character.GetComponent<Mannequin>();

        for (int i = 0; i < mannequin.combatItemSlots.Length; i++)
        {
            CombatItemSlot slot = mannequin.combatItemSlots[i];

            slotViews[i].component = slot.Get();
        }

        slotViews[selectedIndex].GetComponent<UISelectable>().Select();
    }

  
    private void pickItemForSlot(CombatItemSlot slot, int index)
    {
        UIQuickMenuPickCombatItem page = Instantiate(pickCombatItemPrefab);
        page.slot = slot;
        page.slotIndex = index;
        selectedIndex = index;

        menu.NavigateInto(page);
    }
}
