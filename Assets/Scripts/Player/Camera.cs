using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    public Transform _target;        // El personaje que la cámara sigue
    [SerializeField]
    private float _distanceToTarget;  // Distancia entre la cámara y el personaje
    [SerializeField]
    private float _heightToTarget;    // Altura de la cámara respecto al personaje
    [SerializeField]
    private float _angle;            // Ángulo en grados para calcular la posición de la cámara
    [SerializeField]
    private float _verticalOffset;    // Offset adicional para ajustar la vista vertical de la cámara

    void LateUpdate()
    {
        if (_target != null)
        {
            // Obtener la dirección en la que la cámara debe estar, calculada a partir del ángulo
            Vector3 direction = Vector3.right * Mathf.Cos(_angle * Mathf.Deg2Rad) * _distanceToTarget +
                                Vector3.forward * Mathf.Sin(_angle * Mathf.Deg2Rad) * _distanceToTarget;

            // Calcular la posición de la cámara con el offset vertical
            Vector3 targetPosition = _target.position + direction + Vector3.up * (_heightToTarget + _verticalOffset);

            // Mover la cámara a la posición calculada
            transform.position = targetPosition;

            // Hacer que la cámara mire hacia el objetivo, ajustando el punto de vista con el offset vertical
            Vector3 lookAtPosition = _target.position + Vector3.up * (_heightToTarget / 2 + _verticalOffset);
            transform.LookAt(lookAtPosition);
        }
    }
}
