using System;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.UI
{
	// Token: 0x0200021B RID: 539
	public static class ScrollRectExtensions
	{
		// Token: 0x06001E77 RID: 7799 RVA: 0x000DF994 File Offset: 0x000DDB94
		public static void SnapToChild(this ScrollRect scrollRect, RectTransform child)
		{
			Vector2 vector = scrollRect.viewport.transform.InverseTransformPoint(child.position);
			float height = scrollRect.viewport.rect.height;
			bool flag = vector.y > 0f;
			bool flag2 = -vector.y + child.rect.height > height;
			float num = flag ? (-vector.y) : (flag2 ? (-vector.y + child.rect.height - height) : 0f);
			scrollRect.content.anchoredPosition = new Vector2(0f, scrollRect.content.anchoredPosition.y + num);
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000DFA50 File Offset: 0x000DDC50
		public static bool IsVisible(this ScrollRect scrollRect, RectTransform child)
		{
			float height = scrollRect.viewport.rect.height;
			Vector2 vector = scrollRect.viewport.transform.InverseTransformPoint(child.position);
			return vector.y < 0f && -vector.y + child.rect.height < height;
		}
	}
}
