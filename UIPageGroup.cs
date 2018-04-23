using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageGroup : MonoBehaviour {

    [SerializeField]
    private UIPage[] pages;

    private UIPage activePage;

    private int index;

    public void ShowPage(int index)
    {
        if (activePage != null)
        {
            activePage.Hide();
        }

        if (index >= 0 && index < pages.Length)
        {
            activePage = pages[index];
                activePage.Show();
        }
    }

    internal bool IsVisible(int p)
    {
        return activePage != null && index == p;
    }
}
