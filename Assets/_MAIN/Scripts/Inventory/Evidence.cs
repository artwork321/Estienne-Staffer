using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Evidence", menuName = "Evidence/Create new Evidence")]

public class Evidence : ScriptableObject
{   
    public string evidenceName;
    public Sprite icon;
    private GameObject evidencePrefab; // Prefab used for another mode

    private void OnEnable()
    {
        evidencePrefab = Resources.Load<GameObject>("Prefab/Evidence/" + evidenceName);
    }

    public GameObject GetEvidencePrefab() {
        return evidencePrefab;
    }

    public void DisplayEvidence() {
        GraphicPanel panel = GraphicPanelManager.GetInstance().GetPanel("Foreground");
       
        // Get GameObject DisplayEvidence
        Transform displayEvidenceObj = panel.rootPanel.transform.Find("DisplayEvidence");
        displayEvidenceObj.gameObject.SetActive(true);

        // Get Image DisplayArea
        Image currentEvidenceIcon = displayEvidenceObj.Find("DisplayArea").GetComponent<Image>();

        // Change Image to icon
        currentEvidenceIcon.sprite = icon;
    }

}
