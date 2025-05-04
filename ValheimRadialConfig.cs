using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000201 RID: 513
	[CreateAssetMenu(fileName = "ValheimRadialConfig", menuName = "Valheim/Radial/Group Config/Main Group Config")]
	public class ValheimRadialConfig : ScriptableObject, IRadialConfig
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x000D9943 File Offset: 0x000D7B43
		public string LocalizedName
		{
			get
			{
				return "Main";
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x000D994A File Offset: 0x000D7B4A
		public Sprite Sprite
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000D9950 File Offset: 0x000D7B50
		public void InitRadialConfig(RadialBase radial)
		{
			List<RadialMenuElement> list = new List<RadialMenuElement>();
			this.AddHotbarGroup(radial, list);
			this.AddItemGroup(radial, list, "consumables");
			this.AddItemGroup(radial, list, "weaponstools");
			this.AddItemGroup(radial, list, "armor_utility");
			this.AddEmoteGroup(radial, list);
			this.AddItemGroup(radial, list, "allitems");
			if (Player.m_localPlayer != null && Player.m_localPlayer.GetInventory().ContainsItemByName("$item_hammer"))
			{
				this.AddHammer(list);
			}
			else
			{
				this.AddEmpty(list);
			}
			if (radial.LastUsed != null)
			{
				list.Add(radial.LastUsed);
				radial.LastUsed.Hovering = 0f;
			}
			else
			{
				this.AddEmpty(list);
			}
			radial.ConstructRadial(list);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000D9A14 File Offset: 0x000D7C14
		private void AddEmoteGroup(RadialBase radial, List<RadialMenuElement> elements)
		{
			GroupElement groupElement = UnityEngine.Object.Instantiate<GroupElement>(RadialData.SO.GroupElement);
			EmoteGroupConfig config = UnityEngine.Object.Instantiate<EmoteGroupConfig>(RadialData.SO.EmoteGroupConfig);
			groupElement.Init(config, this, radial);
			elements.Add(groupElement);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000D9A54 File Offset: 0x000D7C54
		private void AddHotbarGroup(RadialBase radial, List<RadialMenuElement> elements)
		{
			GroupElement groupElement = UnityEngine.Object.Instantiate<GroupElement>(RadialData.SO.GroupElement);
			HotbarGroupConfig config = UnityEngine.Object.Instantiate<HotbarGroupConfig>(RadialData.SO.HotbarGroupConfig);
			groupElement.Init(config, this, radial);
			elements.Add(groupElement);
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000D9A94 File Offset: 0x000D7C94
		private void AddItemGroup(RadialBase radial, List<RadialMenuElement> elements, string groupName)
		{
			GroupElement groupElement = UnityEngine.Object.Instantiate<GroupElement>(RadialData.SO.GroupElement);
			ItemGroupConfig itemGroupConfig = UnityEngine.Object.Instantiate<ItemGroupConfig>(RadialData.SO.ItemGroupConfig);
			itemGroupConfig.GroupName = groupName;
			groupElement.Init(itemGroupConfig, this, radial);
			elements.Add(groupElement);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000D9AD8 File Offset: 0x000D7CD8
		private void AddEmpty(List<RadialMenuElement> elements)
		{
			EmptyElement emptyElement = UnityEngine.Object.Instantiate<EmptyElement>(RadialData.SO.EmptyElement);
			emptyElement.Init();
			elements.Add(emptyElement);
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000D9B04 File Offset: 0x000D7D04
		private void AddHammer(List<RadialMenuElement> elements)
		{
			HammerItemElement hammerItemElement = UnityEngine.Object.Instantiate<HammerItemElement>(RadialData.SO.HammerItemElement);
			hammerItemElement.Init();
			elements.Add(hammerItemElement);
		}
	}
}
