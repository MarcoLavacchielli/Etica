using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLever : MonoBehaviour
{
    [SerializeField] private Camera cam; // Referencia a la cámara
    [SerializeField] private GameObject interactUI;

    [SerializeField] private float interactionRange = 10f; // Rango máximo del raycast

    private bool isActivated = false; // Estado actual (activado o desactivado)
    private bool selected = false; // Si el objeto está seleccionado

    private bool isCoolingDown = false; // Control del cooldown tras ganar

    private static InteractableLever leverSelected; // Objeto actualmente seleccionado

    private Animator animator; // Referencia al Animator de la palanca

    // Evento que se activa al alternar la palanca
    public event Action OnLeverActivated; 
    public event Func<bool> CanActivateLever; // Función para verificar si la palanca se puede activar

    [SerializeField] private float cooldownDuration = 3f; // Duración del cooldown tras ganar

    void Start()
    {
        // Obtén el Animator del objeto (asegúrate de tener uno en el GameObject de la palanca)
        animator = GetComponent<Animator>();

        // Si no se asignó una cámara, busca automáticamente la cámara principal
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        SelectObject();

        // Activar o desactivar la palanca con la tecla F
        if (selected && Input.GetKeyDown(KeyCode.F))
        {
            TryActivateLever();
        }
    }

    void SelectObject()
    {
        if (cam == null) return;

        // Crea un rayo desde la posición del mouse en la pantalla
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Realiza el raycast y verifica si el rayo impactó el objeto
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Si otro objeto ya está seleccionado, deseleccionarlo
                if (leverSelected != null && leverSelected != this)
                {
                    leverSelected.DeselectObject();
                }

                // Seleccionar este objeto
                leverSelected = this;
                selected = true;

                ShowText(true);
                return;
            }
        }

        // Si no se está apuntando al objeto, deseleccionarlo
        DeselectObject();
        ShowText(false);
    }

    public void DeselectObject()
    {
        selected = false;
    }

    void ShowText(bool show)
    {
        if (interactUI != null)
        {
            interactUI.SetActive(show);  // Muestra u oculta el texto según el valor de 'mostrar'
        }
    }

    void TryActivateLever()
    {
        if (isCoolingDown)
        {
            Debug.Log("La palanca está en cooldown.");
            return;
        }

        if (CanActivateLever != null && !CanActivateLever.Invoke())
        {
            Debug.Log("No tienes suficiente dinero para activar la palanca.");
            return;
        }

        ToggleLever();
    }

    void ToggleLever()
    {
        isActivated = !isActivated;
        ActivateAnimation();

        // Notificar a los suscriptores si se activa la palanca
        if (isActivated && OnLeverActivated != null)
        {
            OnLeverActivated.Invoke();
        }
    }

    void ActivateAnimation()
    {
        // Cambiar la animación dependiendo del estado
        if (animator != null)
        {
            if (isActivated)
            {
                animator.SetTrigger("activarPalanca"); // Asegúrate de tener este Trigger en el Animator
            }
        }
    }

    public void StartCooldown()
    {
        if (!isCoolingDown)
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    private System.Collections.IEnumerator CooldownCoroutine()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCoolingDown = false;
    }
}
