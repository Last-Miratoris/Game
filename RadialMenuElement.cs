using System;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.UI
{
	// Token: 0x0200021A RID: 538
	public class RadialMenuElement : MonoBehaviour
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000DF258 File Offset: 0x000DD458
		public Material BackgroundMaterial
		{
			get
			{
				if (this.m_backgroundMaterial)
				{
					return this.m_backgroundMaterial;
				}
				this.m_backgroundMaterial = new Material(this.Background.material);
				this.Background.material = this.m_backgroundMaterial;
				return this.m_backgroundMaterial;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x000DF2A8 File Offset: 0x000DD4A8
		public RectTransform ElementTransform
		{
			get
			{
				if (this.m_rectTransform)
				{
					return this.m_rectTransform;
				}
				this.m_rectTransform = (base.transform as RectTransform);
				RectTransform rectTransform = this.m_rectTransform;
				RectTransform rectTransform2 = this.m_rectTransform;
				RectTransform rectTransform3 = this.m_rectTransform;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rectTransform3.pivot = vector;
				rectTransform.anchorMin = (rectTransform2.anchorMax = vector);
				this.m_rectTransform.GetChild(1).eulerAngles = Vector3.zero;
				return this.m_rectTransform;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000DF32D File Offset: 0x000DD52D
		// (set) Token: 0x06001E38 RID: 7736 RVA: 0x000DF33A File Offset: 0x000DD53A
		public Vector3 LocalPosition
		{
			get
			{
				return this.ElementTransform.localPosition;
			}
			set
			{
				this.ElementTransform.localPosition = value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000DF348 File Offset: 0x000DD548
		public Image Icon
		{
			get
			{
				return this.m_icon;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x000DF350 File Offset: 0x000DD550
		public Image Background
		{
			get
			{
				return this.m_background;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x000DF358 File Offset: 0x000DD558
		// (set) Token: 0x06001E3C RID: 7740 RVA: 0x000DF360 File Offset: 0x000DD560
		public string Name { get; protected set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x000DF369 File Offset: 0x000DD569
		// (set) Token: 0x06001E3E RID: 7742 RVA: 0x000DF371 File Offset: 0x000DD571
		public string SubTitle { get; protected set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000DF37A File Offset: 0x000DD57A
		// (set) Token: 0x06001E40 RID: 7744 RVA: 0x000DF382 File Offset: 0x000DD582
		public string Description { get; protected set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06001E41 RID: 7745 RVA: 0x000DF38C File Offset: 0x000DD58C
		public string ID
		{
			get
			{
				return base.gameObject.GetInstanceID().ToString();
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06001E42 RID: 7746 RVA: 0x000DF3AC File Offset: 0x000DD5AC
		// (set) Token: 0x06001E43 RID: 7747 RVA: 0x000DF3B4 File Offset: 0x000DD5B4
		public Func<RadialBase, RadialArray<RadialMenuElement>, bool> AdvancedCloseOnInteract { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06001E44 RID: 7748 RVA: 0x000DF3BD File Offset: 0x000DD5BD
		// (set) Token: 0x06001E45 RID: 7749 RVA: 0x000DF3C5 File Offset: 0x000DD5C5
		public Func<bool> CloseOnInteract { get; set; } = () => false;

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000DF3CE File Offset: 0x000DD5CE
		// (set) Token: 0x06001E47 RID: 7751 RVA: 0x000DF3D6 File Offset: 0x000DD5D6
		public Func<bool> Interact { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x000DF3DF File Offset: 0x000DD5DF
		// (set) Token: 0x06001E49 RID: 7753 RVA: 0x000DF3E7 File Offset: 0x000DD5E7
		public Func<bool> SecondaryInteract { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x000DF3F0 File Offset: 0x000DD5F0
		// (set) Token: 0x06001E4B RID: 7755 RVA: 0x000DF3F8 File Offset: 0x000DD5F8
		public Func<RadialBase, int, bool> TryOpenSubRadial { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x000DF401 File Offset: 0x000DD601
		// (set) Token: 0x06001E4D RID: 7757 RVA: 0x000DF413 File Offset: 0x000DD613
		public float Scale
		{
			get
			{
				return this.ElementTransform.localScale.x;
			}
			set
			{
				this.ElementTransform.localScale = Vector3.one * value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x000DF42B File Offset: 0x000DD62B
		// (set) Token: 0x06001E4F RID: 7759 RVA: 0x000DF438 File Offset: 0x000DD638
		public float Alpha
		{
			get
			{
				return this.m_canvasGroup.alpha;
			}
			set
			{
				this.m_canvasGroup.alpha = value;
				this.UnselectedColorAlpha = value;
				this.ActivatedColorAlpha = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x000DF454 File Offset: 0x000DD654
		// (set) Token: 0x06001E51 RID: 7761 RVA: 0x000DF46C File Offset: 0x000DD66C
		public float UnselectedColorAlpha
		{
			get
			{
				return this.BackgroundMaterial.GetColor("_UnselectedColor").a;
			}
			set
			{
				Color color = this.BackgroundMaterial.GetColor("_UnselectedColor");
				float a = Mathf.Clamp(value, 0f, 0.8f);
				color.a = a;
				this.BackgroundMaterial.SetColor("_UnselectedColor", color);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x000DF4B4 File Offset: 0x000DD6B4
		// (set) Token: 0x06001E53 RID: 7763 RVA: 0x000DF4CC File Offset: 0x000DD6CC
		public float ActivatedColorAlpha
		{
			get
			{
				return this.BackgroundMaterial.GetColor("_ActivatedColor").a;
			}
			set
			{
				Color color = this.BackgroundMaterial.GetColor("_ActivatedColor");
				float a = Mathf.Clamp(value, 0f, 0.8f);
				color.a = a;
				this.BackgroundMaterial.SetColor("_ActivatedColor", color);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x000DF514 File Offset: 0x000DD714
		// (set) Token: 0x06001E55 RID: 7765 RVA: 0x000DF529 File Offset: 0x000DD729
		public bool Selected
		{
			get
			{
				return this.BackgroundMaterial.GetInt("_Selected") == 1;
			}
			set
			{
				this.BackgroundMaterial.SetInt("_Selected", value ? 1 : 0);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x000DF542 File Offset: 0x000DD742
		// (set) Token: 0x06001E57 RID: 7767 RVA: 0x000DF554 File Offset: 0x000DD754
		public float Activated
		{
			get
			{
				return this.BackgroundMaterial.GetFloat("_Activated");
			}
			set
			{
				this.BackgroundMaterial.SetFloat("_Activated", value);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x000DF567 File Offset: 0x000DD767
		// (set) Token: 0x06001E59 RID: 7769 RVA: 0x000DF57C File Offset: 0x000DD77C
		public bool Queued
		{
			get
			{
				return this.BackgroundMaterial.GetInt("_Queued") == 1;
			}
			set
			{
				this.BackgroundMaterial.SetInt("_Queued", value ? 1 : 0);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x000DF595 File Offset: 0x000DD795
		// (set) Token: 0x06001E5B RID: 7771 RVA: 0x000DF5A7 File Offset: 0x000DD7A7
		public float Hovering
		{
			get
			{
				return this.BackgroundMaterial.GetFloat("_Hovering");
			}
			set
			{
				this.m_backgroundMaterial.SetFloat("_Hovering", value);
			}
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000DF5BC File Offset: 0x000DD7BC
		internal void OpenAnimation(RadialMenuAnimationManager manager, string id, float duration, float distance, float startOffset, EasingType alphaEasingType, EasingType positionEasingType)
		{
			this.LocalPosition = this.LocalPosition.normalized * (distance + startOffset);
			manager.StartTween<Vector3>(() => this.LocalPosition, delegate(Vector3 val)
			{
				this.LocalPosition = val;
			}, id, this.LocalPosition.normalized * distance, duration, positionEasingType, null, null);
			float alpha = this.Alpha;
			this.Alpha = 0f;
			manager.StartTween<float>(() => this.Alpha, delegate(float val)
			{
				this.Alpha = val;
			}, id, alpha, duration + 0.1f, alphaEasingType, null, null);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000DF660 File Offset: 0x000DD860
		internal void CloseAnimation(RadialMenuAnimationManager manager, string id, float duration, float distance, float startOffset, EasingType alphaEasingType, EasingType positionEasingType)
		{
			manager.StartTween<Vector3>(() => this.LocalPosition, delegate(Vector3 val)
			{
				this.LocalPosition = val;
			}, id, this.LocalPosition.normalized * (distance + startOffset), duration + 0.1f, positionEasingType, null, null);
			manager.StartTween<float>(() => this.Alpha, delegate(float val)
			{
				this.Alpha = val;
			}, id, 0f, duration + 0.1f, alphaEasingType, null, null);
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000DF6E0 File Offset: 0x000DD8E0
		internal void StartHoverSelect(RadialMenuAnimationManager manager, float duration, EasingType easingType, Action onEnd)
		{
			manager.StartUniqueTween<float>(() => this.Hovering, delegate(float val)
			{
				this.Hovering = val;
			}, this.ID + "_hov", 1f, (this.Hovering > 0f) ? (duration - duration * this.Hovering) : duration, easingType, onEnd, null);
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000DF740 File Offset: 0x000DD940
		internal void ResetHoverSelect(RadialMenuAnimationManager manager, float duration, EasingType easingType)
		{
			manager.StartUniqueTween<float>(() => this.Hovering, delegate(float val)
			{
				this.Hovering = val;
			}, this.ID + "_hov", 0f, this.Hovering * duration, easingType, null, null);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000DF78C File Offset: 0x000DD98C
		internal void StartNudge(RadialMenuAnimationManager manager, float distance, float duration, EasingType easingType)
		{
			manager.StartUniqueTween<Vector3>(() => this.LocalPosition, delegate(Vector3 val)
			{
				this.LocalPosition = val;
			}, this.ID + "_nug", this.LocalPosition.normalized * distance, duration, easingType, null, null);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000DF7E0 File Offset: 0x000DD9E0
		internal void ResetNudge(RadialMenuAnimationManager manager, float distance, float duration, EasingType easingType)
		{
			manager.StartUniqueTween<Vector3>(() => this.LocalPosition, delegate(Vector3 val)
			{
				this.LocalPosition = val;
			}, this.ID + "_nug", this.LocalPosition.normalized * distance, duration, easingType, null, null);
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000DF834 File Offset: 0x000DDA34
		internal void EndNudge(RadialMenuAnimationManager manager)
		{
			manager.EndTweens(this.ID + "_nug");
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000DF84C File Offset: 0x000DDA4C
		public void SetSegment(int segments)
		{
			this.SetSegment(UIMath.DirectionToAngleDegrees(this.LocalPosition.normalized, true) / 360f, segments);
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000DF87F File Offset: 0x000DDA7F
		public void SetSegment(int index, int segments)
		{
			this.BackgroundMaterial.SetInt("_Segments", segments);
			this.BackgroundMaterial.SetFloat("_Offset", (float)index / (float)segments);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000DF8A7 File Offset: 0x000DDAA7
		public void SetSegment(float offset, int segments)
		{
			offset = Math.Clamp(offset, 0f, 1f);
			this.BackgroundMaterial.SetInt("_Segments", segments);
			this.BackgroundMaterial.SetFloat("_Offset", offset);
		}

		// Token: 0x04001EDE RID: 7902
		protected const string c_HOVER_SUFFIX = "_hov";

		// Token: 0x04001EDF RID: 7903
		protected const string c_NUDGE_SUFFIX = "_nug";

		// Token: 0x04001EE0 RID: 7904
		[SerializeField]
		protected Image m_icon;

		// Token: 0x04001EE1 RID: 7905
		[SerializeField]
		protected Image m_background;

		// Token: 0x04001EE2 RID: 7906
		[SerializeField]
		protected CanvasGroup m_canvasGroup;

		// Token: 0x04001EE3 RID: 7907
		private Material m_backgroundMaterial;

		// Token: 0x04001EE4 RID: 7908
		private RectTransform m_rectTransform;
	}
}
