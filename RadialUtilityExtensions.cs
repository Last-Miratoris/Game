using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001FA RID: 506
	internal static class RadialUtilityExtensions
	{
		// Token: 0x06001D05 RID: 7429 RVA: 0x000D8E7C File Offset: 0x000D707C
		internal static bool TryGetComponentInParent<T>(this GameObject go, out T result)
		{
			result = go.GetComponentInParent<T>();
			return result != null;
		}
	}
}
