using System;
using System.Collections;
using System.Collections.Generic;
using GUIFramework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F2 RID: 498
	public class KeyboardMouseSettings : SettingsBase
	{
		// Token: 0x06001CB1 RID: 7345 RVA: 0x000D7A18 File Offset: 0x000D5C18
		public override void FixBackButtonNavigation(Button backButton)
		{
			Button componentInChildren = this.m_keys[this.m_keyRows - 1].m_keyTransform.GetComponentInChildren<Button>();
			base.SetNavigation(componentInChildren, SettingsBase.NavigationDirection.OnDown, backButton);
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, componentInChildren);
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000D7A58 File Offset: 0x000D5C58
		public override void FixOkButtonNavigation(Button okButton)
		{
			Button componentInChildren = this.m_keys[this.m_keyRows * this.m_keyCols - 1].m_keyTransform.GetComponentInChildren<Button>();
			base.SetNavigation(componentInChildren, SettingsBase.NavigationDirection.OnDown, okButton);
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, componentInChildren);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000D7A9C File Offset: 0x000D5C9C
		public override void LoadSettings()
		{
			PlayerController.m_mouseSens = PlatformPrefs.GetFloat("MouseSensitivity", PlayerController.m_mouseSens);
			this.m_mouseSensitivitySlider.value = PlatformPrefs.GetFloat("MouseSensitivity", PlayerController.m_mouseSens) / (float)KeyboardMouseSettings.m_mouseSensModifier;
			PlayerController.m_invertMouse = (PlatformPrefs.GetInt("InvertMouse", 0) == 1);
			this.m_invertMouse.isOn = PlayerController.m_invertMouse;
			this.m_quickPieceSelect.isOn = (PlatformPrefs.GetInt("QuickPieceSelect", 0) == 1);
			this.OnMouseSensitivityChanged();
			this.m_bindDialog.SetActive(false);
			this.SetupKeys();
			this.m_scrollRectVisibilityManager = base.GetComponentInChildren<ScrollRectEnsureVisible>();
			this.m_selectedGameObject = EventSystem.current.currentSelectedGameObject;
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000D7B4E File Offset: 0x000D5D4E
		public override void ResetSettings()
		{
			ZInput.instance.Load();
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000D7B5C File Offset: 0x000D5D5C
		public override void SaveSettings()
		{
			PlatformPrefs.SetFloat("MouseSensitivity", this.m_mouseSensitivitySlider.value * (float)KeyboardMouseSettings.m_mouseSensModifier);
			PlatformPrefs.SetInt("InvertMouse", this.m_invertMouse.isOn ? 1 : 0);
			PlatformPrefs.SetInt("QuickPieceSelect", this.m_quickPieceSelect.isOn ? 1 : 0);
			PlayerController.m_mouseSens = this.m_mouseSensitivitySlider.value * (float)KeyboardMouseSettings.m_mouseSensModifier;
			PlayerController.m_invertMouse = this.m_invertMouse.isOn;
			Action saved = this.Saved;
			if (saved == null)
			{
				return;
			}
			saved();
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000D7BF4 File Offset: 0x000D5DF4
		private void Update()
		{
			if (!this.m_bindDialog.activeSelf)
			{
				if (!ZInput.IsGamepadActive() || EventSystem.current.currentSelectedGameObject == this.m_selectedGameObject)
				{
					return;
				}
				this.m_selectedGameObject = EventSystem.current.currentSelectedGameObject;
				ScrollRectEnsureVisible scrollRectVisibilityManager = this.m_scrollRectVisibilityManager;
				if (scrollRectVisibilityManager == null)
				{
					return;
				}
				scrollRectVisibilityManager.CenterOnItem(this.m_selectedGameObject.transform as RectTransform);
				return;
			}
			else
			{
				this.m_blockInputDelay -= Time.unscaledDeltaTime;
				if (this.m_blockInputDelay >= 0f)
				{
					return;
				}
				if (this.InvalidKeyBind())
				{
					this.m_bindDialog.SetActive(false);
					this.InvalidKeybindPopup();
					return;
				}
				if (!ZInput.s_IsRebindActive && this.m_bindDialog.activeSelf)
				{
					this.m_bindDialog.SetActive(false);
					this.UpdateBindings();
					base.StartCoroutine(this.DelayedKeyEnable());
				}
				return;
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000D7CCC File Offset: 0x000D5ECC
		private bool InvalidKeyBind()
		{
			KeyCode[] blockedButtons = this.m_selectedKey.m_blockedButtons;
			for (int i = 0; i < blockedButtons.Length; i++)
			{
				if (ZInput.GetKeyDown(blockedButtons[i], true))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000D7D04 File Offset: 0x000D5F04
		private void InvalidKeybindPopup()
		{
			string header = "$invalid_keybind_header";
			string text = "$invalid_keybind_text";
			UnifiedPopup.Push(new WarningPopup(header, text, delegate()
			{
				UnifiedPopup.Pop();
				base.StartCoroutine(this.DelayedKeyEnable());
			}, true));
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000D7D34 File Offset: 0x000D5F34
		private IEnumerator DelayedKeyEnable()
		{
			if (base.gameObject == null)
			{
				yield break;
			}
			yield return null;
			this.EnableKeys(true);
			this.m_groupHandler.m_defaultElement = this.m_mouseSensitivitySlider.gameObject;
			Settings.instance.BlockNavigation(false);
			yield break;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000D7D44 File Offset: 0x000D5F44
		private void OnDestroy()
		{
			foreach (KeySetting keySetting in this.m_keys)
			{
				keySetting.m_keyTransform.GetComponentInChildren<GuiButton>().onClick.RemoveAllListeners();
			}
			this.m_keys.Clear();
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000D7DB0 File Offset: 0x000D5FB0
		public void OnMouseSensitivityChanged()
		{
			this.m_mouseSensitivityText.text = Mathf.Round(this.m_mouseSensitivitySlider.value * 100f).ToString() + "%";
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000D7DF0 File Offset: 0x000D5FF0
		private void SetupKeys()
		{
			int num = 0;
			int num2 = 0;
			using (List<KeySetting>.Enumerator enumerator = this.m_keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeySetting key = enumerator.Current;
					GuiButton componentInChildren = key.m_keyTransform.GetComponentInChildren<GuiButton>();
					componentInChildren.onClick.AddListener(delegate()
					{
						this.OpenBindDialog(key);
					});
					if (num < this.m_keyRows - 1)
					{
						int num3 = num2 * this.m_keyRows + num + 1;
						if (num3 < this.m_keys.Count)
						{
							GuiButton componentInChildren2 = this.m_keys[num3].m_keyTransform.GetComponentInChildren<GuiButton>();
							base.SetNavigation(componentInChildren, SettingsBase.NavigationDirection.OnDown, componentInChildren2);
						}
					}
					if (num > 0)
					{
						int num3 = num2 * this.m_keyRows + num - 1;
						GuiButton componentInChildren2 = this.m_keys[num3].m_keyTransform.GetComponentInChildren<GuiButton>();
						base.SetNavigation(componentInChildren, SettingsBase.NavigationDirection.OnUp, componentInChildren2);
					}
					if (num2 > 0)
					{
						int num3 = (num2 - 1) * this.m_keyRows + num;
						GuiButton componentInChildren2 = this.m_keys[num3].m_keyTransform.GetComponentInChildren<GuiButton>();
						base.SetNavigation(componentInChildren, SettingsBase.NavigationDirection.OnLeft, componentInChildren2);
					}
					if (num2 < this.m_keyCols - 1)
					{
						int num3 = (num2 + 1) * this.m_keyRows + num;
						if (num3 < this.m_keys.Count)
						{
							GuiButton componentInChildren2 = this.m_keys[num3].m_keyTransform.GetComponentInChildren<GuiButton>();
							base.SetNavigation(componentInChildren, SettingsBase.NavigationDirection.OnRight, componentInChildren2);
						}
					}
					num++;
					if (num % this.m_keyRows == 0)
					{
						num = 0;
						num2++;
					}
				}
			}
			this.UpdateBindings();
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000D7FA4 File Offset: 0x000D61A4
		private void EnableKeys(bool enable)
		{
			foreach (KeySetting keySetting in this.m_keys)
			{
				keySetting.m_keyTransform.GetComponentInChildren<GuiButton>().interactable = enable;
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000D8000 File Offset: 0x000D6200
		private void OpenBindDialog(KeySetting key)
		{
			ZLog.Log("Binding key " + key.m_keyName);
			this.m_selectedKey = key;
			Settings.instance.BlockNavigation(true);
			this.m_bindDialog.SetActive(true);
			this.m_blockInputDelay = 0.2f;
			this.m_groupHandler.m_defaultElement = EventSystem.current.currentSelectedGameObject;
			EventSystem.current.SetSelectedGameObject(this.m_bindDialog.gameObject);
			this.EnableKeys(false);
			ZInput.instance.StartBindKey(key.m_keyName);
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000D808C File Offset: 0x000D628C
		private void UpdateBindings()
		{
			foreach (KeySetting keySetting in this.m_keys)
			{
				keySetting.m_keyTransform.GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = Localization.instance.GetBoundKeyString(keySetting.m_keyName, true);
			}
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000D8100 File Offset: 0x000D6300
		public void ResetBindings()
		{
			ZInput.instance.ResetToDefault("all");
			this.UpdateBindings();
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000D8117 File Offset: 0x000D6317
		public static void SetPlatformSpecificFirstTimeSettings()
		{
			if (!PlayerPrefs.HasKey("MouseSensitivity"))
			{
				PlatformPrefs.SetFloat("MouseSensitivity", (float)KeyboardMouseSettings.m_mouseSensModifier);
			}
		}

		// Token: 0x04001DBE RID: 7614
		[SerializeField]
		private UIGroupHandler m_groupHandler;

		// Token: 0x04001DBF RID: 7615
		[Header("Controls")]
		[SerializeField]
		private Slider m_mouseSensitivitySlider;

		// Token: 0x04001DC0 RID: 7616
		[SerializeField]
		private TMP_Text m_mouseSensitivityText;

		// Token: 0x04001DC1 RID: 7617
		[SerializeField]
		private Toggle m_invertMouse;

		// Token: 0x04001DC2 RID: 7618
		[SerializeField]
		private Toggle m_quickPieceSelect;

		// Token: 0x04001DC3 RID: 7619
		[SerializeField]
		private GameObject m_bindDialog;

		// Token: 0x04001DC4 RID: 7620
		[SerializeField]
		private List<KeySetting> m_keys = new List<KeySetting>();

		// Token: 0x04001DC5 RID: 7621
		[SerializeField]
		private int m_keyRows = 13;

		// Token: 0x04001DC6 RID: 7622
		[SerializeField]
		private int m_keyCols = 2;

		// Token: 0x04001DC7 RID: 7623
		private GameObject m_selectedGameObject;

		// Token: 0x04001DC8 RID: 7624
		private ScrollRectEnsureVisible m_scrollRectVisibilityManager;

		// Token: 0x04001DC9 RID: 7625
		private float m_blockInputDelay;

		// Token: 0x04001DCA RID: 7626
		private KeySetting m_selectedKey;

		// Token: 0x04001DCB RID: 7627
		private static int m_mouseSensModifier = 1;
	}
}
