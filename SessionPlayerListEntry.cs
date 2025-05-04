using System;
using Splatform;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserManagement;

namespace Valheim.UI
{
	// Token: 0x0200021D RID: 541
	public class SessionPlayerListEntry : MonoBehaviour
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x000E03A2 File Offset: 0x000DE5A2
		public bool IsSelected
		{
			get
			{
				return this._selection.enabled;
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001E86 RID: 7814 RVA: 0x000E03B0 File Offset: 0x000DE5B0
		// (remove) Token: 0x06001E87 RID: 7815 RVA: 0x000E03E8 File Offset: 0x000DE5E8
		public event Action<SessionPlayerListEntry> OnKicked;

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000E041D File Offset: 0x000DE61D
		public Selectable FocusObject
		{
			get
			{
				return this._focusPoint;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000E0425 File Offset: 0x000DE625
		public Selectable BlockButton
		{
			get
			{
				return this._blockButton;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000E042D File Offset: 0x000DE62D
		public Selectable KickButton
		{
			get
			{
				return this._kickButton;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000E0435 File Offset: 0x000DE635
		public PlatformUserID User
		{
			get
			{
				return this._user;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x000E043D File Offset: 0x000DE63D
		public bool HasFocusObject
		{
			get
			{
				return this._focusPoint.gameObject.activeSelf;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000E044F File Offset: 0x000DE64F
		public bool HasBlock
		{
			get
			{
				return this._blockButtonImage.gameObject.activeSelf;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000E0461 File Offset: 0x000DE661
		public bool HasKick
		{
			get
			{
				return this._kickButtonImage.gameObject.activeSelf;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x000E0473 File Offset: 0x000DE673
		public bool HasActivatedButtons
		{
			get
			{
				return this._blockButtonImage.gameObject.activeSelf || this._kickButtonImage.gameObject.activeSelf;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000E0499 File Offset: 0x000DE699
		public bool IsSamePlatform
		{
			get
			{
				return this._user.m_platform == PlatformManager.DistributionPlatform.Platform;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000E04B5 File Offset: 0x000DE6B5
		// (set) Token: 0x06001E92 RID: 7826 RVA: 0x000E04C7 File Offset: 0x000DE6C7
		public bool IsOwnPlayer
		{
			get
			{
				return this._outline.gameObject.activeSelf;
			}
			set
			{
				this._outline.gameObject.SetActive(value);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x000E04DA File Offset: 0x000DE6DA
		// (set) Token: 0x06001E94 RID: 7828 RVA: 0x000E04EC File Offset: 0x000DE6EC
		public bool IsHost
		{
			get
			{
				return this._hostIcon.gameObject.activeSelf;
			}
			set
			{
				this._hostIcon.gameObject.SetActive(value);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x000E04FF File Offset: 0x000DE6FF
		// (set) Token: 0x06001E96 RID: 7830 RVA: 0x000E0511 File Offset: 0x000DE711
		private bool CanBeKicked
		{
			get
			{
				return this._kickButtonImage.gameObject.activeSelf;
			}
			set
			{
				this._kickButtonImage.gameObject.SetActive(value && !this.IsHost);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x000E0532 File Offset: 0x000DE732
		// (set) Token: 0x06001E98 RID: 7832 RVA: 0x000E0544 File Offset: 0x000DE744
		private bool CanBeBlocked
		{
			get
			{
				return this._blockButtonImage.gameObject.activeSelf;
			}
			set
			{
				this._blockButtonImage.gameObject.SetActive(value);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x000E0557 File Offset: 0x000DE757
		// (set) Token: 0x06001E9A RID: 7834 RVA: 0x000E055A File Offset: 0x000DE75A
		private bool CanBeMuted
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x000E0561 File Offset: 0x000DE761
		// (set) Token: 0x06001E9C RID: 7836 RVA: 0x000E056C File Offset: 0x000DE76C
		public string Gamertag
		{
			get
			{
				return this._gamertag;
			}
			set
			{
				string text = "";
				if (value != null)
				{
					text += value;
				}
				this._gamertag = text;
				if (this.IsHost)
				{
					text += " (Host)";
				}
				this._gamertagText.text = text;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x000E05B1 File Offset: 0x000DE7B1
		// (set) Token: 0x06001E9E RID: 7838 RVA: 0x000E05BC File Offset: 0x000DE7BC
		public string CharacterName
		{
			get
			{
				return this._characterName;
			}
			set
			{
				string text = value;
				if (!this.IsOwnPlayer)
				{
					text = CensorShittyWords.FilterUGC(text, UGCType.CharacterName, this._user, 0L);
				}
				this._characterName = text;
				this._characterNameText.text = text;
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000E05F6 File Offset: 0x000DE7F6
		private void Awake()
		{
			this._selection.enabled = false;
			this._viewPlayerCard.SetActive(false);
			if (this._button != null)
			{
				this._button.enabled = true;
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000E062C File Offset: 0x000DE82C
		private void Update()
		{
			if (EventSystem.current != null && (EventSystem.current.currentSelectedGameObject == this._focusPoint.gameObject || EventSystem.current.currentSelectedGameObject == this._blockButton.gameObject || EventSystem.current.currentSelectedGameObject == this._kickButton.gameObject || EventSystem.current.currentSelectedGameObject == this._button.gameObject))
			{
				this.SelectEntry();
			}
			else
			{
				this.Deselect();
			}
			this.UpdateFocusPoint();
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000E06CA File Offset: 0x000DE8CA
		public void SelectEntry()
		{
			this._selection.enabled = true;
			this._viewPlayerCard.SetActive(this.IsSamePlatform);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000E06E9 File Offset: 0x000DE8E9
		public void Deselect()
		{
			this._selection.enabled = false;
			this._viewPlayerCard.SetActive(false);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000E0704 File Offset: 0x000DE904
		public void OnBlock()
		{
			if (RelationsManager.IsBlocked(this._user))
			{
				this.OnViewCard();
				return;
			}
			if (MuteList.Contains(this._user))
			{
				MuteList.Unblock(this._user);
			}
			else
			{
				MuteList.Block(this._user);
			}
			this.UpdateBlockButton();
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000E0750 File Offset: 0x000DE950
		private void UpdateButtons()
		{
			this.UpdateBlockButton();
			this.UpdateFocusPoint();
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000E075E File Offset: 0x000DE95E
		private void UpdateFocusPoint()
		{
			this._focusPoint.gameObject.SetActive(!this.HasActivatedButtons);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000E0779 File Offset: 0x000DE979
		public void UpdateBlockButton()
		{
			this._blockButtonImage.sprite = ((MuteList.Contains(this._user) || RelationsManager.IsBlocked(this._user)) ? this._unblockSprite : this._blockSprite);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000E07B0 File Offset: 0x000DE9B0
		public void OnKick()
		{
			if (ZNet.instance != null)
			{
				UnifiedPopup.Push(new YesNoPopup("$menu_kick_player_title", Localization.instance.Localize("$menu_kick_player", new string[]
				{
					this.CharacterName
				}), delegate()
				{
					ZNet.instance.Kick(this.CharacterName);
					Action<SessionPlayerListEntry> onKicked = this.OnKicked;
					if (onKicked != null)
					{
						onKicked(this);
					}
					UnifiedPopup.Pop();
				}, delegate()
				{
					UnifiedPopup.Pop();
				}, true));
			}
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000E0824 File Offset: 0x000DEA24
		public void SetValues(string characterName, PlatformUserID user, bool isHost, bool canBeBlocked, bool canBeKicked)
		{
			this._user = user;
			this.IsHost = isHost;
			this.CharacterName = characterName;
			this.Gamertag = "";
			this._gamerpic.sprite = this.otherPlatformPlayerPic;
			if (this.IsSamePlatform && PlatformManager.DistributionPlatform.RelationsProvider != null)
			{
				PlatformManager.DistributionPlatform.RelationsProvider.GetUserProfileAsync(user, new GetUserProfileCompletedHandler(this.GetUserProfileCompleted), new GetUserProfileFailedHandler(this.GetUserProfileFailed));
			}
			else
			{
				this.UpdateProfile();
			}
			this.CanBeKicked = (!isHost && canBeKicked);
			this.CanBeBlocked = canBeBlocked;
			this.UpdateButtons();
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000E08C4 File Offset: 0x000DEAC4
		private void GetUserProfileCompleted(IUserProfile profile)
		{
			if (this == null)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			this._userProfile = profile;
			this._userProfile.RequestProfilePictureAsync(SessionPlayerListEntry.GetProfilePictureResolution());
			this.UpdateProfile();
			this._userProfile.ProfileDataUpdated += this.UpdateProfile;
			this.UpdateProfilePicture();
			this._userProfile.ProfilePictureUpdated += this.UpdateProfilePicture;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000E0934 File Offset: 0x000DEB34
		private static uint GetProfilePictureResolution()
		{
			if (PlatformManager.DistributionPlatform.HardwareInfoProvider == null)
			{
				return 128U;
			}
			HardwareInfo hardwareInfo = PlatformManager.DistributionPlatform.HardwareInfoProvider.HardwareInfo;
			if (hardwareInfo.m_category == HardwareCategory.Unknown)
			{
				return 128U;
			}
			if (hardwareInfo.m_category < HardwareCategory.Console)
			{
				return 50U;
			}
			if (hardwareInfo.m_category == HardwareCategory.Console && hardwareInfo.m_generation <= 8U)
			{
				return 50U;
			}
			return 128U;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000E0997 File Offset: 0x000DEB97
		private void GetUserProfileFailed(GetUserProfileFailReason failReason)
		{
			switch (failReason)
			{
			default:
				Debug.LogError(string.Format("Failed to get user profile: {0}", failReason));
				break;
			case GetUserProfileFailReason.DifferentPlatformsNotAvailable:
			case GetUserProfileFailReason.SamePlatformNotAvailable:
				break;
			}
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000E09C4 File Offset: 0x000DEBC4
		private void UpdateProfile()
		{
			string gamertag;
			if (this.IsSamePlatform)
			{
				this.Gamertag = this._userProfile.DisplayName;
			}
			else if (ZNet.TryGetServerAssignedDisplayName(this._user, out gamertag))
			{
				this.Gamertag = gamertag;
			}
			this.UpdateButtons();
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000E0A08 File Offset: 0x000DEC08
		private void UpdateProfilePicture()
		{
			if (this.IsSamePlatform && this._userProfile.ProfilePicture != null)
			{
				this._gamerpic.SetSpriteFromTexture(this._userProfile.ProfilePicture);
				return;
			}
			this._gamerpic.sprite = this.otherPlatformPlayerPic;
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000E0A58 File Offset: 0x000DEC58
		public void OnViewCard()
		{
			if (PlatformManager.DistributionPlatform.UIProvider.ShowUserProfile == null)
			{
				return;
			}
			if (!this.IsSamePlatform)
			{
				return;
			}
			PlatformManager.DistributionPlatform.UIProvider.ShowUserProfile.Open(this._user);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000E0A8F File Offset: 0x000DEC8F
		public void RemoveCallbacks()
		{
			if (this._userProfile == null)
			{
				return;
			}
			this._userProfile.ProfileDataUpdated -= this.UpdateProfile;
			this._userProfile.ProfilePictureUpdated -= this.UpdateProfilePicture;
		}

		// Token: 0x04001EF3 RID: 7923
		[SerializeField]
		private Button _button;

		// Token: 0x04001EF4 RID: 7924
		[SerializeField]
		private Selectable _focusPoint;

		// Token: 0x04001EF5 RID: 7925
		[SerializeField]
		private Image _selection;

		// Token: 0x04001EF6 RID: 7926
		[SerializeField]
		private GameObject _viewPlayerCard;

		// Token: 0x04001EF7 RID: 7927
		[SerializeField]
		private Image _outline;

		// Token: 0x04001EF8 RID: 7928
		[Header("Player")]
		[SerializeField]
		private Image _hostIcon;

		// Token: 0x04001EF9 RID: 7929
		[SerializeField]
		private Image _gamerpic;

		// Token: 0x04001EFA RID: 7930
		[SerializeField]
		private Sprite otherPlatformPlayerPic;

		// Token: 0x04001EFB RID: 7931
		[SerializeField]
		private TextMeshProUGUI _gamertagText;

		// Token: 0x04001EFC RID: 7932
		[SerializeField]
		private TextMeshProUGUI _characterNameText;

		// Token: 0x04001EFD RID: 7933
		[Header("Block")]
		[SerializeField]
		private Button _blockButton;

		// Token: 0x04001EFE RID: 7934
		[SerializeField]
		private Image _blockButtonImage;

		// Token: 0x04001EFF RID: 7935
		[SerializeField]
		private Sprite _blockSprite;

		// Token: 0x04001F00 RID: 7936
		[SerializeField]
		private Sprite _unblockSprite;

		// Token: 0x04001F01 RID: 7937
		[Header("Kick")]
		[SerializeField]
		private Button _kickButton;

		// Token: 0x04001F02 RID: 7938
		[SerializeField]
		private Image _kickButtonImage;

		// Token: 0x04001F04 RID: 7940
		private PlatformUserID _user;

		// Token: 0x04001F05 RID: 7941
		private IUserProfile _userProfile;

		// Token: 0x04001F06 RID: 7942
		private string _gamertag;

		// Token: 0x04001F07 RID: 7943
		private string _characterName;
	}
}
