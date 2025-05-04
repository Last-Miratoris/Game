using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.UI
{
	// Token: 0x02000203 RID: 515
	public class ElementInfo : MonoBehaviour
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06001D35 RID: 7477 RVA: 0x000D9B77 File Offset: 0x000D7D77
		public Image BackgroundImage
		{
			get
			{
				if (this.m_background == null)
				{
					this.m_background = base.gameObject.GetComponent<Image>();
				}
				return this.m_background;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x000D9B9E File Offset: 0x000D7D9E
		public Image DurabilityBarBG
		{
			get
			{
				if (this.m_durabilityBarBG == null)
				{
					this.m_durabilityBarBG = this.m_durabilityBar.gameObject.GetComponent<Image>();
				}
				return this.m_durabilityBarBG;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x000D9BCA File Offset: 0x000D7DCA
		public RectTransform InfoTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = (base.transform as RectTransform);
				}
				return this.m_rectTransform;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x000D9BF1 File Offset: 0x000D7DF1
		// (set) Token: 0x06001D39 RID: 7481 RVA: 0x000D9C03 File Offset: 0x000D7E03
		public float Radius
		{
			get
			{
				return this.InfoTransform.sizeDelta.x;
			}
			set
			{
				this.InfoTransform.sizeDelta = Vector2.one * value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x000D9C1B File Offset: 0x000D7E1B
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x000D9C34 File Offset: 0x000D7E34
		public float Alpha
		{
			get
			{
				return this.CutoutMaterial.GetColor("_Color").a;
			}
			set
			{
				this.BGAlpha = value;
				Color color = this.IconMaterial.color;
				color.a = value;
				this.IconMaterial.color = color;
				this.m_title.alpha = value;
				this.m_subTitle.alpha = value;
				color = this.m_durabilityBar.GetColor();
				color.a = value;
				this.m_durabilityBar.SetColor(color);
				color = this.DurabilityBarBG.color;
				color.a = Mathf.Max(value, 0.65f);
				this.DurabilityBarBG.color = color;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x000D9CCA File Offset: 0x000D7ECA
		// (set) Token: 0x06001D3D RID: 7485 RVA: 0x000D9CE4 File Offset: 0x000D7EE4
		public float BGAlpha
		{
			get
			{
				return this.CutoutMaterial.GetColor("_Color").a;
			}
			set
			{
				Color color = this.CutoutMaterial.GetColor("_Color");
				color.a = Mathf.Clamp(value, 0f, 0.8f);
				this.CutoutMaterial.SetColor("_Color", color);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x000D9D2C File Offset: 0x000D7F2C
		public Material CutoutMaterial
		{
			get
			{
				if (this.m_cutoutMaterial)
				{
					return this.m_cutoutMaterial;
				}
				this.m_cutoutMaterial = new Material(this.BackgroundImage.material);
				this.BackgroundImage.material = this.m_cutoutMaterial;
				return this.m_cutoutMaterial;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06001D3F RID: 7487 RVA: 0x000D9D7C File Offset: 0x000D7F7C
		// (set) Token: 0x06001D40 RID: 7488 RVA: 0x000D9DCA File Offset: 0x000D7FCA
		public Material IconMaterial
		{
			get
			{
				if (this.m_iconMaterial)
				{
					return this.m_iconMaterial;
				}
				this.m_iconMaterial = new Material(this.m_icon.material);
				this.m_icon.material = this.m_iconMaterial;
				return this.m_iconMaterial;
			}
			set
			{
				this.m_iconMaterial = value;
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000D9DD4 File Offset: 0x000D7FD4
		public virtual void Clear()
		{
			this.m_subTitle.gameObject.SetActive(true);
			this.m_durabilityBar.gameObject.SetActive(false);
			this.m_title.text = "";
			this.m_subTitle.text = "";
			this.m_icon.gameObject.SetActive(false);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000D9E34 File Offset: 0x000D8034
		public void UpdateDurabilityAndWeightInfo(RadialMenuElement element)
		{
			if (this.m_durabilityBar.gameObject.activeSelf)
			{
				ItemElement itemElement = element as ItemElement;
				if (itemElement != null)
				{
					this.m_durabilityBar.SetValue(itemElement.m_durability.GetSmoothValue());
					this.m_durabilityBar.SetColor(itemElement.m_durability.GetColor());
				}
			}
			if (!this.m_inventoryInfo.gameObject.activeSelf)
			{
				return;
			}
			ThrowElement throwElement = element as ThrowElement;
			if (throwElement != null)
			{
				this.m_inventoryInfo.RefreshWeight(throwElement);
				return;
			}
			this.m_inventoryInfo.RefreshWeight();
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000D9EC0 File Offset: 0x000D80C0
		internal virtual void Set(RadialMenuElement element, RadialMenuAnimationManager animator)
		{
			if (!element)
			{
				this.Clear();
				return;
			}
			this.m_subTitle.gameObject.SetActive(true);
			this.m_durabilityBar.gameObject.SetActive(false);
			bool flag = element is ItemElement || element is ThrowElement;
			this.m_title.text = (flag ? "" : element.Name);
			this.m_subTitle.text = element.SubTitle;
			this.m_icon.gameObject.SetActive(flag);
			if (flag)
			{
				this.m_icon.sprite = element.Icon.sprite;
			}
			if (!this.m_inventoryInfo.gameObject.activeSelf)
			{
				return;
			}
			this.m_inventoryInfo.RefreshInfo();
			ItemElement itemElement = element as ItemElement;
			if (itemElement != null)
			{
				this.m_inventoryInfo.SetElement(itemElement, animator);
				this.<Set>g__SetDurabilityData|32_0(itemElement.m_data);
				return;
			}
			ThrowElement throwElement = element as ThrowElement;
			if (throwElement == null)
			{
				this.m_inventoryInfo.HideToolTip(animator);
				return;
			}
			this.m_inventoryInfo.SetElement(throwElement, animator);
			this.<Set>g__SetDurabilityData|32_0(throwElement.m_data);
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000D9FE0 File Offset: 0x000D81E0
		public virtual void Set(IRadialConfig config, bool updateAlpha = true)
		{
			this.Clear();
			if (config == null)
			{
				return;
			}
			this.m_title.text = config.LocalizedName;
			this.m_inventoryInfo.gameObject.SetActive(config is ItemGroupConfig || config is ThrowGroupConfig);
			this.m_inventoryInfo.RefreshInfo();
			if (!updateAlpha)
			{
				return;
			}
			this.Alpha = 1f;
			this.m_inventoryInfo.SetAlpha(1f);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000DA058 File Offset: 0x000D8258
		internal void OpenAnimation(RadialMenuAnimationManager manager, string id, float duration, float radius, float startOffset, EasingType alphaEasingType, EasingType positionEasingType)
		{
			this.Radius = radius + startOffset;
			this.Alpha = 0f;
			manager.StartTween<float>(() => this.Alpha, delegate(float val)
			{
				this.Alpha = val;
			}, id, 0.8f, duration + 0.1f, alphaEasingType, null, null);
			manager.StartTween<float>(new Action<float>(this.m_inventoryInfo.SetAlpha), id, 0f, 1f, duration + 0.1f, alphaEasingType, null, null);
			manager.StartTween<float>(() => this.Radius, delegate(float val)
			{
				this.Radius = val;
			}, id, radius, duration, positionEasingType, null, null);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000DA100 File Offset: 0x000D8300
		internal void CloseAnimation(RadialMenuAnimationManager manager, string id, float duration, float radius, float startOffset, EasingType alphaEasingType, EasingType positionEasingType)
		{
			manager.StartTween<float>(() => this.Alpha, delegate(float val)
			{
				this.Alpha = val;
			}, id, 0f, duration, alphaEasingType, null, null);
			manager.StartTween<float>(new Action<float>(this.m_inventoryInfo.SetAlpha), id, 1f, 0f, duration, alphaEasingType, null, null);
			manager.StartTween<float>(() => this.Radius, delegate(float val)
			{
				this.Radius = val;
			}, id, radius + startOffset, duration + 0.1f, positionEasingType, delegate()
			{
				this.Radius = radius;
			}, null);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000DA1B8 File Offset: 0x000D83B8
		[CompilerGenerated]
		private void <Set>g__SetDurabilityData|32_0(ItemDrop.ItemData data)
		{
			bool flag = data.m_shared.m_useDurability && data.m_durability < data.GetMaxDurability();
			this.m_durabilityBar.gameObject.SetActive(flag);
			this.m_subTitle.gameObject.SetActive(!flag);
			if (!flag)
			{
				return;
			}
			bool flag2 = data.m_durability <= 0f;
			this.m_durabilityBar.SetValue(flag2 ? 1f : data.GetDurabilityPercentage());
			if (flag2)
			{
				this.m_durabilityBar.SetColor((Mathf.Sin(Time.time * 10f) > 0f) ? Color.red : new Color(0f, 0f, 0f, 0f));
				return;
			}
			this.m_durabilityBar.ResetColor();
		}

		// Token: 0x04001DF5 RID: 7669
		[SerializeField]
		protected TextMeshProUGUI m_title;

		// Token: 0x04001DF6 RID: 7670
		[SerializeField]
		protected Image m_icon;

		// Token: 0x04001DF7 RID: 7671
		[SerializeField]
		protected GuiBar m_durabilityBar;

		// Token: 0x04001DF8 RID: 7672
		[SerializeField]
		protected TextMeshProUGUI m_subTitle;

		// Token: 0x04001DF9 RID: 7673
		[SerializeField]
		protected RadialInventoryInfo m_inventoryInfo;

		// Token: 0x04001DFA RID: 7674
		protected Image m_background;

		// Token: 0x04001DFB RID: 7675
		private Image m_durabilityBarBG;

		// Token: 0x04001DFC RID: 7676
		private RectTransform m_rectTransform;

		// Token: 0x04001DFD RID: 7677
		protected Material m_cutoutMaterial;

		// Token: 0x04001DFE RID: 7678
		protected Material m_iconMaterial;
	}
}
