using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001FB RID: 507
	[CreateAssetMenu(fileName = "EmoteGroupConfig", menuName = "Valheim/Radial/Group Config/Emote Group Config")]
	public class EmoteGroupConfig : ScriptableObject, IRadialConfig
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000D8E98 File Offset: 0x000D7098
		public string LocalizedName
		{
			get
			{
				return Localization.instance.Localize("$radial_emotes");
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000D8EA9 File Offset: 0x000D70A9
		public Sprite Sprite
		{
			get
			{
				return this.m_icon;
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000D8EB4 File Offset: 0x000D70B4
		public void InitRadialConfig(RadialBase radial)
		{
			List<RadialMenuElement> list = new List<RadialMenuElement>();
			for (int i = 0; i < 23; i++)
			{
				EmoteElement emoteElement = UnityEngine.Object.Instantiate<EmoteElement>(RadialData.SO.EmoteElement);
				EmoteDataMapping mapping = RadialData.SO.EmoteMappings.GetMapping((Emotes)i);
				emoteElement.Init(mapping);
				list.Add(emoteElement);
			}
			radial.ConstructRadial(list);
		}

		// Token: 0x04001DED RID: 7661
		[SerializeField]
		protected Sprite m_icon;
	}
}
