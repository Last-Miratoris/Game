using System;
using System.Collections.Generic;
using System.Linq;
using GUIFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valheim.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F3 RID: 499
	public class RadialSettings : SettingsBase
	{
		// Token: 0x06001CC5 RID: 7365 RVA: 0x000D8173 File Offset: 0x000D6373
		public override void FixBackButtonNavigation(Button backButton)
		{
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, this.m_singleUse);
			base.SetNavigation(this.m_singleUse, SettingsBase.NavigationDirection.OnDown, backButton);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000D8191 File Offset: 0x000D6391
		public override void FixOkButtonNavigation(Button okButton)
		{
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, this.m_singleUse);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000D81A4 File Offset: 0x000D63A4
		public override void LoadSettings()
		{
			List<string> list = this.m_hoverSpeedOptionStrings.Values.ToList<string>();
			int value = list.IndexOf(this.m_hoverSpeedOptionStrings[(HoverSelectSpeedSetting)PlatformPrefs.GetInt("RadialHoverSpd", 0)]);
			foreach (string text in list.ToList<string>())
			{
				list[list.IndexOf(text)] = Localization.instance.Localize(text);
			}
			this.m_hoverSelect.ClearOptions();
			this.m_hoverSelect.AddOptions(list);
			this.m_hoverSelect.value = value;
			list = this.m_sprialOptionStrings.Values.ToList<string>();
			value = list.IndexOf(this.m_sprialOptionStrings[(SpiralEffectIntensitySetting)PlatformPrefs.GetInt("RadialSpiral", 2)]);
			foreach (string text2 in list.ToList<string>())
			{
				list[list.IndexOf(text2)] = Localization.instance.Localize(text2);
			}
			this.m_spiralEffect.ClearOptions();
			this.m_spiralEffect.AddOptions(list);
			this.m_spiralEffect.value = value;
			this.m_persistentBackBtn.isOn = (PlatformPrefs.GetInt("RadialPersistentBackBtn", 0) != 0);
			this.m_animateRadial.isOn = (PlatformPrefs.GetInt("RadialAnimateRadial", 1) != 0);
			this.m_doubleTap.isOn = (PlatformPrefs.GetInt("RadialDoubleTap", 0) != 0);
			this.m_flick.isOn = (PlatformPrefs.GetInt("RadialFlick", 0) != 0);
			this.m_singleUse.isOn = (PlatformPrefs.GetInt("RadialSingleUse", 0) != 0);
			this.m_flick.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnFlickUpdated));
			this.m_flick.onValueChanged.AddListener(new UnityAction<bool>(this.OnFlickUpdated));
			this.m_doubleTap.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnDoubleTapUpdated));
			this.m_doubleTap.onValueChanged.AddListener(new UnityAction<bool>(this.OnDoubleTapUpdated));
			this.OnDoubleTapUpdated(this.m_doubleTap.isOn);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000D8400 File Offset: 0x000D6600
		public override void SaveSettings()
		{
			PlatformPrefs.SetInt("RadialPersistentBackBtn", this.m_persistentBackBtn.isOn ? 1 : 0);
			PlatformPrefs.SetInt("RadialAnimateRadial", this.m_animateRadial.isOn ? 1 : 0);
			PlatformPrefs.SetInt("RadialDoubleTap", this.m_doubleTap.isOn ? 1 : 0);
			PlatformPrefs.SetInt("RadialFlick", this.m_flick.isOn ? 1 : 0);
			PlatformPrefs.SetInt("RadialSingleUse", this.m_singleUse.isOn ? 1 : 0);
			PlatformPrefs.SetInt("RadialHoverSpd", this.m_hoverSelect.value);
			PlatformPrefs.SetInt("RadialSpiral", this.m_spiralEffect.value);
			RadialData.SO.UsePersistantBackBtn = this.m_persistentBackBtn.isOn;
			RadialData.SO.EnableToggleAnimation = this.m_animateRadial.isOn;
			RadialData.SO.NudgeSelectedElement = this.m_animateRadial.isOn;
			RadialData.SO.EnableDoubleClick = this.m_doubleTap.isOn;
			RadialData.SO.EnableFlick = this.m_flick.isOn;
			RadialData.SO.EnableSingleUseMode = this.m_singleUse.isOn;
			RadialData.SO.SpiralEffectInsensity = (SpiralEffectIntensitySetting)this.m_spiralEffect.value;
			RadialData.SO.HoverSelectSelectionSpeed = (HoverSelectSpeedSetting)this.m_hoverSelect.value;
			Action saved = this.Saved;
			if (saved == null)
			{
				return;
			}
			saved();
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000D8576 File Offset: 0x000D6776
		private void OnFlickUpdated(bool value)
		{
			if (value && this.m_doubleTap.isOn)
			{
				this.m_doubleTap.isOn = false;
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000D8594 File Offset: 0x000D6794
		private void OnDoubleTapUpdated(bool value)
		{
			if (value && this.m_flick.isOn)
			{
				this.m_flick.isOn = false;
			}
		}

		// Token: 0x04001DCC RID: 7628
		[Header("Radial")]
		[SerializeField]
		private Button m_back;

		// Token: 0x04001DCD RID: 7629
		[SerializeField]
		private GuiDropdown m_hoverSelect;

		// Token: 0x04001DCE RID: 7630
		[SerializeField]
		private Toggle m_persistentBackBtn;

		// Token: 0x04001DCF RID: 7631
		[SerializeField]
		private Toggle m_animateRadial;

		// Token: 0x04001DD0 RID: 7632
		[SerializeField]
		private GuiDropdown m_spiralEffect;

		// Token: 0x04001DD1 RID: 7633
		[SerializeField]
		private Toggle m_doubleTap;

		// Token: 0x04001DD2 RID: 7634
		[SerializeField]
		private Toggle m_flick;

		// Token: 0x04001DD3 RID: 7635
		[SerializeField]
		private Toggle m_singleUse;

		// Token: 0x04001DD4 RID: 7636
		private Dictionary<HoverSelectSpeedSetting, string> m_hoverSpeedOptionStrings = new Dictionary<HoverSelectSpeedSetting, string>
		{
			{
				HoverSelectSpeedSetting.Off,
				"$radial_speed_off"
			},
			{
				HoverSelectSpeedSetting.Slow,
				"$radial_speed_slow"
			},
			{
				HoverSelectSpeedSetting.Medium,
				"$radial_speed_medium"
			},
			{
				HoverSelectSpeedSetting.Fast,
				"$radial_speed_fast"
			}
		};

		// Token: 0x04001DD5 RID: 7637
		private Dictionary<SpiralEffectIntensitySetting, string> m_sprialOptionStrings = new Dictionary<SpiralEffectIntensitySetting, string>
		{
			{
				SpiralEffectIntensitySetting.Off,
				"$settings_spiral_off"
			},
			{
				SpiralEffectIntensitySetting.Slight,
				"$settings_spiral_slight"
			},
			{
				SpiralEffectIntensitySetting.Normal,
				"$settings_spiral_normal"
			}
		};
	}
}
