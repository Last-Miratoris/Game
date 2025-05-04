using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x0200020D RID: 525
	[Serializable]
	public struct ItemGroupMapping
	{
		// Token: 0x04001E18 RID: 7704
		public string Name;

		// Token: 0x04001E19 RID: 7705
		public ItemDrop.ItemData.ItemType[] ItemTypes;

		// Token: 0x04001E1A RID: 7706
		public Sprite Sprite;

		// Token: 0x04001E1B RID: 7707
		public string LocaString;

		// Token: 0x04001E1C RID: 7708
		public static string None = "none";
	}
}
