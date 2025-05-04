using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000200 RID: 512
	[CreateAssetMenu(fileName = "ThrowGroupConfig", menuName = "Valheim/Radial/Group Config/Throw Group Config")]
	public class ThrowGroupConfig : ScriptableObject, IRadialConfig
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x000D96DB File Offset: 0x000D78DB
		public string LocalizedName
		{
			get
			{
				return Localization.instance.Localize("$inventory_drop");
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x000D96EC File Offset: 0x000D78EC
		public Sprite Sprite
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000D96F0 File Offset: 0x000D78F0
		public void InitRadialConfig(RadialBase radial)
		{
			if (this.m_possibleThrowAmounts.Length == 0)
			{
				Debug.LogError("Possible Throw Amounts needs at least one entry!");
				return;
			}
			if (radial.Selected == null && !radial.IsRefresh)
			{
				Debug.LogError("Selected cannot be null when opening throw radial.");
				return;
			}
			RadialMenuElement selected = radial.Selected;
			ItemElement itemElement = selected as ItemElement;
			ItemDrop.ItemData itemData;
			if (itemElement == null)
			{
				ThrowElement throwElement = selected as ThrowElement;
				if (throwElement == null)
				{
					itemData = null;
				}
				else
				{
					itemData = throwElement.m_data;
				}
			}
			else
			{
				itemData = itemElement.m_data;
			}
			ItemDrop.ItemData itemData2 = itemData;
			if (itemData2 == null)
			{
				Debug.LogError("Throw radial must be opened from an ItemElement or ThrowElement.");
				return;
			}
			Player localPlayer = Player.m_localPlayer;
			if (localPlayer != null && !localPlayer.GetInventory().ContainsItem(itemData2))
			{
				radial.Back();
				return;
			}
			List<RadialMenuElement> list = new List<RadialMenuElement>();
			this.Populate(list, radial, itemData2, radial.MaxElementsPerLayer);
			if (list.Count <= 0)
			{
				radial.Back();
				return;
			}
			bool flag = list.Count + 1 < radial.MaxElementsPerLayer;
			if (radial.Selected is ItemElement)
			{
				this.m_storedOffset = (int)UIMath.Mod((float)radial.BackIndex, 360f);
			}
			radial.StartOffset = (int)UIMath.Mod((float)(flag ? this.m_storedOffset : (this.m_storedOffset - 1)), (float)radial.MaxElementsPerLayer);
			radial.ConstructRadial(list);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000D9830 File Offset: 0x000D7A30
		private void Populate(List<RadialMenuElement> elements, RadialBase radial, ItemDrop.ItemData data, int maxElements)
		{
			for (int i = -1; i <= data.m_stack; i++)
			{
				if (i <= 0)
				{
					ThrowElement throwElement = UnityEngine.Object.Instantiate<ThrowElement>(RadialData.SO.ThrowElement);
					throwElement.Init(data, (i == -1) ? data.m_stack : Mathf.CeilToInt((float)data.m_stack * 0.5f), (i == -1) ? "All" : "Half");
					elements.Add(throwElement);
				}
				else if (data.m_stack - i >= 0)
				{
					ThrowElement throwElement2 = UnityEngine.Object.Instantiate<ThrowElement>(RadialData.SO.ThrowElement);
					throwElement2.Init(data, i);
					elements.Add(throwElement2);
				}
			}
			int num = elements.Count + 1;
			if (num < maxElements)
			{
				int num2 = (num % 2 == 0) ? Mathf.FloorToInt((float)num / 2f) : (num / 2);
				RadialMenuElement item = elements[0];
				elements.Insert(num2 + 1, item);
				elements.RemoveAt(0);
				item = elements[0];
				elements.Insert(num2 + 1, item);
				elements.RemoveAt(0);
			}
		}

		// Token: 0x04001DF3 RID: 7667
		[SerializeField]
		protected float[] m_possibleThrowAmounts = new float[0];

		// Token: 0x04001DF4 RID: 7668
		private int m_storedOffset;
	}
}
