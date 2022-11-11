using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StasisObject : MonoBehaviour
{
    public bool activated;
    public Color particleColor;
    
    [SerializeField] private float accumulatedForce;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 hitPoint;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color finalColor;
    [SerializeField] private Transform arrowIndicator;
    [SerializeField] private AudioClip stasisSound;
    [SerializeField] private AudioClip stasisStart;
    [SerializeField] private AudioClip stasisEnd;

    private float _forceLimit = 100;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;
    private MeshRenderer _renderer;

    [Header("Particles")] 
    public Transform startParticleGroup;
    public Transform endParticleGroup;
    
    private static readonly int StasisAmount = Shader.PropertyToID("_StasisAmount");
    private static readonly int NoiseAmount = Shader.PropertyToID("_NoiseAmount");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _renderer = GetComponent<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetStasis(bool state)
    {
        activated = state;
        _rigidbody.isKinematic = state;
        float noise = state ? 1 : 0;

        _renderer.material.SetFloat(StasisAmount, 10);
        _renderer.material.SetFloat(NoiseAmount, noise);
        _renderer.material.DOFloat(1, NoiseAmount, 0.5f);

        switch (state)
        {
            case true:
            {
                _audioSource.PlayOneShot(stasisStart);
                _renderer.material.SetColor(EmissionColor, normalColor);
                StartCoroutine(StasisCount());
            
                startParticleGroup.LookAt(playerPosition);
                ParticleSystem[] particles = startParticleGroup.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem p in particles)
                {
                    p.Play();
                }

                break;
            }
            case false:
            {
                StopAllCoroutines();
                DOTween.KillAll();
                transform.GetChild(0).gameObject.SetActive(false);
                _renderer.material.SetFloat(StasisAmount, 0);

                endParticleGroup.LookAt(playerPosition);
                ParticleSystem[] particles = endParticleGroup.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem p in particles)
                {
                    p.Play();
                }

                if (accumulatedForce < 0) return;
            
                direction = transform.position - hitPoint;
                _rigidbody.AddForceAtPosition(direction * accumulatedForce, hitPoint, ForceMode.Impulse);
                accumulatedForce = 0;
                _audioSource.PlayOneShot(stasisEnd);

                _trailRenderer.startColor = particleColor;
                _trailRenderer.endColor = particleColor;
                _trailRenderer.emitting = true;
                _trailRenderer.DOTime(0, 5).OnComplete(() => _trailRenderer.emitting = false);
                break;
            }
        }
    }

    public void AccumulateForce(float amount, Vector3 point)
    {
        if (!activated) return;

        arrowIndicator.gameObject.SetActive(true);
        float scale = Mathf.Min(arrowIndicator.localScale.z + .3f, 1.8f);
        transform.GetChild(0).DOScaleZ(scale, .15f).SetEase(Ease.OutBack);

        accumulatedForce = Mathf.Min(accumulatedForce += amount, _forceLimit);
        hitPoint = point;

        direction = transform.position - hitPoint;
        transform.GetChild(0).rotation = Quaternion.LookRotation(direction);

        Color c = Color.Lerp(normalColor, finalColor, accumulatedForce / 50);
        transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material.SetColor(Color1, c);
        _renderer.material.SetColor(EmissionColor, c);
        particleColor = c;
    }

    private IEnumerator StasisCount()
    {
        for (int i = 0; i < 20; i++)
        {
            float wait = 1;

            if (i > 4)
                wait = .5f;

            if (i > 12)
                wait = .25f;

            yield return new WaitForSeconds(wait);
            _audioSource.PlayOneShot(stasisSound);
            Sequence s = DOTween.Sequence();
            s.Append(_renderer.material.DOFloat(.5f, StasisAmount, .05f));
            s.AppendInterval(.1f);
            s.Append(_renderer.material.DOFloat(.2f, StasisAmount, .05f));
        }
        
        SetStasis(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var transform1 = transform;
        var position = transform1.position;
        Gizmos.DrawLine(position, position - direction);
    }
}