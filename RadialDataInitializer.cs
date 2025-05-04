using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000211 RID: 529
	public class RadialDataInitializer : MonoBehaviour
	{
		// Token: 0x06001DEC RID: 7660 RVA: 0x000DDA1D File Offset: 0x000DBC1D
		private void Awake()
		{
			RadialData.Init(this._dataObject);
		}

		// Token: 0x04001E59 RID: 7769
		[SerializeReference]
		private RadialDataSO _dataObject;
	}
}
