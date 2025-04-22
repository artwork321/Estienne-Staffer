using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;   

public class Case : MonoBehaviour
{
    public string caseName;

    public string id;

    public GameObject[] investigationPrefab;

    public GameObject[] connectingCluePrefab;

    [SerializeField] private TextAsset mainStory;

    public CharacterSprite[] characterSprite; // TODO: create a class for character info

    public CharacterText[] characterText;

    public List<Evidence> listInitEvidence = new List<Evidence>();
    

    public IEnumerator Running() {
        // Initialize characters
        foreach (var character in characterSprite) {
            CharacterManager.GetInstance().CreateCharacterSprite(character.charName, character.nameColor, character.position);
        }
        
        foreach (var character in characterText) {
            CharacterManager.GetInstance().CreateCharacterText(character.charName);
        }
        
        // Initialize background panel
        GraphicPanel panel = GraphicPanelManager.GetInstance().GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0, true);

        // Initialize evidence
        foreach (Evidence evidence in listInitEvidence) {
                InventoryManager.GetInstance().Add(evidence);
        }
        
        // Enter dialogue
        DialogueManager.GetInstance().EnterDialogue(mainStory, true);

        while(DialogueManager.GetInstance().isEndAllConversations == false) {
            yield return null;
        }

        // End Game
        GameManager.instance.EndingCurrentCase();

        yield return null;
    }
}
