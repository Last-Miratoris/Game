using System;

namespace Valheim.UI
{
	// Token: 0x02000204 RID: 516
	public class EmoteElement : RadialMenuElement
	{
		// Token: 0x06001D4D RID: 7501 RVA: 0x000DA2AC File Offset: 0x000D84AC
		public void Init(EmoteDataMapping mapping)
		{
			if (mapping.Emote == Emotes.Count)
			{
				base.Name = "";
				base.Interact = null;
			}
			else
			{
				base.Name = ((!string.IsNullOrEmpty(mapping.LocaString)) ? Localization.instance.Localize("$" + mapping.LocaString) : mapping.Emote.ToString());
				base.Interact = delegate()
				{
					Emote.DoEmote(mapping.Emote);
					return true;
				};
			}
			base.CloseOnInteract = (() => true);
			this.m_icon.gameObject.SetActive(mapping.Sprite != null);
			this.m_icon.sprite = mapping.Sprite;
		}
	}
}
