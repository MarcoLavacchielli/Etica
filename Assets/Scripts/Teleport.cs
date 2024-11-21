using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportPoint; // Punto de teletransporte asignable desde el inspector

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Teletransporta al jugador al punto asignado
            other.transform.position = teleportPoint.position;
        }

        MoveCamera moveCamera = other.GetComponentInChildren<MoveCamera>();
        if (moveCamera != null)
        {
            moveCamera.transform.position = moveCamera.cameraPosition.position;
        }

    }
}
