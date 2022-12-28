using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class ChangeGraphics : MonoBehaviour
{
    public PostProcessVolume Volume;

    //Bloom variables
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

    //Ambient Occlusion Variables
    private AmbientOcclusion ambientOcclusionEffect;
    public Toggle ambientOcclusionToggle;
    public Slider ambientOcclusionIntensitySlider;
    public Slider ambientOcclusionThicknessSlider;
    public GameObject ambientOcclusionSettings;

    //Auto Exposure Variables
    private AutoExposure autoExposureEffect;
    public Toggle autoExposureToggle;
    public Slider autoExposureFilteringSlider;
    public Slider autoExposureMinimumEVSlider;
    public Slider autoExposureMaximumEVSlider;
    public Slider autoExposureExposureCompensationSlider;
    public GameObject autoExposureSettings;

    //Chromatic Aberration Variables
    private ChromaticAberration chromaticAberrationEffect;
    public Toggle chromaticAberrationToggle;
    public Slider chromaticAberrationIntesitySlider;
    public GameObject chromaticAberrationSettings;

     //Grain Variables
    private Grain grainEffect;
    public Toggle grainToggle;
    public Slider grainIntesitySlider;
    public Slider grainSizeSlider;
    public Slider grainLuminanceContributionSlider;
    public GameObject grainSettings;

    //Vignette Variables
    private Vignette vignetteEffect;
    public Toggle vignetteToggle;
    public Slider vignetteIntesitySlider;
    public Slider vignetteSmoothnessSlider;
    public Slider vignetteRoundnessSlider;
    public GameObject vignetteSettings;

    //Motion Blur Variables
    private MotionBlur motionBlurEffect;
    public Toggle motionBlurToggle;
    public Slider motionBlurShutterAngleSlider;
    public Slider motionBlurSampleCountSlider;
    public GameObject motionBlurSettings;

    //Lens Distortion Variables
    private LensDistortion lensDistortionEffect;
    public Toggle lensDistortionToggle;
    public Slider lensDistortionIntensitySlider;
    public Slider lensDistortionXMultiplierSlider;
    public Slider lensDistortionYMultiplierSlider;
    public Slider lensDistortionCenterXSlider;
    public Slider lensDistortionCenterYSlider;
    public Slider lensDistortionScaleSlider;
    public GameObject lensDistortionSettings;

    //Depth of Field Variables
    private DepthOfField depthOfFieldEffect;
    public Toggle depthOfFieldToggle;
    public Slider depthOfFieldFocusDistanceSlider;
    public Slider depthOfFieldApertureSlider;
    public Slider depthOfFieldFocalLengthSlider;
    public GameObject depthOfFieldSettings;

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
        } else {
            bloomEffect.active = false;
        }

    }

    public void BloomIntensity()
    {
        bloomEffect.intensity.value = bloomIntensitySlider.value*10;
    }

    public void BloomThreshold()
    {
        bloomEffect.threshold.value = bloomThresholdSlider.value*10;
    }

    public void BloomSoftKnee()
    {
        bloomEffect.softKnee.value = bloomSoftKneeSlider.value;
    }

    public void BloomClamp()
    {
        bloomEffect.clamp.value = bloomClampSlider.value*100;
    }

    public void BloomDiffusion()
    {
        bloomEffect.diffusion.value = bloomDiffusionSlider.value*10;
    }

    public void BloomAnamorphicRatio()
    {
        bloomEffect.anamorphicRatio.value = bloomAnamorphicRatioSlider.value;
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
        } else {
            ambientOcclusionEffect.active = false;
        }

    }

    public void AmbientOcclusionIntensity()
    {
        ambientOcclusionEffect.intensity.value = ambientOcclusionIntensitySlider.value*4;
    }

    public void AmbientOcclusionThickness()
    {
        ambientOcclusionEffect.thicknessModifier.value = ambientOcclusionThicknessSlider.value*10;
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
        } else {
            chromaticAberrationEffect.active = false;
        }

    }

    public void ChromaticAberrationIntensity()
    {
        chromaticAberrationEffect.intensity.value = chromaticAberrationIntesitySlider.value;
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
        } else {
            grainEffect.active = false;
        }

    }

    public void GrainIntensity()
    {
        grainEffect.intensity.value = grainIntesitySlider.value;
    }

    public void GrainSize()
    {
        grainEffect.size.value = grainSizeSlider.value*3;
    }

    public void GrainLuminanceContribution()
    {
        grainEffect.lumContrib.value = grainLuminanceContributionSlider.value;
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
        } else {
            vignetteEffect.active = false;
        }

    }

    public void VignetteIntensity()
    {
        vignetteEffect.intensity.value = vignetteIntesitySlider.value;
    }

    public void VignetteSmoothness()
    {
        vignetteEffect.smoothness.value = vignetteSmoothnessSlider.value;
    }

    public void VignetteRoundness()
    {
        vignetteEffect.roundness.value = vignetteRoundnessSlider.value;
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
        } else {
            motionBlurEffect.active = false;
        }

    }

    public void MotionBlurShutterAngle()
    {
        motionBlurEffect.shutterAngle.value = motionBlurShutterAngleSlider.value;
    }

    public void MotionBlurSampleCount()
    {
        motionBlurEffect.sampleCount.value = (int) motionBlurSampleCountSlider.value;
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
        } else {
            lensDistortionEffect.active = false;
        }

    }

    public void LensDistortionIntensity()
    {
        lensDistortionEffect.intensity.value = lensDistortionIntensitySlider.value;
    }

    public void LensDistortionXMultiplier()
    {
        lensDistortionEffect.intensityX.value = lensDistortionXMultiplierSlider.value;
    }

    public void LensDistortionYMultiplier()
    {
        lensDistortionEffect.intensityY.value = lensDistortionYMultiplierSlider.value;
    }

    public void LensDistortionCenterX()
    {
        lensDistortionEffect.centerX.value = lensDistortionCenterXSlider.value;
    }

    public void LensDistortionCenterY()
    {
        lensDistortionEffect.centerY.value = lensDistortionCenterYSlider.value;
    }

    public void LensDistortionScale()
    {
        lensDistortionEffect.scale.value = lensDistortionScaleSlider.value;
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
        } else {
            depthOfFieldEffect.active = false;
        }

    }

    public void DepthOfFieldFocusDistance()
    {
        depthOfFieldEffect.focusDistance.value = depthOfFieldFocusDistanceSlider.value;
    }

    public void DepthOfFieldAperture()
    {
        depthOfFieldEffect.aperture.value = depthOfFieldApertureSlider.value;
    }

    public void DepthOfFieldFocalLength()
    {
        depthOfFieldEffect.focalLength.value = depthOfFieldFocalLengthSlider.value;
    }

}
