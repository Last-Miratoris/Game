using System;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001EE RID: 494
	public enum GraphicsQualityMode
	{
		// Token: 0x04001D76 RID: 7542
		Performance,
		// Token: 0x04001D77 RID: 7543
		Balanced,
		// Token: 0x04001D78 RID: 7544
		Quality,
		// Token: 0x04001D79 RID: 7545
		Constrained,
		// Token: 0x04001D7A RID: 7546
		VeryLow,
		// Token: 0x04001D7B RID: 7547
		[InspectorName("VeryLow")]
		Custom = 100
	}
}
