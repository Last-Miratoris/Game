using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000209 RID: 521
	public class ThrowElement : RadialMenuElement
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06001D65 RID: 7525 RVA: 0x000DA9E8 File Offset: 0x000D8BE8
		public string TotalWeightString
		{
			get
			{
				Player localPlayer = Player.m_localPlayer;
				if (localPlayer == null)
				{
					return "";
				}
				int num = Mathf.CeilToInt(localPlayer.GetInventory().GetTotalWeight());
				int num2 = Mathf.CeilToInt(localPlayer.GetMaxCarryWeight());
				int num3 = Mathf.CeilToInt((float)this.ThrowAmount * this.m_data.GetNonStackedWeight());
				if (num - num3 > num2 && Mathf.Sin(Time.time * 10f) > 0f)
				{
					return string.Format("<color=red>{0} - {1}</color> / {2}", num, num3, num2);
				}
				return string.Format("{0} - {1} / {2}", num, num3, num2);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06001D66 RID: 7526 RVA: 0x000DAA96 File Offset: 0x000D8C96
		public int ThrowAmount
		{
			get
			{
				if (this.m_throwAmount >= 0 || this.m_throwMultiplier < 0f)
				{
					return this.m_throwAmount;
				}
				return Mathf.RoundToInt((float)this.m_data.m_stack * this.m_throwMultiplier);
			}
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000DAACD File Offset: 0x000D8CCD
		public void Init(ItemDrop.ItemData item, int throwAmount, string localString)
		{
			this.Init(item, throwAmount);
			this.m_throwAmountText.text = Localization.instance.Localize(localString);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x000DAAED File Offset: 0x000D8CED
		public void Init(ItemDrop.ItemData item, int throwAmount)
		{
			this.m_throwAmount = throwAmount;
			this.m_throwAmountText.text = this.m_throwAmount.ToString();
			this.SetProperties(item);
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x000DAB14 File Offset: 0x000D8D14
		private void SetProperties(ItemDrop.ItemData item)
		{
			base.Name = "";
			base.Interact = null;
			base.SubTitle = "";
			base.SecondaryInteract = null;
			base.Name = Localization.instance.Localize(item.m_shared.m_name);
			this.m_data = item;
			this.SetInteraction(item);
			this.SetSubTitle(item);
			this.m_icon.sprite = ((item != null) ? item.GetIcon() : null);
			this.m_icon.gameObject.SetActive(false);
			this.SetDescription(item);
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000DABA4 File Offset: 0x000D8DA4
		private void SetDescription(ItemDrop.ItemData item)
		{
			base.Description = item.GetTooltip(-1);
			int num = Mathf.CeilToInt((float)this.ThrowAmount * this.m_data.GetNonStackedWeight());
			string newWeightString = string.Format("\n$item_weight: <color=orange>{0} ({1} - {2} $item_total)</color>", item.GetNonStackedWeight(), item.GetWeight(-1), num);
			base.Description = string.Join("\n", base.Description.Split('\n', StringSplitOptions.None).Select(delegate(string line)
			{
				if (!line.StartsWith("$item_weight:"))
				{
					return line;
				}
				return newWeightString;
			}));
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x000DAC3C File Offset: 0x000D8E3C
		private void SetSubTitle(ItemDrop.ItemData item)
		{
			base.SubTitle = ((item.m_shared.m_maxStackSize > 1) ? string.Format("{0} - {1} / {2}", item.m_stack, this.ThrowAmount, item.m_shared.m_maxStackSize) : "");
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x000DAC94 File Offset: 0x000D8E94
		protected void SetInteraction(ItemDrop.ItemData item)
		{
			base.Interact = delegate()
			{
				if (Player.m_localPlayer)
				{
					if (!Player.m_localPlayer.GetInventory().ContainsItemByName(item.m_shared.m_name))
					{
						return false;
					}
					Player.m_localPlayer.DropItem(null, item, this.ThrowAmount);
				}
				return true;
			};
		}

		// Token: 0x04001E0A RID: 7690
		[SerializeField]
		protected TextMeshProUGUI m_throwAmountText;

		// Token: 0x04001E0B RID: 7691
		[HideInInspector]
		public string m_inventoryAmountText;

		// Token: 0x04001E0C RID: 7692
		[HideInInspector]
		public ItemDrop.ItemData m_data;

		// Token: 0x04001E0D RID: 7693
		protected float m_throwMultiplier = -1f;

		// Token: 0x04001E0E RID: 7694
		protected int m_throwAmount = -1;
	}
}
