using System;
using System.Collections.Generic;
using Splatform;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using UserManagement;

namespace Valheim.UI
{
	// Token: 0x0200021C RID: 540
	public class SessionPlayerList : MonoBehaviour
	{
		// Token: 0x06001E79 RID: 7801 RVA: 0x000DFAB5 File Offset: 0x000DDCB5
		protected void Awake()
		{
			MuteList.Load(new Action(this.Init));
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000DFAC8 File Offset: 0x000DDCC8
		private void Init()
		{
			this.SetEntries();
			foreach (SessionPlayerListEntry sessionPlayerListEntry in this._allPlayers)
			{
				sessionPlayerListEntry.OnKicked += this.OnPlayerWasKicked;
			}
			this._ownPlayer.FocusObject.Select();
			this.UpdateBlockButtons();
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000DFB40 File Offset: 0x000DDD40
		private void UpdateBlockButtons()
		{
			if (this == null)
			{
				return;
			}
			foreach (SessionPlayerListEntry sessionPlayerListEntry in this._allPlayers)
			{
				sessionPlayerListEntry.UpdateBlockButton();
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000DFB9C File Offset: 0x000DDD9C
		private void OnPlayerWasKicked(SessionPlayerListEntry player)
		{
			player.OnKicked -= this.OnPlayerWasKicked;
			this._allPlayers.Remove(player);
			this._remotePlayers.Remove(player);
			UnityEngine.Object.Destroy(player.gameObject);
			this.UpdateNavigation();
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000DFBDC File Offset: 0x000DDDDC
		private void SetEntries()
		{
			this._allPlayers.Add(this._ownPlayer);
			PlatformUserID platformUserID = PlatformManager.DistributionPlatform.LocalUser.PlatformUserID;
			this._players = ZNet.instance.GetPlayerList();
			ZNetPeer serverPeer = ZNet.instance.GetServerPeer();
			ZNet.PlayerInfo? playerInfo;
			if (!ZNet.instance.IsServer() && this._players.TryFindPlayerByPlayername(serverPeer.m_playerName, out playerInfo))
			{
				if (ZNet.m_onlineBackend == OnlineBackendType.Steamworks)
				{
					PlatformUserID user = new PlatformUserID(PlatformManager.DistributionPlatform.Platform, serverPeer.m_socket.GetEndPointString());
					this.CreatePlayerEntry(user, serverPeer.m_playerName, true);
				}
				else
				{
					this.CreatePlayerEntry(playerInfo.Value.m_userInfo.m_id, playerInfo.Value.m_name, true);
				}
			}
			for (int i = 0; i < this._players.Count; i++)
			{
				ZNet.PlayerInfo playerInfo2 = this._players[i];
				if (playerInfo2.m_userInfo.m_id == platformUserID)
				{
					PlatformUserID user2 = new PlatformUserID(PlatformManager.DistributionPlatform.Platform, (ulong)SteamUser.GetSteamID(), true);
					this.SetOwnPlayer(user2, ZNet.instance.IsServer());
				}
				else if (serverPeer == null || playerInfo2.m_name != serverPeer.m_playerName)
				{
					this.CreatePlayerEntry(playerInfo2.m_userInfo.m_id, playerInfo2.m_name, false);
				}
			}
			this.UpdateNavigation();
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000DFD48 File Offset: 0x000DDF48
		private void UpdateNavigation()
		{
			Navigation navigation = new Navigation
			{
				mode = Navigation.Mode.Explicit
			};
			int count = this._allPlayers.Count;
			for (int i = 0; i < count; i++)
			{
				SessionPlayerListEntry sessionPlayerListEntry = this._allPlayers[i];
				SessionPlayerListEntry sessionPlayerListEntry2 = (i < count - 1) ? this._allPlayers[i + 1] : null;
				Navigation navigation2 = sessionPlayerListEntry.BlockButton.navigation;
				navigation2.mode = (sessionPlayerListEntry.HasBlock ? Navigation.Mode.Explicit : Navigation.Mode.None);
				Navigation navigation3 = sessionPlayerListEntry.KickButton.navigation;
				navigation3.mode = (sessionPlayerListEntry.HasKick ? Navigation.Mode.Explicit : Navigation.Mode.None);
				Navigation navigation4 = sessionPlayerListEntry.FocusObject.navigation;
				navigation4.mode = (sessionPlayerListEntry.HasFocusObject ? Navigation.Mode.Explicit : Navigation.Mode.None);
				if (sessionPlayerListEntry2 != null)
				{
					if (!sessionPlayerListEntry.HasActivatedButtons && !sessionPlayerListEntry2.HasActivatedButtons)
					{
						navigation4.selectOnDown = sessionPlayerListEntry2.FocusObject;
					}
					else if (!sessionPlayerListEntry.HasActivatedButtons && sessionPlayerListEntry2.HasActivatedButtons)
					{
						if (sessionPlayerListEntry2.HasBlock)
						{
							navigation4.selectOnDown = sessionPlayerListEntry2.BlockButton;
						}
						else if (sessionPlayerListEntry2.HasKick)
						{
							navigation4.selectOnDown = sessionPlayerListEntry2.KickButton;
						}
					}
					else if (sessionPlayerListEntry.HasActivatedButtons && !sessionPlayerListEntry2.HasActivatedButtons)
					{
						if (sessionPlayerListEntry.HasBlock)
						{
							navigation2.selectOnDown = sessionPlayerListEntry2.FocusObject;
						}
						if (sessionPlayerListEntry.HasKick)
						{
							navigation3.selectOnDown = sessionPlayerListEntry2.FocusObject;
						}
					}
					else
					{
						if (sessionPlayerListEntry.HasBlock)
						{
							if (sessionPlayerListEntry2.HasBlock)
							{
								navigation2.selectOnDown = sessionPlayerListEntry2.BlockButton;
							}
							else if (sessionPlayerListEntry2.HasKick)
							{
								navigation2.selectOnDown = sessionPlayerListEntry2.KickButton;
							}
						}
						if (sessionPlayerListEntry.HasKick)
						{
							if (sessionPlayerListEntry2.HasKick)
							{
								navigation3.selectOnDown = sessionPlayerListEntry2.KickButton;
							}
							else if (sessionPlayerListEntry2.HasBlock)
							{
								navigation3.selectOnDown = sessionPlayerListEntry2.BlockButton;
							}
						}
					}
				}
				else if (sessionPlayerListEntry.HasActivatedButtons)
				{
					if (sessionPlayerListEntry.HasBlock)
					{
						navigation.selectOnUp = sessionPlayerListEntry.BlockButton;
					}
					else if (sessionPlayerListEntry.HasKick)
					{
						navigation.selectOnUp = sessionPlayerListEntry.KickButton;
					}
					if (sessionPlayerListEntry.HasBlock)
					{
						navigation2.selectOnDown = this._backButton;
					}
					if (sessionPlayerListEntry.HasKick)
					{
						navigation3.selectOnDown = this._backButton;
					}
				}
				else
				{
					navigation4.selectOnDown = this._backButton;
					navigation.selectOnUp = sessionPlayerListEntry.FocusObject;
				}
				sessionPlayerListEntry.BlockButton.navigation = navigation2;
				sessionPlayerListEntry.KickButton.navigation = navigation3;
				sessionPlayerListEntry.FocusObject.navigation = navigation4;
			}
			for (int j = count - 1; j >= 0; j--)
			{
				SessionPlayerListEntry sessionPlayerListEntry3 = this._allPlayers[j];
				SessionPlayerListEntry sessionPlayerListEntry4 = (j > 0) ? this._allPlayers[j - 1] : null;
				Navigation navigation5 = sessionPlayerListEntry3.BlockButton.navigation;
				Navigation navigation6 = sessionPlayerListEntry3.KickButton.navigation;
				Navigation navigation7 = sessionPlayerListEntry3.FocusObject.navigation;
				if (sessionPlayerListEntry4 != null)
				{
					if (!sessionPlayerListEntry3.HasActivatedButtons && !sessionPlayerListEntry4.HasActivatedButtons)
					{
						navigation7.selectOnUp = sessionPlayerListEntry4.FocusObject;
					}
					else if (!sessionPlayerListEntry3.HasActivatedButtons && sessionPlayerListEntry4.HasActivatedButtons)
					{
						if (sessionPlayerListEntry4.HasBlock)
						{
							navigation7.selectOnUp = sessionPlayerListEntry4.BlockButton;
						}
						else if (sessionPlayerListEntry4.HasKick)
						{
							navigation7.selectOnUp = sessionPlayerListEntry4.KickButton;
						}
					}
					else if (sessionPlayerListEntry3.HasActivatedButtons && !sessionPlayerListEntry4.HasActivatedButtons)
					{
						if (sessionPlayerListEntry3.HasBlock)
						{
							navigation5.selectOnUp = sessionPlayerListEntry4.FocusObject;
						}
						if (sessionPlayerListEntry3.HasKick)
						{
							navigation6.selectOnUp = sessionPlayerListEntry4.FocusObject;
						}
					}
					else
					{
						if (sessionPlayerListEntry3.HasBlock)
						{
							if (sessionPlayerListEntry4.HasBlock)
							{
								navigation5.selectOnUp = sessionPlayerListEntry4.BlockButton;
							}
							else if (sessionPlayerListEntry4.HasKick)
							{
								navigation5.selectOnUp = sessionPlayerListEntry4.KickButton;
							}
						}
						if (sessionPlayerListEntry3.HasKick)
						{
							if (sessionPlayerListEntry4.HasKick)
							{
								navigation6.selectOnUp = sessionPlayerListEntry4.KickButton;
							}
							else if (sessionPlayerListEntry4.HasBlock)
							{
								navigation6.selectOnUp = sessionPlayerListEntry4.BlockButton;
							}
						}
					}
				}
				sessionPlayerListEntry3.BlockButton.navigation = navigation5;
				sessionPlayerListEntry3.KickButton.navigation = navigation6;
				sessionPlayerListEntry3.FocusObject.navigation = navigation7;
			}
			this._backButton.navigation = navigation;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000E01DC File Offset: 0x000DE3DC
		private void SetOwnPlayer(PlatformUserID user, bool isHost)
		{
			UserInfo localUser = UserInfo.GetLocalUser();
			this._ownPlayer.IsOwnPlayer = true;
			this._ownPlayer.SetValues(localUser.Name, user, isHost, false, false);
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000E0210 File Offset: 0x000DE410
		private void CreatePlayerEntry(PlatformUserID user, string name, bool isHost = false)
		{
			SessionPlayerListEntry sessionPlayerListEntry = UnityEngine.Object.Instantiate<SessionPlayerListEntry>(this._ownPlayer, this._scrollRect.content);
			sessionPlayerListEntry.IsOwnPlayer = false;
			sessionPlayerListEntry.SetValues(name, user, isHost, true, !isHost && ZNet.instance.LocalPlayerIsAdminOrHost());
			if (!isHost)
			{
				this._remotePlayers.Add(sessionPlayerListEntry);
			}
			this._allPlayers.Add(sessionPlayerListEntry);
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000E0270 File Offset: 0x000DE470
		public void OnBack()
		{
			foreach (SessionPlayerListEntry sessionPlayerListEntry in this._allPlayers)
			{
				sessionPlayerListEntry.RemoveCallbacks();
			}
			MuteList.Persist();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000E02D0 File Offset: 0x000DE4D0
		private void Update()
		{
			this.UpdateScrollPosition();
			if (ZInput.GetKeyDown(KeyCode.Escape, true))
			{
				this.OnBack();
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000E02E8 File Offset: 0x000DE4E8
		private void UpdateScrollPosition()
		{
			if (this._scrollRect.verticalScrollbar.gameObject.activeSelf)
			{
				foreach (SessionPlayerListEntry sessionPlayerListEntry in this._allPlayers)
				{
					if (sessionPlayerListEntry.IsSelected && !this._scrollRect.IsVisible(sessionPlayerListEntry.transform as RectTransform))
					{
						this._scrollRect.SnapToChild(sessionPlayerListEntry.transform as RectTransform);
						break;
					}
				}
			}
		}

		// Token: 0x04001EED RID: 7917
		[SerializeField]
		protected SessionPlayerListEntry _ownPlayer;

		// Token: 0x04001EEE RID: 7918
		[SerializeField]
		protected ScrollRect _scrollRect;

		// Token: 0x04001EEF RID: 7919
		[SerializeField]
		protected Button _backButton;

		// Token: 0x04001EF0 RID: 7920
		private List<ZNet.PlayerInfo> _players;

		// Token: 0x04001EF1 RID: 7921
		private readonly List<SessionPlayerListEntry> _remotePlayers = new List<SessionPlayerListEntry>();

		// Token: 0x04001EF2 RID: 7922
		private readonly List<SessionPlayerListEntry> _allPlayers = new List<SessionPlayerListEntry>();
	}
}
