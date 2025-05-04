using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000217 RID: 535
	internal class RadialMenuAnimationManager
	{
		// Token: 0x06001E06 RID: 7686 RVA: 0x000DE2C4 File Offset: 0x000DC4C4
		public bool IsTweenActive(string id)
		{
			foreach (RadialMenuAnimationManager.Tween tween in this._activeTweens)
			{
				if (tween.ID == id && tween.Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000DE330 File Offset: 0x000DC530
		public bool IsTweenPaused(string id)
		{
			foreach (RadialMenuAnimationManager.Tween tween in this._activeTweens)
			{
				if (tween.ID == id && tween.Paused)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000DE39C File Offset: 0x000DC59C
		public bool IsTweenActiveWithEndAction(string id)
		{
			foreach (RadialMenuAnimationManager.Tween tween in this._activeTweens)
			{
				if (tween.ID == id && tween.Active && tween.HasEndAction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000DE410 File Offset: 0x000DC610
		public void EndAll()
		{
			foreach (RadialMenuAnimationManager.Tween tween in this._activeTweens)
			{
				tween.End();
			}
			this._activeTweens.Clear();
			foreach (RadialMenuAnimationManager.EndAction endAction in this._endActions.Values)
			{
				endAction.Cancel();
			}
			this._endActions.Clear();
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000DE4BC File Offset: 0x000DC6BC
		public void CancelTweens(string id)
		{
			this.CancelOrPauseTweens(id, delegate(RadialMenuAnimationManager.Tween tween)
			{
				tween.Cancel();
			});
			RadialMenuAnimationManager.EndAction endAction;
			if (this._endActions.TryGetValue(id, out endAction))
			{
				endAction.Cancel();
			}
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000DE508 File Offset: 0x000DC708
		public void EndTweens(string id)
		{
			this.CancelOrPauseTweens(id, delegate(RadialMenuAnimationManager.Tween tween)
			{
				tween.End();
			});
			RadialMenuAnimationManager.EndAction endAction;
			if (this._endActions.TryGetValue(id, out endAction))
			{
				endAction.Cancel();
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000DE551 File Offset: 0x000DC751
		public void PauseTweens(string id)
		{
			this.CancelOrPauseTweens(id, delegate(RadialMenuAnimationManager.Tween tween)
			{
				tween.Pause();
			});
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000DE579 File Offset: 0x000DC779
		public void UnPauseTweens(string id)
		{
			this.CancelOrPauseTweens(id, delegate(RadialMenuAnimationManager.Tween tween)
			{
				tween.UnPause();
			});
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000DE5A4 File Offset: 0x000DC7A4
		private void CancelOrPauseTweens(string id, Action<RadialMenuAnimationManager.Tween> action)
		{
			foreach (RadialMenuAnimationManager.Tween tween in this._activeTweens.ToList<RadialMenuAnimationManager.Tween>())
			{
				if (!(tween.ID != id))
				{
					action(tween);
					if (!tween.Active && !tween.Paused)
					{
						this._activeTweens.Remove(tween);
					}
				}
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000DE628 File Offset: 0x000DC828
		public void Tick(float deltaTime)
		{
			if (this._activeTweens.Count <= 0)
			{
				return;
			}
			foreach (RadialMenuAnimationManager.Tween tween in this._activeTweens.ToList<RadialMenuAnimationManager.Tween>())
			{
				if (tween.Active && !tween.Paused)
				{
					tween.Tick(deltaTime);
				}
			}
			foreach (RadialMenuAnimationManager.Tween tween2 in this._activeTweens.ToList<RadialMenuAnimationManager.Tween>())
			{
				if (!tween2.Active)
				{
					this._activeTweens.Remove(tween2);
				}
			}
			using (List<RadialMenuAnimationManager.EndAction>.Enumerator enumerator2 = this._endActions.Values.ToList<RadialMenuAnimationManager.EndAction>().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RadialMenuAnimationManager.EndAction a = enumerator2.Current;
					if (!a.HasExecuted && this._activeTweens.All((RadialMenuAnimationManager.Tween t) => t.ID != a.ID))
					{
						a.Execute();
					}
				}
			}
			foreach (RadialMenuAnimationManager.EndAction endAction in this._endActions.Values.ToList<RadialMenuAnimationManager.EndAction>())
			{
				if (endAction.HasExecuted)
				{
					this._endActions.Remove(endAction.ID);
				}
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000DE7DC File Offset: 0x000DC9DC
		public void AddEnd(string id, Action onEnd)
		{
			using (List<RadialMenuAnimationManager.Tween>.Enumerator enumerator = this._activeTweens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == id)
					{
						goto IL_4E;
					}
				}
			}
			Debug.LogWarning("No active tweens with ID: " + id);
			return;
			IL_4E:
			if (this._endActions.ContainsKey(id))
			{
				this._endActions.Remove(id);
			}
			this._endActions.Add(id, new RadialMenuAnimationManager.EndAction(id, onEnd));
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000DE878 File Offset: 0x000DCA78
		public void StartUniqueTween<T>(Func<T> get, Action<T> set, string id, T targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null) where T : struct
		{
			this.CancelTweens(id);
			this.StartTween<T>(get, set, id, targetValue, duration, type, onEnd, onTick);
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000DE8A0 File Offset: 0x000DCAA0
		public void StartTween<T>(Func<T> get, Action<T> set, string id, T targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null) where T : struct
		{
			RadialMenuAnimationManager.Tween tween = (type == EasingType.SmoothDamp) ? RadialMenuAnimationManager.CreateDamp<T>() : RadialMenuAnimationManager.CreateLerp<T>();
			if (tween == null)
			{
				return;
			}
			RadialMenuAnimationManager.DampContainer<T> dampContainer = tween as RadialMenuAnimationManager.DampContainer<T>;
			if (dampContainer != null)
			{
				dampContainer.Init(id, targetValue, get, set, duration, onEnd, null);
			}
			else
			{
				RadialMenuAnimationManager.LerpContainer<T> lerpContainer = tween as RadialMenuAnimationManager.LerpContainer<T>;
				if (lerpContainer != null)
				{
					lerpContainer.Init(id, targetValue, get, set, duration, EasingFunctions.GetFunc(type), onEnd, onTick);
				}
			}
			this._activeTweens.Add(tween);
			tween.Start();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000DE914 File Offset: 0x000DCB14
		public void StartUniqueTween<T>(Action<T> set, string id, T startValue, T targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null) where T : struct
		{
			this.CancelTweens(id);
			this.StartTween<T>(set, id, startValue, targetValue, duration, type, onEnd, onTick);
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000DE93C File Offset: 0x000DCB3C
		public void StartTween<T>(Action<T> set, string id, T startValue, T targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null) where T : struct
		{
			if (type == EasingType.SmoothDamp)
			{
				Debug.LogError("Easing Type 'SmoothDamp' requires a getter. Tween with ID \"" + id + "\" was called with a start value instead.");
				return;
			}
			RadialMenuAnimationManager.LerpContainer<T> lerpContainer = RadialMenuAnimationManager.CreateLerp<T>();
			if (lerpContainer == null)
			{
				return;
			}
			lerpContainer.Init(id, targetValue, startValue, set, duration, EasingFunctions.GetFunc(type), onEnd, onTick);
			this._activeTweens.Add(lerpContainer);
			lerpContainer.Start();
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000DE998 File Offset: 0x000DCB98
		public void StartUniqueAngleTween(Func<float> get, Action<float> set, string id, float targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null)
		{
			this.CancelTweens(id);
			this.StartAngleTween(get, set, id, targetValue, duration, type, onEnd, onTick);
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000DE9C0 File Offset: 0x000DCBC0
		public void StartAngleTween(Func<float> get, Action<float> set, string id, float targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null)
		{
			if (type == EasingType.SmoothDamp)
			{
				Debug.LogError("Smooth Damp currently not supported for angles.");
				return;
			}
			RadialMenuAnimationManager.AngleLerp angleLerp = new RadialMenuAnimationManager.AngleLerp();
			angleLerp.InitAngle(id, targetValue, get, set, duration, EasingFunctions.GetFunc(type), onEnd, onTick);
			this._activeTweens.Add(angleLerp);
			angleLerp.Start();
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000DEA10 File Offset: 0x000DCC10
		public void StartUniqueAngleTween(Action<float> set, string id, float startValue, float targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null)
		{
			this.CancelTweens(id);
			this.StartAngleTween(set, id, startValue, targetValue, duration, type, onEnd, onTick);
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000DEA38 File Offset: 0x000DCC38
		public void StartAngleTween(Action<float> set, string id, float startValue, float targetValue, float duration = 1f, EasingType type = EasingType.Linear, Action onEnd = null, Action onTick = null)
		{
			if (type == EasingType.SmoothDamp)
			{
				Debug.LogError("Smooth Damp currently not supported for angles.");
				return;
			}
			RadialMenuAnimationManager.AngleLerp angleLerp = new RadialMenuAnimationManager.AngleLerp();
			angleLerp.InitAngle(id, targetValue, startValue, set, duration, EasingFunctions.GetFunc(type), onEnd, onTick);
			this._activeTweens.Add(angleLerp);
			angleLerp.Start();
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000DEA88 File Offset: 0x000DCC88
		private static RadialMenuAnimationManager.LerpContainer<T> CreateLerp<T>() where T : struct
		{
			RadialMenuAnimationManager.LerpContainer<T> result;
			if (typeof(T) == typeof(float))
			{
				result = (new RadialMenuAnimationManager.FloatLerp() as RadialMenuAnimationManager.LerpContainer<T>);
			}
			else if (typeof(T) == typeof(Vector2))
			{
				result = (new RadialMenuAnimationManager.Vector2Lerp() as RadialMenuAnimationManager.LerpContainer<T>);
			}
			else if (typeof(T) == typeof(Vector3))
			{
				result = (new RadialMenuAnimationManager.Vector3Lerp() as RadialMenuAnimationManager.LerpContainer<T>);
			}
			else if (typeof(T) == typeof(Quaternion))
			{
				result = (new RadialMenuAnimationManager.QuaternionLerp() as RadialMenuAnimationManager.LerpContainer<T>);
			}
			else
			{
				if (!(typeof(T) == typeof(Color)))
				{
					throw new InvalidOperationException("No lerp container defined for type " + typeof(T).Name);
				}
				result = (new RadialMenuAnimationManager.ColorLerp() as RadialMenuAnimationManager.LerpContainer<T>);
			}
			return result;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000DEB84 File Offset: 0x000DCD84
		private static RadialMenuAnimationManager.DampContainer<T> CreateDamp<T>() where T : struct
		{
			RadialMenuAnimationManager.DampContainer<T> result;
			if (typeof(T) == typeof(float))
			{
				result = (new RadialMenuAnimationManager.FloatDamp() as RadialMenuAnimationManager.DampContainer<T>);
			}
			else if (typeof(T) == typeof(Vector2))
			{
				result = (new RadialMenuAnimationManager.Vector2Damp() as RadialMenuAnimationManager.DampContainer<T>);
			}
			else
			{
				if (!(typeof(T) == typeof(Vector3)))
				{
					throw new InvalidOperationException("No damp container defined for type " + typeof(T).Name);
				}
				result = (new RadialMenuAnimationManager.Vector3Damp() as RadialMenuAnimationManager.DampContainer<T>);
			}
			return result;
		}

		// Token: 0x04001EBE RID: 7870
		private List<RadialMenuAnimationManager.Tween> _activeTweens = new List<RadialMenuAnimationManager.Tween>();

		// Token: 0x04001EBF RID: 7871
		private readonly Dictionary<string, RadialMenuAnimationManager.EndAction> _endActions = new Dictionary<string, RadialMenuAnimationManager.EndAction>();

		// Token: 0x020003C0 RID: 960
		private class EndAction
		{
			// Token: 0x170001E6 RID: 486
			// (get) Token: 0x060023A0 RID: 9120 RVA: 0x000F26E5 File Offset: 0x000F08E5
			// (set) Token: 0x060023A1 RID: 9121 RVA: 0x000F26ED File Offset: 0x000F08ED
			public string ID { get; private set; }

			// Token: 0x170001E7 RID: 487
			// (get) Token: 0x060023A2 RID: 9122 RVA: 0x000F26F6 File Offset: 0x000F08F6
			// (set) Token: 0x060023A3 RID: 9123 RVA: 0x000F26FE File Offset: 0x000F08FE
			public bool HasExecuted { get; private set; }

			// Token: 0x060023A4 RID: 9124 RVA: 0x000F2707 File Offset: 0x000F0907
			public EndAction(string iD, Action onEnd)
			{
				this.ID = iD;
				this.HasExecuted = false;
				this.OnEnd = onEnd;
			}

			// Token: 0x060023A5 RID: 9125 RVA: 0x000F2724 File Offset: 0x000F0924
			public void Execute()
			{
				Action onEnd = this.OnEnd;
				if (onEnd != null)
				{
					onEnd();
				}
				this.HasExecuted = true;
			}

			// Token: 0x060023A6 RID: 9126 RVA: 0x000F273E File Offset: 0x000F093E
			public void Cancel()
			{
				this.HasExecuted = true;
			}

			// Token: 0x0400274E RID: 10062
			private Action OnEnd;
		}

		// Token: 0x020003C1 RID: 961
		private abstract class Tween
		{
			// Token: 0x170001E8 RID: 488
			// (get) Token: 0x060023A7 RID: 9127 RVA: 0x000F2747 File Offset: 0x000F0947
			// (set) Token: 0x060023A8 RID: 9128 RVA: 0x000F274F File Offset: 0x000F094F
			public string ID { get; protected set; }

			// Token: 0x170001E9 RID: 489
			// (get) Token: 0x060023A9 RID: 9129 RVA: 0x000F2758 File Offset: 0x000F0958
			// (set) Token: 0x060023AA RID: 9130 RVA: 0x000F2760 File Offset: 0x000F0960
			public bool Active { get; protected set; }

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x060023AB RID: 9131 RVA: 0x000F2769 File Offset: 0x000F0969
			// (set) Token: 0x060023AC RID: 9132 RVA: 0x000F2771 File Offset: 0x000F0971
			public bool Paused { get; protected set; }

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x060023AD RID: 9133 RVA: 0x000F277A File Offset: 0x000F097A
			// (set) Token: 0x060023AE RID: 9134 RVA: 0x000F2782 File Offset: 0x000F0982
			public bool HasEndAction { get; protected set; }

			// Token: 0x060023AF RID: 9135
			public abstract void Start();

			// Token: 0x060023B0 RID: 9136
			public abstract void Tick(float deltaTime);

			// Token: 0x060023B1 RID: 9137
			public abstract void End();

			// Token: 0x060023B2 RID: 9138 RVA: 0x000F278B File Offset: 0x000F098B
			public void Pause()
			{
				this.Paused = true;
			}

			// Token: 0x060023B3 RID: 9139 RVA: 0x000F2794 File Offset: 0x000F0994
			public void UnPause()
			{
				this.Paused = false;
			}

			// Token: 0x060023B4 RID: 9140 RVA: 0x000F279D File Offset: 0x000F099D
			public void Cancel()
			{
				this.Active = false;
			}
		}

		// Token: 0x020003C2 RID: 962
		private abstract class TweenContainer<T> : RadialMenuAnimationManager.Tween where T : struct
		{
			// Token: 0x060023B6 RID: 9142 RVA: 0x000F27B0 File Offset: 0x000F09B0
			private void SharedInit(string id, T targetValue, float duration, Action onEnd, Action onTick = null)
			{
				base.ID = id;
				base.Active = false;
				this._targetValue = targetValue;
				this._duration = duration;
				this._elapsedTime = 0f;
				this.OnEnd = onEnd;
				this.OnTick = onTick;
				this._hasInitialized = true;
				base.HasEndAction = (onEnd != null);
			}

			// Token: 0x060023B7 RID: 9143 RVA: 0x000F2806 File Offset: 0x000F0A06
			protected void InitInternal(string id, T targetValue, Func<T> get, Action<T> set, float duration = 1f, Action onEnd = null, Action onTick = null)
			{
				this.SharedInit(id, targetValue, duration, onEnd, onTick);
				this.GetValue = get;
				this.SetValue = set;
			}

			// Token: 0x060023B8 RID: 9144 RVA: 0x000F2825 File Offset: 0x000F0A25
			public override void Start()
			{
				if (!this._hasInitialized)
				{
					Debug.LogError("Tween never initialized.");
					return;
				}
				base.Active = true;
			}

			// Token: 0x060023B9 RID: 9145 RVA: 0x000F2844 File Offset: 0x000F0A44
			public override void Tick(float deltaTime)
			{
				if (!base.Active)
				{
					return;
				}
				if (this._elapsedTime < this._duration)
				{
					this._elapsedTime = Mathf.Min(this._elapsedTime + deltaTime, this._duration);
					try
					{
						this.SetValue(this.UpdateValue());
						Action onTick = this.OnTick;
						if (onTick != null)
						{
							onTick();
						}
					}
					catch
					{
						base.Active = false;
					}
					return;
				}
				this.End();
				Action onEnd = this.OnEnd;
				if (onEnd == null)
				{
					return;
				}
				onEnd();
			}

			// Token: 0x060023BA RID: 9146
			protected abstract T UpdateValue();

			// Token: 0x060023BB RID: 9147 RVA: 0x000F28D8 File Offset: 0x000F0AD8
			public override void End()
			{
				try
				{
					this.SetValue(this._targetValue);
					Action onTick = this.OnTick;
					if (onTick != null)
					{
						onTick();
					}
				}
				catch
				{
				}
				base.Active = false;
			}

			// Token: 0x04002753 RID: 10067
			private bool _hasInitialized;

			// Token: 0x04002754 RID: 10068
			protected T _targetValue;

			// Token: 0x04002755 RID: 10069
			protected float _duration;

			// Token: 0x04002756 RID: 10070
			protected float _elapsedTime;

			// Token: 0x04002757 RID: 10071
			private Action OnEnd;

			// Token: 0x04002758 RID: 10072
			private Action OnTick;

			// Token: 0x04002759 RID: 10073
			protected Func<T> GetValue;

			// Token: 0x0400275A RID: 10074
			private Action<T> SetValue;
		}

		// Token: 0x020003C3 RID: 963
		private abstract class LerpContainer<T> : RadialMenuAnimationManager.TweenContainer<T> where T : struct
		{
			// Token: 0x060023BD RID: 9149 RVA: 0x000F292C File Offset: 0x000F0B2C
			private void LerpInit(T startVal, Func<float, float> easeFunction)
			{
				this._startValue = startVal;
				this._alpha = 0f;
				this.GetAlpha = easeFunction;
			}

			// Token: 0x060023BE RID: 9150 RVA: 0x000F2947 File Offset: 0x000F0B47
			public void Init(string id, T targetValue, Func<T> get, Action<T> set, float duration = 1f, Func<float, float> easeFunction = null, Action onEnd = null, Action onTick = null)
			{
				base.InitInternal(id, targetValue, get, set, duration, onEnd, onTick);
				this.LerpInit(this.GetValue(), easeFunction);
			}

			// Token: 0x060023BF RID: 9151 RVA: 0x000F296D File Offset: 0x000F0B6D
			public void Init(string id, T targetValue, T startValue, Action<T> set, float duration = 1f, Func<float, float> easeFunction = null, Action onEnd = null, Action onTick = null)
			{
				base.InitInternal(id, targetValue, null, set, duration, onEnd, onTick);
				this.LerpInit(startValue, easeFunction);
			}

			// Token: 0x060023C0 RID: 9152 RVA: 0x000F2989 File Offset: 0x000F0B89
			protected override T UpdateValue()
			{
				T result = this.Lerp(this._startValue, this._targetValue, this._alpha);
				this._alpha = Mathf.Clamp01(this._elapsedTime / this._duration);
				return result;
			}

			// Token: 0x060023C1 RID: 9153
			protected abstract T Lerp(T start, T end, float alpha);

			// Token: 0x0400275B RID: 10075
			private T _startValue;

			// Token: 0x0400275C RID: 10076
			private float _alpha;

			// Token: 0x0400275D RID: 10077
			protected Func<float, float> GetAlpha;
		}

		// Token: 0x020003C4 RID: 964
		private class FloatLerp : RadialMenuAnimationManager.LerpContainer<float>
		{
			// Token: 0x060023C3 RID: 9155 RVA: 0x000F29C3 File Offset: 0x000F0BC3
			protected override float Lerp(float start, float end, float alpha)
			{
				Func<float, float> getAlpha = this.GetAlpha;
				return Mathf.Lerp(start, end, (getAlpha != null) ? getAlpha(alpha) : alpha);
			}
		}

		// Token: 0x020003C5 RID: 965
		private class Vector2Lerp : RadialMenuAnimationManager.LerpContainer<Vector2>
		{
			// Token: 0x060023C5 RID: 9157 RVA: 0x000F29E7 File Offset: 0x000F0BE7
			protected override Vector2 Lerp(Vector2 start, Vector2 end, float alpha)
			{
				Func<float, float> getAlpha = this.GetAlpha;
				return Vector2.Lerp(start, end, (getAlpha != null) ? getAlpha(alpha) : alpha);
			}
		}

		// Token: 0x020003C6 RID: 966
		private class Vector3Lerp : RadialMenuAnimationManager.LerpContainer<Vector3>
		{
			// Token: 0x060023C7 RID: 9159 RVA: 0x000F2A0B File Offset: 0x000F0C0B
			protected override Vector3 Lerp(Vector3 start, Vector3 end, float alpha)
			{
				Func<float, float> getAlpha = this.GetAlpha;
				return Vector3.Lerp(start, end, (getAlpha != null) ? getAlpha(alpha) : alpha);
			}
		}

		// Token: 0x020003C7 RID: 967
		private class QuaternionLerp : RadialMenuAnimationManager.LerpContainer<Quaternion>
		{
			// Token: 0x060023C9 RID: 9161 RVA: 0x000F2A2F File Offset: 0x000F0C2F
			protected override Quaternion Lerp(Quaternion start, Quaternion end, float alpha)
			{
				Func<float, float> getAlpha = this.GetAlpha;
				return Quaternion.Lerp(start, end, (getAlpha != null) ? getAlpha(alpha) : alpha);
			}
		}

		// Token: 0x020003C8 RID: 968
		private class ColorLerp : RadialMenuAnimationManager.LerpContainer<Color>
		{
			// Token: 0x060023CB RID: 9163 RVA: 0x000F2A53 File Offset: 0x000F0C53
			protected override Color Lerp(Color start, Color end, float alpha)
			{
				Func<float, float> getAlpha = this.GetAlpha;
				return Color.Lerp(start, end, (getAlpha != null) ? getAlpha(alpha) : alpha);
			}
		}

		// Token: 0x020003C9 RID: 969
		private class AngleLerp : RadialMenuAnimationManager.LerpContainer<float>
		{
			// Token: 0x060023CD RID: 9165 RVA: 0x000F2A78 File Offset: 0x000F0C78
			public void InitAngle(string id, float targetValue, float startValue, Action<float> set, float duration = 1f, Func<float, float> easeFunction = null, Action onEnd = null, Action onTick = null)
			{
				startValue = UIMath.Mod(startValue, 360f);
				if (Mathf.Abs(targetValue - startValue) > 180f)
				{
					targetValue = ((targetValue > startValue) ? (targetValue - 360f) : (targetValue + 360f));
				}
				base.Init(id, targetValue, startValue, set, duration, easeFunction, onEnd, onTick);
			}

			// Token: 0x060023CE RID: 9166 RVA: 0x000F2ACC File Offset: 0x000F0CCC
			public void InitAngle(string id, float targetValue, Func<float> get, Action<float> set, float duration = 1f, Func<float, float> easeFunction = null, Action onEnd = null, Action onTick = null)
			{
				if (Mathf.Abs(targetValue - get()) > 180f)
				{
					targetValue = ((targetValue > get()) ? (targetValue - 360f) : (targetValue + 360f));
				}
				base.Init(id, targetValue, get, set, duration, easeFunction, onEnd, onTick);
			}

			// Token: 0x060023CF RID: 9167 RVA: 0x000F2B1B File Offset: 0x000F0D1B
			protected override float Lerp(float start, float end, float alpha)
			{
				Func<float, float> getAlpha = this.GetAlpha;
				return UIMath.Mod(Mathf.Lerp(start, end, (getAlpha != null) ? getAlpha(alpha) : alpha), 360f);
			}
		}

		// Token: 0x020003CA RID: 970
		private abstract class DampContainer<T> : RadialMenuAnimationManager.TweenContainer<T> where T : struct
		{
			// Token: 0x170001EC RID: 492
			// (get) Token: 0x060023D1 RID: 9169 RVA: 0x000F2B49 File Offset: 0x000F0D49
			protected float SmoothTime
			{
				get
				{
					return Mathf.Max(this._duration - this._elapsedTime, 0.0001f);
				}
			}

			// Token: 0x060023D2 RID: 9170 RVA: 0x000F2B62 File Offset: 0x000F0D62
			public void Init(string id, T targetValue, Func<T> get, Action<T> set, float duration = 1f, Action onEnd = null, Action onTick = null)
			{
				base.InitInternal(id, targetValue, get, set, duration, onEnd, onTick);
			}

			// Token: 0x0400275E RID: 10078
			protected T _velocity;
		}

		// Token: 0x020003CB RID: 971
		private class FloatDamp : RadialMenuAnimationManager.DampContainer<float>
		{
			// Token: 0x060023D4 RID: 9172 RVA: 0x000F2B7D File Offset: 0x000F0D7D
			public override void Start()
			{
				this._velocity = 0f;
				base.Start();
			}

			// Token: 0x060023D5 RID: 9173 RVA: 0x000F2B90 File Offset: 0x000F0D90
			protected override float UpdateValue()
			{
				return Mathf.SmoothDamp(this.GetValue(), this._targetValue, ref this._velocity, base.SmoothTime);
			}
		}

		// Token: 0x020003CC RID: 972
		private class Vector2Damp : RadialMenuAnimationManager.DampContainer<Vector2>
		{
			// Token: 0x060023D7 RID: 9175 RVA: 0x000F2BBC File Offset: 0x000F0DBC
			public override void Start()
			{
				this._velocity = Vector2.zero;
				base.Start();
			}

			// Token: 0x060023D8 RID: 9176 RVA: 0x000F2BCF File Offset: 0x000F0DCF
			protected override Vector2 UpdateValue()
			{
				return Vector2.SmoothDamp(this.GetValue(), this._targetValue, ref this._velocity, base.SmoothTime);
			}
		}

		// Token: 0x020003CD RID: 973
		private class Vector3Damp : RadialMenuAnimationManager.DampContainer<Vector3>
		{
			// Token: 0x060023DA RID: 9178 RVA: 0x000F2BFB File Offset: 0x000F0DFB
			public override void Start()
			{
				this._velocity = Vector3.zero;
				base.Start();
			}

			// Token: 0x060023DB RID: 9179 RVA: 0x000F2C0E File Offset: 0x000F0E0E
			protected override Vector3 UpdateValue()
			{
				return Vector3.SmoothDamp(this.GetValue(), this._targetValue, ref this._velocity, base.SmoothTime);
			}
		}
	}
}
