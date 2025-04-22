using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    public Evidence evidenceData; // Reference to ScriptableObject
    private ClickableArea[] clickableAreaList;
    private ClickableArea selectedArea; // Might be null

    public void Awake() {
        clickableAreaList = gameObject.transform.Find("ClickableArea").GetComponentsInChildren<ClickableArea>();
    }

    public ClickableArea GetCurrentSelectedArea()
    {
        return selectedArea;
    }

    public void UpdateSelectedArea(ClickableArea newSelectedArea)
    {
        selectedArea = newSelectedArea;

        foreach(ClickableArea area in clickableAreaList) {
            if (area != selectedArea) {
                area.isSelect = false;
            }
        }
    }

    public bool IsSelectedAreaCorrect(string correctSelectedArea)
    {
        return selectedArea != null && selectedArea.areaName == correctSelectedArea;
    }

    public string GetSelectedAreaName() {
        return selectedArea.areaName;
    }
}
