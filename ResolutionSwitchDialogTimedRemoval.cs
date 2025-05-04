using System;
using TMPro;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F4 RID: 500
	public class ResolutionSwitchDialogTimedRemoval : MonoBehaviour
	{
		// Token: 0x06001CCC RID: 7372 RVA: 0x000D8634 File Offset: 0x000D6834
		private void Update()
		{
			this.m_resCountdownTimer -= Time.unscaledDeltaTime;
			this.m_resSwitchCountdown.text = Mathf.CeilToInt(this.m_resCountdownTimer).ToString();
			if (this.m_resCountdownTimer <= 0f || ZInput.GetButtonDown("JoyBack") || ZInput.GetButtonDown("JoyButtonB") || ZInput.GetKeyDown(KeyCode.Escape, true))
			{
				this.m_graphicsSettings.RevertMode();
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x000D86B7 File Offset: 0x000D68B7
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x000D86BF File Offset: 0x000D68BF
		public float ResCountdownTimer
		{
			get
			{
				return this.m_resCountdownTimer;
			}
			set
			{
				this.m_resCountdownTimer = value;
			}
		}

		// Token: 0x04001DD6 RID: 7638
		[SerializeField]
		private GraphicsSettings m_graphicsSettings;

		// Token: 0x04001DD7 RID: 7639
		private float m_resCountdownTimer = 1f;

		// Token: 0x04001DD8 RID: 7640
		[SerializeField]
		private TMP_Text m_resSwitchCountdown;
	}
}
