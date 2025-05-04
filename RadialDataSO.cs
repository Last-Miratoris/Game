using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Valheim.UI
{
	// Token: 0x02000214 RID: 532
	[CreateAssetMenu(fileName = "RadialData", menuName = "Valheim/Radial/Radial Data", order = 0)]
	public class RadialDataSO : ScriptableObject
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x000DDA34 File Offset: 0x000DBC34
		public float ElementNudgeFactor
		{
			get
			{
				SpiralEffectIntensitySetting spiralEffectInsensity = this.SpiralEffectInsensity;
				float result;
				switch (spiralEffectInsensity)
				{
				case SpiralEffectIntensitySetting.Off:
					result = 0f;
					break;
				case SpiralEffectIntensitySetting.Slight:
					result = this.SlightElementNudgeFactor;
					break;
				case SpiralEffectIntensitySetting.Normal:
					result = this.NormalElementNudgeFactor;
					break;
				default:
					<PrivateImplementationDetails>.ThrowSwitchExpressionException(spiralEffectInsensity);
					break;
				}
				return result;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06001DEF RID: 7663 RVA: 0x000DDA84 File Offset: 0x000DBC84
		public float ElementScaleFactor
		{
			get
			{
				SpiralEffectIntensitySetting spiralEffectInsensity = this.SpiralEffectInsensity;
				float result;
				switch (spiralEffectInsensity)
				{
				case SpiralEffectIntensitySetting.Off:
					result = 1f;
					break;
				case SpiralEffectIntensitySetting.Slight:
					result = this.SlightElementScaleFactor;
					break;
				case SpiralEffectIntensitySetting.Normal:
					result = this.NormalElementScaleFactor;
					break;
				default:
					<PrivateImplementationDetails>.ThrowSwitchExpressionException(spiralEffectInsensity);
					break;
				}
				return result;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x000DDAD4 File Offset: 0x000DBCD4
		public float HoverSelectSpeed
		{
			get
			{
				float result;
				switch (this.HoverSelectSelectionSpeed)
				{
				case HoverSelectSpeedSetting.Off:
					result = -1f;
					break;
				case HoverSelectSpeedSetting.Slow:
					result = this.HoverSelectSlow;
					break;
				case HoverSelectSpeedSetting.Medium:
					result = this.HoverSelectMedium;
					break;
				case HoverSelectSpeedSetting.Fast:
					result = this.HoverSelectFast;
					break;
				default:
					result = -1f;
					break;
				}
				return result;
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000DDB2C File Offset: 0x000DBD2C
		private void OnInspectorChanged(string propertyName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(propertyName);
			if (num <= 1857446455U)
			{
				if (num <= 377232474U)
				{
					if (num != 152591223U)
					{
						if (num == 377232474U)
						{
							if (propertyName == "OrnamentOffset")
							{
								Action onOrnamentOffsetChanged = this.OnOrnamentOffsetChanged;
								if (onOrnamentOffsetChanged == null)
								{
									return;
								}
								onOrnamentOffsetChanged();
								return;
							}
						}
					}
					else if (propertyName == "EnableSelectedOrnament")
					{
						Action onEnableSelectedOrnamentChanged = this.OnEnableSelectedOrnamentChanged;
						if (onEnableSelectedOrnamentChanged == null)
						{
							return;
						}
						onEnableSelectedOrnamentChanged();
						return;
					}
				}
				else if (num != 1349994799U)
				{
					if (num != 1771029790U)
					{
						if (num == 1857446455U)
						{
							if (propertyName == "ElementsDistance")
							{
								Action onElementsDistanceChanged = this.OnElementsDistanceChanged;
								if (onElementsDistanceChanged == null)
								{
									return;
								}
								onElementsDistanceChanged();
								return;
							}
						}
					}
					else if (propertyName == "UsePersistantBackBtn")
					{
						Action onUsePersistantBackBtnChanged = this.OnUsePersistantBackBtnChanged;
						if (onUsePersistantBackBtnChanged == null)
						{
							return;
						}
						onUsePersistantBackBtnChanged();
						return;
					}
				}
				else if (propertyName == "LayerFadeCount")
				{
					Action onLayerFadeCountChanged = this.OnLayerFadeCountChanged;
					if (onLayerFadeCountChanged == null)
					{
						return;
					}
					onLayerFadeCountChanged();
					return;
				}
			}
			else if (num <= 2264593249U)
			{
				if (num != 2146285226U)
				{
					if (num == 2264593249U)
					{
						if (propertyName == "ElementInfoRadius")
						{
							Action onElementInfoRadiusChanged = this.OnElementInfoRadiusChanged;
							if (onElementInfoRadiusChanged == null)
							{
								return;
							}
							onElementInfoRadiusChanged();
							return;
						}
					}
				}
				else if (propertyName == "LayerShowCount")
				{
					Action onLayerShowCountChanged = this.OnLayerShowCountChanged;
					if (onLayerShowCountChanged == null)
					{
						return;
					}
					onLayerShowCountChanged();
					return;
				}
			}
			else if (num != 2288620051U)
			{
				if (num != 2611880595U)
				{
					if (num == 2823944200U)
					{
						if (propertyName == "CursorDistance")
						{
							Action onCursorDistanceChanged = this.OnCursorDistanceChanged;
							if (onCursorDistanceChanged == null)
							{
								return;
							}
							onCursorDistanceChanged();
							return;
						}
					}
				}
				else if (propertyName == "NudgeDistance")
				{
					Action onNudgeDistanceChanged = this.OnNudgeDistanceChanged;
					if (onNudgeDistanceChanged == null)
					{
						return;
					}
					onNudgeDistanceChanged();
					return;
				}
			}
			else if (propertyName == "NudgeSelectedElement")
			{
				Action onNudgeSelectedElementChanged = this.OnNudgeSelectedElementChanged;
				if (onNudgeSelectedElementChanged == null)
				{
					return;
				}
				onNudgeSelectedElementChanged();
				return;
			}
			Debug.Log("No variable with name " + propertyName + " found.");
		}

		// Token: 0x04001E63 RID: 7779
		[Header("Settings")]
		[Header("----------------------------")]
		[Header("Layout Settings")]
		[InspectorChangedEvent]
		public float ElementInfoRadius;

		// Token: 0x04001E64 RID: 7780
		public Action OnElementInfoRadiusChanged;

		// Token: 0x04001E65 RID: 7781
		[InspectorChangedEvent]
		public float CursorDistance;

		// Token: 0x04001E66 RID: 7782
		public Action OnCursorDistanceChanged;

		// Token: 0x04001E67 RID: 7783
		[InspectorChangedEvent]
		public float ElementsDistance;

		// Token: 0x04001E68 RID: 7784
		public Action OnElementsDistanceChanged;

		// Token: 0x04001E69 RID: 7785
		[Space(10f)]
		[Header("Fade Settings")]
		public SpiralEffectIntensitySetting SpiralEffectInsensity;

		// Token: 0x04001E6A RID: 7786
		[FormerlySerializedAs("ElementNudgeFactor")]
		public float NormalElementNudgeFactor;

		// Token: 0x04001E6B RID: 7787
		[FormerlySerializedAs("ElementScaleFactor")]
		public float NormalElementScaleFactor;

		// Token: 0x04001E6C RID: 7788
		public float SlightElementNudgeFactor;

		// Token: 0x04001E6D RID: 7789
		public float SlightElementScaleFactor;

		// Token: 0x04001E6E RID: 7790
		public float ElementFadeDuration;

		// Token: 0x04001E6F RID: 7791
		public float ReFadeMultiplier;

		// Token: 0x04001E70 RID: 7792
		public EasingType ElementFadeEasingType;

		// Token: 0x04001E71 RID: 7793
		public EasingType ElementScaleEasingType;

		// Token: 0x04001E72 RID: 7794
		[Space(10f)]
		[Header("Layer Settings")]
		[InspectorChangedEvent]
		public int LayerShowCount;

		// Token: 0x04001E73 RID: 7795
		public Action OnLayerShowCountChanged;

		// Token: 0x04001E74 RID: 7796
		[InspectorChangedEvent]
		public int LayerFadeCount;

		// Token: 0x04001E75 RID: 7797
		public Action OnLayerFadeCountChanged;

		// Token: 0x04001E76 RID: 7798
		public int[] MaxElementsRange = new int[]
		{
			8,
			12
		};

		// Token: 0x04001E77 RID: 7799
		[Space(10f)]
		[Header("Cursor Settings")]
		[Range(0f, 1f)]
		public float CursorSensitivity;

		// Token: 0x04001E78 RID: 7800
		public float CursorSpeed;

		// Token: 0x04001E79 RID: 7801
		public EasingType CursorEasingType;

		// Token: 0x04001E7A RID: 7802
		[FormerlySerializedAs("HoverSelectSelectedSpeed")]
		[Space(10f)]
		[Header("Interaction Settings")]
		public HoverSelectSpeedSetting HoverSelectSelectionSpeed;

		// Token: 0x04001E7B RID: 7803
		public float HoverSelectSlow;

		// Token: 0x04001E7C RID: 7804
		public float HoverSelectMedium;

		// Token: 0x04001E7D RID: 7805
		public float HoverSelectFast;

		// Token: 0x04001E7E RID: 7806
		[ConditionalHide("EnableHoverSelect", false)]
		public EasingType HoverSelectEasingType;

		// Token: 0x04001E7F RID: 7807
		public float InteractionDelay;

		// Token: 0x04001E80 RID: 7808
		public float HoldCloseDelay;

		// Token: 0x04001E81 RID: 7809
		[Space(10f)]
		[Header("Experimental Features")]
		[InspectorChangedEvent]
		public bool UsePersistantBackBtn;

		// Token: 0x04001E82 RID: 7810
		public Action OnUsePersistantBackBtnChanged;

		// Token: 0x04001E83 RID: 7811
		[Space(5f)]
		public bool DefaultToBackButtonOnNewPage;

		// Token: 0x04001E84 RID: 7812
		[Space(5f)]
		public bool ReSizeOnRefresh;

		// Token: 0x04001E85 RID: 7813
		[Space(5f)]
		public bool ReFadeAtMidnight;

		// Token: 0x04001E86 RID: 7814
		[Space(5f)]
		public bool EnableSingleUseMode;

		// Token: 0x04001E87 RID: 7815
		[Space(5f)]
		public bool AllowSingleItemHoverMenu;

		// Token: 0x04001E88 RID: 7816
		[Space(5f)]
		public bool OpenNormalRadialWhenHoverMenuFails;

		// Token: 0x04001E89 RID: 7817
		[Space(5f)]
		public bool EnableDoubleClick;

		// Token: 0x04001E8A RID: 7818
		[ConditionalHide("EnableDoubleClick", false)]
		public float DoubleClickTime;

		// Token: 0x04001E8B RID: 7819
		[ConditionalHide("EnableDoubleClick", false)]
		public float DoubleClickDelay;

		// Token: 0x04001E8C RID: 7820
		[ConditionalHide("EnableDoubleClick", false)]
		public bool RequireReleaseOnFinalClick;

		// Token: 0x04001E8D RID: 7821
		[Space(5f)]
		public bool EnableFlick;

		// Token: 0x04001E8E RID: 7822
		[ConditionalHide("EnableFlick", false)]
		public float FlickTime;

		// Token: 0x04001E8F RID: 7823
		[Space(5f)]
		[InspectorChangedEvent]
		public bool EnableSelectedOrnament;

		// Token: 0x04001E90 RID: 7824
		public Action OnEnableSelectedOrnamentChanged;

		// Token: 0x04001E91 RID: 7825
		[ConditionalHide("EnableSelectedOrnament", false)]
		[InspectorChangedEvent]
		public float OrnamentOffset;

		// Token: 0x04001E92 RID: 7826
		public Action OnOrnamentOffsetChanged;

		// Token: 0x04001E93 RID: 7827
		[Space(5f)]
		[InspectorChangedEvent]
		public bool NudgeSelectedElement;

		// Token: 0x04001E94 RID: 7828
		public Action OnNudgeSelectedElementChanged;

		// Token: 0x04001E95 RID: 7829
		[ConditionalHide("NudgeSelectedElement", false)]
		[InspectorChangedEvent]
		public float NudgeDistance;

		// Token: 0x04001E96 RID: 7830
		public Action OnNudgeDistanceChanged;

		// Token: 0x04001E97 RID: 7831
		[ConditionalHide("NudgeSelectedElement", false)]
		public float NudgeDuration;

		// Token: 0x04001E98 RID: 7832
		[ConditionalHide("NudgeSelectedElement", false)]
		public EasingType NudgeEasingType;

		// Token: 0x04001E99 RID: 7833
		[Space(5f)]
		public bool EnableToggleAnimation;

		// Token: 0x04001E9A RID: 7834
		[ConditionalHide("EnableToggleAnimation", false)]
		public float ToggleAnimDuration;

		// Token: 0x04001E9B RID: 7835
		[ConditionalHide("EnableToggleAnimation", false)]
		public float ToggleAnimDistance;

		// Token: 0x04001E9C RID: 7836
		[ConditionalHide("EnableToggleAnimation", false)]
		public EasingType TogglePosEasingType;

		// Token: 0x04001E9D RID: 7837
		[ConditionalHide("EnableToggleAnimation", false)]
		public EasingType ToggleAlphaEasingType;

		// Token: 0x04001E9E RID: 7838
		[Space(20f)]
		[Header("References")]
		[Header("----------------------------")]
		[Header("Element Prefabs")]
		public EmptyElement EmptyElement;

		// Token: 0x04001E9F RID: 7839
		public BackElement BackElement;

		// Token: 0x04001EA0 RID: 7840
		public EmoteElement EmoteElement;

		// Token: 0x04001EA1 RID: 7841
		public GroupElement GroupElement;

		// Token: 0x04001EA2 RID: 7842
		public ThrowElement ThrowElement;

		// Token: 0x04001EA3 RID: 7843
		public ItemElement ItemElement;

		// Token: 0x04001EA4 RID: 7844
		public HammerItemElement HammerItemElement;

		// Token: 0x04001EA5 RID: 7845
		[Space(10f)]
		[Header("Mappings References")]
		public ItemGroupMappings ItemGroupMappings;

		// Token: 0x04001EA6 RID: 7846
		public EmoteMappings EmoteMappings;

		// Token: 0x04001EA7 RID: 7847
		[Space(10f)]
		[Header("Radial Group Configs")]
		public OpenRadialConfig OpenConfig;

		// Token: 0x04001EA8 RID: 7848
		public ValheimRadialConfig MainGroupConfig;

		// Token: 0x04001EA9 RID: 7849
		public HotbarGroupConfig HotbarGroupConfig;

		// Token: 0x04001EAA RID: 7850
		public ItemGroupConfig ItemGroupConfig;

		// Token: 0x04001EAB RID: 7851
		public ThrowGroupConfig ThrowGroupConfig;

		// Token: 0x04001EAC RID: 7852
		public EmoteGroupConfig EmoteGroupConfig;
	}
}
