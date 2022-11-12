using System;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [Header("Parameters")] [SerializeField]
    private LayerMask layerMask;

    [SerializeField] private Vector3 hitPoint;

    private float _collisionForce;
    private Rigidbody _rigidBody;

    [Space] [Header("Particle Effects")] [SerializeField]
    private GameObject hitParticle;

    [SerializeField] private GameObject stasisHitParticle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position + transform.forward, -transform.forward, out hit, 8,
                layerMask)) return;
        if (hit.transform.GetComponent<StasisObject>() == null)
            return;

        hitPoint = hit.point;
        _collisionForce = _rigidBody.velocity.magnitude;
        hit.transform.GetComponent<StasisObject>().AccumulateForce(_collisionForce, hit.point);

        Instantiate(hitParticle, hit.point, Quaternion.identity);

        if (!hit.transform.GetComponent<StasisObject>().activated)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (!hit.transform.GetComponent<StasisObject>().activated) return;

        GameObject stasisHit = Instantiate(stasisHitParticle, hit.point, Quaternion.identity);
        ParticleSystem[] stasisParticles = stasisHit.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in stasisParticles)
        {
            var mainParticle = particle.main;
            mainParticle.startColor = hit.transform.GetComponent<StasisObject>().particleColor;
        }

        var main = stasisHit.GetComponent<ParticleSystem>().main;
        main.startColor = hit.transform.GetComponent<StasisObject>().particleColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var transform1 = transform;
        var forward = transform1.forward;
        var ray = new Ray(transform1.position + forward, -forward);
        Gizmos.DrawRay(ray);

        Gizmos.DrawSphere(hitPoint, .1f);
    }
}