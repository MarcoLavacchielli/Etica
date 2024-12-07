using UnityEngine;
using TMPro;


public class PlayerMoneyManager : MonoBehaviour
{
    public float playerMoney = 0f; // Dinero del jugador
    public TextMeshProUGUI moneyText; // Texto en la UI que muestra el dinero
    public int SellableObjects;
    public GameObject escButton; // Botón en el Canvas que se activa con "Esc"
    public GameObject imageGameOver;

    private void Start()
    {
        UpdateMoneyUI();
        SellableObjects = FindObjectsOfType<SellableObject>().Length;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Cambiar el estado del botón (activar/desactivar)
            
            if(escButton.activeSelf == false)
            {
                escButton.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                escButton.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }
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
        //SellItem(0);
    }

    // Actualiza el texto de la UI del dinero
    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney;
    }

    public void SellItem(int itm)
    {
        SellableObjects -= itm;

        if(SellableObjects <= 0 && playerMoney == 0f)
        {
            PlayerMovementTutorial playerMovement = this.GetComponent<PlayerMovementTutorial>();
            playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            imageGameOver.SetActive(true);
            moneyText.text = " ";
        }

    }

    public void ForceGameOver()
    {
        PlayerMovementTutorial playerMovement = this.GetComponent<PlayerMovementTutorial>();
        playerMovement.enabled = false;
        imageGameOver.SetActive(true);
        moneyText.text = " ";
        escButton.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

