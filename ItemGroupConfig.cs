using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001FE RID: 510
	[CreateAssetMenu(fileName = "ItemGroupConfig", menuName = "Valheim/Radial/Group Config/Item Group Config")]
	public class ItemGroupConfig : ScriptableObject, IRadialConfig
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x000D8FFC File Offset: 0x000D71FC
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x000D9004 File Offset: 0x000D7204
		public string GroupName { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x000D900D File Offset: 0x000D720D
		// (set) Token: 0x06001D14 RID: 7444 RVA: 0x000D9015 File Offset: 0x000D7215
		public ItemDrop.ItemData.ItemType[] ItemTypes { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x000D901E File Offset: 0x000D721E
		public string LocalizedName
		{
			get
			{
				return Localization.instance.Localize(this.IsCustom ? this.GroupName : ("$" + RadialData.SO.ItemGroupMappings.GetMapping(this.GroupName).LocaString));
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x000D905E File Offset: 0x000D725E
		public Sprite Sprite
		{
			get
			{
				return RadialData.SO.ItemGroupMappings.GetMapping(this.GroupName).Sprite;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000D907A File Offset: 0x000D727A
		private bool IsCustom
		{
			get
			{
				return RadialData.SO.ItemGroupMappings.Groups.All((ItemGroupMapping g) => g.Name != this.GroupName);
			}
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000D909C File Offset: 0x000D729C
		public void InitRadialConfig(RadialBase radial)
		{
			List<RadialMenuElement> list = new List<RadialMenuElement>();
			Player localPlayer = Player.m_localPlayer;
			Inventory inventory = localPlayer.GetInventory();
			if (this.m_storedList != null)
			{
				foreach (ItemDrop.ItemData item in this.m_storedList)
				{
					this.AddElement(list, item, radial, localPlayer, inventory);
				}
				this.m_storedList = null;
				radial.ConstructRadial(list);
				return;
			}
			if (this.ItemTypes == null)
			{
				this.ItemTypes = RadialData.SO.ItemGroupMappings.GetMapping(this.GroupName).ItemTypes;
			}
			if (this.ItemTypes[0] != ItemDrop.ItemData.ItemType.None)
			{
				foreach (ItemDrop.ItemData item2 in inventory.GetAllItemsOfType(this.ItemTypes, true))
				{
					this.AddElement(list, item2, radial, localPlayer, inventory);
				}
				if (this.m_customItemList.Count > 0 && list.Count > 0 && radial.StartItemIndex == -1)
				{
					if (this.m_customItemList[0] == "type")
					{
						radial.StartItemIndex = list.IndexOf(list.OfType<ItemElement>().FirstOrDefault((ItemElement e) => this.m_customItemList.Contains(e.m_data.m_shared.m_itemType.ToString()))) + 1;
					}
					else
					{
						radial.StartItemIndex = list.IndexOf(list.OfType<ItemElement>().FirstOrDefault((ItemElement e) => this.m_customItemList.Contains(e.m_data.m_shared.m_name))) + 1;
					}
				}
			}
			else
			{
				foreach (ItemDrop.ItemData item3 in from i in inventory.GetAllItemsInGridOrder()
				where this.m_customItemList.Contains(i.m_shared.m_name)
				select i)
				{
					this.AddElement(list, item3, radial, localPlayer, inventory);
				}
			}
			radial.ConstructRadial(list);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000D9294 File Offset: 0x000D7494
		private void AddElement(List<RadialMenuElement> elements, ItemDrop.ItemData item, RadialBase radial, Player player, Inventory playerInventory)
		{
			ItemElement itemElement = UnityEngine.Object.Instantiate<ItemElement>(RadialData.SO.ItemElement);
			itemElement.Init(item);
			elements2.Add(itemElement);
			if (radial2.IsHoverMenu)
			{
				itemElement.AdvancedCloseOnInteract = delegate(RadialBase radial, RadialArray<RadialMenuElement> elements)
				{
					Player localPlayer = Player.m_localPlayer;
					if (!localPlayer)
					{
						return true;
					}
					if (!radial.HoverObject)
					{
						return true;
					}
					IHasHoverMenuExtended hasHoverMenuExtended;
					if (radial.HoverObject.TryGetComponentInParent(out hasHoverMenuExtended))
					{
						return !hasHoverMenuExtended.CanUseItems(localPlayer, radial.HoverObject.GetComponent<Switch>(), false);
					}
					IHasHoverMenu hasHoverMenu;
					return radial.HoverObject.TryGetComponentInParent(out hasHoverMenu) && !hasHoverMenu.CanUseItems(localPlayer, false);
				};
				return;
			}
			if (item.m_shared.m_food > 0f)
			{
				itemElement.AdvancedCloseOnInteract = delegate(RadialBase radial, RadialArray<RadialMenuElement> elements)
				{
					Player localPlayer = Player.m_localPlayer;
					if (localPlayer)
					{
						return (from e in elements.GetArray
						where e is ItemElement
						select e).Cast<ItemElement>().All((ItemElement element) => !localPlayer.CanEat(element.m_data, false));
					}
					return true;
				};
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000D9324 File Offset: 0x000D7524
		public bool ShouldAddItem(ItemDrop.ItemData newItemData)
		{
			if (!this.IsCustom)
			{
				return Array.IndexOf<ItemDrop.ItemData.ItemType>(this.ItemTypes, newItemData.m_shared.m_itemType) > -1;
			}
			return this.m_customItemList.Contains(newItemData.m_shared.m_name);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000D9360 File Offset: 0x000D7560
		public void AddItem(RadialBase radial, ItemDrop.ItemData newItemData, RadialArray<RadialMenuElement> currentElements)
		{
			if (!this.ShouldAddItem(newItemData))
			{
				return;
			}
			List<ItemDrop.ItemData> list = (from ItemElement element in 
				from element in currentElements.GetArray
				where element is ItemElement
				select element
			select element.m_data).ToList<ItemDrop.ItemData>();
			List<ItemDrop.ItemData> list2 = Player.m_localPlayer.GetInventory().GetAllItems().Where(delegate(ItemDrop.ItemData item)
			{
				if (!this.IsCustom)
				{
					return Array.IndexOf<ItemDrop.ItemData.ItemType>(this.ItemTypes, item.m_shared.m_itemType) > -1;
				}
				return this.m_customItemList.Contains(item.m_shared.m_name);
			}).ToList<ItemDrop.ItemData>();
			if (list.Count == list2.Count)
			{
				radial.Refresh();
				return;
			}
			list.Add(newItemData);
			this.m_storedList = list;
			radial.Refresh();
		}

		// Token: 0x04001DF1 RID: 7665
		[HideInInspector]
		public List<string> m_customItemList = new List<string>();

		// Token: 0x04001DF2 RID: 7666
		private List<ItemDrop.ItemData> m_storedList;
	}
}
