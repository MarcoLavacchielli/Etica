using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GatchaController : MonoBehaviour
{
    [Header("Gatcha")]
    [SerializeField] private Image imageLeft;
    [SerializeField] private Image imageCenter;
    [SerializeField] private Image imageRight;

    [SerializeField] private int image1;
    [SerializeField] private int image2;
    [SerializeField] private int image3;

    private bool canActivate = true; // Control de activaciÛn para evitar m˙ltiples ejecuciones simult·neas

    [SerializeField] private float spinDuration = 2f; // DuraciÛn del efecto de rodar
    [SerializeField] private float spinInterval = 0.1f; // Intervalo entre cada cambio de imagen durante el rodar

    [SerializeField] bool wonPrize = false;
    [SerializeField] private float winCooldown = 3f; // Tiempo de espera tras ganar

    [Header("Premios")]
    [SerializeField] private GameObject grandPrizePanel; // Cartel para el Gran Premio
    [SerializeField] private GameObject otherPrizePanel; // Cartel para otros premios
    [SerializeField] private GameObject noPrizePanel;    // Cartel para sin premio

    [SerializeField] private string currentPrize; // Variable para mostrar el premio obtenido

    [Header("UI Textos de Premios")]
    [SerializeField] private TextMeshProUGUI jackpotRewardText; // Cartel para el Jackpot
    [SerializeField] private TextMeshProUGUI grandPrizeRewardText; // Texto para gran premio
    [SerializeField] private TextMeshProUGUI mediumPrizeRewardText; // Texto para premio mediano
    [SerializeField] private TextMeshProUGUI minorPrizeRewardText; // Texto para otros premios
    [SerializeField] private TextMeshProUGUI minorPrizeRewardText1; // Texto para otros premios

    [Header("GestiÛn de Dinero")]
    [SerializeField] private PlayerMoneyManager moneyManager; // Referencia al sistema de dinero
    [SerializeField] private int jackpotReward = 500;
    [SerializeField] private int grandPrizeReward = 300;
    [SerializeField] private int mediumPrizeReward = 200;
    [SerializeField] private int minorPrizeReward = 50;
    [SerializeField] private int playCost = 50;

    [Header("Palanca")]    
    [SerializeField] private InteractableLever lever; // Referencia al script de la palanca

    [Header("Numero de Intentos Min/Max")]
    [SerializeField] private int minGuaranteedWin = 10; // Numero minimo que garantiza una victoria
    [SerializeField] private int maxGuaranteedWin = 16; // Numero maximo que garantiza una victoria

    [Header("Numero de Intentos")]
    [SerializeField] private int guaranteedWinAttempt; // Intento en el que se garantiza una victoria
    [SerializeField] private int failedAttempts = 0; // Contador de intentos fallidos
    

    void Start()
    {
        if (lever == null)
        {
            Debug.LogError("No se asignÅEla palanca en GatchaController.");
            return;
        }

        if (moneyManager == null)
        {
            Debug.LogError("No se asignÅEel PlayerMoneyManager en GatchaController.");
            return;
        }

        // Suscribirse al evento de activaciÛn de la palanca
        lever.OnLeverActivated += ActivateGatcha;


        imageLeft = GameObject.Find("Izquierda").GetComponent<Image>();
        imageCenter = GameObject.Find("Centro").GetComponent<Image>();
        imageRight = GameObject.Find("Derecha").GetComponent<Image>();

        // Asignar los valores a los textos
        if (jackpotRewardText != null)
            jackpotRewardText.text = $"{jackpotReward}";

        if (grandPrizeRewardText != null)
            grandPrizeRewardText.text = $"{grandPrizeReward}";

        if (mediumPrizeRewardText != null)
            mediumPrizeRewardText.text = $"{mediumPrizeReward}";

        if (minorPrizeRewardText != null)
            minorPrizeRewardText.text = $"{minorPrizeReward}";
        
        if (minorPrizeRewardText1 != null)
            minorPrizeRewardText1.text = $"{minorPrizeReward}";


        grandPrizePanel.SetActive(false);
        otherPrizePanel.SetActive(false);
        noPrizePanel.SetActive(false);

        // Generar un n˙mero aleatorio entre 10 y 15 para garantizar una victoria
        guaranteedWinAttempt = Random.Range(minGuaranteedWin, maxGuaranteedWin);
    }

    void OnDestroy()
    {
        // Desuscribirse del evento al destruirse
        if (lever != null)
        {
            lever.OnLeverActivated -= ActivateGatcha;
        }
    }

    void ActivateGatcha()
    {
        if (!canActivate) return; // Evitar m˙ltiples activaciones

        // Verificar si el jugador tiene suficiente dinero
        if (moneyManager.playerMoney < playCost)
        {
            Debug.Log("No tienes suficiente dinero para jugar.");
            return;
        }

        // Restar el costo del juego
        moneyManager.RemoveMoney(playCost);

        canActivate = false;

        // Reiniciar paneles de premios
        grandPrizePanel.SetActive(false);
        otherPrizePanel.SetActive(false);
        noPrizePanel.SetActive(false);

        // Iniciar la rutina de rodar im·genes
        StartCoroutine(SpinAndDetermineResult());
    }

    IEnumerator SpinAndDetermineResult()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            // Generar im·genes aleatorias durante el rodar
            image1 = Random.Range(1, 6);
            imageLeft.sprite = Resources.Load<Sprite>("Sprite/" + image1);

            image2 = Random.Range(1, 6);
            imageCenter.sprite = Resources.Load<Sprite>("Sprite/" + image2);

            image3 = Random.Range(1, 6);
            imageRight.sprite = Resources.Load<Sprite>("Sprite/" + image3);

            elapsedTime += spinInterval;
            yield return new WaitForSeconds(spinInterval);
        }

        // Determinar el resultado final
        image1 = Random.Range(1, 6);
        imageLeft.sprite = Resources.Load<Sprite>("Sprite/" + image1);

        image2 = Random.Range(1, 6);
        imageCenter.sprite = Resources.Load<Sprite>("Sprite/" + image2);

        image3 = Random.Range(1, 6);
        imageRight.sprite = Resources.Load<Sprite>("Sprite/" + image3);            

        if ((image1 == image2) && (image2 == image3))
        {
            currentPrize = DeterminePrize(image1, image2, image3); // Determinar el premio
            AwardPrize(currentPrize); // Otorgar el premio
            ShowPrizePanel(currentPrize); // Mostrar el cartel correspondiente
            failedAttempts = 0; // Reiniciar el contador si gana algo
            wonPrize = true;
        }
        else
        {
            failedAttempts++;
            moneyManager.SellItem(0);

            // Verificar si se alcanzÅEel intento garantizado para ganar
            if (failedAttempts >= guaranteedWinAttempt)
            {
                var guaranteedWin = Random.Range(1, 6);

                // Forzar una victoria asignando im·genes iguales
                image1 = guaranteedWin;
                image2 = guaranteedWin;
                image3 = guaranteedWin;
                imageLeft.sprite = Resources.Load<Sprite>("Sprite/" + image1);
                imageCenter.sprite = Resources.Load<Sprite>("Sprite/" + image2);
                imageRight.sprite = Resources.Load<Sprite>("Sprite/" + image3);

                currentPrize = DeterminePrize(image1, image2, image3);
                AwardPrize(currentPrize); // Otorgar el premio
                ShowPrizePanel(currentPrize); // Mostrar el cartel correspondiente
                failedAttempts = 0; // Reiniciar el contador despuÈs de la victoria

                guaranteedWinAttempt = Random.Range(minGuaranteedWin, maxGuaranteedWin); // Generar un nuevo l˙äite
                wonPrize = true;
            }
            else
            {
                ShowPrizePanel("Sin premio"); // Mostrar cartel de "Sin premio"
            }
        }

        // Esperar si ganÅEalgo, si no reactivar inmediatamente
        if (wonPrize)
        {
            lever.StartCooldown();
            yield return new WaitForSeconds(winCooldown);
            wonPrize = false;
        }

        canActivate = true;
    }

    // FunciÛn para determinar el premio seg˙n las im·genes
    string DeterminePrize(int img1, int img2, int img3)
    {
        if (img1 == 1 && img2 == 1 && img3 == 1)
        {
            return "Jackpot"; // CombinaciÛn especial (3 sietes)
        }
        else if (img1 == 2 && img2 == 2 && img3 == 2)
        {
            return "Gran Premio"; // Otra combinaciÛn especial (3 cerezas)
        }
        else if (img1 == 3 && img2 == 3 && img3 == 3)
        {
            return "Premio Mediano"; // Otra combinaciÛn especial (3 sandias)
        }
        else if (img1 == 4 && img2 == 4 && img3 == 4 || img1 == 5 && img2 == 5 && img3 == 5)
        {
            return "Premio Menor"; // CombinaciÛn genÈrica (3 uvas o 3 peras)
        }
        else
        {
            return "Sin premio"; // En caso de que no haya un premio
        }
    }

    void AwardPrize(string prize)
    {
        switch (prize)
        {
            case "Jackpot":
                moneyManager.AddMoney(jackpotReward);
                break;
            case "Gran Premio":
                moneyManager.AddMoney(grandPrizeReward);
                break;
            case "Premio Mediano":
                moneyManager.AddMoney(mediumPrizeReward);
                break;
            case "Premio Menor":
                moneyManager.AddMoney(minorPrizeReward);
                break;
            default:
                break;
        }
    }

    // FunciÛn para mostrar el cartel correspondiente
    void ShowPrizePanel(string prize)
    {
        // Desactivar todos los carteles primero
        grandPrizePanel.SetActive(false);
        otherPrizePanel.SetActive(false);
        noPrizePanel.SetActive(false);

        // Activar el cartel correspondiente
        if (prize == "Jackpot")
        {
            grandPrizePanel.SetActive(true);
        }
        else if (prize == "Gran Premio" || prize == "Premio Mediano" || prize == "Premio Menor")
        {
            otherPrizePanel.SetActive(true);
        }
        else
        {
            noPrizePanel.SetActive(true);
        }
    }
}
