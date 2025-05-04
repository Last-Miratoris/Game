using System;
using System.Collections.Generic;
using System.Linq;
using GUIFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F0 RID: 496
	public class GraphicsSettings : SettingsBase
	{
		// Token: 0x06001C8C RID: 7308 RVA: 0x000D5C74 File Offset: 0x000D3E74
		public override void FixBackButtonNavigation(Button backButton)
		{
			List<Selectable> list = new List<Selectable>();
			list.Add(this.m_antialiasingToggle);
			list.Add(this.m_softPartToggle);
			list.Add(this.m_sunShaftsToggle);
			list.Add(this.m_chromaticAberrationToggle);
			list.Add(this.m_motionBlurToggle);
			list.Add(this.m_depthOfFieldToggle);
			list.Add(this.m_bloomToggle);
			list.Add(this.m_ssaoToggle);
			list.Add(this.m_tesselationToggle);
			list.Add(this.m_distantShadowsToggle);
			list.Add(this.m_renderScaleSlider);
			list.Add(this.m_pointLightsSlider);
			list.Add(this.m_shadowQualitySlider);
			list.Add(this.m_lightsSlider);
			list.Add(this.m_levelOfDetailSlider);
			list.Add(this.m_vegetationSlider);
			list.Add(this.m_graphicPresetLeft);
			list.Add(this.m_vsyncToggle);
			list.Add(this.m_fpsLimitSlider);
			list.Add(this.m_resolutionDropdown);
			Selectable selectable = list.FirstOrDefault((Selectable t) => t.gameObject.activeSelf);
			base.SetNavigation(selectable, SettingsBase.NavigationDirection.OnDown, backButton);
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, selectable);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000D5DB0 File Offset: 0x000D3FB0
		public override void FixOkButtonNavigation(Button okButton)
		{
			List<Selectable> list = new List<Selectable>();
			list.Add(this.m_antialiasingToggle);
			list.Add(this.m_softPartToggle);
			list.Add(this.m_sunShaftsToggle);
			list.Add(this.m_chromaticAberrationToggle);
			list.Add(this.m_motionBlurToggle);
			list.Add(this.m_depthOfFieldToggle);
			list.Add(this.m_bloomToggle);
			list.Add(this.m_ssaoToggle);
			list.Add(this.m_tesselationToggle);
			list.Add(this.m_distantShadowsToggle);
			list.Add(this.m_renderScaleSlider);
			list.Add(this.m_pointLightsSlider);
			list.Add(this.m_shadowQualitySlider);
			list.Add(this.m_lightsSlider);
			list.Add(this.m_levelOfDetailSlider);
			list.Add(this.m_vegetationSlider);
			list.Add(this.m_graphicPresetLeft);
			list.Add(this.m_vsyncToggle);
			list.Add(this.m_fpsLimitSlider);
			list.Add(this.m_resolutionDropdown);
			Selectable target = list.FirstOrDefault((Selectable t) => t.gameObject.activeSelf);
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, target);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000D5EE0 File Offset: 0x000D40E0
		public override void LoadSettings()
		{
			this.m_pauseUpdateChecks = true;
			this.m_graphicsQualityModes = GraphicsModeManager.GraphicsQualityModes;
			bool flag = this.m_graphicsQualityModes.Count > 1;
			this.m_graphicPresetsRoot.SetActive(flag);
			this.m_graphicPresetLeft.interactable = flag;
			this.m_graphicPresetRight.interactable = flag;
			this.m_canCustomize = this.m_graphicsQualityModes.Contains(GraphicsQualityMode.Custom);
			this.m_oldRes = Screen.currentResolution;
			this.m_oldRes.width = Screen.width;
			this.m_oldRes.height = Screen.height;
			this.m_selectedRes = this.m_oldRes;
			this.FillResList();
			this.m_resDialog.SetActive(false);
			this.m_oldFullscreen = Screen.fullScreen;
			this.m_oldVsync = (PlatformPrefs.GetInt("VSync", 0) == 1);
			this.m_fullscreenToggle.isOn = this.m_oldFullscreen;
			Debug.Log(this.m_oldRes);
			this.m_renderScaleSlider.minValue = 1f;
			this.m_renderScaleSlider.maxValue = 21f;
			this.m_fpsLimitSlider.minValue = 30f;
			this.m_fpsLimitSlider.maxValue = 361f;
			this.SetGraphicsMode(GraphicsModeManager.ActiveGraphicQualityMode, true);
			Settings instance = Settings.instance;
			instance.SharedSettingsChanged = (Action<string, int>)Delegate.Remove(instance.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings instance2 = Settings.instance;
			instance2.SharedSettingsChanged = (Action<string, int>)Delegate.Combine(instance2.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings.instance.SettingsPopupDestroyed -= this.SettingsPopupDestroyed;
			Settings.instance.SettingsPopupDestroyed += this.SettingsPopupDestroyed;
			this.m_pauseUpdateChecks = false;
			this.OnResolutionChanged();
			this.OnQualityChanged();
			this.m_verticalLayoutGroup.spacing = (float)((this.m_resolutionRoot.activeSelf && flag) ? 8 : ((!this.m_resolutionRoot.activeSelf && !flag) ? 30 : 20));
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000D60D9 File Offset: 0x000D42D9
		private void SettingsPopupDestroyed()
		{
			Settings instance = Settings.instance;
			instance.SharedSettingsChanged = (Action<string, int>)Delegate.Remove(instance.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings.instance.SettingsPopupDestroyed -= this.SettingsPopupDestroyed;
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000D6117 File Offset: 0x000D4317
		public override void ResetSettings()
		{
			this.RevertMode();
			this.SetGraphicsMode(GraphicsModeManager.ActiveGraphicQualityMode, false);
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000D612C File Offset: 0x000D432C
		public override void SaveSettings()
		{
			DeviceQualitySettings settingsForMode = GraphicsModeManager.GetSettingsForMode(this.m_currentMode);
			if (!settingsForMode.IsSupportedOnHardware())
			{
				Action saved = this.Saved;
				if (saved == null)
				{
					return;
				}
				saved();
				return;
			}
			else
			{
				int renderScale;
				if (settingsForMode.RenderScaleChangeable)
				{
					renderScale = this.MapRenderScaleVisualToSaved((int)this.m_renderScaleSlider.value);
				}
				else
				{
					renderScale = settingsForMode.RenderScale;
				}
				GraphicsModeManager.SaveSettings(this.m_currentMode, this.m_selectedRes.width, this.m_selectedRes.height, (int)this.m_fpsLimitSlider.value, this.m_vsyncToggle.isOn, (int)this.m_vegetationSlider.value, (int)this.m_levelOfDetailSlider.value, (int)this.m_lightsSlider.value, (int)this.m_shadowQualitySlider.value, (int)this.m_pointLightsSlider.value, (int)this.m_pointLightShadowsSlider.value, renderScale, this.m_distantShadowsToggle.isOn, this.m_tesselationToggle.isOn, this.m_ssaoToggle.isOn, this.m_bloomToggle.isOn, this.m_depthOfFieldToggle.isOn, this.m_motionBlurToggle.isOn, this.m_chromaticAberrationToggle.isOn, this.m_sunShaftsToggle.isOn, this.m_softPartToggle.isOn, this.m_antialiasingToggle.isOn);
				if (!settingsForMode.FixedResolution && this.ResolutionSettingsChanged())
				{
					this.m_destroyAfterResChange = true;
					this.OnTestResolution();
					return;
				}
				Action saved2 = this.Saved;
				if (saved2 == null)
				{
					return;
				}
				saved2();
				return;
			}
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000D62A0 File Offset: 0x000D44A0
		private void SharedSettingsChanged(string setting, int value)
		{
			if (setting == "MotionBlur" && this.m_motionBlurToggle.isOn != (value == 1))
			{
				this.m_motionBlurToggle.isOn = (value == 1);
				return;
			}
			if (setting == "DepthOfField" && this.m_depthOfFieldToggle.isOn != (value == 1))
			{
				this.m_depthOfFieldToggle.isOn = (value == 1);
			}
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000D6308 File Offset: 0x000D4508
		public void OnMotionBlurChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("MotionBlur", this.m_motionBlurToggle.isOn ? 1 : 0);
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000D6334 File Offset: 0x000D4534
		public void OnDepthOfFieldChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("DepthOfField", this.m_depthOfFieldToggle.isOn ? 1 : 0);
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000D6360 File Offset: 0x000D4560
		public void OnQualityChanged()
		{
			if (this.m_pauseUpdateChecks)
			{
				return;
			}
			this.m_vegetationText.text = GraphicsSettings.GetQualityText(Math.Max(0, (int)this.m_vegetationSlider.value - 1));
			this.m_levelOfDetailText.text = GraphicsSettings.GetQualityText((int)this.m_levelOfDetailSlider.value);
			this.m_lightsText.text = GraphicsSettings.GetQualityText((int)this.m_lightsSlider.value);
			this.m_shadowQualityText.text = GraphicsSettings.GetQualityText((int)this.m_shadowQualitySlider.value);
			int pointLightLimit = Settings.GetPointLightLimit((int)this.m_pointLightsSlider.value);
			this.m_pointLightsText.text = GraphicsSettings.GetQualityText((int)this.m_pointLightsSlider.value) + " (" + ((pointLightLimit < 0) ? Localization.instance.Localize("$settings_infinite") : pointLightLimit.ToString()) + ")";
			int pointLightShadowLimit = Settings.GetPointLightShadowLimit((int)this.m_pointLightShadowsSlider.value);
			this.m_pointLightShadowsText.text = GraphicsSettings.GetQualityText((int)this.m_pointLightShadowsSlider.value) + " (" + ((pointLightShadowLimit < 0) ? Localization.instance.Localize("$settings_infinite") : pointLightShadowLimit.ToString()) + ")";
			this.m_fpsLimitText.text = ((this.m_fpsLimitSlider.value > 360f) ? Localization.instance.Localize("$settings_unlimited") : this.m_fpsLimitSlider.value.ToString());
			this.m_renderScaleText.text = ((this.m_renderScaleSlider.value >= this.m_renderScaleSlider.maxValue) ? Localization.instance.Localize("$settings_native") : ((this.m_renderScaleSlider.value == this.m_renderScaleSlider.maxValue - 1f) ? Localization.instance.Localize("$settings_automatic") : (this.m_renderScaleSlider.value / (this.m_renderScaleSlider.maxValue - 1f)).ToString("0%")));
			this.UpdateModeStepperInfo();
			this.m_vsyncRequiresRestartText.gameObject.SetActive(this.m_oldVsync != this.m_vsyncToggle.isOn && false);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000D659C File Offset: 0x000D479C
		private bool MatchesModeSettings(GraphicsQualityMode mode, bool considerChangeable)
		{
			return GraphicsModeManager.MatchesMode(mode, considerChangeable, this.m_selectedRes.width, this.m_selectedRes.height, (int)this.m_fpsLimitSlider.value, this.m_vsyncToggle.isOn, (int)this.m_vegetationSlider.value, (int)this.m_levelOfDetailSlider.value, (int)this.m_lightsSlider.value, (int)this.m_shadowQualitySlider.value, (int)this.m_pointLightsSlider.value, (int)this.m_pointLightShadowsSlider.value, this.MapRenderScaleVisualToSaved((int)this.m_renderScaleSlider.value), this.m_distantShadowsToggle.isOn, this.m_tesselationToggle.isOn, this.m_ssaoToggle.isOn, this.m_bloomToggle.isOn, this.m_depthOfFieldToggle.isOn, this.m_motionBlurToggle.isOn, this.m_chromaticAberrationToggle.isOn, this.m_sunShaftsToggle.isOn, this.m_softPartToggle.isOn, this.m_antialiasingToggle.isOn);
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000D66A5 File Offset: 0x000D48A5
		public void OnResolutionChanged()
		{
			if (this.m_pauseUpdateChecks)
			{
				return;
			}
			this.UpdateModeStepperInfo();
			this.m_testResolutionButton.interactable = this.ResolutionSettingsChanged();
			this.m_testResolutionButton.gameObject.SetActive(this.m_testResolutionButton.interactable);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000D66E4 File Offset: 0x000D48E4
		private bool ResolutionSettingsChanged()
		{
			return this.m_oldRes.width != this.m_selectedRes.width || this.m_oldRes.height != this.m_selectedRes.height || this.m_oldRes.refreshRateRatio.value != this.m_selectedRes.refreshRateRatio.value || this.m_oldFullscreen != this.m_fullscreenToggle.isOn;
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000D6761 File Offset: 0x000D4961
		private void Awake()
		{
			this.m_resolutionDropdown.OnExpandedStateChange += this.OnResolutionDropdownExpanded;
			this.m_resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnResolutionSelected));
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000D6796 File Offset: 0x000D4996
		private void OnDestroy()
		{
			this.m_resolutionDropdown.OnExpandedStateChange -= this.OnResolutionDropdownExpanded;
			this.m_resolutionDropdown.onValueChanged.RemoveListener(new UnityAction<int>(this.OnResolutionSelected));
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000D67CC File Offset: 0x000D49CC
		private void Update()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (this.m_resolutionDropdownScrollRectEnsureVisible && currentSelectedGameObject && ZInput.GamepadActive)
			{
				this.m_resolutionDropdownScrollRectEnsureVisible.CenterOnItem(currentSelectedGameObject.GetComponent<RectTransform>());
			}
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000D6814 File Offset: 0x000D4A14
		private static string GetQualityText(int level)
		{
			switch (level)
			{
			default:
				return Localization.instance.Localize("[$settings_low]");
			case 1:
				return Localization.instance.Localize("[$settings_medium]");
			case 2:
				return Localization.instance.Localize("[$settings_high]");
			case 3:
				return Localization.instance.Localize("[$settings_veryhigh]");
			}
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000D6878 File Offset: 0x000D4A78
		private void SetGraphicsMode(GraphicsQualityMode mode, bool init = false)
		{
			this.m_currentMode = mode;
			this.m_pauseUpdateChecks = true;
			bool flag = false;
			DeviceQualitySettings settingsForMode = GraphicsModeManager.GetSettingsForMode(mode);
			if (init || settingsForMode.FixedResolution)
			{
				this.m_fpsLimitSlider.value = (((float)settingsForMode.FpsLimit < this.m_fpsLimitSlider.minValue) ? this.m_fpsLimitSlider.maxValue : ((float)settingsForMode.FpsLimit));
				this.m_vsyncToggle.isOn = settingsForMode.Vsync;
			}
			foreach (Resolution selectedRes in this.m_resolutions)
			{
				if (settingsForMode.FixedResolution && selectedRes.width == settingsForMode.ResolutionWidth && selectedRes.height == settingsForMode.ResolutionHeight)
				{
					this.m_selectedRes = selectedRes;
					flag = true;
					break;
				}
			}
			this.m_vegetationSlider.value = (float)settingsForMode.Vegetation;
			this.m_levelOfDetailSlider.value = (float)settingsForMode.Lod;
			this.m_lightsSlider.value = (float)settingsForMode.Lights;
			this.m_shadowQualitySlider.value = (float)settingsForMode.ShadowQuality;
			this.m_pointLightsSlider.value = (float)settingsForMode.PointLights;
			this.m_pointLightShadowsSlider.value = (float)settingsForMode.PointLightShadows;
			this.m_renderScaleSlider.value = (float)this.MapRenderScaleSavedToVisual(settingsForMode.RenderScale);
			this.m_distantShadowsToggle.isOn = settingsForMode.DistantShadows;
			this.m_tesselationToggle.isOn = settingsForMode.Tesselation;
			this.m_ssaoToggle.isOn = settingsForMode.SSAO;
			this.m_bloomToggle.isOn = settingsForMode.Bloom;
			this.m_depthOfFieldToggle.isOn = settingsForMode.DepthOfField;
			this.m_motionBlurToggle.isOn = settingsForMode.MotionBlur;
			this.m_chromaticAberrationToggle.isOn = settingsForMode.ChromaticAberration;
			this.m_sunShaftsToggle.isOn = settingsForMode.SunShafts;
			this.m_softPartToggle.isOn = settingsForMode.SoftParticles;
			this.m_antialiasingToggle.isOn = settingsForMode.AntiAliasing;
			if (this.m_canCustomize)
			{
				this.m_pauseUpdateChecks = false;
				this.OnQualityChanged();
				if (flag)
				{
					this.OnResolutionChanged();
				}
				return;
			}
			this.m_devBuildSettingsText.gameObject.SetActive(false);
			this.m_resolutionRoot.SetActive(!settingsForMode.FixedResolution);
			this.m_resolutionDropdown.gameObject.SetActive(!settingsForMode.FixedResolution);
			this.m_fpsLimitSlider.gameObject.SetActive(!settingsForMode.FixedResolution);
			this.m_vsyncToggle.gameObject.SetActive(!settingsForMode.FixedResolution);
			this.m_vegetationSlider.gameObject.SetActive(settingsForMode.VegetationChangeable);
			this.m_levelOfDetailSlider.gameObject.SetActive(settingsForMode.LodChangeable);
			this.m_lightsSlider.gameObject.SetActive(settingsForMode.LightsChangeable);
			this.m_shadowQualitySlider.gameObject.SetActive(settingsForMode.ShadowQualityChangeable);
			this.m_pointLightsSlider.gameObject.SetActive(settingsForMode.PointLightsChangeable);
			this.m_pointLightShadowsSlider.gameObject.SetActive(settingsForMode.PointLightShadowsChangeable);
			this.m_renderScaleSlider.gameObject.SetActive(settingsForMode.RenderScaleChangeable);
			this.m_distantShadowsToggle.transform.parent.gameObject.SetActive(settingsForMode.DistantShadowsChangeable);
			this.m_distantShadowsToggle.gameObject.SetActive(settingsForMode.DistantShadowsChangeable);
			this.m_tesselationToggle.transform.parent.gameObject.SetActive(settingsForMode.TesselationChangeable);
			this.m_tesselationToggle.gameObject.SetActive(settingsForMode.TesselationChangeable);
			this.m_ssaoToggle.transform.parent.gameObject.SetActive(settingsForMode.SSAOChangeable);
			this.m_ssaoToggle.gameObject.SetActive(settingsForMode.SSAOChangeable);
			this.m_bloomToggle.transform.parent.gameObject.SetActive(settingsForMode.BloomChangeable);
			this.m_bloomToggle.gameObject.SetActive(settingsForMode.BloomChangeable);
			this.m_depthOfFieldToggle.transform.parent.gameObject.SetActive(settingsForMode.DepthOfFieldChangeable);
			this.m_depthOfFieldToggle.gameObject.SetActive(settingsForMode.DepthOfFieldChangeable);
			this.m_motionBlurToggle.transform.parent.gameObject.SetActive(settingsForMode.MotionBlurChangeable);
			this.m_motionBlurToggle.gameObject.SetActive(settingsForMode.MotionBlurChangeable);
			this.m_chromaticAberrationToggle.transform.parent.gameObject.SetActive(settingsForMode.ChromaticAberrationChangeable);
			this.m_chromaticAberrationToggle.gameObject.SetActive(settingsForMode.ChromaticAberrationChangeable);
			this.m_sunShaftsToggle.transform.parent.gameObject.SetActive(settingsForMode.SunShaftsChangeable);
			this.m_sunShaftsToggle.gameObject.SetActive(settingsForMode.SunShaftsChangeable);
			this.m_softPartToggle.transform.parent.gameObject.SetActive(settingsForMode.SoftParticlesChangeable);
			this.m_softPartToggle.gameObject.SetActive(settingsForMode.SoftParticlesChangeable);
			this.m_antialiasingToggle.transform.parent.gameObject.SetActive(settingsForMode.AntiAliasingChangeable);
			this.m_antialiasingToggle.gameObject.SetActive(settingsForMode.AntiAliasingChangeable);
			foreach (Selectable selectable in new List<Selectable>
			{
				this.m_graphicPresetLeft,
				this.m_resolutionDropdown,
				this.m_fpsLimitSlider,
				this.m_vsyncToggle,
				this.m_vegetationSlider,
				this.m_levelOfDetailSlider,
				this.m_lightsSlider,
				this.m_shadowQualitySlider,
				this.m_pointLightsSlider,
				this.m_renderScaleSlider,
				this.m_distantShadowsToggle,
				this.m_tesselationToggle,
				this.m_ssaoToggle,
				this.m_bloomToggle,
				this.m_depthOfFieldToggle,
				this.m_motionBlurToggle,
				this.m_chromaticAberrationToggle,
				this.m_sunShaftsToggle,
				this.m_softPartToggle,
				this.m_antialiasingToggle
			})
			{
				if (selectable.gameObject.activeSelf && selectable.interactable)
				{
					this.m_groupHandler.m_defaultElement = selectable.gameObject;
					break;
				}
			}
			List<Selectable> list = new List<Selectable>
			{
				this.m_resolutionDropdown,
				this.m_fpsLimitSlider,
				this.m_vsyncToggle,
				this.m_graphicPresetLeft,
				this.m_vegetationSlider,
				this.m_levelOfDetailSlider,
				this.m_lightsSlider,
				this.m_shadowQualitySlider,
				this.m_pointLightsSlider,
				this.m_renderScaleSlider,
				this.m_distantShadowsToggle,
				this.m_tesselationToggle,
				this.m_ssaoToggle,
				this.m_bloomToggle,
				this.m_depthOfFieldToggle,
				this.m_motionBlurToggle,
				this.m_chromaticAberrationToggle,
				this.m_sunShaftsToggle,
				this.m_softPartToggle,
				this.m_antialiasingToggle
			};
			List<Selectable> list2 = new List<Selectable>(list);
			while (list2.Count > 10)
			{
				Selectable selectable2 = list2[0];
				list2.RemoveAt(0);
				base.SetNavigationToFirstActive(selectable2, SettingsBase.NavigationDirection.OnDown, list2);
			}
			base.SetNavigationToFirstActive(this.m_fullscreenToggle, SettingsBase.NavigationDirection.OnDown, list);
			base.SetNavigationToFirstActive(this.m_testResolutionButton, SettingsBase.NavigationDirection.OnDown, list);
			base.SetNavigationToFirstActive(this.m_graphicPresetRight, SettingsBase.NavigationDirection.OnDown, list.GetRange(4, list.Count - 5));
			list2 = new List<Selectable>(list.GetRange(0, list.Count - 10));
			list2.Reverse();
			while (list2.Count > 0)
			{
				Selectable selectable3 = list2[0];
				list2.RemoveAt(0);
				base.SetNavigationToFirstActive(selectable3, SettingsBase.NavigationDirection.OnUp, list2);
			}
			list2 = new List<Selectable>(list.GetRange(0, list.Count - 10));
			list2.Reverse();
			List<Selectable> list3 = new List<Selectable>(from s in list.GetRange(list.Count - 10, 10)
			where s.gameObject.activeSelf
			select s);
			int num = 0;
			int count = list3.Count;
			foreach (Selectable selectable4 in list3)
			{
				if (num < 3)
				{
					base.SetNavigationToFirstActive(selectable4, SettingsBase.NavigationDirection.OnUp, list2);
				}
				else
				{
					base.SetNavigation(selectable4, SettingsBase.NavigationDirection.OnUp, list3[num - 3]);
				}
				if (num < count - 1)
				{
					base.SetNavigation(selectable4, SettingsBase.NavigationDirection.OnRight, list3[num + 1]);
				}
				if (num < count - 3)
				{
					base.SetNavigation(selectable4, SettingsBase.NavigationDirection.OnDown, list3[num + 3]);
				}
				if (num > 0)
				{
					base.SetNavigation(selectable4, SettingsBase.NavigationDirection.OnLeft, list3[num - 1]);
				}
				num++;
			}
			this.m_pauseUpdateChecks = false;
			this.OnQualityChanged();
			if (flag)
			{
				this.OnResolutionChanged();
			}
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000D71FC File Offset: 0x000D53FC
		private void UpdateModeStepperInfo()
		{
			DeviceQualitySettings settingsForMode = GraphicsModeManager.GetSettingsForMode(this.m_currentMode);
			if (settingsForMode.IsSupportedOnHardware())
			{
				bool flag = this.m_currentMode != GraphicsQualityMode.Custom && this.m_graphicsQualityModes.Contains(GraphicsQualityMode.Custom) && !this.MatchesModeSettings(this.m_currentMode, false);
				this.m_graphicsMode.alpha = 1f;
				this.m_graphicsMode.text = Localization.instance.Localize(settingsForMode.NameTextId) + (flag ? "*" : "");
				this.m_graphicsModeDescr.text = Localization.instance.Localize(flag ? "$settings_quality_mode_customized" : settingsForMode.DescriptionTextId);
				return;
			}
			this.m_graphicsMode.alpha = 0.25f;
			this.m_graphicsMode.text = Localization.instance.Localize(settingsForMode.NameTextId) + "*";
			this.m_graphicsModeDescr.text = Localization.instance.Localize("$settings_quality_mode_not_supported");
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000D7304 File Offset: 0x000D5504
		private GraphicsQualityMode NextGraphicsMode(GraphicsQualityMode mode)
		{
			int num = this.m_graphicsQualityModes.IndexOf(mode);
			if (num < 0 || this.m_graphicsQualityModes.Count == 0)
			{
				if (this.m_graphicsQualityModes.Count <= 0)
				{
					return mode;
				}
				return this.m_graphicsQualityModes[0];
			}
			else
			{
				if (num >= this.m_graphicsQualityModes.Count - 1)
				{
					return this.m_graphicsQualityModes[0];
				}
				return this.m_graphicsQualityModes[num + 1];
			}
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000D7378 File Offset: 0x000D5578
		private GraphicsQualityMode PrevGraphicsMode(GraphicsQualityMode mode)
		{
			int num = this.m_graphicsQualityModes.IndexOf(mode);
			if (num < 0 || this.m_graphicsQualityModes.Count == 0)
			{
				if (this.m_graphicsQualityModes.Count <= 0)
				{
					return mode;
				}
				return this.m_graphicsQualityModes[0];
			}
			else
			{
				if (num == 0)
				{
					return this.m_graphicsQualityModes[this.m_graphicsQualityModes.Count - 1];
				}
				return this.m_graphicsQualityModes[num - 1];
			}
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000D73EA File Offset: 0x000D55EA
		public void OnGraphicPresetLeft()
		{
			this.SetGraphicsMode(this.PrevGraphicsMode(this.m_currentMode), false);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000D73FF File Offset: 0x000D55FF
		public void OnGraphicPresetRight()
		{
			this.SetGraphicsMode(this.NextGraphicsMode(this.m_currentMode), false);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000D7414 File Offset: 0x000D5614
		private void OnResolutionDropdownExpanded(bool expanded)
		{
			Settings.instance.BlockNavigation(expanded);
			if (this.m_resolutionDropdownScrollRectEnsureVisible == null && expanded)
			{
				this.FindResolutionScrollRect();
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000D7438 File Offset: 0x000D5638
		private void FindResolutionScrollRect()
		{
			ScrollRectEnsureVisible[] componentsInChildren = base.GetComponentsInChildren<ScrollRectEnsureVisible>(false);
			if (componentsInChildren.Length == 0)
			{
				ZLog.LogError("Missing ScrollRectEnsureVisible component on Resolution dropdown list!");
				return;
			}
			if (componentsInChildren.Length == 1)
			{
				this.m_resolutionDropdownScrollRectEnsureVisible = componentsInChildren[0];
				return;
			}
			ZLog.LogError("More than one enabled component with ScrollRectEnsureVisible active within graphics tab at a time - not supported!");
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000D7478 File Offset: 0x000D5678
		private void UpdateValidResolutions()
		{
			Resolution[] array = Screen.resolutions;
			if (array.Length == 0)
			{
				array = new Resolution[]
				{
					this.m_oldRes
				};
			}
			this.m_resolutions.Clear();
			foreach (Resolution item in array)
			{
				if ((item.width >= this.m_minResWidth && item.height >= this.m_minResHeight) || item.width == this.m_oldRes.width || item.height == this.m_oldRes.height)
				{
					this.m_resolutions.Add(item);
				}
			}
			foreach (GraphicsQualityMode mode in this.m_graphicsQualityModes)
			{
				DeviceQualitySettings settingsForMode = GraphicsModeManager.GetSettingsForMode(mode);
				if (settingsForMode.FixedResolution)
				{
					Resolution item2 = new Resolution
					{
						width = settingsForMode.ResolutionWidth,
						height = settingsForMode.ResolutionHeight,
						refreshRateRatio = new RefreshRate
						{
							numerator = (uint)settingsForMode.FpsLimit,
							denominator = 1U
						}
					};
					this.m_resolutions.Add(item2);
				}
			}
			if (this.m_resolutions.Count == 0)
			{
				Resolution item3 = new Resolution
				{
					width = 1280,
					height = 720,
					refreshRateRatio = new RefreshRate
					{
						numerator = 60U,
						denominator = 1U
					}
				};
				this.m_resolutions.Add(item3);
			}
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000D7624 File Offset: 0x000D5824
		private void FillResList()
		{
			this.UpdateValidResolutions();
			this.m_resolutionDropdown.ClearOptions();
			List<string> list = new List<string>();
			this.m_resolutionOptions.Clear();
			int num = 0;
			int num2 = -1;
			foreach (Resolution item in this.m_resolutions)
			{
				string text = string.Format("{0}x{1}", item.width, item.height);
				if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen || !list.Contains(text))
				{
					if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
					{
						list.Add(string.Format("{0} {1}hz", text, Mathf.Round((float)item.refreshRateRatio.value)));
					}
					else
					{
						list.Add(text);
					}
					if (this.m_selectedRes.width == item.width && this.m_selectedRes.height == item.height)
					{
						num2 = num;
					}
					this.m_resolutionOptions.Add(item);
					num++;
				}
			}
			this.m_resolutionDropdown.AddOptions(list);
			if (num2 > -1)
			{
				this.m_resolutionDropdown.value = num2;
			}
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000D7768 File Offset: 0x000D5968
		public void OnResolutionSelected(int index)
		{
			this.m_selectedRes = this.m_resolutionOptions[index];
			Debug.Log(this.m_selectedRes);
			this.OnResolutionChanged();
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000D7794 File Offset: 0x000D5994
		public void OnResSwitchOK()
		{
			this.m_resSwitchDialog.SetActive(false);
			this.m_oldRes = this.m_selectedRes;
			this.m_oldFullscreen = this.m_fullscreenToggle.isOn;
			this.OnResolutionChanged();
			Settings.instance.BlockNavigation(false);
			if (this.m_destroyAfterResChange)
			{
				Action saved = this.Saved;
				if (saved == null)
				{
					return;
				}
				saved();
			}
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000D77F3 File Offset: 0x000D59F3
		public void OnResSwitchCancel()
		{
			this.m_resSwitchDialog.SetActive(false);
			this.RevertMode();
			Settings.instance.BlockNavigation(false);
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000D7814 File Offset: 0x000D5A14
		public void OnTestResolution()
		{
			this.ApplyResolution();
			this.m_resSwitchDialog.SetActive(true);
			if (this.m_resSwitchDialog.transform.parent == base.transform)
			{
				this.m_resSwitchDialog.transform.parent = this.m_resSwitchDialog.transform.parent.parent;
			}
			this.m_resSwitchDialog.GetComponent<ResolutionSwitchDialogTimedRemoval>().ResCountdownTimer = 5f;
			EventSystem.current.SetSelectedGameObject(this.m_resolutionOk.gameObject);
			Settings.instance.BlockNavigation(true);
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000D78AC File Offset: 0x000D5AAC
		private void ApplyResolution()
		{
			if (Screen.width == this.m_selectedRes.width && Screen.height == this.m_selectedRes.height && this.m_fullscreenToggle.isOn == Screen.fullScreen)
			{
				return;
			}
			Screen.SetResolution(this.m_selectedRes.width, this.m_selectedRes.height, this.m_fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, this.m_selectedRes.refreshRateRatio);
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000D7927 File Offset: 0x000D5B27
		private int MapRenderScaleVisualToSaved(int visualValue)
		{
			if (visualValue >= (int)this.m_renderScaleSlider.maxValue)
			{
				return 20;
			}
			if (visualValue != (int)this.m_renderScaleSlider.maxValue - 1)
			{
				return visualValue;
			}
			return 0;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000D794F File Offset: 0x000D5B4F
		private int MapRenderScaleSavedToVisual(int savedValue)
		{
			if (savedValue <= 0)
			{
				return (int)this.m_renderScaleSlider.maxValue - 1;
			}
			if (savedValue < 20)
			{
				return savedValue;
			}
			return (int)this.m_renderScaleSlider.maxValue;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x000D7978 File Offset: 0x000D5B78
		public void RevertMode()
		{
			this.m_selectedRes = this.m_oldRes;
			this.m_fullscreenToggle.isOn = this.m_oldFullscreen;
			this.ApplyResolution();
			this.OnResolutionChanged();
			Settings.instance.BlockNavigation(false);
			if (this.m_destroyAfterResChange)
			{
				Action saved = this.Saved;
				if (saved == null)
				{
					return;
				}
				saved();
			}
		}

		// Token: 0x04001D7C RID: 7548
		[SerializeField]
		private UIGroupHandler m_groupHandler;

		// Token: 0x04001D7D RID: 7549
		[SerializeField]
		private VerticalLayoutGroup m_verticalLayoutGroup;

		// Token: 0x04001D7E RID: 7550
		[SerializeField]
		private TMP_Text m_devBuildSettingsText;

		// Token: 0x04001D7F RID: 7551
		[SerializeField]
		private GameObject m_devBuildSettingFramePrefab;

		// Token: 0x04001D80 RID: 7552
		[SerializeField]
		private TMP_Text m_devGraphicsModeValuesText;

		// Token: 0x04001D81 RID: 7553
		[SerializeField]
		private TMP_Text m_devPlayerPrefsValuesText;

		// Token: 0x04001D82 RID: 7554
		[Header("Resolution")]
		[SerializeField]
		private GameObject m_resolutionRoot;

		// Token: 0x04001D83 RID: 7555
		[SerializeField]
		private GuiDropdown m_resolutionDropdown;

		// Token: 0x04001D84 RID: 7556
		[SerializeField]
		private Toggle m_fullscreenToggle;

		// Token: 0x04001D85 RID: 7557
		[SerializeField]
		private GuiButton m_testResolutionButton;

		// Token: 0x04001D86 RID: 7558
		[SerializeField]
		private GameObject m_resDialog;

		// Token: 0x04001D87 RID: 7559
		[SerializeField]
		private GameObject m_resListElement;

		// Token: 0x04001D88 RID: 7560
		[SerializeField]
		private RectTransform m_resListRoot;

		// Token: 0x04001D89 RID: 7561
		[SerializeField]
		private Scrollbar m_resListScroll;

		// Token: 0x04001D8A RID: 7562
		[SerializeField]
		private GameObject m_resSwitchDialog;

		// Token: 0x04001D8B RID: 7563
		[SerializeField]
		private GuiButton m_resolutionOk;

		// Token: 0x04001D8C RID: 7564
		[SerializeField]
		private Toggle m_vsyncToggle;

		// Token: 0x04001D8D RID: 7565
		[SerializeField]
		private TMP_Text m_vsyncRequiresRestartText;

		// Token: 0x04001D8E RID: 7566
		[SerializeField]
		private int m_minResWidth = 1280;

		// Token: 0x04001D8F RID: 7567
		[SerializeField]
		private int m_minResHeight = 720;

		// Token: 0x04001D90 RID: 7568
		[Header("Graphic Presets")]
		[SerializeField]
		private GameObject m_graphicPresetsRoot;

		// Token: 0x04001D91 RID: 7569
		[SerializeField]
		private TMP_Text m_graphicsMode;

		// Token: 0x04001D92 RID: 7570
		[SerializeField]
		private Button m_graphicPresetLeft;

		// Token: 0x04001D93 RID: 7571
		[SerializeField]
		private Button m_graphicPresetRight;

		// Token: 0x04001D94 RID: 7572
		[SerializeField]
		private TMP_Text m_graphicsModeDescr;

		// Token: 0x04001D95 RID: 7573
		[Header("Quality")]
		[SerializeField]
		private Slider m_vegetationSlider;

		// Token: 0x04001D96 RID: 7574
		[SerializeField]
		private TMP_Text m_vegetationText;

		// Token: 0x04001D97 RID: 7575
		[SerializeField]
		private Slider m_levelOfDetailSlider;

		// Token: 0x04001D98 RID: 7576
		[SerializeField]
		private TMP_Text m_levelOfDetailText;

		// Token: 0x04001D99 RID: 7577
		[SerializeField]
		private Slider m_lightsSlider;

		// Token: 0x04001D9A RID: 7578
		[SerializeField]
		private TMP_Text m_lightsText;

		// Token: 0x04001D9B RID: 7579
		[SerializeField]
		private Slider m_shadowQualitySlider;

		// Token: 0x04001D9C RID: 7580
		[SerializeField]
		private TMP_Text m_shadowQualityText;

		// Token: 0x04001D9D RID: 7581
		[SerializeField]
		private Slider m_pointLightsSlider;

		// Token: 0x04001D9E RID: 7582
		[SerializeField]
		private TMP_Text m_pointLightsText;

		// Token: 0x04001D9F RID: 7583
		[SerializeField]
		private Slider m_pointLightShadowsSlider;

		// Token: 0x04001DA0 RID: 7584
		[SerializeField]
		private TMP_Text m_pointLightShadowsText;

		// Token: 0x04001DA1 RID: 7585
		[SerializeField]
		private Slider m_fpsLimitSlider;

		// Token: 0x04001DA2 RID: 7586
		[SerializeField]
		private TMP_Text m_fpsLimitText;

		// Token: 0x04001DA3 RID: 7587
		[SerializeField]
		private Slider m_renderScaleSlider;

		// Token: 0x04001DA4 RID: 7588
		[SerializeField]
		private TMP_Text m_renderScaleText;

		// Token: 0x04001DA5 RID: 7589
		[Header("Graphics")]
		[SerializeField]
		private Toggle m_distantShadowsToggle;

		// Token: 0x04001DA6 RID: 7590
		[SerializeField]
		private Toggle m_tesselationToggle;

		// Token: 0x04001DA7 RID: 7591
		[SerializeField]
		private Toggle m_ssaoToggle;

		// Token: 0x04001DA8 RID: 7592
		[SerializeField]
		private Toggle m_bloomToggle;

		// Token: 0x04001DA9 RID: 7593
		[SerializeField]
		private Toggle m_depthOfFieldToggle;

		// Token: 0x04001DAA RID: 7594
		[SerializeField]
		private Toggle m_motionBlurToggle;

		// Token: 0x04001DAB RID: 7595
		[SerializeField]
		private Toggle m_chromaticAberrationToggle;

		// Token: 0x04001DAC RID: 7596
		[SerializeField]
		private Toggle m_sunShaftsToggle;

		// Token: 0x04001DAD RID: 7597
		[SerializeField]
		private Toggle m_softPartToggle;

		// Token: 0x04001DAE RID: 7598
		[SerializeField]
		private Toggle m_antialiasingToggle;

		// Token: 0x04001DAF RID: 7599
		private List<Resolution> m_resolutions = new List<Resolution>();

		// Token: 0x04001DB0 RID: 7600
		private List<Resolution> m_resolutionOptions = new List<Resolution>();

		// Token: 0x04001DB1 RID: 7601
		private ScrollRectEnsureVisible m_resolutionDropdownScrollRectEnsureVisible;

		// Token: 0x04001DB2 RID: 7602
		private bool m_oldFullscreen;

		// Token: 0x04001DB3 RID: 7603
		private Resolution m_oldRes;

		// Token: 0x04001DB4 RID: 7604
		private Resolution m_selectedRes;

		// Token: 0x04001DB5 RID: 7605
		private bool m_oldVsync;

		// Token: 0x04001DB6 RID: 7606
		private bool m_destroyAfterResChange;

		// Token: 0x04001DB7 RID: 7607
		private bool m_pauseUpdateChecks;

		// Token: 0x04001DB8 RID: 7608
		private GraphicsQualityMode m_currentMode;

		// Token: 0x04001DB9 RID: 7609
		private List<GraphicsQualityMode> m_graphicsQualityModes;

		// Token: 0x04001DBA RID: 7610
		private bool m_canCustomize;
	}
}
