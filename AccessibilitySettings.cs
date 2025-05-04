using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001E4 RID: 484
	public class AccessibilitySettings : SettingsBase
	{
		// Token: 0x06001BEC RID: 7148 RVA: 0x000D2549 File Offset: 0x000D0749
		public override void FixBackButtonNavigation(Button backButton)
		{
			base.SetNavigation(this.m_motionblurToggle, SettingsBase.NavigationDirection.OnDown, backButton);
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, this.m_motionblurToggle);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000D2567 File Offset: 0x000D0767
		public override void FixOkButtonNavigation(Button okButton)
		{
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, this.m_depthOfFieldToggle);
			base.SetNavigation(this.m_depthOfFieldToggle, SettingsBase.NavigationDirection.OnDown, okButton);
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000D2588 File Offset: 0x000D0788
		public override void LoadSettings()
		{
			this.m_oldGuiScale = PlatformPrefs.GetFloat("GuiScale", 1f);
			this.m_guiScaleSlider.value = this.m_oldGuiScale * 100f;
			this.m_toggleRun.isOn = (PlatformPrefs.GetInt("ToggleRun", ZInput.IsGamepadActive() ? 1 : 0) == 1);
			this.m_immersiveCamera.isOn = (PlatformPrefs.GetInt("ShipCameraTilt", 1) == 1);
			this.m_cameraShake.isOn = (PlatformPrefs.GetInt("CameraShake", 1) == 1);
			this.m_reduceFlashingLights.isOn = (PlatformPrefs.GetInt("ReduceFlashingLights", 0) == 1);
			this.m_motionblurToggle.isOn = (PlatformPrefs.GetInt("MotionBlur", 1) == 1);
			this.m_depthOfFieldToggle.isOn = (PlatformPrefs.GetInt("DOF", 1) == 1);
			this.m_closedCaptionsToggle.isOn = (PlatformPrefs.GetInt("ClosedCaptions", 0) == 1);
			this.m_soundIndicatorsToggle.isOn = (PlatformPrefs.GetInt("DirectionalSoundIndicators", 0) == 1);
			Settings.ReduceFlashingLights = this.m_reduceFlashingLights.isOn;
			Settings.DirectionalSoundIndicators = this.m_soundIndicatorsToggle.isOn;
			Settings.ClosedCaptions = this.m_closedCaptionsToggle.isOn;
			Settings instance = Settings.instance;
			instance.SharedSettingsChanged = (Action<string, int>)Delegate.Remove(instance.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings instance2 = Settings.instance;
			instance2.SharedSettingsChanged = (Action<string, int>)Delegate.Combine(instance2.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings.instance.SettingsPopupDestroyed -= this.SettingsPopupDestroyed;
			Settings.instance.SettingsPopupDestroyed += this.SettingsPopupDestroyed;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000D273B File Offset: 0x000D093B
		private void SettingsPopupDestroyed()
		{
			Settings instance = Settings.instance;
			instance.SharedSettingsChanged = (Action<string, int>)Delegate.Remove(instance.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings.instance.SettingsPopupDestroyed -= this.SettingsPopupDestroyed;
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000D277C File Offset: 0x000D097C
		public override void SaveSettings()
		{
			PlatformPrefs.SetFloat("GuiScale", this.m_guiScaleSlider.value / 100f);
			PlatformPrefs.SetInt("ToggleRun", this.m_toggleRun.isOn ? 1 : 0);
			PlatformPrefs.SetInt("ShipCameraTilt", this.m_immersiveCamera.isOn ? 1 : 0);
			PlatformPrefs.SetInt("CameraShake", this.m_cameraShake.isOn ? 1 : 0);
			PlatformPrefs.SetInt("ReduceFlashingLights", this.m_reduceFlashingLights.isOn ? 1 : 0);
			PlatformPrefs.SetInt("ClosedCaptions", this.m_closedCaptionsToggle.isOn ? 1 : 0);
			PlatformPrefs.SetInt("DirectionalSoundIndicators", this.m_soundIndicatorsToggle.isOn ? 1 : 0);
			Settings.ReduceFlashingLights = this.m_reduceFlashingLights.isOn;
			Settings.ClosedCaptions = this.m_closedCaptionsToggle.isOn;
			Settings.DirectionalSoundIndicators = this.m_soundIndicatorsToggle.isOn;
			Action saved = this.Saved;
			if (saved == null)
			{
				return;
			}
			saved();
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000D2886 File Offset: 0x000D0A86
		public override void ResetSettings()
		{
			GuiScaler.SetScale(this.m_oldGuiScale);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000D2894 File Offset: 0x000D0A94
		private void SharedSettingsChanged(string setting, int value)
		{
			if (setting == "MotionBlur" && this.m_motionblurToggle.isOn != (value == 1))
			{
				this.m_motionblurToggle.isOn = (value == 1);
				return;
			}
			if (setting == "DepthOfField" && this.m_depthOfFieldToggle.isOn != (value == 1))
			{
				this.m_depthOfFieldToggle.isOn = (value == 1);
				return;
			}
			if (setting == "ToggleRun" && this.m_toggleRun.isOn != (value == 1))
			{
				this.m_toggleRun.isOn = (value == 1);
				return;
			}
			if (setting == "ClosedCaptions" && this.m_closedCaptionsToggle.isOn != (value == 1))
			{
				this.m_closedCaptionsToggle.isOn = (value == 1);
				return;
			}
			if (setting == "DirectionalSoundIndicators" && this.m_soundIndicatorsToggle.isOn != (value == 1))
			{
				this.m_soundIndicatorsToggle.isOn = (value == 1);
			}
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000D2988 File Offset: 0x000D0B88
		public void OnUIScaleChanged()
		{
			this.m_guiScaleText.text = this.m_guiScaleSlider.value.ToString() + "%";
			GuiScaler.SetScale(this.m_guiScaleSlider.value / 100f);
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000D29D3 File Offset: 0x000D0BD3
		public void OnMotionBlurChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("MotionBlur", this.m_motionblurToggle.isOn ? 1 : 0);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000D29FF File Offset: 0x000D0BFF
		public void OnDepthOfFieldChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("DepthOfField", this.m_depthOfFieldToggle.isOn ? 1 : 0);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000D2A2B File Offset: 0x000D0C2B
		public void OnToggleRunChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("ToggleRun", this.m_toggleRun.isOn ? 1 : 0);
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000D2A57 File Offset: 0x000D0C57
		public void OnClosedCaptionsChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("ClosedCaptions", this.m_closedCaptionsToggle.isOn ? 1 : 0);
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000D2A83 File Offset: 0x000D0C83
		public void OnDirectionalSoundIndicatorsChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("DirectionalSoundIndicators", this.m_soundIndicatorsToggle.isOn ? 1 : 0);
		}

		// Token: 0x04001CDC RID: 7388
		private float m_oldGuiScale;

		// Token: 0x04001CDD RID: 7389
		[Header("Accessibility")]
		[SerializeField]
		private Slider m_guiScaleSlider;

		// Token: 0x04001CDE RID: 7390
		[SerializeField]
		private TMP_Text m_guiScaleText;

		// Token: 0x04001CDF RID: 7391
		[SerializeField]
		private Toggle m_toggleRun;

		// Token: 0x04001CE0 RID: 7392
		[SerializeField]
		private Toggle m_immersiveCamera;

		// Token: 0x04001CE1 RID: 7393
		[SerializeField]
		private Toggle m_cameraShake;

		// Token: 0x04001CE2 RID: 7394
		[SerializeField]
		private Toggle m_reduceFlashingLights;

		// Token: 0x04001CE3 RID: 7395
		[SerializeField]
		private Toggle m_motionblurToggle;

		// Token: 0x04001CE4 RID: 7396
		[SerializeField]
		private Toggle m_depthOfFieldToggle;

		// Token: 0x04001CE5 RID: 7397
		[SerializeField]
		private Toggle m_closedCaptionsToggle;

		// Token: 0x04001CE6 RID: 7398
		[SerializeField]
		private Toggle m_soundIndicatorsToggle;
	}
}
