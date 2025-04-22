using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GraphicObject
{   
    private const string NAME_FORMAT = "Graphic - [{0}]";

    public RawImage renderer;


    public string graphicPath = "";

    public string graphicName {get; private set;}

    private GraphicLayer layer;

    // Control Coroutin 
    public Coroutine co_fadingIn = null;
    public Coroutine co_fadingOut = null;

    public GraphicObject(GraphicLayer layer, string graphicPath, Texture tex) {
        this.graphicPath = graphicPath;
        this.layer = layer;

        GameObject ob = new GameObject();
        ob.transform.SetParent(layer.panel);
        renderer = ob.AddComponent<RawImage>();

        graphicName = tex.name;

        InitGraphic();

        renderer.name = string.Format(NAME_FORMAT, graphicName);

        renderer.texture = tex;
    }

    private void InitGraphic() {
        renderer.transform.localPosition = Vector3.zero;
        renderer.transform.localScale = Vector3.one;

        RectTransform rect = renderer.GetComponent<RectTransform>();
        
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.one;
    }


    GraphicPanelManager panelManager => GraphicPanelManager.GetInstance();

    public Coroutine FadeIn(float speed = 1f) {
        if (co_fadingOut != null) {
            panelManager.StopCoroutine(co_fadingOut);
        }

        if (co_fadingIn != null) {
            return co_fadingIn;
        }

        co_fadingIn = panelManager.StartCoroutine(Fading(1f, speed));

        return co_fadingIn;
    }

    public void Show() {
        Color currentColor = renderer.color;
        currentColor.a = 1f;
        renderer.color = currentColor;
    }

    public Coroutine FadeOut(float speed = 1f) {
        if (co_fadingIn != null) {
            panelManager.StopCoroutine(co_fadingIn);
        }

        if (co_fadingOut != null) {
            return co_fadingOut; 
        }

        co_fadingOut = panelManager.StartCoroutine(Fading(0f, speed));

        return co_fadingOut;
    }

    public void Hide() {
        Color currentColor = renderer.color;
        currentColor.a = 0f;
        renderer.color = currentColor;
        Destroy();
    }

    private IEnumerator Fading(float target, float speed) {
        Color currentColor = renderer.color;

        while(renderer.color.a != target) {
            float opacity = Mathf.MoveTowards(renderer.color.a, target, speed * Time.deltaTime);
            currentColor.a = opacity;
            renderer.color = currentColor;

            yield return null;
        }

        co_fadingIn = null;
        co_fadingOut = null;

        if (target == 0) {
            Destroy();
        }
        else {
            DestroyBackgroundGraphicOnLayer();
        }
    }

    // Destroy graphic object
    private void Destroy() {

        if (layer.currentGraphic != null && layer.currentGraphic.renderer == renderer) {
            layer.currentGraphic = null;
        }

        Object.Destroy(renderer.gameObject);
    }

    private void DestroyBackgroundGraphicOnLayer() {
        layer.DestroyOldGraphics();
    }
}
