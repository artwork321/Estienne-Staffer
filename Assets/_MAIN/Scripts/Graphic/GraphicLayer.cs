using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GraphicLayer
{   
    public const string LAYER_OBJECT_NAME_FORMAT = "Layer: {0}";
    public int layerDepth = 0;
    public Transform panel;

    public GraphicObject currentGraphic =  null;
    private List<GraphicObject> oldGraphics = new List<GraphicObject>();

    // Set Texture for a graphic layer
    public void SetTexture(string filePath, float transitionSpeed = 1f) {
        Texture tex = Resources.Load<Texture>(filePath);

        if (tex == null) {
            Debug.LogError("Could not load graphic from path: " + filePath);
            return;
        }

        SetTexture(tex, transitionSpeed, filePath);
    }

    public void SetTexture(Texture tex, float transitionSpeed = 1f, string filePath = null) {
        CreateGraphic(tex, transitionSpeed, filePath);
    }

    private void CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath) {
        GraphicObject newGraphic = null;

        if (graphicData is Texture)
            newGraphic = new GraphicObject(this, filePath, graphicData as Texture);

        // Default visibility is 0 to set a fade in effect
        Color currentColor = newGraphic.renderer.color;
        currentColor.a = 0f;
        newGraphic.renderer.color = currentColor;

        if (currentGraphic != null && !oldGraphics.Contains(currentGraphic))
            oldGraphics.Add(currentGraphic);

        currentGraphic = newGraphic;

        // Finish coroutines of old graphics
        FinishOldGraphicsCoroutine();

        currentGraphic.FadeIn(transitionSpeed);
    }

    public void FinishOldGraphicsCoroutine() {
        foreach(var g in oldGraphics) {
            if (g.co_fadingOut != null) {
                GraphicPanelManager.GetInstance().StopCoroutine(g.co_fadingOut);
                g.co_fadingOut = null;
                g.Hide();
            }
            else if (g.co_fadingIn != null) {
                GraphicPanelManager.GetInstance().StopCoroutine(g.co_fadingIn);
                g.co_fadingIn = null;
                g.Show();
            }
        }
    }

    // Destroy old graphics below the current graphic
    public void DestroyOldGraphics() {
        foreach(var g in oldGraphics) {
            Object.Destroy(g.renderer.gameObject);
        }

        oldGraphics.Clear();
    }

    // Clear all graphics from one layer
    public void Clear() {
        if (currentGraphic != null) {
            oldGraphics.Add(currentGraphic);
            currentGraphic = null;
        }

        FinishOldGraphicsCoroutine();

        foreach(var g in oldGraphics) {
            g.FadeOut();
        }

        oldGraphics.Clear();
    }
}
