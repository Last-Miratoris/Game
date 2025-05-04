using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001FD RID: 509
	public interface IRadialConfig
	{
		// Token: 0x06001D0E RID: 7438
		void InitRadialConfig(RadialBase radial);

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06001D0F RID: 7439
		string LocalizedName { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06001D10 RID: 7440
		Sprite Sprite { get; }
	}
}
