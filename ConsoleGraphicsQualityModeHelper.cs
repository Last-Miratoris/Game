using System;

namespace Valheim.SettingsGui
{
	// Token: 0x020001EF RID: 495
	public static class ConsoleGraphicsQualityModeHelper
	{
		// Token: 0x06001C8A RID: 7306 RVA: 0x000D5C6D File Offset: 0x000D3E6D
		public static int ToInt(this GraphicsQualityMode mode)
		{
			return (int)mode;
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000D5C70 File Offset: 0x000D3E70
		public static GraphicsQualityMode ToGraphicQualityMode(this int mode)
		{
			return (GraphicsQualityMode)mode;
		}
	}
}
