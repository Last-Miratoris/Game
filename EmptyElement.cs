using System;

namespace Valheim.UI
{
	// Token: 0x02000205 RID: 517
	public class EmptyElement : RadialMenuElement
	{
		// Token: 0x06001D4F RID: 7503 RVA: 0x000DA3AE File Offset: 0x000D85AE
		public void Init()
		{
			base.Name = "Empty";
			base.Interact = (() => false);
		}
	}
}
