using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;

    [SerializeField] private List<Character> characterList = new List<Character>();
    [SerializeField] private GameObject characterPanel;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Awake(): Found more than one CharacterManager");
        }

        instance = this;
    }

    public static CharacterManager GetInstance() {
        return instance;
    }


    public Character GetCharacter(string charName) {
       
        foreach (Character character in characterList) {
            if (character.charName == charName) {
                return character;
            }
        }

        return null;
    }


    public void CreateCharacterSprite(string charName, Color nameColor, Vector2 position) {
        if (GetCharacter(charName) != null) {
            Debug.LogWarning("CreateCharacter(): Character name already exists: " + charName);
        }

        // Create character with given attributes
        GameObject newCharacterPanel = CreateCharacterObject(charName);
        Character newCharacter = new CharacterSprite(newCharacterPanel, charName, nameColor, position);
        
        characterList.Add(newCharacter);
    }
    
 
    public void CreateCharacterText(string charName) {
        if (GetCharacter(charName) != null) {
            Debug.LogWarning("CreateCharacter(): Character name already exists: " + charName);
        }

        // Create character with given attributes
        GameObject newCharacterPanel = CreateCharacterObject(charName);
        Character newCharacter = new CharacterText(newCharacterPanel, charName);
        

        characterList.Add(newCharacter);
    }


    public GameObject CreateCharacterObject(string charName, bool isSprite = false) {

        Transform existingChar = characterPanel.transform.Find(charName);

        if (existingChar == null) {
            // Create new Object that contains the image of the character
            GameObject characterObject = new GameObject(charName);
            RectTransform rect = characterObject.AddComponent<RectTransform>();
            characterObject.transform.SetParent(characterPanel.transform, false);

            rect.localScale = Vector3.one;
            rect.localPosition = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.one;

            return characterObject;
        }
        else {
            Debug.LogWarning("Already created this character object");
            return null;
        }
    }

    // Hide All Characters
    public void HideAllCharacters(float speed = 0.5f) {
        foreach (Character character in characterList) {
            if (character is CharacterSprite characterSprite && characterSprite.isInvisible) {
                characterSprite.FadingOut(speed);
            }
        }
    }

    // Destroy All Character Objects
    public IEnumerator DestroyAllCharacters() {
        foreach (Character character in characterList) {
            if (character.currentGraphicComponent != null) {
                CharacterSprite characterSprite = character as CharacterSprite;

                if (characterSprite != null) {
                    // Wait until the character is no longer processing
                    while (!characterSprite.isDoneRunningChar) {
                        yield return null;
                    }
                }

                Object.Destroy(character.currentGraphicComponent.transform.parent.gameObject);
            }
            else {
                Debug.Log("currentGraphicComponent is null for character: " + character.charName);

                GameObject characterObject = characterPanel.transform.Find(character.charName)?.gameObject;

                if (characterObject != null) {
                    Object.Destroy(characterObject);
                } else {
                    Debug.LogWarning("GameObject with name " + character.charName + " not found under characterPanel.");
                }
            }
            yield return null;
        }

        characterList.Clear();
    }

}
