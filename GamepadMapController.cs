using System;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001E9 RID: 489
	public class GamepadMapController : MonoBehaviour
	{
		// Token: 0x06001C4F RID: 7247 RVA: 0x000D46AC File Offset: 0x000D28AC
		private void Start()
		{
			Localization.OnLanguageChange = (Action)Delegate.Combine(Localization.OnLanguageChange, new Action(this.OnLanguageChange));
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000D46CE File Offset: 0x000D28CE
		private void OnDestroy()
		{
			Localization.OnLanguageChange = (Action)Delegate.Remove(Localization.OnLanguageChange, new Action(this.OnLanguageChange));
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x000D46F0 File Offset: 0x000D28F0
		public InputLayout VisibleLayout
		{
			get
			{
				return this.visibleLayout;
			}
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000D46F8 File Offset: 0x000D28F8
		public void Show(InputLayout layout, GamepadMapType type = GamepadMapType.Default)
		{
			this.visibleType = type;
			this.visibleLayout = layout;
			switch (type)
			{
			case GamepadMapType.PS:
				if (this.psMapInstance == null)
				{
					this.psMapInstance = UnityEngine.Object.Instantiate<GamepadMap>(this.psMapPrefab, this.root);
					goto IL_C3;
				}
				goto IL_C3;
			case GamepadMapType.SteamXbox:
				if (this.steamDeckXboxMapInstance == null)
				{
					this.steamDeckXboxMapInstance = UnityEngine.Object.Instantiate<GamepadMap>(this.steamDeckXboxMapPrefab, this.root);
					goto IL_C3;
				}
				goto IL_C3;
			case GamepadMapType.SteamPS:
				if (this.steamDeckPSMapInstance == null)
				{
					this.steamDeckPSMapInstance = UnityEngine.Object.Instantiate<GamepadMap>(this.steamDeckPSMapPrefab, this.root);
					goto IL_C3;
				}
				goto IL_C3;
			}
			if (this.xboxMapInstance == null)
			{
				this.xboxMapInstance = UnityEngine.Object.Instantiate<GamepadMap>(this.xboxMapPrefab, this.root);
			}
			IL_C3:
			this.UpdateGamepadMap();
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000D47CE File Offset: 0x000D29CE
		private void OnLanguageChange()
		{
			this.UpdateGamepadMap();
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000D47D8 File Offset: 0x000D29D8
		private void UpdateGamepadMap()
		{
			if (this.psMapInstance != null)
			{
				this.psMapInstance.gameObject.SetActive(this.visibleType == GamepadMapType.PS);
				if (this.visibleType == GamepadMapType.PS)
				{
					this.psMapInstance.UpdateMap(this.visibleLayout);
				}
			}
			if (this.steamDeckXboxMapInstance != null)
			{
				this.steamDeckXboxMapInstance.gameObject.SetActive(this.visibleType == GamepadMapType.SteamXbox);
				if (this.visibleType == GamepadMapType.SteamXbox)
				{
					this.steamDeckXboxMapInstance.UpdateMap(this.visibleLayout);
				}
			}
			if (this.steamDeckPSMapInstance != null)
			{
				this.steamDeckPSMapInstance.gameObject.SetActive(this.visibleType == GamepadMapType.SteamPS);
				if (this.visibleType == GamepadMapType.SteamPS)
				{
					this.steamDeckPSMapInstance.UpdateMap(this.visibleLayout);
				}
			}
			if (this.xboxMapInstance != null)
			{
				this.xboxMapInstance.gameObject.SetActive(this.visibleType == GamepadMapType.Default);
				if (this.visibleType == GamepadMapType.Default)
				{
					this.xboxMapInstance.UpdateMap(this.visibleLayout);
				}
			}
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000D48E8 File Offset: 0x000D2AE8
		public static string GetLayoutStringId(InputLayout layout)
		{
			switch (layout)
			{
			case InputLayout.Default:
				return "$settings_controller_classic";
			case InputLayout.Alternative2:
				return "$settings_controller_default 2";
			}
			return "$settings_controller_default";
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000D490F File Offset: 0x000D2B0F
		public static InputLayout NextLayout(InputLayout mode)
		{
			if (mode + 1 < InputLayout.Count)
			{
				return mode + 1;
			}
			return InputLayout.Default;
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x000D491C File Offset: 0x000D2B1C
		public static InputLayout PrevLayout(InputLayout mode)
		{
			if (mode - InputLayout.Alternative1 >= 0)
			{
				return mode - 1;
			}
			return InputLayout.Alternative2;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x000D492C File Offset: 0x000D2B2C
		public static GamepadMapType GetType(GamepadGlyphs currentGlyphs = GamepadGlyphs.Auto, bool steamDeck = false)
		{
			if (currentGlyphs == GamepadGlyphs.Auto)
			{
				currentGlyphs = ZInput.ConnectedGamepadTypeGlyphs();
			}
			GamepadGlyphs gamepadGlyphs = currentGlyphs;
			GamepadMapType result;
			if (gamepadGlyphs != GamepadGlyphs.Xbox)
			{
				if (gamepadGlyphs != GamepadGlyphs.Playstation)
				{
					result = GamepadMapType.Default;
				}
				else if (steamDeck)
				{
					result = GamepadMapType.SteamPS;
				}
				else
				{
					result = GamepadMapType.PS;
				}
			}
			else if (steamDeck)
			{
				result = GamepadMapType.SteamXbox;
			}
			else
			{
				result = GamepadMapType.Default;
			}
			return result;
		}

		// Token: 0x04001D3F RID: 7487
		[SerializeField]
		private GamepadMap xboxMapPrefab;

		// Token: 0x04001D40 RID: 7488
		[SerializeField]
		private GamepadMap psMapPrefab;

		// Token: 0x04001D41 RID: 7489
		[SerializeField]
		private GamepadMap steamDeckXboxMapPrefab;

		// Token: 0x04001D42 RID: 7490
		[SerializeField]
		private GamepadMap steamDeckPSMapPrefab;

		// Token: 0x04001D43 RID: 7491
		[SerializeField]
		private RectTransform root;

		// Token: 0x04001D44 RID: 7492
		private GamepadMap xboxMapInstance;

		// Token: 0x04001D45 RID: 7493
		private GamepadMap psMapInstance;

		// Token: 0x04001D46 RID: 7494
		private GamepadMap steamDeckXboxMapInstance;

		// Token: 0x04001D47 RID: 7495
		private GamepadMap steamDeckPSMapInstance;

		// Token: 0x04001D48 RID: 7496
		private GamepadMapType visibleType;

		// Token: 0x04001D49 RID: 7497
		private InputLayout visibleLayout;
	}
}
