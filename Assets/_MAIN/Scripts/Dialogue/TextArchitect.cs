using System.Collections;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class TextArchitect
{
    // Public properties for managing the TMP_Text component
    public TMP_Text tmpro;
    private Story currentStory;

    // Private fields for storing the text and status of the typing process
    private string targetText = "";
    private string preText = ""; // Text already rendered
    
    private Coroutine buildProcess = null;

    // Speed control for the typewriter effect
    private const float BaseSpeed = 1f;
    private float speedMultiplier = 1f;
    private int characterMultiplier = 1;

    // Property to check if the text is currently being built
    public bool isBuilding => buildProcess != null;

    // Property to control text speed, factoring in the multiplier
    public float Speed
    {
        get => BaseSpeed * speedMultiplier;
        set => speedMultiplier = value;
    }

    // Constructor that accepts a TMP_Text reference to control
    public TextArchitect(TMP_Text tmpro)
    {
        this.tmpro = tmpro;
    }

    // Property to calculate characters per cycle based on speed
    private int CharactersPerCycle
    {
        get
        {
            if (speedMultiplier <= 2f) return characterMultiplier;
            if (speedMultiplier <= 2.5f) return characterMultiplier * 2;
            return characterMultiplier * 3;
        }
    }

    // Start the typing effect for a given text
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;
        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    // The main coroutine that handles the typing animation
    private IEnumerator Building()
    {
        PrepareText();
        yield return TypeTextWithDelay();
        OnComplete();
    }

    // Prepares the TMP_Text component for the typing animation
    private void PrepareText()
    {
        tmpro.color = tmpro.color; // Ensure color is set (although redundant here)
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (!string.IsNullOrEmpty(preText))
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    // Typing animation using a typewriter effect, controlled by speed
    private IEnumerator TypeTextWithDelay()
    {   
        if (tmpro.text.Trim() != "COMMAND:" && !string.IsNullOrWhiteSpace(tmpro.text.Trim())) {
            AudioManager.GetInstance().PlaySoundEffect("Audio/SFX/Retro_single", loop: true);

            while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
            {
                tmpro.maxVisibleCharacters += CharactersPerCycle;
                yield return new WaitForSeconds(0.015f / Speed);
            }

            // Stop talking effect when the line finished
            AudioManager.GetInstance().StopSoundEffect("Retro_single");
        }

        // Display choices after the current line finished
        if (ChoiceManager.GetInstance() != null) {
            ChoiceManager.GetInstance().Show(currentStory);                    
        }
        
    }

    // Stop the typing process if it's currently running
    public void Stop()
    {
        if (!isBuilding) return;

        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    private void OnComplete() {
        buildProcess = null;
    }

    // Force completion of the typing process, immediately showing all text and choices
    public void ForceComplete()
    {
        Stop();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        AudioManager.GetInstance().StopSoundEffect("Retro_single");
        if (ChoiceManager.GetInstance() != null) ChoiceManager.GetInstance().Show(currentStory);
    }

    public void setStory(Story story) {
        this.currentStory = story;
    }
}
