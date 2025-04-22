using UnityEngine;
using Ink.Runtime;

public class InkExternalFunction
{
    public void Bind(Story story) {
        story.BindExternalFunction("EnterInvestigationMode", (int id) => {GameManager.instance.EnterInvestigationMode(id);});    

        story.BindExternalFunction("EnterConnectingClueMode", (int id) => {GameManager.instance.EnterConnectingClueMode(id);});        
    }

    public void UnBind(Story story) {
    }

    // public void UnBind(Story story) {
    //     story.UnbindExternalFunction("HideDialogue");
    // }
}

