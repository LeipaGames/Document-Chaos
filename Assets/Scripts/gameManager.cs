using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    public int money = 20;
    public UIDocument uiDocument;
    private Label moneyLabel;

    // ========== ========== ========== ========== ========== Start & Update

    void Start()
    {
        moneyLabel = uiDocument.rootVisualElement.Q<Label>("Money");
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) {
            Application.Quit();
        }
    }

    // ========== ========== ========== ========== ========== Other Functions

    public void UpdateLabel()
    {
        moneyLabel.text = $"MONEY: ${money}";
    }
}