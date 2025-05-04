using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000207 RID: 519
	public class HammerItemElement : GroupElement
	{
		// Token: 0x06001D57 RID: 7511 RVA: 0x000DA554 File Offset: 0x000D8754
		public void Init()
		{
			Player localPlayer = Player.m_localPlayer;
			List<ItemDrop.ItemData> list = (from e in localPlayer.GetInventory().GetAllItemsInGridOrder()
			where HammerItemElement.IsHammer(e) && e.m_durability > 0f
			select e).ToList<ItemDrop.ItemData>();
			if (list.Count <= 0)
			{
				return;
			}
			this.m_hammerRef = (list.FirstOrDefault((ItemDrop.ItemData i) => i.m_equipped) ?? list[0]);
			base.Icon.sprite = this.m_hammerIcon;
			this.SetInteraction(this.m_hammerRef);
			base.Activated = (this.m_hammerRef.m_equipped ? ((localPlayer.GetActionQueueCount() > 0) ? 0f : 1f) : 0f);
			base.Name = Localization.instance.Localize("$radial_hammer");
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000DA63D File Offset: 0x000D883D
		private static bool IsHammer(ItemDrop.ItemData item)
		{
			return item != null && item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Tool && item.m_shared.m_skillType == Skills.SkillType.Swords && item.m_shared.m_name.Contains("hammer");
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000DA678 File Offset: 0x000D8878
		protected void SetInteraction(ItemDrop.ItemData item)
		{
			base.Interact = delegate()
			{
				if (Player.m_localPlayer)
				{
					if (item.m_equipped)
					{
						Player.m_localPlayer.UseItem(null, item, false);
						if (HammerItemElement.m_lastLeftItem != null)
						{
							Player.m_localPlayer.EquipItem(HammerItemElement.m_lastLeftItem, true);
							HammerItemElement.m_lastLeftItem = null;
						}
						if (HammerItemElement.m_lastRightItem != null)
						{
							Player.m_localPlayer.EquipItem(HammerItemElement.m_lastRightItem, true);
							HammerItemElement.m_lastRightItem = null;
						}
					}
					else
					{
						HammerItemElement.m_lastLeftItem = Player.m_localPlayer.LeftItem;
						HammerItemElement.m_lastRightItem = Player.m_localPlayer.RightItem;
						Player.m_localPlayer.UseItem(null, item, false);
						Player.m_localPlayer.SetSelectedPiece(Vector2Int.zero);
					}
				}
				this.Activated = (this.m_hammerRef.m_equipped ? 1f : 0f);
				return true;
			};
		}

		// Token: 0x04001E00 RID: 7680
		private static ItemDrop.ItemData m_lastLeftItem;

		// Token: 0x04001E01 RID: 7681
		private static ItemDrop.ItemData m_lastRightItem;

		// Token: 0x04001E02 RID: 7682
		[SerializeField]
		protected Sprite m_hammerIcon;

		// Token: 0x04001E03 RID: 7683
		protected ItemDrop.ItemData m_hammerRef;
	}
}
