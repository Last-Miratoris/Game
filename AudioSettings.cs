using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001E5 RID: 485
	public class AudioSettings : SettingsBase
	{
		// Token: 0x06001BFA RID: 7162 RVA: 0x000D2AB7 File Offset: 0x000D0CB7
		public override void FixBackButtonNavigation(Button backButton)
		{
			base.SetNavigation(this.m_continousMusic, SettingsBase.NavigationDirection.OnDown, backButton);
			base.SetNavigation(backButton, SettingsBase.NavigationDirection.OnUp, this.m_continousMusic);
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000D2AD5 File Offset: 0x000D0CD5
		public override void FixOkButtonNavigation(Button okButton)
		{
			base.SetNavigation(okButton, SettingsBase.NavigationDirection.OnUp, this.m_continousMusic);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000D2AE8 File Offset: 0x000D0CE8
		public override void LoadSettings()
		{
			this.m_volumeSlider.value = PlatformPrefs.GetFloat("MasterVolume", AudioListener.volume);
			this.m_sfxVolumeSlider.value = PlatformPrefs.GetFloat("SfxVolume", 1f);
			this.m_musicVolumeSlider.value = PlatformPrefs.GetFloat("MusicVolume", 1f);
			this.m_continousMusic.isOn = (PlatformPrefs.GetInt("ContinousMusic", 1) == 1);
			this.m_oldVolume = this.m_volumeSlider.value;
			this.m_oldSfxVolume = this.m_sfxVolumeSlider.value;
			this.m_oldMusicVolume = this.m_musicVolumeSlider.value;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000D2B90 File Offset: 0x000D0D90
		public override void SaveSettings()
		{
			PlatformPrefs.SetFloat("MasterVolume", this.m_volumeSlider.value);
			PlatformPrefs.SetFloat("MusicVolume", this.m_musicVolumeSlider.value);
			PlatformPrefs.SetFloat("SfxVolume", this.m_sfxVolumeSlider.value);
			PlatformPrefs.SetInt("ContinousMusic", this.m_continousMusic.isOn ? 1 : 0);
			Settings.ContinousMusic = this.m_continousMusic.isOn;
			Action saved = this.Saved;
			if (saved == null)
			{
				return;
			}
			saved();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000D2C17 File Offset: 0x000D0E17
		public override void ResetSettings()
		{
			AudioListener.volume = this.m_oldVolume;
			MusicMan.m_masterMusicVolume = this.m_oldMusicVolume;
			AudioMan.SetSFXVolume(this.m_oldSfxVolume);
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000D2C3C File Offset: 0x000D0E3C
		public void OnAudioChanged()
		{
			AudioListener.volume = this.m_volumeSlider.value;
			this.m_volumeText.text = Mathf.Round(AudioListener.volume * 100f).ToString() + "%";
			MusicMan.m_masterMusicVolume = this.m_musicVolumeSlider.value;
			this.m_musicVolumeText.text = Mathf.Round(MusicMan.m_masterMusicVolume * 100f).ToString() + "%";
			AudioMan.SetSFXVolume(this.m_sfxVolumeSlider.value);
			this.m_sfxVolumeText.text = Mathf.Round(AudioMan.GetSFXVolume() * 100f).ToString() + "%";
		}

		// Token: 0x04001CE7 RID: 7399
		[Header("Audio")]
		[SerializeField]
		private Slider m_volumeSlider;

		// Token: 0x04001CE8 RID: 7400
		[SerializeField]
		private TMP_Text m_volumeText;

		// Token: 0x04001CE9 RID: 7401
		[SerializeField]
		private Slider m_sfxVolumeSlider;

		// Token: 0x04001CEA RID: 7402
		[SerializeField]
		private TMP_Text m_sfxVolumeText;

		// Token: 0x04001CEB RID: 7403
		[SerializeField]
		private Slider m_musicVolumeSlider;

		// Token: 0x04001CEC RID: 7404
		[SerializeField]
		private TMP_Text m_musicVolumeText;

		// Token: 0x04001CED RID: 7405
		[SerializeField]
		private Toggle m_continousMusic;

		// Token: 0x04001CEE RID: 7406
		[SerializeField]
		private AudioMixer m_masterMixer;

		// Token: 0x04001CEF RID: 7407
		private float m_oldVolume;

		// Token: 0x04001CF0 RID: 7408
		private float m_oldSfxVolume;

		// Token: 0x04001CF1 RID: 7409
		private float m_oldMusicVolume;
	}
}
