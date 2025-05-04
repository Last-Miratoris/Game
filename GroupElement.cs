using System;
using System.Collections;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000206 RID: 518
	public class GroupElement : RadialMenuElement
	{
		// Token: 0x06001D51 RID: 7505 RVA: 0x000DA3E8 File Offset: 0x000D85E8
		public void Init(IRadialConfig config, IRadialConfig backConfig, RadialBase radial)
		{
			if (config == null)
			{
				base.Name = "";
				base.Interact = null;
			}
			else
			{
				base.Name = config.LocalizedName;
				base.Interact = delegate()
				{
					radial.QueuedOpen(config, backConfig);
					return true;
				};
			}
			this.m_icon.gameObject.SetActive(config.Sprite != null);
			this.m_icon.sprite = config.Sprite;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x000DA488 File Offset: 0x000D8688
		public virtual void ChangeToSelectColor()
		{
			if (this.m_colorChangeCoroutine != null)
			{
				Hud.instance.StopCoroutine(this.m_colorChangeCoroutine);
			}
			this.m_colorChangeCoroutine = Hud.instance.StartCoroutine(this.ChangeColor(base.BackgroundMaterial.GetColor("_SelectedColor"), 0.1f));
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000DA4D8 File Offset: 0x000D86D8
		public virtual void ChangeToDeselectColor()
		{
			if (this.m_colorChangeCoroutine != null)
			{
				Hud.instance.StopCoroutine(this.m_colorChangeCoroutine);
			}
			this.m_colorChangeCoroutine = Hud.instance.StartCoroutine(this.ChangeColor(Color.white, 0.1f));
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000DA512 File Offset: 0x000D8712
		protected IEnumerator ChangeColor(Color targetColor, float speed)
		{
			if (this.m_icon == null)
			{
				yield break;
			}
			float alpha = 0f;
			float duration = 0f;
			Color startColor = this.m_icon.color;
			while (this.m_icon != null && duration <= speed + 0.1f)
			{
				this.m_icon.color = Color.Lerp(startColor, targetColor, alpha);
				duration += Time.deltaTime;
				alpha = Mathf.Clamp01(duration / speed);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000DA52F File Offset: 0x000D872F
		protected void OnDisable()
		{
			if (this.m_colorChangeCoroutine != null)
			{
				Hud.instance.StopCoroutine(this.m_colorChangeCoroutine);
			}
		}

		// Token: 0x04001DFF RID: 7679
		protected Coroutine m_colorChangeCoroutine;
	}
}
