using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class ChoiceManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public static ChoiceManager instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    public bool isWaitingOnUserChoice { get; private set; }


    private void Awake() {

        if (instance != null) {
            Debug.LogWarning("Found more than one ChoiceManager");
        }

        instance = this;
    }

    void Start() {
        Hide();
        isWaitingOnUserChoice = false;

        // get all of the choices text
        choicesText  = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public static ChoiceManager GetInstance() {
        return instance;
    }

    public void Show(Story currentStory) {

        if (currentStory && currentStory.currentChoices.Count > 0) {
            canvasGroup.alpha = 1f;  // visible
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isWaitingOnUserChoice = true;
            GenerateChoices(currentStory);
        }
    }

    public void Hide() {
        canvasGroup.alpha = 0f;  // Invisible
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void MakeChoice(Story currentStory, int choiceIndex) {
        if (choiceIndex < 0 || choiceIndex > choices.Length-1) {
                return;
        }

        Debug.Log("Make choice: " + choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        isWaitingOnUserChoice = false;  // No longer waiting for a choice
        Hide();              
    }


    public void GenerateChoices(Story currentStory) {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length) {
            Debug.LogError("Too many choices were given");
        }

        // Update buttons
        int index = 0;
        foreach (Choice choice in currentChoices) {

            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;

            // Get the Button component from the GameObject
            Button button = choices[index].GetComponent<Button>();

            // Remove existing listeners and add a new listener
            button.onClick.RemoveAllListeners();
            int buttonIndex = index;
            
            button.onClick.AddListener(() => {
                Debug.Log("Button Clicked: " + buttonIndex);  // Log the click
                MakeChoice(currentStory, buttonIndex);
            });

            index++;
        }

        // Hide remaining buttons
        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
    }

    
}
