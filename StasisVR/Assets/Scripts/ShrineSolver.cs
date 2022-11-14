using System;
using UnityEngine;

public class ShrineSolver : MonoBehaviour
{
    [SerializeField] private float objectCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StasisObject>())
        {
            objectCounter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<StasisObject>())
        {
            objectCounter--;
        }
    }
}