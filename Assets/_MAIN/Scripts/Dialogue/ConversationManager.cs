using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ConversationManager
{

    public Story currentStory;

    public ConversationStack conversationStack;

    public TextArchitect architect;   

    public bool isRunning => process != null;

    private Coroutine process = null;

    private bool userPrompt = false;

    public ConversationManager(TextArchitect architect) {

        this.architect = architect;
        this.conversationStack = new ConversationStack();
        DialogueManager.GetInstance().onUserPrompt_Next += OnUserPrompt_Next;
        
    }

    public Story Pop() {
        return conversationStack.Pop();
    }

    public void Push(Story story) {
        conversationStack.Push(story); 
    }

    private void OnUserPrompt_Next() {
        // Prevent story continuation if we're waiting on user choice
        if (ChoiceManager.GetInstance() != null && ChoiceManager.GetInstance().isWaitingOnUserChoice) {
            Debug.Log("waiting for a choice");
            return;
        }
        
        userPrompt = true;
    }

    public void StartConversation(System.Action onComplete = null) {
        currentStory  =  conversationStack.tail;
        architect.setStory(currentStory);
        StopConversation();

        // Save current story then run new dialogue
        process = DialogueManager.GetInstance().StartCoroutine(RunningConversation(onComplete));
    }


    public void StopConversation() {
        if (!isRunning) return;

        DialogueManager.GetInstance().StopCoroutine(process);
        process = null;
        Debug.Log(isRunning);
    }

    private IEnumerator RunningConversation(System.Action onComplete = null) {

        while(currentStory.canContinue) {
            yield return ContinueStory();    
        }

        // End the process of current story if it finishes
        if (!currentStory.canContinue) {
            DialogueManager.GetInstance().HideConversation();
            conversationStack.Pop();
            process = null;
            
            // Invoke the callback if provided
            onComplete?.Invoke();
        }
    }


    // Continue next line of Ink Script
    public IEnumerator ContinueStory() {       
        string line = currentStory.Continue();

        // Execuate all command lines firsts
        while (line.Trim() == "COMMAND:") {
            TagManager.GetInstance().HandleTags(currentStory.currentTags);   
            line =  currentStory.Continue();
        }

        TagManager.GetInstance().HandleTags(currentStory.currentTags);  

        if (line.Trim() != "COMMANDNONEXTLINE:" && !string.IsNullOrWhiteSpace(line.Trim())){
            // Update name tag
            DialogueManager.GetInstance().UpdateNameTag();

            // Show dialogue box
            DialogueManager.GetInstance().ShowDialogueBox();

            // Run dialogue            
            yield return BuildDialogue(line);
        }
        // wait for user input
        yield  return WaitForUserInput();
    }

    private IEnumerator BuildDialogue(string line) {
        architect.Build(line);    

        while(architect.isBuilding) {
            if (userPrompt) {
                architect.ForceComplete();
                userPrompt = false;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForUserInput() {  
 
        while(!userPrompt & isRunning) {
            yield return null;
        }

        userPrompt = false;
    }

}

