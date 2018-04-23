using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextReveal : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float RevealSpeed = 1.0f;

    private TMP_Text m_textMeshPro;

    void Awake()
    {
        // Get Reference to TextMeshPro Component
        m_textMeshPro = GetComponent<TMP_Text>();
        m_textMeshPro.enableWordWrapping = true;
    }


    public void Reveal(string text)
    {
        StartCoroutine(ShowText(text));
    }


    IEnumerator ShowText(string label)
    {
        m_textMeshPro.text = label;

        // Force and update of the mesh to get valid information.
        m_textMeshPro.ForceMeshUpdate();

        int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int visibleCount = 0;

        while (visibleCount < totalVisibleCharacters)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            counter += 1;

            yield return new WaitForSeconds(0.10f / RevealSpeed);
        }
    }

}
