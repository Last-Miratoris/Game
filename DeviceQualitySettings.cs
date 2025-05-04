using System;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001E7 RID: 487
	[CreateAssetMenu(fileName = "New device quality settings", menuName = "Device Quality Settings")]
	public class DeviceQualitySettings : ScriptableObject
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x000D2D08 File Offset: 0x000D0F08
		// (set) Token: 0x06001C02 RID: 7170 RVA: 0x000D2D10 File Offset: 0x000D0F10
		public GraphicsQualityMode GraphicQualityMode
		{
			get
			{
				return this.m_graphicsQualityMode;
			}
			set
			{
				this.m_graphicsQualityMode = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x000D2D19 File Offset: 0x000D0F19
		public PlatformHardware PlatformHardware
		{
			get
			{
				return this.m_platformHardware;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x000D2D21 File Offset: 0x000D0F21
		public bool IsDefault
		{
			get
			{
				return this.m_default;
			}
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000D2D2C File Offset: 0x000D0F2C
		internal void Apply()
		{
			PlatformPrefs.SetInt("FPSLimit", this.FpsLimit);
			PlatformPrefs.SetInt("VSync", this.Vsync ? 1 : 0);
			PlatformPrefs.SetInt("ClutterQuality", this.Vegetation);
			PlatformPrefs.SetInt("LodBias", this.Lod);
			PlatformPrefs.SetInt("Lights", this.Lights);
			PlatformPrefs.SetInt("ShadowQuality", this.ShadowQuality);
			PlatformPrefs.SetInt("PointLights", this.PointLights);
			PlatformPrefs.SetInt("PointLightShadows", this.PointLightShadows);
			PlatformPrefs.SetFloat("RenderScale", (float)this.RenderScale / 20f);
			PlatformPrefs.SetInt("DistantShadows", this.DistantShadows ? 1 : 0);
			PlatformPrefs.SetInt("Tesselation", this.Tesselation ? 1 : 0);
			PlatformPrefs.SetInt("SSAO", this.SSAO ? 1 : 0);
			PlatformPrefs.SetInt("Bloom", this.Bloom ? 1 : 0);
			PlatformPrefs.SetInt("DOF", this.DepthOfField ? 1 : 0);
			PlatformPrefs.SetInt("MotionBlur", this.MotionBlur ? 1 : 0);
			PlatformPrefs.SetInt("ChromaticAberration", this.ChromaticAberration ? 1 : 0);
			PlatformPrefs.SetInt("SunShafts", this.SunShafts ? 1 : 0);
			PlatformPrefs.SetInt("SoftPart", this.SoftParticles ? 1 : 0);
			PlatformPrefs.SetInt("AntiAliasing", this.AntiAliasing ? 1 : 0);
			if (this.m_fixedResolution)
			{
				PlatformPrefs.SetInt("FPSLimit", this.m_fpsLimit);
				Screen.SetResolution(this.m_resolutionWidth, this.m_resolutionHeight, FullScreenMode.FullScreenWindow, new RefreshRate
				{
					numerator = (uint)this.m_fpsLimit,
					denominator = 1U
				});
			}
			Settings.ApplyQualitySettings();
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000D2F00 File Offset: 0x000D1100
		public void LoadFromPlatformPrefs()
		{
			this.m_fpsLimit = PlatformPrefs.GetInt("FPSLimit", -1);
			this.m_vsync = (PlatformPrefs.GetInt("VSync", 0) == 1);
			this.m_vegetation = PlatformPrefs.GetInt("ClutterQuality", 3);
			this.m_lod = PlatformPrefs.GetInt("LodBias", 2);
			this.m_lights = PlatformPrefs.GetInt("Lights", 2);
			this.m_shadowQuality = PlatformPrefs.GetInt("ShadowQuality", 2);
			this.m_pointLights = PlatformPrefs.GetInt("PointLights", 3);
			this.m_pointLightShadows = PlatformPrefs.GetInt("PointLightShadows", 2);
			this.m_renderScale = (int)Mathf.Round(PlatformPrefs.GetFloat("RenderScale", 1f) * 20f);
			this.m_distantShadows = (PlatformPrefs.GetInt("DistantShadows", 1) == 1);
			this.m_tesselation = (PlatformPrefs.GetInt("Tesselation", 1) == 1);
			this.m_ssao = (PlatformPrefs.GetInt("SSAO", 1) == 1);
			this.m_bloom = (PlatformPrefs.GetInt("Bloom", 1) == 1);
			this.m_depthOfField = (PlatformPrefs.GetInt("DOF", 1) == 1);
			this.m_motionBlur = (PlatformPrefs.GetInt("MotionBlur", 1) == 1);
			this.m_chromaticAberration = (PlatformPrefs.GetInt("ChromaticAberration", 1) == 1);
			this.m_sunShafts = (PlatformPrefs.GetInt("SunShafts", 1) == 1);
			this.m_softParticles = (PlatformPrefs.GetInt("SoftPart", 1) == 1);
			this.m_antiAliasing = (PlatformPrefs.GetInt("AntiAliasing", 1) == 1);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000D3081 File Offset: 0x000D1281
		public bool IsSupportedOnHardware()
		{
			return true;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x000D3084 File Offset: 0x000D1284
		// (set) Token: 0x06001C09 RID: 7177 RVA: 0x000D308C File Offset: 0x000D128C
		public string NameTextId
		{
			get
			{
				return this.m_nameTextId;
			}
			set
			{
				if (this.m_graphicsQualityMode != GraphicsQualityMode.Custom)
				{
					throw new Exception(string.Format("Only values from {0} are allowed to be set via script", GraphicsQualityMode.Custom));
				}
				this.m_nameTextId = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06001C0A RID: 7178 RVA: 0x000D30B6 File Offset: 0x000D12B6
		// (set) Token: 0x06001C0B RID: 7179 RVA: 0x000D30BE File Offset: 0x000D12BE
		public string DescriptionTextId
		{
			get
			{
				return this.m_descriptionTextId;
			}
			set
			{
				if (this.m_graphicsQualityMode != GraphicsQualityMode.Custom)
				{
					throw new Exception(string.Format("Only values from {0} are allowed to be set via script", GraphicsQualityMode.Custom));
				}
				this.m_descriptionTextId = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x000D30E8 File Offset: 0x000D12E8
		// (set) Token: 0x06001C0D RID: 7181 RVA: 0x000D30F0 File Offset: 0x000D12F0
		public bool FixedResolution
		{
			get
			{
				return this.m_fixedResolution;
			}
			set
			{
				if (this.m_graphicsQualityMode != GraphicsQualityMode.Custom)
				{
					throw new Exception(string.Format("Only values from {0} are allowed to be set via script", GraphicsQualityMode.Custom));
				}
				this.m_fixedResolution = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x000D311A File Offset: 0x000D131A
		// (set) Token: 0x06001C0F RID: 7183 RVA: 0x000D3154 File Offset: 0x000D1354
		public int ResolutionWidth
		{
			get
			{
				if (!this.m_fixedResolution)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_resolutionWidth", this.m_resolutionWidth);
				}
				return this.m_resolutionWidth;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_resolutionWidth = value;
					return;
				}
				if (!this.m_fixedResolution)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_resolutionWidth", value);
					this.m_resolutionWidth = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000D31C5 File Offset: 0x000D13C5
		// (set) Token: 0x06001C11 RID: 7185 RVA: 0x000D31FC File Offset: 0x000D13FC
		public int ResolutionHeight
		{
			get
			{
				if (!this.m_fixedResolution)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_resolutionHeight", this.m_resolutionHeight);
				}
				return this.m_resolutionHeight;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_resolutionHeight = value;
					return;
				}
				if (!this.m_fixedResolution)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_resolutionHeight", value);
					this.m_resolutionHeight = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x000D326D File Offset: 0x000D146D
		// (set) Token: 0x06001C13 RID: 7187 RVA: 0x000D32A4 File Offset: 0x000D14A4
		public int FpsLimit
		{
			get
			{
				if (!this.m_fixedResolution)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_fpsLimit", this.m_fpsLimit);
				}
				return this.m_fpsLimit;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_fpsLimit = value;
					return;
				}
				if (!this.m_fixedResolution)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_fpsLimit", value);
					this.m_fpsLimit = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000D3315 File Offset: 0x000D1515
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x000D3358 File Offset: 0x000D1558
		public bool Vsync
		{
			get
			{
				if (!this.m_fixedResolution)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_vsync", this.m_vsync ? 1 : 0) == 1;
				}
				return this.m_vsync;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_vsync = value;
					return;
				}
				if (!this.m_fixedResolution)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_vsync", value ? 1 : 0);
					this.m_vsync = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000D33CF File Offset: 0x000D15CF
		public bool VegetationChangeable
		{
			get
			{
				return this.m_vegetationChangeable;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06001C17 RID: 7191 RVA: 0x000D33D7 File Offset: 0x000D15D7
		// (set) Token: 0x06001C18 RID: 7192 RVA: 0x000D3410 File Offset: 0x000D1610
		public int Vegetation
		{
			get
			{
				if (this.m_vegetationChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_vegetation", this.m_vegetation);
				}
				return this.m_vegetation;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_vegetation = value;
					return;
				}
				if (this.m_vegetationChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_vegetation", value);
					this.m_vegetation = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x000D3481 File Offset: 0x000D1681
		public bool LightsChangeable
		{
			get
			{
				return this.m_lightsChangeable;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x000D3489 File Offset: 0x000D1689
		// (set) Token: 0x06001C1B RID: 7195 RVA: 0x000D34C0 File Offset: 0x000D16C0
		public int Lights
		{
			get
			{
				if (this.m_lightsChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_lights", this.m_lights);
				}
				return this.m_lights;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_lights = value;
					return;
				}
				if (this.m_lightsChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_lights", value);
					this.m_lights = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x000D3531 File Offset: 0x000D1731
		public bool LodChangeable
		{
			get
			{
				return this.m_lodChangeable;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x000D3539 File Offset: 0x000D1739
		// (set) Token: 0x06001C1E RID: 7198 RVA: 0x000D3570 File Offset: 0x000D1770
		public int Lod
		{
			get
			{
				if (this.m_lodChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_lod", this.m_lod);
				}
				return this.m_lod;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_lod = value;
					return;
				}
				if (this.m_lodChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_lod", value);
					this.m_lod = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x000D35E1 File Offset: 0x000D17E1
		public bool ShadowQualityChangeable
		{
			get
			{
				return this.m_shadowQualityChangeable;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06001C20 RID: 7200 RVA: 0x000D35E9 File Offset: 0x000D17E9
		// (set) Token: 0x06001C21 RID: 7201 RVA: 0x000D3620 File Offset: 0x000D1820
		public int ShadowQuality
		{
			get
			{
				if (this.m_shadowQualityChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_shadowQuality", this.m_shadowQuality);
				}
				return this.m_shadowQuality;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_shadowQuality = value;
					return;
				}
				if (this.m_shadowQualityChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_shadowQuality", value);
					this.m_shadowQuality = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x000D3691 File Offset: 0x000D1891
		public bool PointLightsChangeable
		{
			get
			{
				return this.m_pointLightsChangeable;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x000D3699 File Offset: 0x000D1899
		// (set) Token: 0x06001C24 RID: 7204 RVA: 0x000D36D0 File Offset: 0x000D18D0
		public int PointLights
		{
			get
			{
				if (this.m_pointLightsChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_pointLights", this.m_pointLights);
				}
				return this.m_pointLights;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_pointLights = value;
					return;
				}
				if (this.m_pointLightsChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_pointLights", value);
					this.m_pointLights = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x000D3741 File Offset: 0x000D1941
		public bool PointLightShadowsChangeable
		{
			get
			{
				return this.m_pointLightShadowsChangeable;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x000D3749 File Offset: 0x000D1949
		// (set) Token: 0x06001C27 RID: 7207 RVA: 0x000D3780 File Offset: 0x000D1980
		public int PointLightShadows
		{
			get
			{
				if (this.m_pointLightShadowsChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_pointLightShadows", this.m_pointLightShadows);
				}
				return this.m_pointLightShadows;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_pointLightShadows = value;
					return;
				}
				if (this.m_pointLightShadowsChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_pointLightShadows", value);
					this.m_pointLightShadows = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x000D37F1 File Offset: 0x000D19F1
		public bool RenderScaleChangeable
		{
			get
			{
				return this.m_renderScaleChangeable;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x000D37F9 File Offset: 0x000D19F9
		// (set) Token: 0x06001C2A RID: 7210 RVA: 0x000D3830 File Offset: 0x000D1A30
		public int RenderScale
		{
			get
			{
				if (this.m_renderScaleChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_renderScale", this.m_renderScale);
				}
				return this.m_renderScale;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_renderScale = value;
					return;
				}
				if (this.m_renderScaleChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_renderScale", value);
					this.m_renderScale = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x000D38A1 File Offset: 0x000D1AA1
		public bool DistantShadowsChangeable
		{
			get
			{
				return this.m_distantShadowsChangeable;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000D38A9 File Offset: 0x000D1AA9
		// (set) Token: 0x06001C2D RID: 7213 RVA: 0x000D38EC File Offset: 0x000D1AEC
		public bool DistantShadows
		{
			get
			{
				if (this.m_distantShadowsChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_distantShadows", this.m_distantShadows ? 1 : 0) == 1;
				}
				return this.m_distantShadows;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_distantShadows = value;
					return;
				}
				if (this.m_distantShadowsChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_distantShadows", value ? 1 : 0);
					this.m_distantShadows = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06001C2E RID: 7214 RVA: 0x000D3963 File Offset: 0x000D1B63
		public bool TesselationChangeable
		{
			get
			{
				return this.m_tesselationChangeable;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06001C2F RID: 7215 RVA: 0x000D396B File Offset: 0x000D1B6B
		// (set) Token: 0x06001C30 RID: 7216 RVA: 0x000D39AC File Offset: 0x000D1BAC
		public bool Tesselation
		{
			get
			{
				if (this.m_tesselationChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_tesselation", this.m_tesselation ? 1 : 0) == 1;
				}
				return this.m_tesselation;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_tesselation = value;
					return;
				}
				if (this.m_tesselationChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_tesselation", value ? 1 : 0);
					this.m_tesselation = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06001C31 RID: 7217 RVA: 0x000D3A23 File Offset: 0x000D1C23
		public bool SSAOChangeable
		{
			get
			{
				return this.m_ssaoChangeable;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06001C32 RID: 7218 RVA: 0x000D3A2B File Offset: 0x000D1C2B
		// (set) Token: 0x06001C33 RID: 7219 RVA: 0x000D3A6C File Offset: 0x000D1C6C
		public bool SSAO
		{
			get
			{
				if (this.m_ssaoChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_ssao", this.m_ssao ? 1 : 0) == 1;
				}
				return this.m_ssao;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_ssao = value;
					return;
				}
				if (this.m_ssaoChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_ssao", value ? 1 : 0);
					this.m_ssao = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000D3AE3 File Offset: 0x000D1CE3
		public bool BloomChangeable
		{
			get
			{
				return this.m_bloomChangeable;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x000D3AEB File Offset: 0x000D1CEB
		// (set) Token: 0x06001C36 RID: 7222 RVA: 0x000D3B2C File Offset: 0x000D1D2C
		public bool Bloom
		{
			get
			{
				if (this.m_bloomChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_bloom", this.m_bloom ? 1 : 0) == 1;
				}
				return this.m_bloom;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_bloom = value;
					return;
				}
				if (this.m_bloomChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_bloom", value ? 1 : 0);
					this.m_bloom = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06001C37 RID: 7223 RVA: 0x000D3BA3 File Offset: 0x000D1DA3
		public bool DepthOfFieldChangeable
		{
			get
			{
				return this.m_depthOfFieldChangeable;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x000D3BAB File Offset: 0x000D1DAB
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x000D3BEC File Offset: 0x000D1DEC
		public bool DepthOfField
		{
			get
			{
				if (this.m_depthOfFieldChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_depthOfField", this.m_depthOfField ? 1 : 0) == 1;
				}
				return this.m_depthOfField;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_depthOfField = value;
					return;
				}
				if (this.m_depthOfFieldChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_depthOfField", value ? 1 : 0);
					this.m_depthOfField = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x000D3C63 File Offset: 0x000D1E63
		public bool MotionBlurChangeable
		{
			get
			{
				return this.m_motionBlurChangeable;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x000D3C6B File Offset: 0x000D1E6B
		// (set) Token: 0x06001C3C RID: 7228 RVA: 0x000D3CAC File Offset: 0x000D1EAC
		public bool MotionBlur
		{
			get
			{
				if (this.m_motionBlurChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_motionBlur", this.m_motionBlur ? 1 : 0) == 1;
				}
				return this.m_motionBlur;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_motionBlur = value;
					return;
				}
				if (this.m_motionBlurChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_motionBlur", value ? 1 : 0);
					this.m_motionBlur = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x000D3D23 File Offset: 0x000D1F23
		public bool ChromaticAberrationChangeable
		{
			get
			{
				return this.m_chromaticAberrationChangeable;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x000D3D2B File Offset: 0x000D1F2B
		// (set) Token: 0x06001C3F RID: 7231 RVA: 0x000D3D6C File Offset: 0x000D1F6C
		public bool ChromaticAberration
		{
			get
			{
				if (this.m_chromaticAberrationChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_chromaticAberration", this.m_chromaticAberration ? 1 : 0) == 1;
				}
				return this.m_chromaticAberration;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_chromaticAberration = value;
					return;
				}
				if (this.m_chromaticAberrationChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_chromaticAberration", value ? 1 : 0);
					this.m_chromaticAberration = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x000D3DE3 File Offset: 0x000D1FE3
		public bool SunShaftsChangeable
		{
			get
			{
				return this.m_sunShaftsChangeable;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x000D3DEB File Offset: 0x000D1FEB
		// (set) Token: 0x06001C42 RID: 7234 RVA: 0x000D3E2C File Offset: 0x000D202C
		public bool SunShafts
		{
			get
			{
				if (this.m_sunShaftsChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_sunShafts", this.m_sunShafts ? 1 : 0) == 1;
				}
				return this.m_sunShafts;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_sunShafts = value;
					return;
				}
				if (this.m_sunShaftsChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_sunShafts", value ? 1 : 0);
					this.m_sunShafts = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x000D3EA3 File Offset: 0x000D20A3
		public bool SoftParticlesChangeable
		{
			get
			{
				return this.m_softParticlesChangeable;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x000D3EAB File Offset: 0x000D20AB
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x000D3EEC File Offset: 0x000D20EC
		public bool SoftParticles
		{
			get
			{
				if (this.m_softParticlesChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_softParticles", this.m_softParticles ? 1 : 0) == 1;
				}
				return this.m_softParticles;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_softParticles = value;
					return;
				}
				if (this.m_softParticlesChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_softParticles", value ? 1 : 0);
					this.m_softParticles = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x000D3F63 File Offset: 0x000D2163
		public bool AntiAliasingChangeable
		{
			get
			{
				return this.m_antiAliasingChangeable;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x000D3F6B File Offset: 0x000D216B
		// (set) Token: 0x06001C48 RID: 7240 RVA: 0x000D3FAC File Offset: 0x000D21AC
		public bool AntiAliasing
		{
			get
			{
				if (this.m_antiAliasingChangeable)
				{
					return PlatformPrefs.GetInt(this.m_graphicsQualityMode.ToString() + "_m_antiAliasing", this.m_antiAliasing ? 1 : 0) == 1;
				}
				return this.m_antiAliasing;
			}
			set
			{
				if (this.m_graphicsQualityMode == GraphicsQualityMode.Custom)
				{
					this.m_antiAliasing = value;
					return;
				}
				if (this.m_antiAliasingChangeable)
				{
					PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_antiAliasing", value ? 1 : 0);
					this.m_antiAliasing = value;
					return;
				}
				Debug.Log(string.Format("Only values from {0} or settings marked as changeable are allowed to be set via script. Ignored value for mode {1}", GraphicsQualityMode.Custom, this.m_graphicsQualityMode));
			}
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000D4024 File Offset: 0x000D2224
		public void ClearChanged()
		{
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_vsync", this.m_vsync ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "_m_vegetation", this.m_vegetation);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_lod", this.m_lod);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_lights", this.m_lights);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_shadowQuality", this.m_shadowQuality);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_pointLights", this.m_pointLights);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_pointLightShadows", this.m_pointLightShadows);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_renderScale", this.m_renderScale);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_distantShadows", this.m_distantShadows ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_tesselation", this.m_tesselation ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_ssao", this.m_ssao ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_bloom", this.m_bloom ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_depthOfField", this.m_depthOfField ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_motionBlur", this.m_motionBlur ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_chromaticAberration", this.m_chromaticAberration ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_sunShafts", this.m_sunShafts ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_softParticles", this.m_softParticles ? 1 : 0);
			PlatformPrefs.SetInt(this.m_graphicsQualityMode.ToString() + "m_antiAliasing", this.m_antiAliasing ? 1 : 0);
		}

		// Token: 0x04001CFC RID: 7420
		[SerializeField]
		protected GraphicsQualityMode m_graphicsQualityMode;

		// Token: 0x04001CFD RID: 7421
		[SerializeField]
		protected PlatformHardware m_platformHardware;

		// Token: 0x04001CFE RID: 7422
		[SerializeField]
		protected string m_nameTextId;

		// Token: 0x04001CFF RID: 7423
		[SerializeField]
		protected string m_descriptionTextId;

		// Token: 0x04001D00 RID: 7424
		[SerializeField]
		protected bool m_default;

		// Token: 0x04001D01 RID: 7425
		[SerializeField]
		protected bool m_fixedResolution;

		// Token: 0x04001D02 RID: 7426
		[SerializeField]
		protected int m_resolutionWidth = 1920;

		// Token: 0x04001D03 RID: 7427
		[SerializeField]
		protected int m_resolutionHeight = 1080;

		// Token: 0x04001D04 RID: 7428
		[SerializeField]
		[Range(-1f, 361f)]
		protected int m_fpsLimit = 60;

		// Token: 0x04001D05 RID: 7429
		[SerializeField]
		protected bool m_vsync;

		// Token: 0x04001D06 RID: 7430
		[SerializeField]
		protected bool m_vegetationChangeable = true;

		// Token: 0x04001D07 RID: 7431
		[SerializeField]
		[Range(1f, 3f)]
		protected int m_vegetation;

		// Token: 0x04001D08 RID: 7432
		[SerializeField]
		protected bool m_lightsChangeable = true;

		// Token: 0x04001D09 RID: 7433
		[SerializeField]
		[Range(0f, 2f)]
		protected int m_lights;

		// Token: 0x04001D0A RID: 7434
		[SerializeField]
		protected bool m_lodChangeable = true;

		// Token: 0x04001D0B RID: 7435
		[SerializeField]
		[Range(0f, 3f)]
		protected int m_lod;

		// Token: 0x04001D0C RID: 7436
		[SerializeField]
		protected bool m_shadowQualityChangeable = true;

		// Token: 0x04001D0D RID: 7437
		[SerializeField]
		[Range(0f, 2f)]
		protected int m_shadowQuality;

		// Token: 0x04001D0E RID: 7438
		[SerializeField]
		protected bool m_pointLightsChangeable = true;

		// Token: 0x04001D0F RID: 7439
		[SerializeField]
		[Range(0f, 3f)]
		protected int m_pointLights;

		// Token: 0x04001D10 RID: 7440
		[SerializeField]
		protected bool m_pointLightShadowsChangeable = true;

		// Token: 0x04001D11 RID: 7441
		[SerializeField]
		[Range(0f, 3f)]
		protected int m_pointLightShadows;

		// Token: 0x04001D12 RID: 7442
		[SerializeField]
		protected bool m_renderScaleChangeable = true;

		// Token: 0x04001D13 RID: 7443
		[SerializeField]
		[Range(0f, 20f)]
		protected int m_renderScale;

		// Token: 0x04001D14 RID: 7444
		public const int c_renderScaleMaxInt = 20;

		// Token: 0x04001D15 RID: 7445
		[SerializeField]
		protected bool m_ssaoChangeable = true;

		// Token: 0x04001D16 RID: 7446
		[SerializeField]
		protected bool m_ssao;

		// Token: 0x04001D17 RID: 7447
		[SerializeField]
		protected bool m_tesselationChangeable = true;

		// Token: 0x04001D18 RID: 7448
		[SerializeField]
		protected bool m_tesselation;

		// Token: 0x04001D19 RID: 7449
		[SerializeField]
		protected bool m_distantShadowsChangeable = true;

		// Token: 0x04001D1A RID: 7450
		[SerializeField]
		protected bool m_distantShadows;

		// Token: 0x04001D1B RID: 7451
		[SerializeField]
		protected bool m_softParticlesChangeable = true;

		// Token: 0x04001D1C RID: 7452
		[SerializeField]
		protected bool m_softParticles;

		// Token: 0x04001D1D RID: 7453
		[SerializeField]
		protected bool m_antiAliasingChangeable = true;

		// Token: 0x04001D1E RID: 7454
		[SerializeField]
		protected bool m_antiAliasing;

		// Token: 0x04001D1F RID: 7455
		[SerializeField]
		protected bool m_bloomChangeable = true;

		// Token: 0x04001D20 RID: 7456
		[SerializeField]
		protected bool m_bloom;

		// Token: 0x04001D21 RID: 7457
		[SerializeField]
		protected bool m_depthOfFieldChangeable = true;

		// Token: 0x04001D22 RID: 7458
		[SerializeField]
		protected bool m_depthOfField;

		// Token: 0x04001D23 RID: 7459
		[SerializeField]
		protected bool m_motionBlurChangeable = true;

		// Token: 0x04001D24 RID: 7460
		[SerializeField]
		protected bool m_motionBlur;

		// Token: 0x04001D25 RID: 7461
		[SerializeField]
		protected bool m_chromaticAberrationChangeable = true;

		// Token: 0x04001D26 RID: 7462
		[SerializeField]
		protected bool m_chromaticAberration;

		// Token: 0x04001D27 RID: 7463
		[SerializeField]
		protected bool m_sunShaftsChangeable = true;

		// Token: 0x04001D28 RID: 7464
		[SerializeField]
		protected bool m_sunShafts;
	}
}
