using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CurrentSettings : MonoBehaviour
{

    //Bindings
    public InputBinding RotateCameraRight;
    public InputBinding RotateCameraLeft;
    public InputBinding MoveRight;
    public InputBinding MoveLeft;
    public InputBinding Jump;
    public InputBinding PickupObject;
    public InputBinding ThrowObject;

    //Audio
    public bool MasterOn = true;
    public float MasterVolume = 1f;
    public bool MusicMute = true;
    public float MusicVolume = 1f;
    public bool SoundEffectsMute = false;
    public float SoundEffectsVolume = 1f;

    //Graphics
    public bool BloomOn = false;
    public float BloomIntensity = 0f;
    public float BloomThreshold = 0f;
    public float BloomSoftKnee = 0f;
    public float BloomClamp = 0f;
    public float BloomAnamorphicRatio = 0f;
    public float BloomDiffusion = 0f;

    public bool AmbientOcclusionOn = false;
    public float AmbientOcclusionIntensity = 0f;
    public float AmbientOcclusionThickness = 0f;

    public bool ChromaticAberrationOn;
    public float ChromaticAberrationIntensity;

    public bool GrainOn;
    public float GrainIntensity;
    public float GrainSize;
    public float GrainLuminanceContribution;

    public bool VignetteOn;
    public float VignetteIntensity;
    public float VignetteSmoothness;
    public float VignetteRoundness;

    public bool MotionBlurOn;
    public float MotionBlurShutterAngle;
    public float MotionBlurSampleCount;

    public bool LensDistortionOn;
    public float LensDistortionIntensity;
    public float LensDistortionXMultiplier;
    public float LensDistortionYMultiplier;
    public float LensDistortionCenterX;
    public float LensDistortionCenterY;
    public float LensDistortionScale;

    public bool DepthOfFieldOn;
    public float DepthOfFieldFocusDistance;
    public float DepthOfFieldAperture;
    public float DepthOfFieldFocalLength;

    public ChangeSound changeSound;
    public ChangeGraphics changeGraphics;
    public RebindingDisplay rebindingDisplay;

    [SerializeField] private InputAction Action;
    [SerializeField] private PlayerController playerController;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public PlayerActionsScript.PlayerActions script;

    void Start()
    {
        MasterOn = true;
        MasterVolume = 1f;
        MusicMute = true;
        MusicVolume = 1f;
        SoundEffectsMute = false;
        SoundEffectsVolume = 1f;

        BloomOn = false;
    }

    void Awake()
    {

    }

    void Update()
    {
        try
        {
            changeSound.masterToggle.isOn = MasterOn;
            changeSound.masterVolumeSlider.value = MasterVolume;

            changeSound.musicToggle.isOn = !MusicMute;
            changeSound.musicVolumeSlider.value = MusicVolume;

            changeSound.soundEffectsToggle.isOn = !SoundEffectsMute;
            changeSound.soundEffectsVolumeSlider.value = SoundEffectsVolume;

            changeGraphics.bloomToggle.isOn = BloomOn;
            changeGraphics.bloomIntensitySlider.value = BloomIntensity/10;
            changeGraphics.bloomThresholdSlider.value = BloomThreshold/10;
            changeGraphics.bloomSoftKneeSlider.value = BloomSoftKnee;
            changeGraphics.bloomClampSlider.value = BloomClamp/100;
            changeGraphics.bloomDiffusionSlider.value = BloomDiffusion/10;
            changeGraphics.bloomAnamorphicRatioSlider.value = BloomAnamorphicRatio;

            changeGraphics.ambientOcclusionToggle.isOn = AmbientOcclusionOn;
            changeGraphics.ambientOcclusionIntensitySlider.value = AmbientOcclusionIntensity/4;
            changeGraphics.ambientOcclusionThicknessSlider.value = AmbientOcclusionThickness/10;

            changeGraphics.chromaticAberrationToggle.isOn = ChromaticAberrationOn;
            changeGraphics.chromaticAberrationIntesitySlider.value = ChromaticAberrationIntensity;

            changeGraphics.grainToggle.isOn = GrainOn;
            changeGraphics.grainIntesitySlider.value = GrainIntensity;
            changeGraphics.grainSizeSlider.value = GrainSize/3;
            changeGraphics.grainLuminanceContributionSlider.value = GrainLuminanceContribution;

            changeGraphics.vignetteToggle.isOn = VignetteOn;
            changeGraphics.vignetteIntesitySlider.value = VignetteIntensity;
            changeGraphics.vignetteSmoothnessSlider.value = VignetteSmoothness;
            changeGraphics.vignetteRoundnessSlider.value = VignetteRoundness;

            changeGraphics.motionBlurToggle.isOn = MotionBlurOn;
            changeGraphics.motionBlurShutterAngleSlider.value = MotionBlurShutterAngle;
            changeGraphics.motionBlurSampleCountSlider.value = (int) MotionBlurSampleCount;

            changeGraphics.lensDistortionToggle.isOn = LensDistortionOn;
            changeGraphics.lensDistortionIntensitySlider.value = LensDistortionIntensity;
            changeGraphics.lensDistortionXMultiplierSlider.value = LensDistortionXMultiplier;
            changeGraphics.lensDistortionYMultiplierSlider.value = LensDistortionYMultiplier;
            changeGraphics.lensDistortionCenterXSlider.value = LensDistortionCenterX;
            changeGraphics.lensDistortionCenterYSlider.value = LensDistortionCenterY;
            changeGraphics.lensDistortionScaleSlider.value = LensDistortionScale;

            changeGraphics.depthOfFieldToggle.isOn = DepthOfFieldOn;
            changeGraphics.depthOfFieldFocusDistanceSlider.value = DepthOfFieldFocusDistance;
            changeGraphics.depthOfFieldApertureSlider.value = DepthOfFieldAperture;
            changeGraphics.depthOfFieldFocalLengthSlider.value = DepthOfFieldFocalLength;

            rebindingDisplay.camRight.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(RotateCameraRight.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            rebindingDisplay.camLeft.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(RotateCameraLeft.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            rebindingDisplay.movRight.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(MoveRight.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            rebindingDisplay.movLeft.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(MoveLeft.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            rebindingDisplay.throwObj.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(ThrowObject.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            rebindingDisplay.pickupObj.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(PickupObject.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            rebindingDisplay.jumpText.GetComponent<TMPro.TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(Jump.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        } catch {
        }



    }
}
