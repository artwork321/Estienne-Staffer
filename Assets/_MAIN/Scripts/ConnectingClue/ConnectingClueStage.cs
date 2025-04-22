using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class ConnectingClueStage : MonoBehaviour
{
    // correct evidence id
    public string firstEvidence;
    public string secondEvidence;
    public string firstEvidenceCorrectSelectedArea; // null
    public string secondEvidenceCorrectSelectedArea;

    // Selected Evidence Slot
    public Transform firstSelectedEvidence;
    public Transform secondSelectedEvidence;

    public Transform evidenceListContent;
    public GameObject evidenceSlotPrefab; 

    public Image checkAnswer;
    private Sprite correctAnswer;
    private Sprite falseAnswer;

    public void Awake() {
        correctAnswer = Resources.Load<Sprite>("Graphics/Other/correct");
        falseAnswer = Resources.Load<Sprite>("Graphics/Other/incorrect");
    }

    // Initialize EvidenceList
    public void ShowEvidenceList() {
        
        // Create evidence gameObject from prefab
        foreach (var item in InventoryManager.GetInstance().evidenceList) {
            GameObject obj = Instantiate(evidenceSlotPrefab, evidenceListContent) as GameObject ;

            Image eviIcon = obj.transform.Find("EvidenceImage").GetComponent<Image>();
            eviIcon.sprite = item.icon;

            // Add event for click button
            Button eviButton = obj.GetComponent<Button>();
            eviButton.onClick.RemoveAllListeners();
            eviButton.onClick.AddListener(() => SelectEvidence(item));
            
        }
    }

    public void CheckEvidence() {
        StartCoroutine(CheckSelectedEvidence());
    }

    public IEnumerator CheckSelectedEvidence() {
        bool isCorrect = false;
        
        // Check if second evidence is needed
        if (secondEvidence == null || secondEvidence == "") {
            Debug.Log("One Evidence Case");

            // One evidence case
            if (firstSelectedEvidence.childCount > 0) {
                EvidenceItem firstSelectedEvidenceObject = firstSelectedEvidence.GetChild(0).GetComponent<EvidenceItem>();
                string firstSelectedEvidenceName = firstSelectedEvidenceObject.evidenceData.evidenceName;

                if (firstSelectedEvidenceName == firstEvidence && 
                    firstSelectedEvidenceObject.IsSelectedAreaCorrect(firstEvidenceCorrectSelectedArea)) {
                    isCorrect = true;
                }
            }

            
 
        }
        else {
            Debug.Log("Two Evidence Case");

            // Two evidence case
            // Check if both slots are occupied
            if (firstSelectedEvidence.childCount > 0 && secondSelectedEvidence.childCount > 0) {

                // Get EvidenceItem components for selected evidences
                EvidenceItem firstSelectedEvidenceObject = firstSelectedEvidence.GetChild(0).GetComponent<EvidenceItem>();
                EvidenceItem secondSelectedEvidenceObject = secondSelectedEvidence.GetChild(0).GetComponent<EvidenceItem>();

                string firstSelectedEvidenceName = firstSelectedEvidenceObject.evidenceData.evidenceName;
                string secondSelectedEvidenceName = secondSelectedEvidenceObject.evidenceData.evidenceName;

                if (firstSelectedEvidenceName == firstEvidence && secondSelectedEvidenceName == secondEvidence) {
                    if (firstSelectedEvidenceObject.IsSelectedAreaCorrect(firstEvidenceCorrectSelectedArea)
                    && secondSelectedEvidenceObject.IsSelectedAreaCorrect(secondEvidenceCorrectSelectedArea)) {
                        isCorrect = true;
                    }
                }
                else if (firstSelectedEvidenceName == secondEvidence && secondSelectedEvidenceName == firstEvidence) {
                    if (secondSelectedEvidenceObject.IsSelectedAreaCorrect(firstEvidenceCorrectSelectedArea) 
                    && firstSelectedEvidenceObject.IsSelectedAreaCorrect(secondEvidenceCorrectSelectedArea)) {
                        
                        isCorrect = true;
                    }
                }
            }
        }

        // Update answer UI based on correctness
        if (isCorrect) {
            checkAnswer.sprite = correctAnswer;
            checkAnswer.color = new Color(checkAnswer.color.r, checkAnswer.color.g, checkAnswer.color.b, 1f);
            yield return new WaitForSeconds(0.5f);

            GameManager.instance.SwitchToDialogueMode();  // Switch to dialogue mode if correct
        }
        else {
            checkAnswer.sprite = falseAnswer;
            checkAnswer.color = new Color(checkAnswer.color.r, checkAnswer.color.g, checkAnswer.color.b, 1f);
        }
    }


    // View Evidence on available slot
    public void SelectEvidence(Evidence evidence) {

        // Check for available slot
        GameObject evidencePrefab = evidence.GetEvidencePrefab();

        if (firstSelectedEvidence.childCount == 0) {
            // First Evidence for the left slot
            GameObject obj = Instantiate(evidencePrefab, firstSelectedEvidence) as GameObject;
        }
        else if (secondSelectedEvidence.gameObject.activeSelf & secondSelectedEvidence.childCount == 0) {
            // First Evidence for the right slot
            GameObject obj = Instantiate(evidencePrefab, secondSelectedEvidence) as GameObject;
        }
        else if (!secondSelectedEvidence.gameObject.activeSelf) {
            // One evidence Case
            DestroyAllChildren(firstSelectedEvidence);
            GameObject obj = Instantiate(evidencePrefab, firstSelectedEvidence) as GameObject;
        }
        else {
            // Two Evidence case
            // Move the second evidence to the first slot
            DestroyAllChildren(firstSelectedEvidence);
            secondSelectedEvidence.GetChild(0).SetParent(firstSelectedEvidence, false);
            firstSelectedEvidence.GetChild(0).GetComponent<RectTransform>().anchoredPosition  = Vector2.zero;

            // Insert new evidence to the second slot
            DestroyAllChildren(secondSelectedEvidence);
            GameObject obj2 = Instantiate(evidencePrefab, secondSelectedEvidence) as GameObject;
        }
    }


    private void DestroyAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

}
