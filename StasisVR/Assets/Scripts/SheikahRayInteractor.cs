using System;
using UnityEngine;

    public class SheikahRayInteractor : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private ActivateSheikah sheikah;

        private StasisObject _stasisObject;
        private MeshRenderer _meshRenderer;
        private Material _oldMaterial;
        private bool _highlightApplied;

        private void Update()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.localPosition, transform.TransformDirection(Vector3.forward), out hit, 100, layerMask))
            {
                if (_highlightApplied && sheikah.hasActivated)
                {
                    _stasisObject = hit.transform.gameObject.GetComponent<StasisObject>();
                    _stasisObject.SetStasis(true);
                    sheikah.hasActivated = false;
                }
                
                if (_highlightApplied) return;
                _meshRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                _oldMaterial = _meshRenderer.material;
                _meshRenderer.material = highlightMaterial;
                // _stasisObject = hit.transform.gameObject.GetComponent<StasisObject>();
                // _stasisObject.SetStasis(true);
                _highlightApplied = true;
            }
            else
            {
                if (!_highlightApplied) return;
                DisableMeshRender();
                _highlightApplied = false;
            }
        }

        private void OnDisable()
        {
            if (_meshRenderer == null) return;
            DisableMeshRender();
        }

        private void DisableMeshRender()
        {
            _meshRenderer.material = _oldMaterial;
            _meshRenderer = null;
            _highlightApplied = false;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            Gizmos.DrawRay(transform.position, direction);
        }
    }