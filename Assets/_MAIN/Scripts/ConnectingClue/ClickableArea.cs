using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickableArea : MonoBehaviour
{
    public string areaName;

    public bool isSelect;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    // Highlight object when the mouse enters
    public void OnClueEnter()
    {
        if (img != null)
        {
            SetAlpha(0.7f);
        }
    }

    // Remove highlight when the mouse exits
    public void OnClueExit()
    {
        if (img != null & !isSelect)
        {
            SetAlpha(0f);
        }
    }

    // Cancel other buttons and highlight the selected button
    public void OnClueClick()
    {
        Debug.Log("Clicked on area: " + areaName);
        isSelect = true;

        // Highlight the clicked area
        if (img != null)
        {
            SetAlpha(0.7f);
        }

        // Remove highlight from the previously selected area
        GameObject attachedEvi = gameObject.transform.parent?.parent?.gameObject;
        EvidenceItem item = attachedEvi?.GetComponent<EvidenceItem>();
        ClickableArea previousSelectedArea = item?.GetCurrentSelectedArea();

        if (previousSelectedArea != null && previousSelectedArea != this)
        {
            Image prevImg = previousSelectedArea.GetComponent<Image>();
            if (prevImg != null)
            {
                prevImg.color = new Color(prevImg.color.r, prevImg.color.g, prevImg.color.b, 0f);
            }
        }

        // Update the new selected area
        item?.UpdateSelectedArea(this);
    }

    // Utility function to set alpha
    private void SetAlpha(float alpha)
    {
        Color newColor = img.color;
        newColor.a = alpha;
        img.color = newColor;
    }
}
