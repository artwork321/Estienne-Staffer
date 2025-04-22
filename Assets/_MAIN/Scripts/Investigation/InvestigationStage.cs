using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InvestigationStage : MonoBehaviour
{
    private int cluesFound = 0;
    [SerializeField] private int totalClues;
    public GameObject cluesGameObject;
    public List<Evidence> listSumEvidence = new List<Evidence>();

    public void IncreaseClueFound() {
        cluesFound++;
    }

    public int GetTotalClues() {
        return totalClues;
    }
    
    public int GetClueFound() {
        return cluesFound;
    }
}
