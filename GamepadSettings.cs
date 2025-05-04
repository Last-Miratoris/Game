using System;
using System.Collections.Generic;
using System.Linq;
using GUIFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001EB RID: 491
	public class GamepadSettings : SettingsBase
	{
		// Token: 0x06001C5D RID: 7261 RVA: 0x000D4988 File Offset: 0x000D2B88
		public override void FixBackButtonNavigation(Button backButton)
		{
			base.SetNavigation(this.m_gamepadSensitivitySlider, SettingsBase.NavigationDirection.OnDown, backButton);
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, this.m_gamepadSensitivitySlider);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000D49A6 File Offset: 0x000D2BA6
		public override void FixOkButtonNavigation(Button okButton)
		{
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, this.m_gamepadSensitivitySlider);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000D49B8 File Offset: 0x000D2BB8
		public override void LoadSettings()
		{
			PlayerController.m_gamepadSens = PlatformPrefs.GetFloat("GamepadSensitivity", PlayerController.m_gamepadSens);
			PlayerController.m_invertCameraY = (PlatformPrefs.GetInt("InvertCameraY", PlatformPrefs.GetInt("InvertMouse", 0)) == 1);
			PlayerController.m_invertCameraX = (PlatformPrefs.GetInt("InvertCameraX", 0) == 1);
			this.m_initialLayout = ZInput.InputLayout;
			this.m_currentLayout = this.m_initialLayout;
			if (PlatformPrefs.GetInt("AltGlyphs", 99) != 99)
			{
				this.m_initialGlyph = ((PlatformPrefs.GetInt("AltGlyphs", 0) == 1) ? GamepadGlyphs.Playstation : GamepadGlyphs.Auto);
				PlayerPrefs.DeleteKey("AltGlyphs");
			}
			else
			{
				string[] names = Enum.GetNames(typeof(GamepadGlyphs));
				this.m_initialGlyph = (GamepadGlyphs)Array.IndexOf<string>(names, PlatformPrefs.GetString("gamepad_glyphs", "Auto"));
			}
			ZInput.CurrentGlyph = this.m_initialGlyph;
			this.m_initialSwapTriggers = ZInput.SwapTriggers;
			this.m_gamepadEnabled.isOn = ZInput.IsGamepadEnabled();
			this.m_gamepadSensitivitySlider.value = PlayerController.m_gamepadSens;
			this.m_invertCameraY.isOn = PlayerController.m_invertCameraY;
			this.m_invertCameraX.isOn = PlayerController.m_invertCameraX;
			this.m_swapTriggers.isOn = this.m_initialSwapTriggers;
			this.m_glyphs.ClearOptions();
			this.m_glyphOptions = Enum.GetNames(typeof(GamepadGlyphs)).ToList<string>();
			this.m_glyphs.AddOptions(this.m_glyphOptions);
			this.m_glyphs.value = this.m_glyphOptions.IndexOf(this.m_initialGlyph.ToString());
			this.m_glyphs.onValueChanged.RemoveListener(new UnityAction<int>(this.OnGamepadGlyphChanged));
			this.m_glyphs.onValueChanged.AddListener(new UnityAction<int>(this.OnGamepadGlyphChanged));
			this.m_gamepadMapController.Show(this.m_initialLayout, GamepadMapType.Default);
			this.OnLayoutChanged();
			this.OnZInputLayoutChanged();
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000D4B9C File Offset: 0x000D2D9C
		public override void ResetSettings()
		{
			this.m_glyphs.value = Enum.GetNames(typeof(GamepadGlyphs)).ToList<string>().IndexOf(this.m_initialGlyph.ToString());
			this.m_currentLayout = this.m_initialLayout;
			this.m_swapTriggers.isOn = this.m_initialSwapTriggers;
			this.OnLayoutChanged();
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000D4C04 File Offset: 0x000D2E04
		public override void SaveSettings()
		{
			PlatformPrefs.SetFloat("GamepadSensitivity", this.m_gamepadSensitivitySlider.value);
			PlatformPrefs.SetInt("InvertCameraY", this.m_invertCameraY.isOn ? 1 : 0);
			PlatformPrefs.SetInt("InvertCameraX", this.m_invertCameraX.isOn ? 1 : 0);
			PlatformPrefs.SetInt("SwapTriggers", this.m_swapTriggers.isOn ? 1 : 0);
			PlatformPrefs.SetString("gamepad_glyphs", this.m_glyphs.options[this.m_glyphs.value].text);
			PlayerController.m_gamepadSens = this.m_gamepadSensitivitySlider.value;
			PlayerController.m_invertCameraY = this.m_invertCameraY.isOn;
			PlayerController.m_invertCameraX = this.m_invertCameraX.isOn;
			ZInput.SwapTriggers = this.m_swapTriggers.isOn;
			ZInput.SetGamepadEnabled(this.m_gamepadEnabled.isOn);
			Action saved = this.Saved;
			if (saved == null)
			{
				return;
			}
			saved();
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000D4D04 File Offset: 0x000D2F04
		public void OnGamepadSensitivityChanged()
		{
			this.m_cameraSensitivityText.text = Mathf.Round(this.m_gamepadSensitivitySlider.value * 100f).ToString() + "%";
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000D4D44 File Offset: 0x000D2F44
		public void OnLayoutLeft()
		{
			this.m_currentLayout = GamepadMapController.PrevLayout(this.m_gamepadMapController.VisibleLayout);
			this.OnLayoutChanged();
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000D4D62 File Offset: 0x000D2F62
		public void OnLayoutRight()
		{
			this.m_currentLayout = GamepadMapController.NextLayout(this.m_gamepadMapController.VisibleLayout);
			this.OnLayoutChanged();
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000D4D80 File Offset: 0x000D2F80
		public void OnLayoutChanged()
		{
			if (ZInput.instance == null)
			{
				return;
			}
			ZInput.SwapTriggers = this.m_swapTriggers.isOn;
			ZInput.instance.ChangeLayout(this.m_currentLayout);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000D4DAC File Offset: 0x000D2FAC
		public void OnGamepadGlyphChanged(int newValue)
		{
			ZInput.CurrentGlyph = (GamepadGlyphs)Enum.Parse(typeof(GamepadGlyphs), this.m_glyphs.options[this.m_glyphs.value].text);
			this.OnLayoutChanged();
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000D4DF8 File Offset: 0x000D2FF8
		private void OnZInputLayoutChanged()
		{
			this.m_gamepadMapController.Show(this.m_currentLayout, GamepadMapController.GetType(ZInput.CurrentGlyph, Settings.IsSteamRunningOnSteamDeck()));
			this.m_layoutText.text = Localization.instance.Localize(GamepadMapController.GetLayoutStringId(this.m_currentLayout));
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000D4E45 File Offset: 0x000D3045
		private void OnEnable()
		{
			ZInput.OnInputLayoutChanged += this.OnZInputLayoutChanged;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000D4E58 File Offset: 0x000D3058
		private void OnDisable()
		{
			ZInput.OnInputLayoutChanged -= this.OnZInputLayoutChanged;
		}

		// Token: 0x04001D4C RID: 7500
		[SerializeField]
		private UIGroupHandler m_groupHandler;

		// Token: 0x04001D4D RID: 7501
		[Header("Gamepad")]
		[SerializeField]
		private Toggle m_gamepadEnabled;

		// Token: 0x04001D4E RID: 7502
		[SerializeField]
		private Slider m_gamepadSensitivitySlider;

		// Token: 0x04001D4F RID: 7503
		[SerializeField]
		private TMP_Text m_cameraSensitivityText;

		// Token: 0x04001D50 RID: 7504
		[SerializeField]
		private Button m_leftLayoutButton;

		// Token: 0x04001D51 RID: 7505
		[SerializeField]
		private Button m_rightLayoutButton;

		// Token: 0x04001D52 RID: 7506
		[SerializeField]
		private GamepadMapController m_gamepadMapController;

		// Token: 0x04001D53 RID: 7507
		[SerializeField]
		private TMP_Text m_layoutText;

		// Token: 0x04001D54 RID: 7508
		[SerializeField]
		private Toggle m_swapTriggers;

		// Token: 0x04001D55 RID: 7509
		[SerializeField]
		private GuiDropdown m_glyphs;

		// Token: 0x04001D56 RID: 7510
		[SerializeField]
		private Toggle m_invertCameraY;

		// Token: 0x04001D57 RID: 7511
		[SerializeField]
		private Toggle m_invertCameraX;

		// Token: 0x04001D58 RID: 7512
		[SerializeField]
		private GameObject m_emptyToggleShift;

		// Token: 0x04001D59 RID: 7513
		private const string GlyphsXbox = "Xbox";

		// Token: 0x04001D5A RID: 7514
		private const string GlyphsPlaystation = "Playstation";

		// Token: 0x04001D5B RID: 7515
		private List<string> m_glyphOptions = new List<string>
		{
			"Xbox",
			"Playstation"
		};

		// Token: 0x04001D5C RID: 7516
		private GamepadGlyphs m_initialGlyph;

		// Token: 0x04001D5D RID: 7517
		private InputLayout m_initialLayout;

		// Token: 0x04001D5E RID: 7518
		private InputLayout m_currentLayout;

		// Token: 0x04001D5F RID: 7519
		private bool m_initialAlternativeGlyphs;

		// Token: 0x04001D60 RID: 7520
		private bool m_initialSwapTriggers;
	}
}
