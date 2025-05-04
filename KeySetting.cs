using System;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F1 RID: 497
	[Serializable]
	public class KeySetting
	{
		// Token: 0x04001DBB RID: 7611
		public string m_keyName = "";

		// Token: 0x04001DBC RID: 7612
		public RectTransform m_keyTransform;

		// Token: 0x04001DBD RID: 7613
		public KeyCode[] m_blockedButtons;
	}
}
