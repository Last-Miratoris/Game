using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F6 RID: 502
	public class SettingsTooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06001CDA RID: 7386 RVA: 0x000D87A4 File Offset: 0x000D69A4
		private void Start()
		{
			if (this.m_tooltip == null)
			{
				Debug.LogWarning("No tooltip object set, removing tooltip component from " + base.gameObject.name);
				UnityEngine.Object.Destroy(this);
				return;
			}
			this.m_selectable = base.GetComponent<Selectable>();
			if (this.m_selectable == null)
			{
				Debug.LogWarning("No selectable found, removing tooltip component from " + base.gameObject.name);
				UnityEngine.Object.Destroy(this);
				return;
			}
			Transform transform = Utils.FindChild(this.m_tooltip.transform, "Topic", Utils.IterativeSearchType.DepthFirst);
			this.m_topic = ((transform != null) ? transform.GetComponent<TMP_Text>() : null);
			Transform transform2 = Utils.FindChild(this.m_tooltip.transform, "Text", Utils.IterativeSearchType.DepthFirst);
			this.m_text = ((transform2 != null) ? transform2.GetComponent<TMP_Text>() : null);
			Transform transform3 = Utils.FindChild(this.m_tooltip.transform, "Background", Utils.IterativeSearchType.DepthFirst);
			this.m_background = ((transform3 != null) ? transform3.GetComponent<Image>() : null);
			if (SettingsTooltip.s_current == null)
			{
				this.m_tooltip.gameObject.SetActive(false);
			}
			ZInput.OnInputLayoutChanged += this.OnInputLayoutChanged;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000D88C2 File Offset: 0x000D6AC2
		public void OnInputLayoutChanged()
		{
			if (!this.m_shown)
			{
				return;
			}
			this.Hide();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000D88D4 File Offset: 0x000D6AD4
		private void Update()
		{
			if (!ZInput.IsGamepadActive())
			{
				return;
			}
			if (SettingsTooltip.s_current == this.m_selectable && EventSystem.current.currentSelectedGameObject != this.m_selectable.gameObject)
			{
				this.Hide();
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != this.m_selectable.gameObject || (SettingsTooltip.s_current == this.m_selectable && this.m_shown))
			{
				return;
			}
			this.Show();
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000D895B File Offset: 0x000D6B5B
		private void OnDestroy()
		{
			ZInput.OnInputLayoutChanged -= this.OnInputLayoutChanged;
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000D896E File Offset: 0x000D6B6E
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (SettingsTooltip.s_current == this.m_selectable && this.m_shown)
			{
				return;
			}
			this.Show();
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000D8991 File Offset: 0x000D6B91
		public void OnPointerExit(PointerEventData eventData)
		{
			if (ZInput.GamepadActive)
			{
				return;
			}
			this.Hide();
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000D89A1 File Offset: 0x000D6BA1
		private void Show()
		{
			SettingsTooltip.s_current = this.m_selectable;
			this.m_shown = true;
			base.StartCoroutine(this.DelayedShow());
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000D89C2 File Offset: 0x000D6BC2
		private void Hide()
		{
			this.m_tooltip.gameObject.SetActive(false);
			this.m_shown = false;
			if (SettingsTooltip.s_current == this.m_selectable)
			{
				SettingsTooltip.s_current = null;
			}
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000D89F4 File Offset: 0x000D6BF4
		private IEnumerator DelayedShow()
		{
			yield return new WaitForSecondsRealtime(this.m_showDelay);
			if (SettingsTooltip.s_current != this.m_selectable || !this.m_shown)
			{
				yield break;
			}
			if (this.m_topic != null)
			{
				this.m_topic.text = Localization.instance.Localize(this.m_topicId);
			}
			RectTransform component;
			if (this.m_text != null)
			{
				this.m_text.text = Localization.instance.Localize(this.m_textId);
				if (this.m_background != null)
				{
					this.m_tooltip.gameObject.SetActive(true);
					this.m_topic.ForceMeshUpdate(false, false);
					this.m_text.ForceMeshUpdate(false, false);
					this.m_tooltip.gameObject.SetActive(false);
					yield return 0;
					component = this.m_background.gameObject.GetComponent<RectTransform>();
					component.sizeDelta = new Vector2(component.sizeDelta.x, this.m_topic.textBounds.size.y + this.m_text.textBounds.size.y + 15f);
					component = this.m_text.gameObject.GetComponent<RectTransform>();
					Vector2 anchoredPosition = component.anchoredPosition;
					anchoredPosition.y = -this.m_topic.textBounds.size.y - 10f;
					component.anchoredPosition = anchoredPosition;
				}
			}
			Vector3[] array = new Vector3[4];
			Transform transform = this.m_selectable.transform.Find("Background");
			if (transform != null)
			{
				component = transform.GetComponent<RectTransform>();
			}
			else
			{
				component = this.m_selectable.gameObject.GetComponent<RectTransform>();
			}
			component.GetWorldCorners(array);
			Vector2 vector = new Vector2(array[3].x - array[0].x, array[1].y - array[0].y);
			Vector3[] array2 = new Vector3[4];
			component = this.m_background.gameObject.GetComponent<RectTransform>();
			component.GetWorldCorners(array2);
			Vector2 vector2 = new Vector2(array2[3].x - array2[0].x, array2[1].y - array2[0].y);
			float num = vector2.x / component.rect.width;
			switch (this.m_tooltipAlignment)
			{
			case SettingsTooltip.TooltipAlignment.Left:
				this.m_tooltip.transform.position = new Vector2(array[0].x - vector2.x / 2f - (float)this.m_space * num, array[2].y - vector.y / 2f);
				break;
			case SettingsTooltip.TooltipAlignment.Bottom:
				this.m_tooltip.transform.position = new Vector2(array[0].x + vector.x / 2f, array[0].y - vector2.y / 2f - (float)this.m_space * num);
				break;
			case SettingsTooltip.TooltipAlignment.Right:
				this.m_tooltip.transform.position = new Vector2(array[2].x + vector2.x / 2f + (float)this.m_space * num, array[2].y - vector.y / 2f);
				break;
			case SettingsTooltip.TooltipAlignment.Top:
				this.m_tooltip.transform.position = new Vector2(array[1].x + vector.x / 2f, array[1].y + vector2.y / 2f + (float)this.m_space * num);
				break;
			}
			this.m_tooltip.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x04001DDA RID: 7642
		[SerializeField]
		private GameObject m_tooltip;

		// Token: 0x04001DDB RID: 7643
		[SerializeField]
		private float m_showDelay = 0.2f;

		// Token: 0x04001DDC RID: 7644
		[SerializeField]
		private int m_space = 15;

		// Token: 0x04001DDD RID: 7645
		[SerializeField]
		private string m_textId;

		// Token: 0x04001DDE RID: 7646
		[SerializeField]
		private string m_topicId;

		// Token: 0x04001DDF RID: 7647
		[SerializeField]
		private SettingsTooltip.TooltipAlignment m_tooltipAlignment = SettingsTooltip.TooltipAlignment.Right;

		// Token: 0x04001DE0 RID: 7648
		private static Selectable s_current;

		// Token: 0x04001DE1 RID: 7649
		private Selectable m_selectable;

		// Token: 0x04001DE2 RID: 7650
		private Image m_background;

		// Token: 0x04001DE3 RID: 7651
		private TMP_Text m_text;

		// Token: 0x04001DE4 RID: 7652
		private TMP_Text m_topic;

		// Token: 0x04001DE5 RID: 7653
		private bool m_shown;

		// Token: 0x020003A7 RID: 935
		public enum TooltipAlignment
		{
			// Token: 0x0400270B RID: 9995
			Left,
			// Token: 0x0400270C RID: 9996
			Bottom,
			// Token: 0x0400270D RID: 9997
			Right,
			// Token: 0x0400270E RID: 9998
			Top
		}
	}
}
