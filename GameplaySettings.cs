using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001EC RID: 492
	public class GameplaySettings : SettingsBase
	{
		// Token: 0x06001C6B RID: 7275 RVA: 0x000D4E94 File Offset: 0x000D3094
		public override void FixBackButtonNavigation(Button backButton)
		{
			base.SetNavigation(this.m_deleteAccount, SettingsBase.NavigationDirection.OnDown, backButton);
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, this.m_deleteAccount);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000D4EB2 File Offset: 0x000D30B2
		public override void FixOkButtonNavigation(Button okButton)
		{
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, this.m_deleteAccount);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000D4EC2 File Offset: 0x000D30C2
		public static void SetControllerSpecificFirstTimeSettings()
		{
			if (!PlayerPrefs.HasKey("AttackTowardsPlayerLookDir"))
			{
				PlatformPrefs.SetInt("AttackTowardsPlayerLookDir", ZInput.GamepadActive ? 1 : 0);
			}
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000D4EE8 File Offset: 0x000D30E8
		public override void LoadSettings()
		{
			GameplaySettings.SetControllerSpecificFirstTimeSettings();
			this.m_communityTranslation.gameObject.SetActive(false);
			this.m_languageKey = Localization.instance.GetSelectedLanguage();
			this.m_toggleRun.isOn = (PlatformPrefs.GetInt("ToggleRun", ZInput.IsGamepadActive() ? 1 : 0) == 1);
			this.m_toggleAttackTowardsPlayerLookDir.isOn = (PlatformPrefs.GetInt("AttackTowardsPlayerLookDir", 0) == 1);
			this.m_showKeyHints.isOn = (PlatformPrefs.GetInt("KeyHints", 1) == 1);
			this.m_tutorialsEnabled.isOn = (PlatformPrefs.GetInt("TutorialsEnabled", 1) == 1);
			this.m_reduceBGUsage.isOn = (PlatformPrefs.GetInt("ReduceBackgroundUsage", 0) == 1);
			this.m_autoBackups.value = (float)PlatformPrefs.GetInt("AutoBackups", 4);
			this.UpdateLanguageText();
			this.OnAutoBackupsChanged();
			ZInput.instance.ChangeLayout(ZInput.InputLayout);
			Settings instance = Settings.instance;
			instance.SharedSettingsChanged = (Action<string, int>)Delegate.Remove(instance.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings instance2 = Settings.instance;
			instance2.SharedSettingsChanged = (Action<string, int>)Delegate.Combine(instance2.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings.instance.SettingsPopupDestroyed -= this.SettingsPopupDestroyed;
			Settings.instance.SettingsPopupDestroyed += this.SettingsPopupDestroyed;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000D504C File Offset: 0x000D324C
		private void SettingsPopupDestroyed()
		{
			Settings instance = Settings.instance;
			instance.SharedSettingsChanged = (Action<string, int>)Delegate.Remove(instance.SharedSettingsChanged, new Action<string, int>(this.SharedSettingsChanged));
			Settings.instance.SettingsPopupDestroyed -= this.SettingsPopupDestroyed;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000D508C File Offset: 0x000D328C
		public override void SaveSettings()
		{
			PlatformPrefs.SetInt("KeyHints", this.m_showKeyHints.isOn ? 1 : 0);
			PlatformPrefs.SetInt("ToggleRun", this.m_toggleRun.isOn ? 1 : 0);
			PlatformPrefs.SetInt("AttackTowardsPlayerLookDir", this.m_toggleAttackTowardsPlayerLookDir.isOn ? 1 : 0);
			PlatformPrefs.SetInt("TutorialsEnabled", this.m_tutorialsEnabled.isOn ? 1 : 0);
			PlatformPrefs.SetInt("ReduceBackgroundUsage", this.m_reduceBGUsage.isOn ? 1 : 0);
			PlatformPrefs.SetInt("AutoBackups", (int)this.m_autoBackups.value);
			ZInput.ToggleRun = this.m_toggleRun.isOn;
			Raven.m_tutorialsEnabled = this.m_tutorialsEnabled.isOn;
			Settings.ReduceBackgroundUsage = this.m_reduceBGUsage.isOn;
			if (Player.m_localPlayer != null)
			{
				Player.m_localPlayer.AttackTowardsPlayerLookDir = this.m_toggleAttackTowardsPlayerLookDir.isOn;
			}
			Localization.instance.SetLanguage(this.m_languageKey);
			Action saved = this.Saved;
			if (saved == null)
			{
				return;
			}
			saved();
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000D51A8 File Offset: 0x000D33A8
		private void SharedSettingsChanged(string setting, int value)
		{
			if (setting == "ToggleRun" && this.m_toggleRun.isOn != (value == 1))
			{
				this.m_toggleRun.isOn = (value == 1);
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000D51D7 File Offset: 0x000D33D7
		public void OnLanguageLeft()
		{
			this.m_languageKey = Localization.instance.GetPrevLanguage(this.m_languageKey);
			this.UpdateLanguageText();
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000D51F5 File Offset: 0x000D33F5
		public void OnLanguageRight()
		{
			this.m_languageKey = Localization.instance.GetNextLanguage(this.m_languageKey);
			this.UpdateLanguageText();
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000D5214 File Offset: 0x000D3414
		private void UpdateLanguageText()
		{
			this.m_language.text = Localization.instance.Localize("$language_" + this.m_languageKey.ToLower());
			this.m_communityTranslation.gameObject.SetActive(this.m_language.text.Contains("*"));
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000D5270 File Offset: 0x000D3470
		public void OnResetTutorial()
		{
			Player.ResetSeenTutorials();
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000D5278 File Offset: 0x000D3478
		public void OnDeleteAccount()
		{
			if (!PlayFabManager.IsLoggedIn)
			{
				UnifiedPopup.Push(new WarningPopup("", "$settings_deleteplayfabaccount_not_logged_in", new PopupButtonCallback(UnifiedPopup.Pop), true));
				return;
			}
			if (ZNet.instance != null)
			{
				UnifiedPopup.Push(new WarningPopup("", "$settings_deleteplayfabaccount_ingamewarning", new PopupButtonCallback(UnifiedPopup.Pop), true));
				return;
			}
			UnifiedPopup.Push(new YesNoPopup("$settings_deleteplayfabaccount", "$settings_deleteplayfabaccount_text", delegate()
			{
				UnifiedPopup.Pop();
				PlayFabManager.instance.DeletePlayerTitleAccount();
			}, new PopupButtonCallback(UnifiedPopup.Pop), true));
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000D5320 File Offset: 0x000D3520
		public void OnAutoBackupsChanged()
		{
			this.m_autoBackupsText.text = ((this.m_autoBackups.value == 1f) ? "0" : this.m_autoBackups.value.ToString());
			this.m_cloudStorageWarning.gameObject.SetActive(this.m_autoBackups.value > (float)this.m_showCloudWarningBackupThreshold);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000D5388 File Offset: 0x000D3588
		public void OnToggleRunChanged()
		{
			Action<string, int> sharedSettingsChanged = Settings.instance.SharedSettingsChanged;
			if (sharedSettingsChanged == null)
			{
				return;
			}
			sharedSettingsChanged("ToggleRun", this.m_toggleRun.isOn ? 1 : 0);
		}

		// Token: 0x04001D61 RID: 7521
		[Header("Gameplay")]
		[SerializeField]
		private TMP_Text m_language;

		// Token: 0x04001D62 RID: 7522
		[SerializeField]
		private TMP_Text m_communityTranslation;

		// Token: 0x04001D63 RID: 7523
		[SerializeField]
		private TMP_Text m_cloudStorageWarning;

		// Token: 0x04001D64 RID: 7524
		[SerializeField]
		private Toggle m_toggleRun;

		// Token: 0x04001D65 RID: 7525
		[SerializeField]
		private Toggle m_toggleAttackTowardsPlayerLookDir;

		// Token: 0x04001D66 RID: 7526
		[SerializeField]
		private Toggle m_showKeyHints;

		// Token: 0x04001D67 RID: 7527
		[SerializeField]
		private Toggle m_tutorialsEnabled;

		// Token: 0x04001D68 RID: 7528
		[SerializeField]
		private Button m_resetTutorial;

		// Token: 0x04001D69 RID: 7529
		[SerializeField]
		private Toggle m_reduceBGUsage;

		// Token: 0x04001D6A RID: 7530
		[SerializeField]
		private Slider m_autoBackups;

		// Token: 0x04001D6B RID: 7531
		[SerializeField]
		private TMP_Text m_autoBackupsText;

		// Token: 0x04001D6C RID: 7532
		[SerializeField]
		private Button m_deleteAccount;

		// Token: 0x04001D6D RID: 7533
		private string m_languageKey = "";

		// Token: 0x04001D6E RID: 7534
		private int m_showCloudWarningBackupThreshold = 4;

		// Token: 0x04001D6F RID: 7535
		private const string c_AttackTowardsPlayerLookDirString = "AttackTowardsPlayerLookDir";
	}
}
