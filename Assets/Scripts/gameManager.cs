using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public int money = 20;
    public UIDocument uiDocument;
    private Label moneyLabel;

    // ========== ========== ========== ========== ========== Other Functions

    void Start()
    {
        moneyLabel = uiDocument.rootVisualElement.Q<Label>("Money");
    }

    public void UpdateLabel()
    {
        moneyLabel.text = $"MONEY: ${money}";
    }
}