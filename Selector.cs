using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Valheim.UI
{
	// Token: 0x020001F7 RID: 503
	public class Selector : MonoBehaviour
	{
		// Token: 0x06001CE4 RID: 7396 RVA: 0x000D8A25 File Offset: 0x000D6C25
		public void SetText(string text)
		{
			if (this.label != null)
			{
				this.label.text = text;
			}
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000D8A41 File Offset: 0x000D6C41
		public void OnLeftButtonClicked()
		{
			UnityEvent onLeftButtonClickedEvent = this.OnLeftButtonClickedEvent;
			if (onLeftButtonClickedEvent == null)
			{
				return;
			}
			onLeftButtonClickedEvent.Invoke();
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000D8A53 File Offset: 0x000D6C53
		public void OnRightButtonClicked()
		{
			UnityEvent onRightButtonClickedEvent = this.OnRightButtonClickedEvent;
			if (onRightButtonClickedEvent == null)
			{
				return;
			}
			onRightButtonClickedEvent.Invoke();
		}

		// Token: 0x04001DE6 RID: 7654
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001DE7 RID: 7655
		public UnityEvent OnLeftButtonClickedEvent;

		// Token: 0x04001DE8 RID: 7656
		public UnityEvent OnRightButtonClickedEvent;
	}
}
