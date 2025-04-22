using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class InvestigationManager : MonoBehaviour
{ 
    [SerializeField] private Image textMode;
    public GameObject investigationRoot;
    private GameObject currentInvestigationInstance;
    private InvestigationStage currentInvestigationStage;

    private static InvestigationManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("Awake(): Found more than one investigation Manager");
            Destroy(gameObject);  // Ensures only one instance exists
        }
    }

    public static InvestigationManager GetInstance()  {
        return instance;
    }

    public void StartInvestigationStage(int id) {
        currentInvestigationInstance = Instantiate(GameManager.instance.currentCase.investigationPrefab[id], 
                                    Vector3.zero, Quaternion.identity, investigationRoot.transform) as GameObject;

        currentInvestigationStage = currentInvestigationInstance.GetComponent<InvestigationStage>();
    }
    
    public GameObject getCurrentInvestigationInstance() {
        return currentInvestigationInstance;
    }
    
    public InvestigationStage GetCurrentInvestigationStage() {
        return currentInvestigationStage;
    }

    public void DestroyCurrentInvestigationInstance() {
        Destroy(currentInvestigationInstance);
    }

    public void OnClueFound(Clue clue)
    {   
        if (!clue.hasFound) {
            clue.hasFound = true;
            currentInvestigationStage.IncreaseClueFound();
            Debug.Log("Clue found!");
        }
    }

    public void IfFoundAll() {
        if (currentInvestigationStage.GetClueFound() >= currentInvestigationStage.GetTotalClues()){

            Debug.Log("All clues found. Returning to Visual Novel Mode.");

            // Update inventory with new evidence
            foreach (Evidence sumEvidence in currentInvestigationStage.listSumEvidence) {
                InventoryManager.GetInstance().Add(sumEvidence);
            }
            
            GameManager.instance.SwitchToDialogueMode(); // Continue Story after all clues have been found
        }
        else {
            GameManager.instance.EnterInvestigationMode();
        }
    }

    // Display a large title to indicate entering investigation mode
    public IEnumerator EnterInvestigationText() { 

        Vector2 currentTextPosition = textMode.GetComponent<RectTransform>().anchoredPosition; 
        
        float y = currentTextPosition.y;
    
        // Move the character to the target position
        while (y < 0f)
        {   
            y = Mathf.MoveTowards(y, 0f, 1280f * 1f * Time.deltaTime);
            
            Vector2 newPosition = new Vector2(currentTextPosition.x, y);
            
            textMode.GetComponent<RectTransform>().anchoredPosition = newPosition;

            yield return null;
        }
        textMode.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        yield return InvestigationTextDisappear();
    }

    public IEnumerator InvestigationTextDisappear() { // todo: Text fore mode
        float currentHeight = textMode.GetComponent<RectTransform>().sizeDelta.y;
    
        // Move the character to the target position
        while (currentHeight > 0f)
        {   
            currentHeight = Mathf.MoveTowards(currentHeight, 0f, 1280f * 0.1f * Time.deltaTime);
            
            textMode.GetComponent<RectTransform>().sizeDelta = new Vector2(textMode.GetComponent<RectTransform>().sizeDelta.x, currentHeight);

            yield return null;
        }

        // Return it to original state
        textMode.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -433f);
        textMode.GetComponent<RectTransform>().sizeDelta = new Vector2(textMode.GetComponent<RectTransform>().sizeDelta.x, 144.47f);        
    }
}
