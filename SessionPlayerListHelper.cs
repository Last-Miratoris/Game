using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Valheim.UI
{
	// Token: 0x0200021E RID: 542
	public static class SessionPlayerListHelper
	{
		// Token: 0x06001EB2 RID: 7858 RVA: 0x000E0AF9 File Offset: 0x000DECF9
		public static IEnumerator SetSpriteFromUri(this Image image, string uri)
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
			yield return www.SendWebRequest();
			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				Texture2D content = DownloadHandlerTexture.GetContent(www);
				image.sprite = Sprite.Create(content, new Rect(0f, 0f, (float)content.width, (float)content.height), new Vector2(0.5f, 0.5f));
				image.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			yield break;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000E0B10 File Offset: 0x000DED10
		public static void SetSpriteFromTexture(this Image image, Texture2D texture)
		{
			image.sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f));
			image.transform.localScale = new Vector3(1f, 1f, 1f);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000E0B74 File Offset: 0x000DED74
		public static void SetSpriteFromSteamImageId(this Image image, int imageId)
		{
			if (imageId <= 0)
			{
				image.SetTransparent();
				return;
			}
			uint num;
			uint num2;
			if (SteamUtils.GetImageSize(imageId, out num, out num2))
			{
				uint num3 = num * num2 * 4U;
				byte[] array = new byte[num3];
				Texture2D texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false, true);
				if (SteamUtils.GetImageRGBA(imageId, array, (int)num3))
				{
					texture2D.LoadRawTextureData(array);
					texture2D.FlipInYDirection();
					image.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
				}
			}
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000E0C08 File Offset: 0x000DEE08
		private static void SetTransparent(this Image image)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			texture2D.SetPixels(new Color[]
			{
				new Color(0f, 0f, 0f, 0f)
			});
			image.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x000E0C84 File Offset: 0x000DEE84
		private static void FlipInYDirection(this Texture2D texture)
		{
			Color[] pixels = texture.GetPixels();
			Color[] array = new Color[pixels.Length];
			int num = 0;
			for (int i = texture.height - 1; i >= 0; i--)
			{
				for (int j = 0; j < texture.width; j++)
				{
					array[num] = pixels[i * texture.height + j];
					num++;
				}
			}
			texture.SetPixels(array);
			texture.Apply();
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x000E0CF4 File Offset: 0x000DEEF4
		public static bool TryFindPlayerByZDOID(this List<ZNet.PlayerInfo> players, ZDOID playerID, out ZNet.PlayerInfo? playerInfo)
		{
			playerInfo = null;
			for (int i = 0; i < players.Count; i++)
			{
				ZNet.PlayerInfo playerInfo2 = players[i];
				if (playerInfo2.m_characterID == playerID)
				{
					playerInfo = new ZNet.PlayerInfo?(playerInfo2);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000E0D40 File Offset: 0x000DEF40
		public static bool TryFindPlayerByPlayername(this List<ZNet.PlayerInfo> players, string name, out ZNet.PlayerInfo? playerInfo)
		{
			playerInfo = null;
			for (int i = 0; i < players.Count; i++)
			{
				ZNet.PlayerInfo playerInfo2 = players[i];
				if (playerInfo2.m_name == name)
				{
					playerInfo = new ZNet.PlayerInfo?(playerInfo2);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000E0D8A File Offset: 0x000DEF8A
		public static bool IsBanned(string characterName)
		{
			return ZNet.instance.Banned.Contains(characterName);
		}
	}
}
