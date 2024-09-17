using UnityEngine;
using TMPro;

public class SellableObject : MonoBehaviour
{
    public string itemName; // Nombre del objeto
    public float itemValue; // Valor del objeto
    public GameObject sellUI; // Referencia al UI que aparece frente al objeto
    public TextMeshProUGUI sellText; // Texto que muestra la información de venta
    private bool isPlayerNearby = false; // Saber si el jugador está cerca

    // Referencia al manager del dinero (puede ser otro script en el personaje o un GameManager)
    public PlayerMoneyManager playerMoneyManager;

    void Start()
    {
        // Asegúrate de que el UI de venta esté desactivado al principio
        sellUI.SetActive(false);
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla "E", vender el objeto
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            SellItem();
        }
    }

    // Método para vender el objeto
    void SellItem()
    {
        // Añadir el valor del objeto al dinero del jugador
        playerMoneyManager.AddMoney(itemValue);
        // Destruir el objeto después de venderlo
        Destroy(gameObject);
    }

    // Detectar si el jugador entra en el área del objeto
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Mostrar la UI de venta
            sellText.text = "Press E to sell " + itemName + " for $" + itemValue;
            sellUI.SetActive(true);
            isPlayerNearby = true;
            Debug.Log("me activo");
        }
    }

    // Detectar si el jugador sale del área del objeto
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ocultar la UI de venta
            sellUI.SetActive(false);
            isPlayerNearby = false;
        }
    }
}
