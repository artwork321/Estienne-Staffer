using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public List<Evidence> evidenceList = new List<Evidence>();

    public Transform inventoryContent;
    public GameObject evidenceSlotPrefab; //contain Evidence prefab
    
    private void Awake() {
       if (instance != null) {
            Debug.LogWarning("Awake(): Found more than one Inventory System");
        }
        instance = this;
    }

    public static InventoryManager GetInstance()  {
        return instance;
    } 

    public void Add(Evidence newEvidence) {
        evidenceList.Add(newEvidence);
    }
    
    public void Remove(Evidence evidence) {
        evidenceList.Remove(evidence);
    }

    public void showInventory() {   

        // Clean old display content
        cleanInventory();

        // Create evidence gameObject from prefab
        foreach (var item in evidenceList) {
            GameObject obj = Instantiate(evidenceSlotPrefab, inventoryContent) as GameObject ;

            TMP_Text eviName = obj.transform.Find("EvidenceName").GetComponent<TMP_Text>();
            Image eviIcon = obj.transform.Find("EvidenceImage").GetComponent<Image>();

            eviIcon.sprite = item.icon;
            eviName.text = item.evidenceName;

            // Add event for click button
            Button eviButton = obj.GetComponent<Button>();
            eviButton.onClick.RemoveAllListeners();
            eviButton.onClick.AddListener(() => item.DisplayEvidence());
            
        }
    }

    // Clear all game object of evidence in inventory
    public void cleanInventory() {
        // Clean inventory
        foreach (Transform item  in inventoryContent) {
            Destroy(item.gameObject);
        }
    }
}
