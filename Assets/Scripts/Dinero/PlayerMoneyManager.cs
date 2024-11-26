using UnityEngine;
using TMPro;

public class PlayerMoneyManager : MonoBehaviour
{
    public float playerMoney = 0f; // Dinero del jugador
    public TextMeshProUGUI moneyText; // Texto en la UI que muestra el dinero

    private void Start()
    {
        UpdateMoneyUI();
    }
    // Método para añadir dinero
    public void AddMoney(float amount)
    {
        playerMoney += amount;
        UpdateMoneyUI();
    }
    
    // Método para quitar dinero
    public void RemoveMoney(float amount)
    {
        playerMoney -= amount;
        UpdateMoneyUI();
    }

    // Actualiza el texto de la UI del dinero
    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney;
    }
}

