using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINumericBlockBar : MonoBehaviour {

    [SerializeField]
    private Color filledColor;

    [SerializeField]
    private Color emptyColor;

    [SerializeField]
    private float blockHeight;

    private int _max;

    private int _current;

    private GameObject[] fills;


    public void SetValues(int current, int max)
    {
        if (_max != max)
        {
            _max = max;
            CreateBlocks();
        }

        _current = current;

        for (int i = 0; i < _max; i++)
        {
            fills[i].SetActive(i < _current); 
        }
    }

	// Update is called once per frame
	private void CreateBlocks () {

        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        fills = new GameObject[_max];

        for (int i = 0; i < _max; i++)
        {
            fills[i] = createBlock();
        }
	}

    private GameObject createBlock()
    {
        GameObject parentObject = new GameObject("Block_Parent");
        parentObject.AddComponent<RectTransform>();

        Image image = parentObject.AddComponent<Image>();
        image.color = emptyColor;
        LayoutElement layout = parentObject.AddComponent<LayoutElement>();
        layout.preferredHeight = blockHeight;

        GameObject childObject = new GameObject("Block_Child");
        Image childImage = childObject.AddComponent<Image>();
        childImage.color = filledColor;
        LayoutElement childLayout = childObject.AddComponent<LayoutElement>();
        childLayout.ignoreLayout = true;
        RectTransform childRect = childObject.GetComponent<RectTransform>();
        childRect.anchorMin = Vector3.zero;
        childRect.anchorMax = Vector3.one;

        childObject.transform.SetParent(parentObject.transform);

        parentObject.transform.SetParent(transform, false);
        return childObject;
    }
}
