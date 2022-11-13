using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private Animator animator;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimSpeed = Animator.StringToHash("animSpeed");

    private void OnEnable()
    {
        move.action.started += AnimateLegs;
        move.action.canceled += StopAnimation;
    }

    private void OnDisable()
    {
        move.action.started -= AnimateLegs;
        move.action.canceled -= StopAnimation;
    }

    private void AnimateLegs(InputAction.CallbackContext obj)
    {
        bool isWalkingFoward = move.action.ReadValue<Vector2>().y > 0;

        if(isWalkingFoward )
        {
            animator.SetBool(IsMoving, true);
            animator.SetFloat(AnimSpeed, 1.0f);
        }
        else
        {
            animator.SetBool(IsMoving, true);
            animator.SetFloat(AnimSpeed, -1.0f);
        }
    }

    private void StopAnimation(InputAction.CallbackContext obj)
    {
        animator.SetBool(IsMoving, false);
        animator.SetFloat(AnimSpeed, 0.0f);
    }
}
