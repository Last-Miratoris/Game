using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x0200020C RID: 524
	[CreateAssetMenu(fileName = "EmoteMappings", menuName = "Valheim/Radial/Mappings/Emote Mappings")]
	public class EmoteMappings : ScriptableObject
	{
		// Token: 0x06001D76 RID: 7542 RVA: 0x000DAF4C File Offset: 0x000D914C
		public EmoteDataMapping GetMapping(Emotes emote)
		{
			if (this._emotes != null)
			{
				for (int i = 0; i < this._emotes.Length; i++)
				{
					if (this._emotes[i].Emote == emote)
					{
						return this._emotes[i];
					}
				}
			}
			return new EmoteDataMapping
			{
				Emote = Emotes.Count
			};
		}

		// Token: 0x04001E17 RID: 7703
		[SerializeField]
		protected EmoteDataMapping[] _emotes;
	}
}
