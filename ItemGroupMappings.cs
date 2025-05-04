using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x0200020E RID: 526
	[CreateAssetMenu(fileName = "ItemGroupMappings", menuName = "Valheim/Radial/Mappings/Item Group Mappings")]
	public class ItemGroupMappings : ScriptableObject
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x000DAFBB File Offset: 0x000D91BB
		public ItemGroupMapping[] Groups
		{
			get
			{
				return this._itemGroups;
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000DAFC4 File Offset: 0x000D91C4
		public ItemGroupMapping GetMapping(string group)
		{
			if (this._itemGroups != null)
			{
				for (int i = 0; i < this._itemGroups.Length; i++)
				{
					if (this._itemGroups[i].Name == group)
					{
						return this._itemGroups[i];
					}
				}
			}
			return new ItemGroupMapping
			{
				Name = ItemGroupMapping.None,
				ItemTypes = new ItemDrop.ItemData.ItemType[1]
			};
		}

		// Token: 0x04001E1D RID: 7709
		[SerializeField]
		protected ItemGroupMapping[] _itemGroups;
	}
}
