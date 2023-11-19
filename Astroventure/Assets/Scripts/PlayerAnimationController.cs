using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    Vector2 movement;
    enum DirectionStates  {Forward=1,Right=2,Backward=3,Left=4}

    [SerializeField] DirectionStates directionStates;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        directionStates = (DirectionStates)Mathf.CeilToInt((((transform.rotation.eulerAngles.y + 45) % 360) / 90));
    }

    private void OnMove(InputValue value)
    {
        movement=value.Get<Vector2>();

        if(movement.magnitude>0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
    }

    private void OnRoll(InputValue value)
    {
        animator.SetTrigger("Roll");
    }

    private void OnAttack(InputValue value)
    {
        
        animator.SetTrigger("Attack");

    }

}
