using System;

namespace Valheim.UI
{
	// Token: 0x02000202 RID: 514
	public class BackElement : GroupElement
	{
		// Token: 0x06001D33 RID: 7475 RVA: 0x000D9B38 File Offset: 0x000D7D38
		public void Init(RadialBase radial)
		{
			base.Name = "Back";
			base.Interact = delegate()
			{
				radial.Back();
				return true;
			};
		}
	}
}
