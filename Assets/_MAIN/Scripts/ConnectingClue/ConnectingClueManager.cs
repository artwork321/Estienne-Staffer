using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class ConnectingClueManager : MonoBehaviour
{
    public GameObject connectingClueRoot;
    private GameObject currentConnectingClueInstance;
    private ConnectingClueStage currentConnectingClueStage;

    private static ConnectingClueManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("Awake(): Found more than one ConnectingClueManager");
            Destroy(gameObject);  // Ensures only one instance exists
        }
    }

    public static ConnectingClueManager GetInstance()  {
        return instance;
    }

    public void StartConnectingClueStage(int id) {
        currentConnectingClueInstance = Instantiate(GameManager.instance.currentCase.connectingCluePrefab[id], 
                                    Vector3.zero, Quaternion.identity, connectingClueRoot.transform) as GameObject;

        currentConnectingClueStage = currentConnectingClueInstance.GetComponent<ConnectingClueStage>();
    }
    
    public GameObject GetCurrentConnectingClueInstance() {
        return currentConnectingClueInstance;
    }
    
    public ConnectingClueStage GetCurrentConnectingClueStage() {
        return currentConnectingClueStage;
    }

    public void DestroyCurrentConnectingClueInstance() {
        Destroy(currentConnectingClueInstance);
    }

}
