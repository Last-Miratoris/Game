using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001FC RID: 508
	[CreateAssetMenu(fileName = "HotbarGroupConfig", menuName = "Valheim/Radial/Group Config/Hotbar Group Config")]
	public class HotbarGroupConfig : ScriptableObject, IRadialConfig
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000D8F12 File Offset: 0x000D7112
		public string LocalizedName
		{
			get
			{
				return Localization.instance.Localize("$radial_hotbar");
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000D8F23 File Offset: 0x000D7123
		public Sprite Sprite
		{
			get
			{
				return this.m_icon;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000D8F2C File Offset: 0x000D712C
		public void InitRadialConfig(RadialBase radial)
		{
			List<RadialMenuElement> list = new List<RadialMenuElement>();
			foreach (ItemDrop.ItemData itemData in Player.m_localPlayer.GetInventory().GetHotbar(true))
			{
				RadialMenuElement item;
				if (itemData == null)
				{
					EmptyElement emptyElement = UnityEngine.Object.Instantiate<EmptyElement>(RadialData.SO.EmptyElement);
					emptyElement.Init();
					item = emptyElement;
				}
				else
				{
					ItemElement itemElement = UnityEngine.Object.Instantiate<ItemElement>(RadialData.SO.ItemElement);
					itemElement.Init(itemData);
					itemElement.CloseOnInteract = (() => true);
					itemElement.AdvancedCloseOnInteract = null;
					item = itemElement;
				}
				list.Add(item);
			}
			radial.ConstructRadial(list);
		}

		// Token: 0x04001DEE RID: 7662
		[SerializeField]
		protected Sprite m_icon;
	}
}
