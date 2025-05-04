using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.UI
{
	// Token: 0x0200020A RID: 522
	public class ThrowElementInfo : ElementInfo
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000DACE1 File Offset: 0x000D8EE1
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x000DACEE File Offset: 0x000D8EEE
		public Sprite ItemIcon
		{
			get
			{
				return this.m_itemIcon.sprite;
			}
			set
			{
				this.m_itemIcon.sprite = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x000DACFC File Offset: 0x000D8EFC
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x000DAD39 File Offset: 0x000D8F39
		public Material ItemIconMaterial
		{
			get
			{
				if (this.m_iconMaterial == null)
				{
					this.m_itemIconMaterial = new Material(this.m_itemIcon.material);
					this.m_itemIcon.material = this.m_itemIconMaterial;
				}
				return this.m_itemIconMaterial;
			}
			set
			{
				this.m_itemIconMaterial = value;
			}
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000DAD44 File Offset: 0x000D8F44
		public void Init(ItemDrop.ItemData item)
		{
			if (item == null)
			{
				return;
			}
			this.ItemIcon = item.GetIcon();
			this.m_itemName = Localization.instance.Localize(item.m_shared.m_name);
			Color color = this.ItemIconMaterial.color;
			color.a = 1f;
			this.ItemIconMaterial.color = color;
			this.m_data = item;
			this.Clear();
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000DADB0 File Offset: 0x000D8FB0
		internal override void Set(RadialMenuElement element, RadialMenuAnimationManager animator)
		{
			ThrowElement throwElement = element as ThrowElement;
			if (throwElement != null)
			{
				this.m_title.text = this.m_itemName;
				this.m_subTitle.text = throwElement.SubTitle;
				this.m_icon.gameObject.SetActive(true);
				this.m_itemIcon.gameObject.SetActive(true);
				this.m_amountText.gameObject.SetActive(true);
				this.m_amountText.text = throwElement.m_inventoryAmountText;
				return;
			}
			if (element is BackElement)
			{
				this.Clear();
				this.m_title.text = element.Name;
				this.m_amountText.gameObject.SetActive(false);
				this.m_icon.gameObject.SetActive(false);
				return;
			}
			this.Clear();
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000DAE78 File Offset: 0x000D9078
		public override void Clear()
		{
			this.m_amountText.gameObject.SetActive(true);
			this.m_title.text = this.m_itemName;
			this.m_subTitle.text = "";
			this.m_icon.gameObject.SetActive(true);
			if (this.m_data == null)
			{
				return;
			}
			if (this.m_data.m_shared.m_maxStackSize > 1)
			{
				this.m_amountText.text = string.Format("{0} / {1}", this.m_data.m_stack, this.m_data.m_shared.m_maxStackSize);
				return;
			}
			this.m_amountText.text = string.Format("{0}", this.m_data.m_stack);
		}

		// Token: 0x04001E0F RID: 7695
		[SerializeField]
		protected Image m_itemIcon;

		// Token: 0x04001E10 RID: 7696
		[SerializeField]
		protected TextMeshProUGUI m_amountText;

		// Token: 0x04001E11 RID: 7697
		protected ItemDrop.ItemData m_data;

		// Token: 0x04001E12 RID: 7698
		protected string m_itemName;

		// Token: 0x04001E13 RID: 7699
		protected Material m_itemIconMaterial;
	}
}
