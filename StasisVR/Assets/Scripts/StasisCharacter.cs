using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using System;

public class StasisCharacter : MonoBehaviour
{
    [Header("Collision")] public LayerMask layerMask;

    public Animator anim;

    [Space] [Header("Aim and Zoom")] public bool stasisAim;
    public float zoomDuration = .3f;
    private float originalFOV;
    public float zoomFOV;
    private Vector3 originalCameraOffset;
    public Vector3 zoomCameraOffset;

    [Space] [Header("Target")] public Transform target;

    [Space] [Header("Colors")] public Color highligthedColor;
    public Color normalColor;
    public Color finalColor;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StasisAim(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            StasisAim(false);
        }

        if (stasisAim)
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (target != hit.transform)
                {
                    if (target != null && target.GetComponent<StasisObject>() != null)
                        target.GetComponent<Renderer>().material.SetColor("_EmissionColor", normalColor);

                    target = hit.transform;
                    if (target.GetComponent<StasisObject>() != null)
                    {
                        if (!target.GetComponent<StasisObject>().activated)
                            target.GetComponent<Renderer>().material.SetColor("_EmissionColor", highligthedColor);
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                if (target != null)
                {
                    if (target.GetComponent<StasisObject>() != null)
                        if (!target.GetComponent<StasisObject>().activated)
                            target.GetComponent<Renderer>().material.SetColor("_EmissionColor", normalColor);

                    target = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!stasisAim)
            {
                anim.SetTrigger("slash");
                StartCoroutine(WaitFrame());
            }
            else
            {
                if (target != null && target.GetComponent<StasisObject>() != null)
                {
                    bool stasis = target.GetComponent<StasisObject>().activated;
                    target.GetComponent<StasisObject>().SetStasis(!stasis);
                    StasisAim(false);
                }
            }
        }
    }


    IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();
        if (!anim.GetBool("attacking"))
            anim.SetBool("attacking", true);
    }

    void StasisAim(bool state)
    {
        stasisAim = state;
        float fov = state ? zoomFOV : originalFOV;
        Vector3 offset = state ? zoomCameraOffset : originalCameraOffset;
        float stasisEffect = state ? .4f : 0;

        StasisObject[] stasisObjects = FindObjectsOfType<StasisObject>();
        foreach (StasisObject obj in stasisObjects)
        {
            if (!obj.activated)
            {
                obj.GetComponent<Renderer>().material.SetColor("_EmissionColor", normalColor);
                obj.GetComponent<Renderer>().material.SetFloat("_StasisAmount", stasisEffect);
            }
        }

    }
}