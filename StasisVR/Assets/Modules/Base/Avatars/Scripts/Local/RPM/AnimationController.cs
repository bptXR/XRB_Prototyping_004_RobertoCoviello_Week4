using UnityEngine;
using UnityEngine.InputSystem;

namespace BearingPoint.Avatars.Local.RPM
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private InputActionReference move;

        [SerializeField] private Animator animator;

        private void OnEnable()
        {
            move.action.started += AnimateLegs;
            move.action.canceled += StopAnimation;
        }
    
        private void AnimateLegs(InputAction.CallbackContext obj)
        {
            bool isMovingForward = move.action.ReadValue<Vector2>().y > 0;

            if (isMovingForward)
            {
                animator.SetBool("Walking", true);
                animator.SetFloat("AnimSpeed", 1);
            }
            else
            {
                animator.SetBool("Walking", true);
                animator.SetFloat("AnimSpeed", -1);
            }
        }

        private void StopAnimation(InputAction.CallbackContext obj)
        {
            animator.SetBool("Walking", false);
            animator.SetFloat("AnimSpeed", 0);
        }

        private void OnDisable()
        {
            move.action.started -= AnimateLegs;
            move.action.canceled -= StopAnimation;
        }
    }
}
