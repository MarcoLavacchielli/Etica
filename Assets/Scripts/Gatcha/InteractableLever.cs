using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLever : MonoBehaviour
{
    [SerializeField] private Camera cam; // Referencia a la c·mara
    [SerializeField] private GameObject interactUI;

    [SerializeField] private float interactionRange = 10f; // Rango m·ximo del raycast

    private bool isActivated = false; // Estado actual (activado o desactivado)
    private bool selected = false; // Si el objeto estÅEseleccionado

    private bool isCoolingDown = false; // Control del cooldown tras ganar

    private static InteractableLever leverSelected; // Objeto actualmente seleccionado

    private Animator animator; // Referencia al Animator de la palanca

    // Evento que se activa al alternar la palanca
    public event Action OnLeverActivated; 
    public event Func<bool> CanActivateLever; // FunciÛn para verificar si la palanca se puede activar

    [SerializeField] private float cooldownDuration = 3f; // DuraciÛn del cooldown tras ganar

    void Start()
    {
        // ObtÈn el Animator del objeto (aseg˙rate de tener uno en el GameObject de la palanca)
        animator = GetComponent<Animator>();

        // Si no se asignÅEuna c·mara, busca autom·ticamente la c·mara principal
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

        // Crea un rayo desde la posiciÛn del mouse en la pantalla
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Realiza el raycast y verifica si el rayo impactÅEel objeto
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Si otro objeto ya esta seleccionado, deseleccionarlo
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

        // Si no se esta apuntando al objeto, deseleccionarlo
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
            interactUI.SetActive(show);  // Muestra u oculta el texto segun el valor de 'mostrar'
        }
    }

    void TryActivateLever()
    {
        if (isCoolingDown)
        {
            Debug.Log("La palanca estÅEen cooldown.");
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
        // Cambiar la animaciÛn dependiendo del estado
        if (animator != null)
        {
            if (isActivated)
            {
                animator.SetTrigger("activarPalanca"); // Asegurate de tener este Trigger en el Animator
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
