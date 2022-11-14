using System;
using UnityEngine;

public class ShrineSolver : MonoBehaviour
{
    [SerializeField] private float objectCounter = 0;
    [SerializeField] private float counterThreshold = 2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shrineSolvedSound;
    [SerializeField] private GameObject[] particleEffects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StasisObject>())
        {
            objectCounter++;
            if (objectCounter != counterThreshold) return;
            audioSource.PlayOneShot(shrineSolvedSound);
            foreach (var p in particleEffects)
            {
                p.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<StasisObject>())
        {
            objectCounter--;
            if (objectCounter == counterThreshold) return;
            foreach (var p in particleEffects)
            {
                p.SetActive(false);
            }
        }
    }
}