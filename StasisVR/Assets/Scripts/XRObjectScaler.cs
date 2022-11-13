using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
    [RequireComponent(typeof(XRSocketInteractor))]
    public class XRObjectScaler : MonoBehaviour
    {
        [SerializeField] private float scaleFactor = 1;

        private XRSocketInteractor _xrSocketInteractor;
        private Dictionary<Transform, Vector3> _cachedTransforms;

        private void Awake()
        {
            _xrSocketInteractor = GetComponent<XRSocketInteractor>();
            _cachedTransforms = new Dictionary<Transform, Vector3>();

            _xrSocketInteractor.selectEntered.AddListener(ApplyScale);
            _xrSocketInteractor.selectExited.AddListener(ResetScale);
        }

        private void OnDestroy()
        {
            _xrSocketInteractor.selectEntered.RemoveListener(ApplyScale);
            _xrSocketInteractor.selectExited.RemoveListener(ResetScale);
        }

        private void ApplyScale(SelectEnterEventArgs eventArgs)
        {
            Transform transformEntered = eventArgs.interactableObject.transform;
            Vector3 objectScaleOriginal = transformEntered.localScale;

            if (_cachedTransforms.ContainsKey(transformEntered)) return;

            _cachedTransforms.Add(transformEntered, objectScaleOriginal);
            transformEntered.localScale = objectScaleOriginal * scaleFactor;
        }

        private void ResetScale(SelectExitEventArgs eventArgs)
        {
            Transform transformExited = eventArgs.interactableObject.transform;

            if (!_cachedTransforms.ContainsKey(transformExited)) return;

            transformExited.localScale = _cachedTransforms[transformExited];
            _cachedTransforms.Remove(transformExited);
        }
    }
}