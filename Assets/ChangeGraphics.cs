using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class ChangeGraphics : MonoBehaviour
{
    public PostProcessVolume Volume;

    // Bloom variables
    private Bloom bloomEffect;
    public Camera cam;
    public Toggle bloomToggle;
    public Slider bloomIntensitySlider;
    public Slider bloomThresholdSlider;
    public Slider bloomSoftKneeSlider;
    public Slider bloomClampSlider;
    public Slider bloomDiffusionSlider;
    public Slider bloomAnamorphicRatioSlider;
    public GameObject bloomSettings;

    // Ambient Occlusion Variables
    private AmbientOcclusion ambientOcclusionEffect;
    public Toggle ambientOcclusionToggle;
    public Slider ambientOcclusionIntensitySlider;
    public Slider ambientOcclusionThicknessSlider;
    public GameObject ambientOcclusionSettings;

    // Auto Exposure Variables
    private AutoExposure autoExposureEffect;
    public Toggle autoExposureToggle;
    public Slider autoExposureFilteringSlider;
    public Slider autoExposureMinimumEVSlider;
    public Slider autoExposureMaximumEVSlider;
    public Slider autoExposureExposureCompensationSlider;
    public GameObject autoExposureSettings;

    // Chromatic Aberration Variables
    private ChromaticAberration chromaticAberrationEffect;
    public Toggle chromaticAberrationToggle;
    public Slider chromaticAberrationIntesitySlider;
    public GameObject chromaticAberrationSettings;

    // Grain Variables
    private Grain grainEffect;
    public Toggle grainToggle;
    public Slider grainIntesitySlider;
    public Slider grainSizeSlider;
    public Slider grainLuminanceContributionSlider;
    public GameObject grainSettings;

    // Vignette Variables
    private Vignette vignetteEffect;
    public Toggle vignetteToggle;
    public Slider vignetteIntesitySlider;
    public Slider vignetteSmoothnessSlider;
    public Slider vignetteRoundnessSlider;
    public GameObject vignetteSettings;

    // Motion Blur Variables
    private MotionBlur motionBlurEffect;
    public Toggle motionBlurToggle;
    public Slider motionBlurShutterAngleSlider;
    public Slider motionBlurSampleCountSlider;
    public GameObject motionBlurSettings;

    // Lens Distortion Variables
    private LensDistortion lensDistortionEffect;
    public Toggle lensDistortionToggle;
    public Slider lensDistortionIntensitySlider;
    public Slider lensDistortionXMultiplierSlider;
    public Slider lensDistortionYMultiplierSlider;
    public Slider lensDistortionCenterXSlider;
    public Slider lensDistortionCenterYSlider;
    public Slider lensDistortionScaleSlider;
    public GameObject lensDistortionSettings;

    // Depth of Field Variables
    private DepthOfField depthOfFieldEffect;
    public Toggle depthOfFieldToggle;
    public Slider depthOfFieldFocusDistanceSlider;
    public Slider depthOfFieldApertureSlider;
    public Slider depthOfFieldFocalLengthSlider;
    public GameObject depthOfFieldSettings;

    public CurrentSettings currentSettings;

    private void Update()
    {
        currentSettings = GameObject.FindGameObjectWithTag("CurrentSettings").GetComponent<CurrentSettings>();
        currentSettings.changeGraphics = this.gameObject.GetComponent<ChangeGraphics>();
    }

    private void Start()
    {
        Volume = Volume.GetComponent<PostProcessVolume>();
        Volume.profile.TryGetSettings(out bloomEffect);
        Volume.profile.TryGetSettings(out ambientOcclusionEffect);
        Volume.profile.TryGetSettings(out autoExposureEffect);
        Volume.profile.TryGetSettings(out chromaticAberrationEffect);
        Volume.profile.TryGetSettings(out grainEffect);
        Volume.profile.TryGetSettings(out vignetteEffect);
        Volume.profile.TryGetSettings(out motionBlurEffect);
        Volume.profile.TryGetSettings(out lensDistortionEffect);
        Volume.profile.TryGetSettings(out depthOfFieldEffect);

        bloomToggle = bloomToggle.GetComponent<Toggle>();
        bloomIntensitySlider = bloomIntensitySlider.GetComponent<Slider>();
        bloomThresholdSlider = bloomThresholdSlider.GetComponent<Slider>();
        bloomSoftKneeSlider = bloomSoftKneeSlider.GetComponent<Slider>();
        bloomClampSlider = bloomClampSlider.GetComponent<Slider>();
        bloomDiffusionSlider = bloomDiffusionSlider.GetComponent<Slider>();
        bloomAnamorphicRatioSlider = bloomAnamorphicRatioSlider.GetComponent<Slider>();

        ambientOcclusionIntensitySlider = ambientOcclusionIntensitySlider.GetComponent<Slider>();
        ambientOcclusionThicknessSlider = ambientOcclusionThicknessSlider.GetComponent<Slider>();

        chromaticAberrationIntesitySlider = chromaticAberrationIntesitySlider.GetComponent<Slider>();

        vignetteIntesitySlider = vignetteIntesitySlider.GetComponent<Slider>();
        vignetteSmoothnessSlider = vignetteSmoothnessSlider.GetComponent<Slider>();
        vignetteRoundnessSlider = vignetteRoundnessSlider.GetComponent<Slider>();

        motionBlurShutterAngleSlider = motionBlurShutterAngleSlider.GetComponent<Slider>();
        motionBlurSampleCountSlider = motionBlurSampleCountSlider.GetComponent<Slider>();

        lensDistortionIntensitySlider = lensDistortionIntensitySlider.GetComponent<Slider>();
        lensDistortionXMultiplierSlider = lensDistortionXMultiplierSlider.GetComponent<Slider>();
        lensDistortionYMultiplierSlider = lensDistortionYMultiplierSlider.GetComponent<Slider>();
        lensDistortionCenterXSlider = lensDistortionCenterXSlider.GetComponent<Slider>();
        lensDistortionCenterYSlider = lensDistortionCenterYSlider.GetComponent<Slider>();
        lensDistortionScaleSlider = lensDistortionScaleSlider.GetComponent<Slider>();
    }

    //Bloom Methods
    
    public void ShowBloomSettings()
    {
        if(bloomSettings.active)
        {
            bloomSettings.SetActive(false);
        } else
        {
            bloomSettings.SetActive(true);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            grainSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleBloom()
    {
        if(bloomToggle.isOn)
        {
            bloomEffect.active = true;
            currentSettings.BloomOn = true;
        } else {
            bloomEffect.active = false;
            currentSettings.BloomOn = false;
        }

    }

    public void BloomIntensity()
    {
        bloomEffect.intensity.value = bloomIntensitySlider.value*10;
        currentSettings.BloomIntensity = bloomIntensitySlider.value*10;
    }

    public void BloomThreshold()
    {
        bloomEffect.threshold.value = bloomThresholdSlider.value*10;
        currentSettings.BloomThreshold = bloomThresholdSlider.value*10;
    }

    public void BloomSoftKnee()
    {
        bloomEffect.softKnee.value = bloomSoftKneeSlider.value;
        currentSettings.BloomSoftKnee = bloomSoftKneeSlider.value;
    }

    public void BloomClamp()
    {
        bloomEffect.clamp.value = bloomClampSlider.value*100;
        currentSettings.BloomClamp = bloomClampSlider.value*100;
    }

    public void BloomDiffusion()
    {
        bloomEffect.diffusion.value = bloomDiffusionSlider.value*10;
        currentSettings.BloomDiffusion = bloomDiffusionSlider.value*10;
    }

    public void BloomAnamorphicRatio()
    {
        bloomEffect.anamorphicRatio.value = bloomAnamorphicRatioSlider.value;
        currentSettings.BloomAnamorphicRatio = bloomAnamorphicRatioSlider.value;
    }

    //AO Methods

    public void ShowAmbientOcclusionSettings()
    {
        if(ambientOcclusionSettings.active)
        {
            ambientOcclusionSettings.SetActive(false);
        } else
        {
            ambientOcclusionSettings.SetActive(true);
            bloomSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            grainSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }
    }

    public void ToggleAmbientOcclusion()
    {
        if(ambientOcclusionToggle.isOn)
        {
            ambientOcclusionEffect.active = true;
            currentSettings.AmbientOcclusionOn = true;
        } else {
            ambientOcclusionEffect.active = false;
            currentSettings.AmbientOcclusionOn = false;
        }

    }

    public void AmbientOcclusionIntensity()
    {
        ambientOcclusionEffect.intensity.value = ambientOcclusionIntensitySlider.value*4;
        currentSettings.AmbientOcclusionIntensity = ambientOcclusionIntensitySlider.value*4;
    }

    public void AmbientOcclusionThickness()
    {
        ambientOcclusionEffect.thicknessModifier.value = ambientOcclusionThicknessSlider.value*10;
        currentSettings.AmbientOcclusionThickness = ambientOcclusionThicknessSlider.value*10;
    }

    //Auto Exposure Methods

    public void ShowAutoExposureSettings()
    {
        if(autoExposureSettings.active)
        {
            autoExposureSettings.SetActive(false);
        } else
        {
            autoExposureSettings.SetActive(true);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            grainSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleAutoExposure()
    {
        if(autoExposureToggle.isOn)
        {
            autoExposureEffect.active = true;
        } else {
            autoExposureEffect.active = false;
        }

    }

    public void AutoExposureFiltering()
    {
        var test = new Vector2(0, autoExposureFilteringSlider.value);
        autoExposureEffect.filtering.value = test;
    }

    public void AutoExposureMinimumEV()
    {
        autoExposureEffect.minLuminance.value = autoExposureMinimumEVSlider.value;
    }

    public void AutoExposureMaximumEV()
    {
        autoExposureEffect.maxLuminance.value = autoExposureMaximumEVSlider.value;
    }

    public void AutoExposureExposureCompensation()
    {
        autoExposureEffect.keyValue.value = autoExposureExposureCompensationSlider.value;
    }

    //Chromatic Aberration Methods

    public void ShowChromaticAberrationSettings()
    {
        if(chromaticAberrationSettings.active)
        {
            chromaticAberrationSettings.SetActive(false);
        } else
        {
            chromaticAberrationSettings.SetActive(true);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            grainSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleChromaticAberration()
    {
        if(chromaticAberrationToggle.isOn)
        {
            chromaticAberrationEffect.active = true;
            currentSettings.ChromaticAberrationOn = true;
        } else {
            chromaticAberrationEffect.active = false;
            currentSettings.ChromaticAberrationOn = false;
        }

    }

    public void ChromaticAberrationIntensity()
    {
        chromaticAberrationEffect.intensity.value = chromaticAberrationIntesitySlider.value;
        currentSettings.ChromaticAberrationIntensity = chromaticAberrationIntesitySlider.value;
    }

    //Grain Methods

    public void ShowGrainSettings()
    {
        if(grainSettings.active)
        {
            grainSettings.SetActive(false);
        } else
        {
            grainSettings.SetActive(true);
            chromaticAberrationSettings.SetActive(false);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleGrain()
    {
        if(grainToggle.isOn)
        {
            grainEffect.active = true;
            currentSettings.GrainOn = true;
        } else {
            grainEffect.active = false;
            currentSettings.GrainOn = false;
        }

    }

    public void GrainIntensity()
    {
        grainEffect.intensity.value = grainIntesitySlider.value;
        currentSettings.GrainIntensity = grainIntesitySlider.value;
    }

    public void GrainSize()
    {
        grainEffect.size.value = grainSizeSlider.value*3;
        currentSettings.GrainSize = grainSizeSlider.value*3;
    }

    public void GrainLuminanceContribution()
    {
        grainEffect.lumContrib.value = grainLuminanceContributionSlider.value;
        currentSettings.GrainLuminanceContribution = grainLuminanceContributionSlider.value;
    }

    //Vignette Methods

    public void ShowVignetteSettings()
    {
        if(vignetteSettings.active)
        {
            vignetteSettings.SetActive(false);
        } else
        {
            vignetteSettings.SetActive(true);
            grainSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleVignette()
    {
        if(vignetteToggle.isOn)
        {
            vignetteEffect.active = true;
            currentSettings.VignetteOn = true;
        } else {
            vignetteEffect.active = false;
            currentSettings.VignetteOn = false;
        }

    }

    public void VignetteIntensity()
    {
        vignetteEffect.intensity.value = vignetteIntesitySlider.value;
        currentSettings.VignetteIntensity = vignetteIntesitySlider.value;
    }

    public void VignetteSmoothness()
    {
        vignetteEffect.smoothness.value = vignetteSmoothnessSlider.value;
        currentSettings.VignetteSmoothness = vignetteSmoothnessSlider.value;
    }

    public void VignetteRoundness()
    {
        vignetteEffect.roundness.value = vignetteRoundnessSlider.value;
        currentSettings.VignetteRoundness = vignetteRoundnessSlider.value;
    }

    //Motion Blur Methods

    public void ShowMotionBlurSettings()
    {
        if(motionBlurSettings.active)
        {
            motionBlurSettings.SetActive(false);
        } else
        {
            motionBlurSettings.SetActive(true);
            vignetteSettings.SetActive(false);
            grainSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            lensDistortionSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleMotionBlur()
    {
        if(motionBlurToggle.isOn)
        {
            motionBlurEffect.active = true;
            currentSettings.MotionBlurOn = true;
        } else {
            motionBlurEffect.active = false;
            currentSettings.MotionBlurOn = false;
        }

    }

    public void MotionBlurShutterAngle()
    {
        motionBlurEffect.shutterAngle.value = motionBlurShutterAngleSlider.value;
        currentSettings.MotionBlurShutterAngle = motionBlurShutterAngleSlider.value;
    }

    public void MotionBlurSampleCount()
    {
        motionBlurEffect.sampleCount.value = (int) motionBlurSampleCountSlider.value;
        currentSettings.MotionBlurSampleCount = (int) motionBlurSampleCountSlider.value;
    }

    //Lens Distortion Methods

    public void ShowLensDistortionSettings()
    {
        if(lensDistortionSettings.active)
        {
            lensDistortionSettings.SetActive(false);
        } else
        {
            lensDistortionSettings.SetActive(true);
            motionBlurSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            grainSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
            depthOfFieldSettings.SetActive(false);
        }

    }

    public void ToggleLensDistortion()
    {
        if(lensDistortionToggle.isOn)
        {
            lensDistortionEffect.active = true;
            currentSettings.LensDistortionOn = true;
        } else {
            lensDistortionEffect.active = false;
            currentSettings.LensDistortionOn = false;
        }

    }

    public void LensDistortionIntensity()
    {
        lensDistortionEffect.intensity.value = lensDistortionIntensitySlider.value;
        currentSettings.LensDistortionIntensity = lensDistortionIntensitySlider.value;
    }

    public void LensDistortionXMultiplier()
    {
        lensDistortionEffect.intensityX.value = lensDistortionXMultiplierSlider.value;
        currentSettings.LensDistortionXMultiplier = lensDistortionXMultiplierSlider.value;
    }

    public void LensDistortionYMultiplier()
    {
        lensDistortionEffect.intensityY.value = lensDistortionYMultiplierSlider.value;
        currentSettings.LensDistortionYMultiplier = lensDistortionYMultiplierSlider.value;
    }

    public void LensDistortionCenterX()
    {
        lensDistortionEffect.centerX.value = lensDistortionCenterXSlider.value;
        currentSettings.LensDistortionCenterX = lensDistortionCenterXSlider.value;
    }

    public void LensDistortionCenterY()
    {
        lensDistortionEffect.centerY.value = lensDistortionCenterYSlider.value;
        currentSettings.LensDistortionCenterY = lensDistortionCenterYSlider.value;
    }

    public void LensDistortionScale()
    {
        lensDistortionEffect.scale.value = lensDistortionScaleSlider.value;
        currentSettings.LensDistortionScale = lensDistortionScaleSlider.value;
    }

    //Depth of Field Methods

    public void ShowDepthOfFieldSettings()
    {
        if(depthOfFieldSettings.active)
        {
            depthOfFieldSettings.SetActive(false);
        } else
        {
            depthOfFieldSettings.SetActive(true);
            lensDistortionSettings.SetActive(false);
            motionBlurSettings.SetActive(false);
            vignetteSettings.SetActive(false);
            grainSettings.SetActive(false);
            chromaticAberrationSettings.SetActive(false);
            bloomSettings.SetActive(false);
            ambientOcclusionSettings.SetActive(false);
            autoExposureSettings.SetActive(false);
        }

    }

    public void ToggleDepthOfField()
    {
        if(depthOfFieldToggle.isOn)
        {
            depthOfFieldEffect.active = true;
            currentSettings.DepthOfFieldOn = true;
        } else {
            depthOfFieldEffect.active = false;
            currentSettings.DepthOfFieldOn = false;
        }

    }

    public void DepthOfFieldFocusDistance()
    {
        depthOfFieldEffect.focusDistance.value = depthOfFieldFocusDistanceSlider.value;
        currentSettings.DepthOfFieldFocusDistance = depthOfFieldFocusDistanceSlider.value;
    }

    public void DepthOfFieldAperture()
    {
        depthOfFieldEffect.aperture.value = depthOfFieldApertureSlider.value;
        currentSettings.DepthOfFieldAperture = depthOfFieldApertureSlider.value;
    }

    public void DepthOfFieldFocalLength()
    {
        depthOfFieldEffect.focalLength.value = depthOfFieldFocalLengthSlider.value;
        currentSettings.DepthOfFieldFocalLength = depthOfFieldApertureSlider.value;
    }

}
