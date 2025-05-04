using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001FF RID: 511
	public static class RadialConfigHelper
	{
		// Token: 0x06001D22 RID: 7458 RVA: 0x000D94DC File Offset: 0x000D76DC
		public static void SetXYControls(this RadialBase radial)
		{
			radial.GetControllerDirection = (() => ZInput.GetValue<Vector2>("RadialStick"));
			radial.GetMouseDirection = (() => RadialConfigHelper.GetMouseDirection(radial));
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000D9538 File Offset: 0x000D7738
		private static Vector2 GetMouseDirection(RadialBase radial)
		{
			Vector2 a = ZInput.mousePosition;
			Vector2 infoPosition = radial.InfoPosition;
			return (a - infoPosition).normalized;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000D9564 File Offset: 0x000D7764
		public static void SetItemInteractionControls(this RadialBase radial)
		{
			radial.GetConfirm = (() => (ZInput.GetButtonLastPressedTimer("JoyRadialInteract") < 0.33f && ZInput.GetButtonUp("JoyRadialInteract")) || ZInput.GetMouseButtonUp(0));
			radial.GetSingleUse = (() => (RadialData.SO.EnableSingleUseMode && ZInput.GetButtonLastPressedTimer("JoyRadial") > RadialData.SO.HoldCloseDelay && ZInput.GetButtonUp("JoyRadial")) || (RadialData.SO.EnableSingleUseMode && ZInput.GetButtonLastPressedTimer("OpenRadial") > RadialData.SO.HoldCloseDelay && ZInput.GetButtonUp("OpenRadial")) || (RadialData.SO.EnableSingleUseMode && ZInput.GetButtonLastPressedTimer("OpenEmote") > RadialData.SO.HoldCloseDelay && ZInput.GetButtonUp("OpenEmote")));
			radial.GetBack = (() => ZInput.GetButtonUp("JoyRadialBack") || (ZInput.GetButtonUp("JoyRadialClose") && !radial.IsTopLevel) || (!ZInput.GetKey(KeyCode.LeftShift, true) && ZInput.GetMouseButtonUp(1)));
			radial.GetThrow = (() => (ZInput.GetButtonLastPressedTimer("JoyRadialSecondaryInteract") < 0.33f && ZInput.GetButtonUp("JoyRadialSecondaryInteract")) || (ZInput.GetKey(KeyCode.LeftShift, true) && ZInput.GetButtonLastPressedTimer("RadialSecondaryInteract") < 0.33f && ZInput.GetButtonUp("RadialSecondaryInteract")));
			radial.GetOpenThrowMenu = (() => (ZInput.GetButtonPressedTimer("JoyRadialSecondaryInteract") > 0.33f && ZInput.GetButton("JoyRadialSecondaryInteract")) || (ZInput.GetKey(KeyCode.LeftShift, true) && ZInput.GetButtonPressedTimer("RadialSecondaryInteract") > 0.33f && ZInput.GetButton("RadialSecondaryInteract")));
			radial.GetClose = (() => (ZInput.GetButtonUp("JoyRadialClose") && radial.IsTopLevel) || ZInput.GetButtonDown("JoyRadial") || ZInput.GetKeyDown(KeyCode.Escape, true) || ZInput.GetKeyDown(KeyCode.BackQuote, true) || ZInput.GetButtonDown("JoyMenu") || ZInput.GetButtonDown("JoyMap") || ZInput.GetButtonDown("Map") || ZInput.GetButtonDown("JoyChat") || ZInput.GetButtonDown("Chat") || ZInput.GetButtonDown("Console") || ZInput.GetButtonDown("OpenRadial") || ZInput.GetButtonDown("OpenEmote") || (!RadialData.SO.EnableSingleUseMode && ZInput.GetButtonLastPressedTimer("JoyRadial") > RadialData.SO.HoldCloseDelay && ZInput.GetButtonUp("JoyRadial")) || (!RadialData.SO.EnableSingleUseMode && ZInput.GetButtonLastPressedTimer("OpenRadial") > RadialData.SO.HoldCloseDelay && ZInput.GetButtonUp("OpenRadial")) || (!RadialData.SO.EnableSingleUseMode && ZInput.GetButtonLastPressedTimer("OpenEmote") > RadialData.SO.HoldCloseDelay && ZInput.GetButtonUp("OpenEmote")));
			radial.GetFlick = (() => ZInput.GetRadialTap());
			radial.GetDoubleTap = (() => ZInput.GetRadialMultiTap());
			ZInput.UpdateRadialMultiTap(RadialData.SO.DoubleClickTime, RadialData.SO.DoubleClickDelay, 2, RadialData.SO.RequireReleaseOnFinalClick);
			ZInput.UpdateRadialTapTime(RadialData.SO.FlickTime);
		}
	}
}
