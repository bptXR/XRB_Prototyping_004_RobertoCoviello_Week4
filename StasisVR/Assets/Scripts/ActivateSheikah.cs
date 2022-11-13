using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateSheikah : XRGrabInteractable
{
    [SerializeField] private XRDirectInteractor rightHand;
    [SerializeField] private XRDirectInteractor leftHand;

    [SerializeField] private SheikahRayInteractor sheikahRayInteractor;
    [SerializeField] private UniversalRendererData renderSettings;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource bulletSound;

    public bool hasActivated;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject == leftHand || args.interactorObject == rightHand)
        {
            RenderSettings(true);
            sheikahRayInteractor.enabled = true;
        }
        else
        {
            RenderSettings(false);
            sheikahRayInteractor.enabled = false;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        RenderSettings(false);
        sheikahRayInteractor.enabled = false;
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);

        hasActivated = true;
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);

        hasActivated = false;
    }

    private void RenderSettings(bool enable)
    {
        renderSettings.rendererFeatures[0].SetActive(enable);
        renderSettings.rendererFeatures[1].SetActive(enable);
        renderSettings.rendererFeatures[2].SetActive(enable);
    }
}