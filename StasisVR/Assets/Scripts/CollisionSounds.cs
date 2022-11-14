using System;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fallLow;
    [SerializeField] private AudioClip fallMedium;
    [SerializeField] private AudioClip fallHigh;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            // Nothing
            return;
        }
        else
        {
            switch (collision.relativeVelocity.magnitude)
            {
                case >= 1 and < 10:
                    audioSource.PlayOneShot(fallLow);
                    break;
                case >= 10 and < 15:
                    audioSource.PlayOneShot(fallMedium);
                    break;
                case >= 15:
                    audioSource.PlayOneShot(fallHigh);
                    break;
            }
        }
    }
}