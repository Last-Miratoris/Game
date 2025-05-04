using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000216 RID: 534
	public class RadialKeyHints : MonoBehaviour
	{
		// Token: 0x06001E04 RID: 7684 RVA: 0x000DE27B File Offset: 0x000DC47B
		private void Update()
		{
			if (this.m_Next != null)
			{
				this.m_Next.SetActive(ZInput.IsGamepadActive());
			}
			if (this.m_Prev != null)
			{
				this.m_Prev.SetActive(ZInput.IsGamepadActive());
			}
		}

		// Token: 0x04001EBC RID: 7868
		[SerializeField]
		protected GameObject m_Next;

		// Token: 0x04001EBD RID: 7869
		[SerializeField]
		protected GameObject m_Prev;
	}
}
