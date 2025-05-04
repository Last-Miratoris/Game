using System;
using TMPro;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000208 RID: 520
	public class ItemElement : RadialMenuElement
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x000DA6B3 File Offset: 0x000D88B3
		// (set) Token: 0x06001D5C RID: 7516 RVA: 0x000DA6BB File Offset: 0x000D88BB
		public Func<GameObject, bool> HoverMenuInteract { get; set; }

		// Token: 0x06001D5D RID: 7517 RVA: 0x000DA6C4 File Offset: 0x000D88C4
		public void Init(ItemDrop.ItemData item)
		{
			base.Name = "";
			base.Interact = null;
			base.SubTitle = "";
			base.SecondaryInteract = null;
			this.m_go.SetActive(item != null);
			if (item == null)
			{
				return;
			}
			this.m_data = item;
			this.SetInteraction(item);
			base.Name = Localization.instance.Localize(item.m_shared.m_name);
			base.Description = item.GetTooltip(-1);
			this.m_icon.sprite = ((item != null) ? item.GetIcon() : null);
			base.Activated = (this.m_data.m_equipped ? 1f : 0f);
			this.SetAmount(item);
			this.SetDurability(item);
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000DA782 File Offset: 0x000D8982
		public void UpdateQueueAndActivation(float progress, Player.MinorActionData data, int queueCount)
		{
			base.Queued = (progress <= 0.01f && (base.Queued || queueCount > 1));
			base.Activated = ((data.m_type == Player.MinorActionData.ActionType.Unequip) ? (1f - progress) : progress);
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000DA7BD File Offset: 0x000D89BD
		public void UpdateQueueAndActivation(bool equipActionQueued)
		{
			base.Queued = equipActionQueued;
			base.Activated = (this.m_data.m_equipped ? 1f : 0f);
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000DA7E8 File Offset: 0x000D89E8
		protected virtual void SetInteraction(ItemDrop.ItemData item)
		{
			base.Interact = delegate()
			{
				if (Player.m_localPlayer)
				{
					Player.m_localPlayer.UseItem(null, item, false);
				}
				return true;
			};
			this.HoverMenuInteract = delegate(GameObject hoverObject)
			{
				if (Player.m_localPlayer)
				{
					Player.m_localPlayer.TryUseItemOnInteractable(item, hoverObject, false);
				}
				return true;
			};
			base.SecondaryInteract = delegate()
			{
				if (Player.m_localPlayer)
				{
					Player.m_localPlayer.DropItem(null, item, 1);
				}
				return true;
			};
			base.TryOpenSubRadial = delegate(RadialBase menu, int currentIndex)
			{
				if (this.m_data.m_stack <= 1 || this.m_data.m_shared.m_maxStackSize <= 1)
				{
					return false;
				}
				menu.BackIndex = currentIndex;
				menu.QueuedOpen(RadialData.SO.ThrowGroupConfig, menu.CurrentConfig);
				return true;
			};
			base.CloseOnInteract = (() => this.m_data == null || !this.m_data.IsEquipable());
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000DA863 File Offset: 0x000D8A63
		public void UpdateDurabilityAndAmount()
		{
			this.SetAmount(this.m_data);
			this.SetDurability(this.m_data);
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000DA880 File Offset: 0x000D8A80
		protected virtual void SetAmount(ItemDrop.ItemData item)
		{
			if (item.m_shared.m_maxStackSize > 1)
			{
				this.m_amount.gameObject.SetActive(true);
				if (this.m_stackText != item.m_stack)
				{
					this.m_amount.text = string.Format("{0} / {1}", item.m_stack, item.m_shared.m_maxStackSize);
					this.m_stackText = item.m_stack;
				}
				base.SubTitle = this.m_amount.text;
				return;
			}
			this.m_amount.gameObject.SetActive(false);
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000DA91C File Offset: 0x000D8B1C
		protected virtual void SetDurability(ItemDrop.ItemData item)
		{
			bool flag = item.m_shared.m_useDurability && item.m_durability < item.GetMaxDurability();
			this.m_durability.gameObject.SetActive(flag);
			if (flag)
			{
				if (item.m_durability <= 0f)
				{
					this.m_durability.SetValue(1f);
					this.m_durability.SetColor((Mathf.Sin(Time.time * 10f) > 0f) ? Color.red : new Color(0f, 0f, 0f, 0f));
					return;
				}
				this.m_durability.SetValue(item.GetDurabilityPercentage());
				this.m_durability.ResetColor();
			}
		}

		// Token: 0x04001E04 RID: 7684
		public GameObject m_go;

		// Token: 0x04001E05 RID: 7685
		public GuiBar m_durability;

		// Token: 0x04001E06 RID: 7686
		public TMP_Text m_amount;

		// Token: 0x04001E07 RID: 7687
		public int m_stackText = -1;

		// Token: 0x04001E08 RID: 7688
		public ItemDrop.ItemData m_data;
	}
}
