using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private InputManager inputManager; // Might update when fix input manager

    private int currentState = 1;

    public Case currentCase;

    public Camera vnCamera;
    public Camera invCamera;
    public Camera connectCamera;

    public Canvas investigateCanvas; 
    public Canvas mainCanvas;
    public Canvas connectCanvas;

    private void Awake() {

        if (instance == null) {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            instance =  this;
        }
        else {
            DestroyImmediate(gameObject);
            return;
        }
    }

    // TO-DO: run different cases
    public void StartCase() {
        StartCoroutine(currentCase.Running());
    } 

    public void EnterInvestigationMode(int id = -1) {
        StartCoroutine(SwitchToInvestigationMode(id));
    }

    public void EnterConnectingClueMode(int id = -1) {
        StartCoroutine(SwitchToConnectingClueMode(id));
    }
    

    // Switch to investigation mode
    public IEnumerator SwitchToInvestigationMode(int id = -1) {
        
        // 1. Disable VN input. - TODO: CLICCK TOO FAST ENABLE NEXT LINE??????????
        inputManager.DisableVisualNovelInput();
        Debug.Log("CANT PRESS ANYMORE");

        // 2. Stop script player.
        DialogueManager.GetInstance().PauseConversation(1f);
        yield return new WaitForSeconds(0.6f);

        if (id != -1) {
            InvestigationManager.GetInstance().StartInvestigationStage(id);
        }
        
        // 4. Switch cameras.        
        vnCamera.enabled = false;
        AudioListener vnAudioListener = vnCamera.gameObject.GetComponent<AudioListener>();
        vnAudioListener.enabled = false;
        var graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = false;

        invCamera.enabled = true;
        AudioListener invAudioListener = invCamera.gameObject.GetComponent<AudioListener>();
        invAudioListener.enabled = true;
        graphicRaycaster = investigateCanvas.gameObject.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = true;   

        // 5. Enable Investigation Prefab
        Debug.Log("Activate Investigation Prefab");
        if (id != -1) {
            // Intro text effect
            yield return InvestigationManager.GetInstance().EnterInvestigationText();
            
        }

        InvestigationManager.GetInstance().GetCurrentInvestigationStage().cluesGameObject.SetActive(true);
        currentState = 2;
    }

    // Switch to connecting clue mode
    public IEnumerator SwitchToConnectingClueMode(int id = -1) {
        
        // 1. Disable VN input
        inputManager.DisableVisualNovelInput();
        Debug.Log("CANT PRESS ANYMORE");

        // 2. Stop script player.
        DialogueManager.GetInstance().PauseConversation(1f);
        DialogueManager.GetInstance().Hide();
        yield return new WaitForSeconds(1f);

        if (id != -1) {
            ConnectingClueManager.GetInstance().StartConnectingClueStage(id);
        }
        
        // 4. Switch cameras. TODO: Smoonth transition
        vnCamera.enabled = false;
        AudioListener vnAudioListener = vnCamera.gameObject.GetComponent<AudioListener>();
        vnAudioListener.enabled = false;
        var graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = false;

        connectCamera.enabled = true;
        AudioListener connectAudioListener = connectCamera.gameObject.GetComponent<AudioListener>();
        connectAudioListener.enabled = true;
        graphicRaycaster = connectCanvas.gameObject.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = true;   

        // 5. Enable Connecting Clue Prefab
        Debug.Log("Activate Clue Mode Prefab");
        if (id != -1) {
            AudioManager.GetInstance().StopAllTrack();
            AudioManager.GetInstance().PlayTrack("Audio/Music/Upbeat");
            ConnectingClueManager.GetInstance().GetCurrentConnectingClueStage().ShowEvidenceList();
        }
        currentState = 3;
    }

    // Switch back to dialogue mode
    public void SwitchToDialogueMode(Story story = null)
    {
        // 1. Switch cameras.
        vnCamera.enabled = true;
        AudioListener vnAudioListener = vnCamera.gameObject.GetComponent<AudioListener>();
        vnAudioListener.enabled = true;
        var graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = true;

        invCamera.enabled = false;
        AudioListener invAudioListener = invCamera.gameObject.GetComponent<AudioListener>();
        invAudioListener.enabled = false;
        graphicRaycaster = investigateCanvas.gameObject.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = false; 

        connectCamera.enabled = false;
        AudioListener connectAudioListener = connectCamera.gameObject.GetComponent<AudioListener>();
        connectAudioListener.enabled = false;
        graphicRaycaster = connectCanvas.gameObject.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = false;  

        // 2. Load and play new script or continue old script        
        if (story != null & currentState == 2) {
            InvestigationManager investigationMode = InvestigationManager.GetInstance();

            if (investigationMode == null) {
                Debug.LogError("Could not find investigation mode");
            }
            DialogueManager.GetInstance().EnterDialogue(story, false, () =>{
                                                            investigationMode.IfFoundAll();
                                                            });

        }
        else {
            InvestigationManager.GetInstance().DestroyCurrentInvestigationInstance();
            ConnectingClueManager.GetInstance().DestroyCurrentConnectingClueInstance();

            DialogueManager.GetInstance().Continue();
        }

        // 3. Enable Visual Novel input.
        inputManager.EnableVisualNovelInput();
        currentState = 1;
    }

    public IEnumerator EndCase() {
        AudioManager.GetInstance().StopAllTrack();
        AudioManager.GetInstance().StopAllSoundEffect();
        GraphicPanelManager.GetInstance().Clear();

        yield return StartCoroutine(CharacterManager.GetInstance().DestroyAllCharacters());

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("EndScene");
    }

    public void EndingCurrentCase() {
        StartCoroutine(EndCase());
    }
}






