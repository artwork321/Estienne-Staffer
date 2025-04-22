using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public abstract class Character
{
    public Vector2 DEFAULT_POSITION = new Vector2(0f, 0f);
    public Color DEFAULT_COLOR_NAME = Color.black;

    // List to store emotion images
    public Image currentGraphicComponent;


    // Character attributes
    public string charName;
    public string displayName;
    public Color nameColor;
    public Vector2 position;
    public bool isInvisible;


    // Container of the image
    public GameObject rootPanel;

    // Construction
    public Character(GameObject characterObject, string charName, Color nameColor, Vector2 position) {
        this.charName = charName;
        this.displayName = this.charName;
        this.nameColor = nameColor;
        this.position = position;
        this.isInvisible = false;

        rootPanel = characterObject;
    }

    public Character(GameObject characterObject, string charName) {
        this.charName = charName;
        this.displayName = this.charName;
        this.nameColor = DEFAULT_COLOR_NAME;
        this.position = DEFAULT_POSITION;
        this.isInvisible = false;

        rootPanel = characterObject;
    }


    public void SetVisibility(string visibilityTag) {
        isInvisible = visibilityTag == "true";
    }


    // Update character state based on Ink tags
    public void SetDisplayName(string displayName = null) {
        this.displayName = displayName;
    }
}
