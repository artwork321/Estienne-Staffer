using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class CharacterSprite : Character
{   

    private const string DEFAULT_EMOTION_STATE = "normal";

    // Coroutine control
    private Coroutine co_fadingIn = null;
    private Coroutine co_fadingOut = null;
    private Coroutine co_moving = null;
    public bool isDoneRunningChar => co_fadingIn == null && co_fadingOut == null && co_moving == null;


    public CharacterSprite(GameObject characterObject, string charName, Color nameColor, Vector2 position) : base(characterObject, charName, nameColor, position) {
        
        // Initialize Image component
        GameObject characterSprite = new GameObject("Image");
        characterSprite.transform.SetParent(characterObject.transform, false);
        Image imageComponent = characterSprite.AddComponent<Image>();
        imageComponent.raycastTarget = false;
        imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 0f);     
        
        //Adjust the height to match the resolution height
        imageComponent.transform.localPosition = Vector3.zero;
        RectTransform rect = imageComponent.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.one;
        imageComponent.preserveAspect = true;
        
        // Set up default graphic and position
        currentGraphicComponent = imageComponent;
        InitGraphic();        
    }

    public void InitGraphic() {
        SetPositionImage(position);
        ChangeEmotionImage(DEFAULT_EMOTION_STATE);
    }

    public void Hide() {
        Image imageComponent = rootPanel.transform.Find("Image").GetComponent<Image>();
        imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 0f);
    }


    // Make the sprite visible by setting the opaque to 1
    public void Show() {
        Image imageComponent = rootPanel.transform.Find("Image").GetComponent<Image>();
        imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 1f);
    }


    // Show or hide a character with effect
    public void ShowOrHideCharacter(bool isFade) {

        if (isInvisible) {
            if (isFade) {
                FadingIn(0.3f);
            }
            else {
                Show();
            }
        }
        else {
            if (isFade) {
                FadingOut(0.4f);
            }
            else {
                Hide();
            }
        }
    }


    public Coroutine FadingOut(float fadeDuration = 1f){   

        if (co_fadingIn != null) {
            CharacterManager.GetInstance().StopCoroutine(co_fadingIn);
        }

        if (co_fadingOut != null) {
            return co_fadingOut;
        }

        co_fadingOut = CharacterManager.GetInstance().StartCoroutine(Fading(0f, fadeDuration));

        return co_fadingOut;
    }


    public Coroutine FadingIn(float fadeDuration = 1f){   

        if (co_fadingOut != null) {
            CharacterManager.GetInstance().StopCoroutine(co_fadingOut);
        }

        if (co_fadingIn != null) {
            return co_fadingIn;
        }

        co_fadingIn = CharacterManager.GetInstance().StartCoroutine(Fading(1f,  fadeDuration));

        return co_fadingIn;
    }


    public IEnumerator Fading(float targetOpacity, float fadeDuration = 1f)
    {   
        Image imageComponent = rootPanel.transform.Find("Image").GetComponent<Image>();
        Color c = imageComponent.color;

        float originalAlpha = c.a;

        float timeElapsed = 0f;

        // Fade over the given duration
        while (timeElapsed < fadeDuration)
        {
            // Calculate the new alpha
            c.a = Mathf.Lerp(originalAlpha, targetOpacity, timeElapsed / fadeDuration);
            imageComponent.color = c;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        c.a = targetOpacity;
        imageComponent.color = c;

        co_fadingIn = null;
        co_fadingOut = null;
    }


    // Move character to a new position with requested effect
    public Coroutine MoveToPosition(bool isMove, Vector2 targetPosition, float speed = 1f) {
        
        if (isMove) {
            Debug.Log("Moving character: " + charName);

            if (co_moving != null) {
                CharacterManager.GetInstance().StopCoroutine(co_moving);
            }
            
            co_moving = CharacterManager.GetInstance().StartCoroutine(Moving(targetPosition, speed));
            return co_moving;
        }
        else {
            Debug.Log("Just jump character: " + charName);
            SetPositionImage(targetPosition);
            return null;
        }
    }


    public void SetPositionImage(Vector2 targetPosition) {
        Image imageComponent = rootPanel.transform.Find("Image").GetComponent<Image>();
        imageComponent.rectTransform.anchoredPosition = targetPosition;
        position = targetPosition;
    }

    public IEnumerator Moving(Vector2 targetPosition, float speed = 1f)
    {   
        Vector2 currentPosition = currentGraphicComponent.rectTransform.anchoredPosition;

        // Determine the direction of movement (left or right)
        int direction = (currentPosition.x > targetPosition.x) ? -1 : 1;
        
        float x = currentPosition.x;
    
        // Move the character to the target position
        while (x != targetPosition.x)
        {   
            x = Mathf.MoveTowards(x, targetPosition.x, 1280f * speed * Time.deltaTime);
            
            Vector2 newPosition = new Vector2(x, currentPosition.y);
            
            currentGraphicComponent.rectTransform.anchoredPosition = newPosition;

            yield return null;
        }

        currentGraphicComponent.rectTransform.anchoredPosition = targetPosition;
        position = targetPosition;
        co_moving = null;
    }


    public Sprite GetEmotionSprite(string emotion) {
        string folderName = "Graphics/Characters";

        // Load all sprites in the folder
        Sprite[] sprites = Resources.LoadAll<Sprite>(folderName);

        // Find the sprite that matches the character name and emotion
        foreach (Sprite sprite in sprites) {
            string[] components = sprite.name.Split('_');
            if (components.Length < 2) {
                Debug.LogWarning("Invalid sprite name: " + sprite.name);
                continue;
            }

            if (components[0] == charName && components[1].ToLower() == emotion.ToLower()) {
                return sprite;
            }
        }

        Debug.LogError("Could not find sprite for " + charName + " with emotion " + emotion);
        return null;
    }



    public void ChangeEmotionImage(string emotion) {
        currentGraphicComponent.sprite = GetEmotionSprite(emotion);
    }
    

    // Flip sprite to face left or right -- FIX
    public void ChangeFaceDirection(bool isRight) {

        Image imageComponent = rootPanel.transform.Find("Image").GetComponent<Image>();

        // Remain the ratio of the image
        if (isRight) {
            imageComponent.rectTransform.localScale = new Vector3(
                -1 *  Mathf.Abs(imageComponent.rectTransform.localScale.x), 
                imageComponent.rectTransform.localScale.y, 
                1);
        }
        else {
            imageComponent.rectTransform.localScale = new Vector3(
                Mathf.Abs(imageComponent.rectTransform.localScale.x), 
                imageComponent.rectTransform.localScale.y, 
                1); 
        }

    }

}
