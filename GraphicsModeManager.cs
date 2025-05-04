using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001ED RID: 493
	public static class GraphicsModeManager
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x000D53CE File Offset: 0x000D35CE
		public static DeviceQualitySettings CurrentDeviceQualitySettings
		{
			get
			{
				return GraphicsModeManager._currentDeviceSettings;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x000D53D5 File Offset: 0x000D35D5
		public static PlatformHardware ThisPlatformHardware
		{
			get
			{
				if (Settings.IsSteamRunningOnSteamDeck())
				{
					return PlatformHardware.SteamDeck;
				}
				if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
				{
					return PlatformHardware.MacOSX;
				}
				return PlatformHardware.Standalone;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x000D53F7 File Offset: 0x000D35F7
		public static List<GraphicsQualityMode> GraphicsQualityModes
		{
			get
			{
				return GraphicsModeManager._graphicsQualityModes;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x000D5400 File Offset: 0x000D3600
		// (set) Token: 0x06001C7E RID: 7294 RVA: 0x000D5432 File Offset: 0x000D3632
		public static GraphicsQualityMode ActiveGraphicQualityMode
		{
			get
			{
				GraphicsQualityMode graphicsQualityMode = PlatformPrefs.GetInt("GraphicsQualityMode", 0).ToGraphicQualityMode();
				if (GraphicsModeManager.GraphicsQualityModes.Contains(graphicsQualityMode))
				{
					return graphicsQualityMode;
				}
				return GraphicsModeManager.SetToDefault();
			}
			set
			{
				if (!GraphicsModeManager.GraphicsQualityModes.Contains(value))
				{
					Debug.LogWarning(string.Format("Graphics quality mode {0} not supported on platform hardware.", value));
					return;
				}
				PlatformPrefs.SetInt("GraphicsQualityMode", value.ToInt());
				GraphicsModeManager.ApplyMode(value);
			}
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000D546E File Offset: 0x000D366E
		public static bool HasCustomInNonDev()
		{
			return true;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000D5474 File Offset: 0x000D3674
		public static DeviceQualitySettings CustomDeviceQualitySettings
		{
			get
			{
				if (GraphicsModeManager._deviceQualitySettings.Keys.Contains(GraphicsQualityMode.Custom))
				{
					return GraphicsModeManager._deviceQualitySettings[GraphicsQualityMode.Custom];
				}
				DeviceQualitySettings deviceQualitySettings = ScriptableObject.CreateInstance<DeviceQualitySettings>();
				deviceQualitySettings.GraphicQualityMode = GraphicsQualityMode.Custom;
				deviceQualitySettings.name = "Custom";
				deviceQualitySettings.NameTextId = "$settings_quality_mode_custom";
				deviceQualitySettings.LoadFromPlatformPrefs();
				GraphicsModeManager._deviceQualitySettings[GraphicsQualityMode.Custom] = deviceQualitySettings;
				return deviceQualitySettings;
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000D54DC File Offset: 0x000D36DC
		public static GraphicsQualityMode SetToDefault()
		{
			int num = -1;
			foreach (KeyValuePair<GraphicsQualityMode, DeviceQualitySettings> keyValuePair in GraphicsModeManager._deviceQualitySettings)
			{
				if (keyValuePair.Value.IsDefault)
				{
					num = keyValuePair.Key.ToInt();
					break;
				}
			}
			if (num < 0)
			{
				PlatformPrefs.SetInt("GraphicsQualityMode", GraphicsQualityMode.Custom.ToInt());
				num = GraphicsModeManager.CustomDeviceQualitySettings.GraphicQualityMode.ToInt();
			}
			PlatformPrefs.SetInt("GraphicsQualityMode", num);
			return num.ToGraphicQualityMode();
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000D557C File Offset: 0x000D377C
		public static void Initialize()
		{
			if (GraphicsModeManager._initialized)
			{
				return;
			}
			GraphicsModeManager._deviceQualitySettings = new Dictionary<GraphicsQualityMode, DeviceQualitySettings>();
			GraphicsModeManager._graphicsQualityModes = new List<GraphicsQualityMode>();
			foreach (DeviceQualitySettings deviceQualitySettings in Resources.LoadAll<DeviceQualitySettings>("QualitySettings").ToList<DeviceQualitySettings>().FindAll((DeviceQualitySettings s) => s.PlatformHardware == GraphicsModeManager.ThisPlatformHardware))
			{
				GraphicsModeManager._deviceQualitySettings[deviceQualitySettings.GraphicQualityMode] = deviceQualitySettings;
				if (deviceQualitySettings.GraphicQualityMode != GraphicsQualityMode.Constrained)
				{
					GraphicsModeManager._graphicsQualityModes.Add(deviceQualitySettings.GraphicQualityMode);
				}
			}
			GraphicsModeManager._graphicsQualityModes.Add(GraphicsQualityMode.Custom);
			if (PlatformPrefs.GetInt("GraphicsQualityMode", -1) < 0)
			{
				GraphicsModeManager.SetToDefault();
			}
			if (GraphicsModeManager.ApplyMode(GraphicsModeManager.ActiveGraphicQualityMode))
			{
				GraphicsModeManager._initialized = true;
				return;
			}
			ZLog.LogError("Could not initialize Quality Settings");
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000D5678 File Offset: 0x000D3878
		private static bool ApplyMode(GraphicsQualityMode mode)
		{
			Debug.Log(string.Format("Trying to set graphic mode to '{0}'", mode));
			if (!GraphicsModeManager._deviceQualitySettings.Keys.Contains(mode) || !GraphicsModeManager._deviceQualitySettings[mode].IsSupportedOnHardware())
			{
				Debug.Log(string.Format("Graphic mode '{0}' is not or no longer supported. Fallback to default mode for platform...", mode));
				mode = GraphicsModeManager.SetToDefault();
			}
			if (!GraphicsModeManager._deviceQualitySettings.Keys.Contains(mode))
			{
				return false;
			}
			GraphicsModeManager._currentDeviceSettings = GraphicsModeManager._deviceQualitySettings[mode];
			GraphicsModeManager._currentDeviceSettings.Apply();
			Debug.Log(string.Format("Graphic mode '{0}' applied!", mode));
			return true;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000D571E File Offset: 0x000D391E
		public static DeviceQualitySettings GetSettingsForMode(GraphicsQualityMode mode)
		{
			if (GraphicsModeManager._deviceQualitySettings.Keys.Contains(mode))
			{
				return GraphicsModeManager._deviceQualitySettings[mode];
			}
			if (mode == GraphicsQualityMode.Custom)
			{
				return GraphicsModeManager.CustomDeviceQualitySettings;
			}
			return null;
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000D574C File Offset: 0x000D394C
		public static bool MatchesMode(GraphicsQualityMode mode, bool considerChangeable, int resolutionWidth, int resolutionHeight, int fpsLimit, bool vsync, int vegetation, int levelOfDetail, int lights, int shadowQuality, int pointLights, int pointLightShadows, int renderScale, bool distantShadows, bool tesselation, bool ssao, bool bloom, bool depthOfField, bool motionBlur, bool chromaticAberration, bool sunShafts, bool softParticles, bool antiAliasing)
		{
			DeviceQualitySettings settingsForMode = GraphicsModeManager.GetSettingsForMode(mode);
			return !(settingsForMode == null) && ((!settingsForMode.FixedResolution || (resolutionWidth == settingsForMode.ResolutionWidth && resolutionHeight == settingsForMode.ResolutionHeight && fpsLimit == settingsForMode.FpsLimit && vsync == settingsForMode.Vsync)) && (vegetation == settingsForMode.Vegetation || (considerChangeable && settingsForMode.VegetationChangeable)) && (levelOfDetail == settingsForMode.Lod || (considerChangeable && settingsForMode.LodChangeable)) && (lights == settingsForMode.Lights || (considerChangeable && settingsForMode.LightsChangeable)) && (shadowQuality == settingsForMode.ShadowQuality || (considerChangeable && settingsForMode.ShadowQualityChangeable)) && (pointLights == settingsForMode.PointLights || (considerChangeable && settingsForMode.PointLightsChangeable)) && (pointLightShadows == settingsForMode.PointLightShadows || (considerChangeable && settingsForMode.PointLightShadowsChangeable)) && (renderScale == settingsForMode.RenderScale || (considerChangeable && settingsForMode.RenderScaleChangeable)) && (distantShadows == settingsForMode.DistantShadows || (considerChangeable && settingsForMode.DistantShadowsChangeable)) && (tesselation == settingsForMode.Tesselation || (considerChangeable && settingsForMode.TesselationChangeable)) && (ssao == settingsForMode.SSAO || (considerChangeable && settingsForMode.SSAOChangeable)) && (bloom == settingsForMode.Bloom || (considerChangeable && settingsForMode.BloomChangeable)) && (chromaticAberration == settingsForMode.ChromaticAberration || (considerChangeable && settingsForMode.ChromaticAberrationChangeable)) && (sunShafts == settingsForMode.SunShafts || (considerChangeable && settingsForMode.SunShaftsChangeable)) && (softParticles == settingsForMode.SoftParticles || (considerChangeable && settingsForMode.SoftParticles))) && (antiAliasing == settingsForMode.AntiAliasing || (considerChangeable && settingsForMode.AntiAliasingChangeable));
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000D591C File Offset: 0x000D3B1C
		public static void SaveSettings(GraphicsQualityMode mode, int resolutionWidth, int resolutionHeight, int fpsLimit, bool Vsync, int vegetation, int levelOfDetail, int lights, int shadowQuality, int pointLights, int pointLightShadows, int renderScale, bool distantShadows, bool tesselation, bool ssao, bool bloom, bool depthOfField, bool motionBlur, bool chromaticAberration, bool sunShafts, bool softParticles, bool antiAliasing)
		{
			if (GraphicsModeManager.MatchesMode(mode, false, resolutionWidth, resolutionHeight, fpsLimit, Vsync, vegetation, levelOfDetail, lights, shadowQuality, pointLights, pointLightShadows, renderScale, distantShadows, tesselation, ssao, bloom, depthOfField, motionBlur, chromaticAberration, sunShafts, softParticles, antiAliasing))
			{
				DeviceQualitySettings deviceQualitySettings = GraphicsModeManager._deviceQualitySettings[mode];
				deviceQualitySettings.FpsLimit = fpsLimit;
				deviceQualitySettings.Vsync = Vsync;
				deviceQualitySettings.DepthOfField = depthOfField;
				deviceQualitySettings.MotionBlur = motionBlur;
				GraphicsModeManager.ActiveGraphicQualityMode = mode;
				GraphicsModeManager.SyncFieldsToModes(deviceQualitySettings);
				return;
			}
			if (GraphicsModeManager._graphicsQualityModes.Contains(GraphicsQualityMode.Custom))
			{
				GraphicsModeManager.CustomDeviceQualitySettings.ResolutionWidth = resolutionWidth;
				GraphicsModeManager.CustomDeviceQualitySettings.ResolutionHeight = resolutionHeight;
				GraphicsModeManager.CustomDeviceQualitySettings.FpsLimit = fpsLimit;
				GraphicsModeManager.CustomDeviceQualitySettings.Vsync = Vsync;
				GraphicsModeManager.CustomDeviceQualitySettings.Vegetation = vegetation;
				GraphicsModeManager.CustomDeviceQualitySettings.Lod = levelOfDetail;
				GraphicsModeManager.CustomDeviceQualitySettings.Lights = lights;
				GraphicsModeManager.CustomDeviceQualitySettings.ShadowQuality = shadowQuality;
				GraphicsModeManager.CustomDeviceQualitySettings.PointLights = pointLights;
				GraphicsModeManager.CustomDeviceQualitySettings.PointLightShadows = pointLightShadows;
				GraphicsModeManager.CustomDeviceQualitySettings.RenderScale = renderScale;
				GraphicsModeManager.CustomDeviceQualitySettings.DistantShadows = distantShadows;
				GraphicsModeManager.CustomDeviceQualitySettings.Tesselation = tesselation;
				GraphicsModeManager.CustomDeviceQualitySettings.SSAO = ssao;
				GraphicsModeManager.CustomDeviceQualitySettings.Bloom = bloom;
				GraphicsModeManager.CustomDeviceQualitySettings.DepthOfField = depthOfField;
				GraphicsModeManager.CustomDeviceQualitySettings.MotionBlur = motionBlur;
				GraphicsModeManager.CustomDeviceQualitySettings.ChromaticAberration = chromaticAberration;
				GraphicsModeManager.CustomDeviceQualitySettings.SunShafts = sunShafts;
				GraphicsModeManager.CustomDeviceQualitySettings.SoftParticles = softParticles;
				GraphicsModeManager.CustomDeviceQualitySettings.AntiAliasing = antiAliasing;
				GraphicsModeManager.ActiveGraphicQualityMode = GraphicsQualityMode.Custom;
				GraphicsModeManager.SyncFieldsToModes(GraphicsModeManager.CustomDeviceQualitySettings);
				return;
			}
			if (GraphicsModeManager.MatchesMode(mode, true, resolutionWidth, resolutionHeight, fpsLimit, Vsync, vegetation, levelOfDetail, lights, shadowQuality, pointLights, pointLightShadows, renderScale, distantShadows, tesselation, ssao, bloom, depthOfField, motionBlur, chromaticAberration, sunShafts, softParticles, antiAliasing))
			{
				DeviceQualitySettings settingsForMode = GraphicsModeManager.GetSettingsForMode(mode);
				settingsForMode.FpsLimit = fpsLimit;
				settingsForMode.Vsync = Vsync;
				settingsForMode.Vegetation = vegetation;
				settingsForMode.Lod = levelOfDetail;
				settingsForMode.Lights = lights;
				settingsForMode.ShadowQuality = shadowQuality;
				settingsForMode.PointLights = pointLights;
				settingsForMode.PointLightShadows = pointLightShadows;
				settingsForMode.RenderScale = renderScale;
				settingsForMode.DistantShadows = distantShadows;
				settingsForMode.Tesselation = tesselation;
				settingsForMode.SSAO = ssao;
				settingsForMode.Bloom = bloom;
				settingsForMode.DepthOfField = depthOfField;
				settingsForMode.MotionBlur = motionBlur;
				settingsForMode.ChromaticAberration = chromaticAberration;
				settingsForMode.SunShafts = sunShafts;
				settingsForMode.SoftParticles = softParticles;
				settingsForMode.AntiAliasing = antiAliasing;
				GraphicsModeManager.ActiveGraphicQualityMode = mode;
				GraphicsModeManager.SyncFieldsToModes(settingsForMode);
				return;
			}
			Debug.LogError("This shouldn't happen");
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000D5B94 File Offset: 0x000D3D94
		private static void SyncFieldsToModes(DeviceQualitySettings fromSettings)
		{
			foreach (KeyValuePair<GraphicsQualityMode, DeviceQualitySettings> keyValuePair in GraphicsModeManager._deviceQualitySettings)
			{
				if (keyValuePair.Key != fromSettings.GraphicQualityMode && keyValuePair.Key != GraphicsQualityMode.Constrained)
				{
					keyValuePair.Value.Vsync = fromSettings.Vsync;
					keyValuePair.Value.FpsLimit = fromSettings.FpsLimit;
					keyValuePair.Value.MotionBlur = fromSettings.MotionBlur;
					keyValuePair.Value.DepthOfField = fromSettings.DepthOfField;
				}
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000D5C40 File Offset: 0x000D3E40
		public static void Reset()
		{
			GraphicsModeManager.ApplyMode(GraphicsModeManager.ActiveGraphicQualityMode);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000D5C4D File Offset: 0x000D3E4D
		public static void OnConstrainedModeActivated(bool isRunningInBackground)
		{
			if (!GraphicsModeManager._initialized)
			{
				return;
			}
			if (isRunningInBackground)
			{
				GraphicsModeManager.ApplyMode(GraphicsQualityMode.Constrained);
				return;
			}
			GraphicsModeManager.ApplyMode(GraphicsModeManager.ActiveGraphicQualityMode);
		}

		// Token: 0x04001D70 RID: 7536
		private const string GRAPHICS_QUALITY_MODE = "GraphicsQualityMode";

		// Token: 0x04001D71 RID: 7537
		private static bool _initialized;

		// Token: 0x04001D72 RID: 7538
		private static DeviceQualitySettings _currentDeviceSettings;

		// Token: 0x04001D73 RID: 7539
		private static List<GraphicsQualityMode> _graphicsQualityModes;

		// Token: 0x04001D74 RID: 7540
		private static Dictionary<GraphicsQualityMode, DeviceQualitySettings> _deviceQualitySettings;
	}
}
