using System;
using UnityEngine;

namespace RPM
{
    [Serializable]
    public class MapTransform
    {
        public Transform VRTarget;
        public Transform IKTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        public void MapVRAvatar()
        {
            IKTarget.position = VRTarget.TransformPoint(trackingPositionOffset);
            IKTarget.rotation = VRTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }

    public class AvatarController : MonoBehaviour
    {
        [SerializeField] private MapTransform head;
        [SerializeField] private MapTransform leftHand;
        [SerializeField] private MapTransform rightHand;

        [SerializeField] private float turnSmoothness;

        [SerializeField] private Transform IKHead;

        [SerializeField] private Vector3 headBodyOffset;

        // Change to work with Fusion FixedUpdateNetwork and Runner.DeltaTime
        void LateUpdate()
        {
            transform.position = IKHead.position + headBodyOffset;
            transform.forward = Vector3.Lerp(
                transform.forward,
                Vector3.ProjectOnPlane(IKHead.forward, Vector3.up).normalized,
                Time.deltaTime * turnSmoothness);
            ;
            head.MapVRAvatar();
            leftHand.MapVRAvatar();
            rightHand.MapVRAvatar();
        }
    }
}