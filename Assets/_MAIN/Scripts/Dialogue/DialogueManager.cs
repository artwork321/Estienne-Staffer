using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    private ConversationManager conversationManager;
    private InkExternalFunction inkExternalFunction;

    public bool isDialoguePlaying =>  conversationManager.isRunning;  
    public bool isEndAllConversations =>  !isDialoguePlaying && conversationManager.conversationStack.isEmpty();    
    private Coroutine displayLineCoroutine;

    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] public TextArchitect architect {get; private set;}
    [SerializeField] public TMP_Text nameTag;
    [SerializeField] public TMP_Text dialogueText;
    [SerializeField] private CanvasGroup mainCanvas;

    // Control hide and show dialogue
    private Coroutine co_showing = null;
    private Coroutine co_hiding = null;

    public bool isShowing => co_showing != null;
    public bool isHiding => co_hiding != null;
    public bool isFading => isHiding || isShowing;

    public bool isVisible => co_showing != null || mainCanvas.alpha > 0;

    // Control user input
    public delegate void DialogueManagerEvent();
    public event DialogueManagerEvent onUserPrompt_Next;


    private void Awake() {
        
        if (instance != null) {
            Debug.LogWarning("Awake(): Found more than one Dialogue System");
        }
        instance = this;
        architect = new TextArchitect(dialogueText);
        conversationManager = new ConversationManager(architect);  
        inkExternalFunction = new InkExternalFunction();
    }

    public static DialogueManager GetInstance() {
        return instance;
    }

    public void OnUserPrompt_Next() {
        onUserPrompt_Next?.Invoke();
    }

    // Reading new Ink Script and enter new dialogue
    public void EnterDialogue(TextAsset inkJSON, bool useExternalFunction = false, System.Action onComplete = null) {
        Story newStory = new Story(inkJSON.text);       
        EnterDialogue(newStory, useExternalFunction, onComplete);
    }

    // Reading new Ink Script and enter new dialogue
    public void EnterDialogue(Story story, bool useExternalFunction = false,  System.Action onComplete = null) {
        conversationManager.Push(story);  
        Debug.Log(conversationManager.conversationStack.Count());

        if (useExternalFunction) {
            inkExternalFunction.Bind(story);
            inkExternalFunction.UnBind(story);
        }

        Show(); // fade in the dialogue screen

        conversationManager.StartConversation(onComplete);
    }

    // Continue newest story
    public void Continue() { 
        Debug.Log(conversationManager.conversationStack.Count());

        Show(); // fade in the dialogue screen

        conversationManager.StartConversation();
    }

    // Hide dialogue UI
    public void HideDialogueBox() {
        dialoguePanel.SetActive(false);
    }

    // Hide Dialogue
    public void HideConversation(float speed = 0.5f) {
        CharacterManager.GetInstance().HideAllCharacters(speed);
        HideDialogueBox();
    }

    public void PauseConversation(float speed = 0.5f) {
        HideConversation(speed);
        conversationManager.StopConversation();
    }

    // Show dialogue UI
    public void ShowDialogueBox() {
        dialoguePanel.SetActive(true);
    }

    public Coroutine Show() {

        ShowDialogueBox();

        if (isShowing) {
            return null;
        }
        else if (isHiding) {
            // Stop hiding before showing
            StopCoroutine(co_hiding);
            co_hiding = null;
        }

        co_showing =  StartCoroutine(Fading(1f));

        return co_showing;
    }

    public Coroutine Hide() {
        HideDialogueBox();

        if (isHiding) {
            return null;
        }
        else if (isShowing) {
            // Stop hiding before showing
            StopCoroutine(co_showing);
            co_showing = null;
        }

        co_hiding =  StartCoroutine(Fading(0f));
        AudioManager.GetInstance().StopAllTrack();
        AudioManager.GetInstance().StopAllSoundEffect();

        return co_hiding;
    }


    private IEnumerator Fading(float targetAlpha) {
        float alpha = mainCanvas.alpha;

        while(alpha != targetAlpha) {
            alpha = Mathf.MoveTowards(alpha, targetAlpha, 3f * Time.deltaTime);
            mainCanvas.alpha = alpha;

            yield return null;
        }

        co_showing = null;
        co_hiding = null;
    }

    // Set the name tag text
    public void UpdateNameTag(){
        
        string speakingCharacterName = TagManager.GetInstance().GetSpeakingCharacterName(conversationManager.currentStory.currentTags);
        Character speakingCharacter = CharacterManager.GetInstance().GetCharacter(speakingCharacterName);

        if (speakingCharacter != null){
            nameTag.text = speakingCharacter.displayName;
            nameTag.color = speakingCharacter.nameColor;
        }
        else {
            // Debug.LogWarning("UpdateNameTag(): Could not find the character with provided name tag: " + speakingCharacterName);
            nameTag.text = "";
        }
    }
    
}
