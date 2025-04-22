using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI; 
using Ink.Runtime;

public class Clue : MonoBehaviour
{
    public string clueName;
    [SerializeField] private TextAsset dialogueScript;
    public bool hasFound = false;
    private InvestigationManager investigationManager = null;

    public void Awake() {
        investigationManager = InvestigationManager.GetInstance();
    }

    public void OnClueClick(){
        
        if (DialogueManager.GetInstance().isDialoguePlaying)
            return;

        Story storyScript = new Story(dialogueScript.text);

        if (storyScript.variablesState["objectName"] != null) {
            storyScript.variablesState["objectName"] = clueName;
            Debug.Log($"Set objectName to: {storyScript.variablesState["objectName"]}");

        }        
        
        investigationManager.OnClueFound(this);
        GameManager.instance.SwitchToDialogueMode(storyScript); // Run script attached to the clue
    }

}
