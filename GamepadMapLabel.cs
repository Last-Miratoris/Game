using System;
using TMPro;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001EA RID: 490
	public class GamepadMapLabel : MonoBehaviour
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x000D4970 File Offset: 0x000D2B70
		public TextMeshProUGUI Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x000D4978 File Offset: 0x000D2B78
		public TextMeshProUGUI Button
		{
			get
			{
				return this.button;
			}
		}

		// Token: 0x04001D4A RID: 7498
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001D4B RID: 7499
		[SerializeField]
		private TextMeshProUGUI button;
	}
}
