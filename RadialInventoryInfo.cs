using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.UI
{
	// Token: 0x02000215 RID: 533
	public class RadialInventoryInfo : MonoBehaviour
	{
		// Token: 0x06001DF3 RID: 7667 RVA: 0x000DDD7C File Offset: 0x000DBF7C
		internal void SetElement(ItemElement element, RadialMenuAnimationManager animator)
		{
			this.OverwriteWeightString(this.MakeInventoryWeightString(Player.m_localPlayer));
			this.SetTooltip(element.Name, element.Description, element.m_data, animator);
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000DDDA8 File Offset: 0x000DBFA8
		internal void SetElement(ThrowElement element, RadialMenuAnimationManager animator)
		{
			this.OverwriteWeightString(element.TotalWeightString);
			this.SetTooltip(element.Name, element.Description, element.m_data, animator);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000DDDD0 File Offset: 0x000DBFD0
		private void SetTooltip(string name, string description, ItemDrop.ItemData data, RadialMenuAnimationManager animator)
		{
			this.SetArmorString(data);
			if (this.m_currentTooltipHeight < this.m_toolTipMinHeight)
			{
				this.ResizeTooltipHeight(this.m_toolTipMinHeight);
			}
			this.m_itemTitleText.text = name;
			if (description == null)
			{
				return;
			}
			this.m_itemTooltipText.text = Localization.instance.Localize(description);
			this.StartResize(animator);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000DDE2C File Offset: 0x000DC02C
		private void SetArmorString(ItemDrop.ItemData data)
		{
			if (!this.m_armorText.gameObject.activeSelf)
			{
				return;
			}
			float num;
			if (data.TryGetArmorDifference(out num))
			{
				string str = Player.m_localPlayer.GetBodyArmor().ToString();
				string str2 = ((num > 0f) ? "<color=green>+" : ((num == 0f) ? "<color=orange>+" : "<color=red>")) + num.ToString() + "</color>";
				this.m_armorText.text = str + " " + str2;
				return;
			}
			this.m_armorText.text = Player.m_localPlayer.GetBodyArmor().ToString();
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000DDED4 File Offset: 0x000DC0D4
		internal void HideToolTip(RadialMenuAnimationManager animator)
		{
			animator.StartUniqueTween<float>(() => this.m_currentTooltipHeight, delegate(float newHeight)
			{
				this.ResizeTooltipHeight(newHeight);
				if (this.MinHeightCheck())
				{
					animator.CancelTweens("RadialInfoResize");
				}
			}, "RadialInfoResize", 0f, this.m_toolTipReSizeSpeed, this.m_reSizeEasingType, null, null);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000DDF30 File Offset: 0x000DC130
		public void OverwriteWeightString(string newWeightString)
		{
			if (this.m_inventoryWeightText.gameObject.activeSelf)
			{
				this.m_inventoryWeightText.text = newWeightString;
			}
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x000DDF50 File Offset: 0x000DC150
		public string MakeInventoryWeightString(Player localPlayer)
		{
			int num = Mathf.CeilToInt(localPlayer.GetInventory().GetTotalWeight());
			int num2 = Mathf.CeilToInt(localPlayer.GetMaxCarryWeight());
			if (num > num2 && Mathf.Sin(Time.time * 10f) > 0f)
			{
				return string.Format("<color=red>{0}</color> / {1}", num, num2);
			}
			return string.Format("{0} / {1}", num, num2);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000DDFC4 File Offset: 0x000DC1C4
		private void StartResize(RadialMenuAnimationManager animator)
		{
			float num = this.m_itemTooltipText.gameObject.activeSelf ? (this.m_itemTooltipText.GetPreferredValues().y + this.m_toolTipMinHeight + 10f) : this.m_toolTipMinHeight;
			num = Mathf.Clamp(num, this.m_toolTipMinHeight, this.m_toolTipMaxHeight);
			if (num < this.m_toolTipMinHeight)
			{
				this.HideToolTip(animator);
				return;
			}
			animator.StartUniqueTween<float>(() => this.m_currentTooltipHeight, new Action<float>(this.ResizeTooltipHeight), "RadialInfoResize", num, this.m_toolTipReSizeSpeed, this.m_reSizeEasingType, null, null);
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000DE05F File Offset: 0x000DC25F
		protected bool MinHeightCheck()
		{
			if (this.m_currentTooltipHeight >= this.m_toolTipMinHeight)
			{
				return false;
			}
			this.ResizeTooltipHeight(0f);
			this.m_itemTitleText.text = "";
			return true;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000DE08D File Offset: 0x000DC28D
		protected void ResizeTooltipHeight(float newHeight)
		{
			this.m_itemTooltip.sizeDelta = new Vector2(this.m_itemTooltip.sizeDelta.x, newHeight);
			this.m_currentTooltipHeight = newHeight;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000DE0B8 File Offset: 0x000DC2B8
		public void RefreshInfo()
		{
			Player localPlayer = Player.m_localPlayer;
			bool active = localPlayer != null;
			this.m_armorText.gameObject.SetActive(active);
			this.m_inventoryWeightText.gameObject.SetActive(active);
			this.m_itemTooltip.gameObject.SetActive(active);
			if (this.m_armorText.gameObject.activeSelf)
			{
				this.m_armorText.text = localPlayer.GetBodyArmor().ToString();
			}
			this.RefreshWeight();
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000DE137 File Offset: 0x000DC337
		public void RefreshWeight()
		{
			if (Player.m_localPlayer != null && this.m_inventoryWeightText.gameObject.activeSelf)
			{
				this.m_inventoryWeightText.text = this.MakeInventoryWeightString(Player.m_localPlayer);
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000DE16E File Offset: 0x000DC36E
		public void RefreshWeight(ThrowElement element)
		{
			this.OverwriteWeightString(element.TotalWeightString);
			this.m_itemTooltipText.text = Localization.instance.Localize(element.Description);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000DE198 File Offset: 0x000DC398
		public void SetAlpha(float newAlpha)
		{
			Color black = Color.black;
			black.a = Mathf.Min(newAlpha, 0.8f);
			this.m_armorBG.color = black;
			this.m_weightBG.color = black;
			Color white = Color.white;
			white.a = newAlpha;
			this.m_armorIcon.color = white;
			this.m_weightIcon.color = white;
			this.m_armorText.alpha = newAlpha;
			this.m_inventoryWeightText.alpha = newAlpha;
			this.m_tooltipBG.color = black;
			this.m_itemTooltipText.alpha = newAlpha;
			this.m_itemTitleText.alpha = newAlpha;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000DE237 File Offset: 0x000DC437
		private void OnEnable()
		{
			this.RefreshInfo();
			this.ResizeTooltipHeight(0f);
		}

		// Token: 0x04001EAD RID: 7853
		[SerializeField]
		private Image m_tooltipBG;

		// Token: 0x04001EAE RID: 7854
		[SerializeField]
		private Image m_armorBG;

		// Token: 0x04001EAF RID: 7855
		[SerializeField]
		private Image m_armorIcon;

		// Token: 0x04001EB0 RID: 7856
		[SerializeField]
		private Image m_weightBG;

		// Token: 0x04001EB1 RID: 7857
		[SerializeField]
		private Image m_weightIcon;

		// Token: 0x04001EB2 RID: 7858
		[SerializeField]
		protected TextMeshProUGUI m_armorText;

		// Token: 0x04001EB3 RID: 7859
		[SerializeField]
		protected TextMeshProUGUI m_inventoryWeightText;

		// Token: 0x04001EB4 RID: 7860
		[SerializeField]
		protected float m_toolTipMinHeight = 75f;

		// Token: 0x04001EB5 RID: 7861
		[SerializeField]
		protected float m_toolTipMaxHeight = 700f;

		// Token: 0x04001EB6 RID: 7862
		[SerializeField]
		protected float m_toolTipReSizeSpeed = 10f;

		// Token: 0x04001EB7 RID: 7863
		[SerializeField]
		private EasingType m_reSizeEasingType;

		// Token: 0x04001EB8 RID: 7864
		[SerializeField]
		protected RectTransform m_itemTooltip;

		// Token: 0x04001EB9 RID: 7865
		[SerializeField]
		protected TextMeshProUGUI m_itemTitleText;

		// Token: 0x04001EBA RID: 7866
		[SerializeField]
		protected TextMeshProUGUI m_itemTooltipText;

		// Token: 0x04001EBB RID: 7867
		private float m_currentTooltipHeight;
	}
}
