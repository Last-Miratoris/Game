using System;

namespace Valheim.UI
{
	// Token: 0x02000210 RID: 528
	public static class RadialData
	{
		// Token: 0x06001DEB RID: 7659 RVA: 0x000DD968 File Offset: 0x000DBB68
		public static void Init(RadialDataSO dataSO)
		{
			RadialData.SO = dataSO;
			RadialData.SO.UsePersistantBackBtn = (PlatformPrefs.GetInt("RadialPersistentBackBtn", 0) != 0);
			RadialData.SO.EnableToggleAnimation = (PlatformPrefs.GetInt("RadialAnimateRadial", 1) != 0);
			RadialData.SO.EnableDoubleClick = (PlatformPrefs.GetInt("RadialDoubleTap", 0) != 0);
			RadialData.SO.EnableFlick = (PlatformPrefs.GetInt("RadialFlick", 0) != 0);
			RadialData.SO.EnableSingleUseMode = (PlatformPrefs.GetInt("RadialSingleUse", 0) != 0);
			RadialData.SO.HoverSelectSelectionSpeed = (HoverSelectSpeedSetting)PlatformPrefs.GetInt("RadialHoverSpd", 0);
			RadialData.SO.SpiralEffectInsensity = (SpiralEffectIntensitySetting)PlatformPrefs.GetInt("RadialSpiral", 2);
		}

		// Token: 0x04001E58 RID: 7768
		public static RadialDataSO SO;
	}
}
