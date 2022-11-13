using UnityEngine;
using UnityEngine.InputSystem;

namespace BearingPoint.Avatars.Local.RPM
{
    public class HandAnimationController : MonoBehaviour
    {
        [SerializeField] private InputActionReference index;
        [SerializeField] private InputActionReference grab;

        [SerializeField] private Animator animator;

        private void OnEnable()
        {
            index.action.started += AnimateIndex;
            index.action.canceled += StopIndex;

            grab.action.started += AnimateGrab;
            grab.action.canceled += StopGrab;
        }

        private void AnimateIndex(InputAction.CallbackContext obj)
        {
            animator.SetFloat("Trigger", 1);
        }

        private void StopIndex(InputAction.CallbackContext obj)
        {
            animator.SetFloat("Trigger", 0);
        }

        private void AnimateGrab(InputAction.CallbackContext obj)
        {
            animator.SetFloat("Grip", 1);
        }

        private void StopGrab(InputAction.CallbackContext obj)
        {
            animator.SetFloat("Grip", 0);
        }

        private void OnDisable()
        {
            index.action.started -= AnimateIndex;
            index.action.canceled -= StopIndex;

            grab.action.started -= AnimateGrab;
            grab.action.canceled -= StopGrab;
        }
    }
}