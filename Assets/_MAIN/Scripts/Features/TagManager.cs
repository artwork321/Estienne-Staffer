using UnityEngine;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TagManager : MonoBehaviour
{
    private static TagManager instance;
    private const string SPEAKER_TAG = "speaker";
    private const string BACKGROUND_TAG = "background";
    private const string CINEMATIC_TAG = "cinematic";
    private const string EXPRESSION_TAG = "expression";
    private const string ALIAS_TAG = "alias";
    private const string VISIBILITY_TAG = "visibility";
    private const string FLIP_TAG = "flip";
    private const string MOVEMENT_TAG = "moveto";
    private const string SOUND_EFFECT_TAG = "sound";
    private const string MUSIC_TAG = "music";
    
    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Awake(): Found more than one TagManager");
        }

        instance = this;
    }

    public static TagManager GetInstance() {
        return instance;
    }
    
    public void HandleTags(List<string> currentTags) {

        Dictionary<string, string[]> parsedTags = new Dictionary<string, string[]>();

        // Get all the tags and their values
        foreach (string tag in currentTags) {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length < 2) {
                Debug.LogWarning("HandleTags(): Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string[] tagValue = new string[splitTag.Length - 1];
            
            Array.Copy(splitTag, 1, tagValue, 0, tagValue.Length);

            parsedTags[tagKey] = tagValue;
        }
        

        if (parsedTags.ContainsKey(SPEAKER_TAG)) {
            Character speaker = CharacterManager.GetInstance().GetCharacter(parsedTags[SPEAKER_TAG][0]);

            // Change display name temporarily
            if (parsedTags.ContainsKey(ALIAS_TAG)) {
                HandleAliasTag(speaker, parsedTags[ALIAS_TAG]);
            }
    
            // Change things related to image of character
            if (speaker is CharacterSprite characterSprite) {
                if (parsedTags.ContainsKey(VISIBILITY_TAG)) {
                    HandleVisibilityTag(characterSprite, parsedTags[VISIBILITY_TAG]);
                }

                if (parsedTags.ContainsKey(FLIP_TAG)) {
                    HandleDirectionTag(characterSprite, parsedTags[FLIP_TAG]);
                }
                
                if (parsedTags.ContainsKey(EXPRESSION_TAG)) {
                    HandleEmotionTag(characterSprite, parsedTags[EXPRESSION_TAG]);
                }

                if (parsedTags.ContainsKey(MOVEMENT_TAG)) {
                    HandleMovementTags(characterSprite, parsedTags[MOVEMENT_TAG]);
                }
            }
        }

        // Change background according to the given picture name
        if (parsedTags.ContainsKey(BACKGROUND_TAG)) {
            HandleBackgroundTags(parsedTags[BACKGROUND_TAG]);
        }

        // Change cinematic graphic according to the given picture name
        if (parsedTags.ContainsKey(CINEMATIC_TAG)) {
            HandleCinematicTag(parsedTags[CINEMATIC_TAG]);
        }

        // Change music and sound effect
        if (parsedTags.ContainsKey(MUSIC_TAG)) {
            HandleMusicTag(parsedTags[MUSIC_TAG]);
        }

        if (parsedTags.ContainsKey(SOUND_EFFECT_TAG)) {
            HandleSoundEffect(parsedTags[SOUND_EFFECT_TAG]);
        }
    }

    public void HandleAliasTag(Character speaker, string[] aliasTagValue) {
        if (aliasTagValue.Length < 1) {
            Debug.LogError("Invalid alias tag value.");
            return;
        }
        speaker.SetDisplayName(aliasTagValue[0]);
    }


    public void HandleEmotionTag(CharacterSprite speaker, string[] expressionTagValue) {
        if (expressionTagValue.Length < 1) {
            Debug.LogError("Invalid expression tag value.");
            speaker.Hide();
            return;
        }

        speaker.ChangeEmotionImage(expressionTagValue[0]);
    }


    public void HandleDirectionTag(CharacterSprite speaker, string[] directionTagValue) {
        if (directionTagValue.Length != 1) {
            Debug.LogError("Invalid flip tag value.");
            return;
        }

        if (directionTagValue[0] == "true") {
            speaker.ChangeFaceDirection(true);
        }
        else {
            speaker.ChangeFaceDirection(false);
        }
        
    }

    public void HandleVisibilityTag(CharacterSprite speaker, string[] visibilityTagValues) {
        if (visibilityTagValues.Length < 1) {
            Debug.LogError("Missing value in visibility tag.");
            return;
        }
        speaker.SetVisibility(visibilityTagValues[0]);

        bool isFade = false;
        if (visibilityTagValues.Length == 2 && visibilityTagValues[1] == "fading") {
            isFade = true;
        }
        
        speaker.ShowOrHideCharacter(isFade);
    }

    
    public void HandleMovementTags(CharacterSprite speaker, string[] moveTagValues) {
    
        if (moveTagValues.Length < 1) {
            Debug.LogError("Missing value in moveTagValues tag.");
            return;
        }
        
        bool isMove = false;
        if (moveTagValues.Length == 2  && moveTagValues[1] == "moving") {
            isMove = true;
        }
        
        Vector2 targetPosition = new Vector2(float.Parse(moveTagValues[0]), speaker.position.y);
        speaker.MoveToPosition(isMove, targetPosition);
    }


    // Change background according to tag value which is a background name
    public void HandleBackgroundTags(string[] backgroundTagValue) {
        if (backgroundTagValue.Length != 1) {
            Debug.LogError("Background tag value is invalid.");
            return;
        }

        GraphicPanel panel = GraphicPanelManager.GetInstance().GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0, true);

        if (backgroundTagValue[0] == "none") {
            layer.Clear();
            return;
        }

        layer.SetTexture("Graphics/BG Images/" + backgroundTagValue[0]);
    }

    // Change background according to tag value which is a background name
    public void HandleCinematicTag(string[] cinematicTagValue) {
        Debug.Log(cinematicTagValue[0]);
        if (cinematicTagValue.Length != 1) {
            Debug.LogError("Background tag value is invalid.");
            return;
        }

        GraphicPanel panel = GraphicPanelManager.GetInstance().GetPanel("Cinematic");
        GraphicLayer layer = panel.GetLayer(0, true);

        if (cinematicTagValue[0] == "none") {
            layer.Clear();
            return;
        }

        layer.SetTexture("Graphics/Gallery/" + cinematicTagValue[0]);
    }

    // Stop old music and play new music
    public void HandleMusicTag(string[] musicTagValue) {

        if (musicTagValue.Length < 1) {
            Debug.LogError("Music tag value is invalid.");
            return;
        }

        AudioManager.GetInstance().StopAllTrack(); // stop old musics
        if (musicTagValue[0] == "") {
            return;
        }

        // Start new music combinations
        for (int i = 0; i < musicTagValue.Length; i++){
            Debug.Log(musicTagValue[i]);
            AudioManager.GetInstance().PlayTrack("Audio/Music/" + musicTagValue[i]);
        }
        
    }


    // Stop old music and play new music
    public void HandleSoundEffect(string[] soundEffectTagValue) {

        if (soundEffectTagValue.Length != 1) {
            Debug.LogError("sfx tag value is invalid.");
            return;
        }

        if (soundEffectTagValue[0] == "") {
            AudioManager.GetInstance().StopAllSoundEffect();
            return;
        }

        for (int i = 0; i < soundEffectTagValue.Length; i++) {
            AudioManager.GetInstance().PlaySoundEffect("Audio/SFX/" + soundEffectTagValue[i]);
        }
        
    }


    public string GetSpeakingCharacterName(List<string> currentTags) {
        
        foreach (string tag in currentTags) {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length < 1) {
                Debug.LogError("GetSpeakingCharacter(): Tag could not be appropriately parsed");
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            if(tagKey == SPEAKER_TAG) {
                Character speaker = CharacterManager.GetInstance().GetCharacter(tagValue);
                return speaker.charName;
            }
        }

        return null;
    }
}
