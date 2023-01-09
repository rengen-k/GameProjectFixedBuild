using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class LoadSettings : MonoBehaviour
{
    [SerializeField] private InputAction Action;
    public CurrentSettings currentSettings;
    [SerializeField] private PlayerController playerController;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public PostProcessVolume Volume;
    private Bloom bloomEffect;
    private AmbientOcclusion ambientOcclusionEffect;
    private AutoExposure autoExposureEffect;
    private ChromaticAberration chromaticAberrationEffect;
    private Grain grainEffect;
    private Vignette vignetteEffect;
    private MotionBlur motionBlurEffect;
    private LensDistortion lensDistortionEffect;
    private DepthOfField depthOfFieldEffect;

    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        try
        {
            currentSettings = GameObject.FindGameObjectWithTag("CurrentSettings").GetComponent<CurrentSettings>();
        } catch {
            currentSettings = GameObject.FindGameObjectWithTag("LevelCurrentSettings").GetComponent<CurrentSettings>();
        }
        Volume = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();
        Volume.profile.TryGetSettings(out bloomEffect);
        Volume.profile.TryGetSettings(out ambientOcclusionEffect);
        Volume.profile.TryGetSettings(out autoExposureEffect);
        Volume.profile.TryGetSettings(out chromaticAberrationEffect);
        Volume.profile.TryGetSettings(out grainEffect);
        Volume.profile.TryGetSettings(out vignetteEffect);
        Volume.profile.TryGetSettings(out motionBlurEffect);
        Volume.profile.TryGetSettings(out lensDistortionEffect);
        Volume.profile.TryGetSettings(out depthOfFieldEffect);

        bloomEffect.active = currentSettings.BloomOn;
        bloomEffect.intensity.value = currentSettings.BloomIntensity;
        bloomEffect.threshold.value = currentSettings.BloomThreshold;
        bloomEffect.softKnee.value = currentSettings.BloomSoftKnee;
        bloomEffect.clamp.value = currentSettings.BloomClamp;
        bloomEffect.diffusion.value = currentSettings.BloomDiffusion;
        bloomEffect.anamorphicRatio.value = currentSettings.BloomAnamorphicRatio;

        ambientOcclusionEffect.active = currentSettings.AmbientOcclusionOn;
        ambientOcclusionEffect.intensity.value = currentSettings.AmbientOcclusionIntensity;
        ambientOcclusionEffect.thicknessModifier.value = currentSettings.AmbientOcclusionThickness;

        chromaticAberrationEffect.active = currentSettings.ChromaticAberrationOn;
        chromaticAberrationEffect.intensity.value = currentSettings.ChromaticAberrationIntensity;

        grainEffect.active = currentSettings.GrainOn;
        grainEffect.intensity.value = currentSettings.GrainIntensity;
        grainEffect.size.value = currentSettings.GrainSize;
        grainEffect.lumContrib.value = currentSettings.GrainLuminanceContribution;

        vignetteEffect.active = currentSettings.VignetteOn;
        vignetteEffect.intensity.value = currentSettings.VignetteIntensity;
        vignetteEffect.smoothness.value = currentSettings.VignetteSmoothness;
        vignetteEffect.roundness.value = currentSettings.VignetteRoundness;

        motionBlurEffect.active = currentSettings.MotionBlurOn;
        motionBlurEffect.shutterAngle.value = currentSettings.MotionBlurShutterAngle;
        motionBlurEffect.sampleCount.value = (int) currentSettings.MotionBlurSampleCount;

        lensDistortionEffect.active = currentSettings.LensDistortionOn;
        lensDistortionEffect.intensity.value = currentSettings.LensDistortionIntensity;
        lensDistortionEffect.intensityX.value = currentSettings.LensDistortionXMultiplier;
        lensDistortionEffect.intensityY.value = currentSettings.LensDistortionYMultiplier;
        lensDistortionEffect.centerX.value = currentSettings.LensDistortionCenterX;
        lensDistortionEffect.centerY.value = currentSettings.LensDistortionCenterY;
        lensDistortionEffect.scale.value = currentSettings.LensDistortionScale;

        depthOfFieldEffect.active = currentSettings.DepthOfFieldOn;
        depthOfFieldEffect.focusDistance.value = currentSettings.DepthOfFieldFocusDistance;
        depthOfFieldEffect.aperture.value = currentSettings.DepthOfFieldAperture;
        depthOfFieldEffect.focalLength.value = currentSettings.DepthOfFieldFocalLength;
        
        Action = playerController.playerActionsScript.Player.Jump;
        Action.Disable();
        Action.ApplyBindingOverride(currentSettings.Jump);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Look;
        Action.Disable();
        Action.ApplyBindingOverride(4, currentSettings.RotateCameraRight);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Look;
        Action.Disable();
        Action.ApplyBindingOverride(3, currentSettings.RotateCameraLeft);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Move;
        Action.Disable();
        Action.ApplyBindingOverride(7, currentSettings.MoveRight);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Move;
        Action.Disable();
        Action.ApplyBindingOverride(6, currentSettings.MoveLeft);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Fire;
        Action.Disable();
        Action.ApplyBindingOverride(5, currentSettings.ThrowObject);
        Action.Enable();

        Action = playerController.playerActionsScript.Player.Interact;
        Action.Disable();
        Action.ApplyBindingOverride(0, currentSettings.PickupObject);
        Action.Enable();
    }

}
