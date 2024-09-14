using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    
    public Rigidbody rb;
    public Animator animator;
    public CapsuleCollider capsuleCollider;
    public float jumpForce;
    public float playerSpeed;
    Control _control;
    Movement _movement;


    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Start()
    {
        _movement = new Movement(rb, jumpForce, playerSpeed, this.transform, animator, capsuleCollider);
        _control = new Control(_movement, this.transform);
        //myMaterial.color = Color.white;
        _movement.OnAwake();
    }

    void Update()
    {
        _control.NormalControls();
        
    }

    private void FixedUpdate()
    {
        Vector3 inputs = _control.GetMovementInputs();
        _movement.Move(inputs);
    }

    public void InFloor()
    {
        _movement._ContF = 1;
    }
    public void OutofFloor()
    {
        _movement._ContF = -1;
        //jumpSound.Play();
    }
}
