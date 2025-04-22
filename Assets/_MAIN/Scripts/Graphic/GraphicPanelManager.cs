using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class GraphicPanelManager : MonoBehaviour
{
    public static GraphicPanelManager instance;

    [SerializeField] private GraphicPanel[] allPanels;

    public const float DEFAULT_TRANSITION_SPEED = 3f;

    private void Awake() {
        instance = this;
    }

    public static GraphicPanelManager GetInstance() {
        return instance;
    }

    public GraphicPanel GetPanel(string name) {
        name = name.ToLower();
        foreach (var panel in allPanels) {
            if (panel.panelName.ToLower() == name) {
                return panel;
            }
        }

        return null;
    }

    public void Clear() {
        foreach(var panel in allPanels) {
            panel.Clear();
        }
    }


}
