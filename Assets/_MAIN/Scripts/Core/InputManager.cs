using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Button pressedArea;

    private bool isVNInputEnabled = true;  // Flag to control input

    void Update()
    {   
        if (pressedArea.interactable && isVNInputEnabled && (Input.GetKeyDown(KeyCode.Space))) 
        {
            LineAdvance();
        }   
    }   

    public void LineAdvance() {
        StartCoroutine(LineAdvanceCooldown());
    }

    public IEnumerator LineAdvanceCooldown() {
        pressedArea.interactable = false;
        DialogueManager.GetInstance().OnUserPrompt_Next();
        yield return new WaitForSeconds(0.1f);
        pressedArea.interactable = true;
    }

    // Disable both UI and key inputs
    public void DisableVisualNovelInput() {
        isVNInputEnabled = false; // Prevents key input
        pressedArea.gameObject.SetActive(false); // Disables UI input
    }

    // Enable both UI and key inputs
    public void EnableVisualNovelInput() {
        isVNInputEnabled = true; 
        pressedArea.gameObject.SetActive(true);
    }
}
