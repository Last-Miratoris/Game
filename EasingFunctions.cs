using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x02000219 RID: 537
	internal static class EasingFunctions
	{
		// Token: 0x06001E1C RID: 7708 RVA: 0x000DEC48 File Offset: 0x000DCE48
		internal static Func<float, float> GetFunc(EasingType type)
		{
			switch (type)
			{
			case EasingType.Linear:
				return null;
			case EasingType.SineIn:
				return new Func<float, float>(EasingFunctions.SineIn);
			case EasingType.SineOut:
				return new Func<float, float>(EasingFunctions.SineOut);
			case EasingType.SineInOut:
				return new Func<float, float>(EasingFunctions.SineInOut);
			case EasingType.QuadIn:
				return new Func<float, float>(EasingFunctions.QuadIn);
			case EasingType.QuadOut:
				return new Func<float, float>(EasingFunctions.QuadOut);
			case EasingType.QuadInOut:
				return new Func<float, float>(EasingFunctions.QuadInOut);
			case EasingType.CubicIn:
				return new Func<float, float>(EasingFunctions.CubicIn);
			case EasingType.CubicOut:
				return new Func<float, float>(EasingFunctions.CubicOut);
			case EasingType.CubicInOut:
				return new Func<float, float>(EasingFunctions.CubicInOut);
			case EasingType.QuartIn:
				return new Func<float, float>(EasingFunctions.QuartIn);
			case EasingType.QuartOut:
				return new Func<float, float>(EasingFunctions.QuartOut);
			case EasingType.QuartInOut:
				return new Func<float, float>(EasingFunctions.QuartInOut);
			case EasingType.QuintIn:
				return new Func<float, float>(EasingFunctions.QuintIn);
			case EasingType.QuintOut:
				return new Func<float, float>(EasingFunctions.QuintOut);
			case EasingType.QuintInOut:
				return new Func<float, float>(EasingFunctions.QuintInOut);
			case EasingType.ExpoIn:
				return new Func<float, float>(EasingFunctions.ExpoIn);
			case EasingType.ExpoOut:
				return new Func<float, float>(EasingFunctions.ExpoOut);
			case EasingType.ExpoInOut:
				return new Func<float, float>(EasingFunctions.ExpoInOut);
			case EasingType.CircIn:
				return new Func<float, float>(EasingFunctions.CircIn);
			case EasingType.CircOut:
				return new Func<float, float>(EasingFunctions.CircOut);
			case EasingType.CircInOut:
				return new Func<float, float>(EasingFunctions.CircInOut);
			case EasingType.BackIn:
				return new Func<float, float>(EasingFunctions.BackIn);
			case EasingType.BackOut:
				return new Func<float, float>(EasingFunctions.BackOut);
			case EasingType.BackInOut:
				return new Func<float, float>(EasingFunctions.BackInOut);
			}
			return null;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000DEE67 File Offset: 0x000DD067
		private static float SineIn(float t)
		{
			return 1f - Mathf.Cos(t * 3.1415927f / 2f);
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000DEE81 File Offset: 0x000DD081
		private static float SineOut(float t)
		{
			return Mathf.Cos(t * 3.1415927f / 2f);
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000DEE95 File Offset: 0x000DD095
		private static float SineInOut(float t)
		{
			return -(Mathf.Cos(3.1415927f * t) - 1f) / 2f;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x000DEEB0 File Offset: 0x000DD0B0
		private static float QuadIn(float t)
		{
			return t * t;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000DEEB5 File Offset: 0x000DD0B5
		private static float QuadOut(float t)
		{
			return 1f - (1f - t) * (1f - t);
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000DEECC File Offset: 0x000DD0CC
		private static float QuadInOut(float t)
		{
			if (t >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
			}
			return 2f * t * t;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000DEF03 File Offset: 0x000DD103
		private static float CubicIn(float t)
		{
			return t * t * t;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000DEF0A File Offset: 0x000DD10A
		private static float CubicOut(float t)
		{
			return 1f - Mathf.Pow(1f - t, 3f);
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000DEF23 File Offset: 0x000DD123
		private static float CubicInOut(float t)
		{
			if (t >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
			}
			return 4f * t * t * t;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000DEF5C File Offset: 0x000DD15C
		private static float QuartIn(float t)
		{
			return t * t * t * t;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000DEF65 File Offset: 0x000DD165
		private static float QuartOut(float t)
		{
			return 1f - Mathf.Pow(1f - t, 4f);
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000DEF7E File Offset: 0x000DD17E
		private static float QuartInOut(float t)
		{
			if (t >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 4f) / 2f;
			}
			return 8f * t * t * t * t;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000DEFB9 File Offset: 0x000DD1B9
		private static float QuintIn(float t)
		{
			return t * t * t * t * t;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000DEFC4 File Offset: 0x000DD1C4
		private static float QuintOut(float t)
		{
			return 1f - Mathf.Pow(1f - t, 5f);
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000DEFDD File Offset: 0x000DD1DD
		private static float QuintInOut(float t)
		{
			if (t >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f;
			}
			return 16f * t * t * t * t * t;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000DF01A File Offset: 0x000DD21A
		private static float ExpoIn(float t)
		{
			if (!Mathf.Approximately(t, 0f))
			{
				return Mathf.Pow(2f, 10f * t - 10f);
			}
			return 0f;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000DF046 File Offset: 0x000DD246
		private static float ExpoOut(float t)
		{
			if (!Mathf.Approximately(t, 1f))
			{
				return Mathf.Pow(2f, -10f * t);
			}
			return 1f;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000DF06C File Offset: 0x000DD26C
		private static float ExpoInOut(float t)
		{
			if (Mathf.Approximately(t, 0f))
			{
				return 0f;
			}
			if (Mathf.Approximately(t, 1f))
			{
				return 1f;
			}
			if (t >= 0.5f)
			{
				return (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f;
			}
			return Mathf.Pow(2f, 20f * t - 10f) / 2f;
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000DF0E8 File Offset: 0x000DD2E8
		private static float CircIn(float t)
		{
			return 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2f));
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000DF106 File Offset: 0x000DD306
		private static float CircOut(float t)
		{
			return Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000DF124 File Offset: 0x000DD324
		private static float CircInOut(float t)
		{
			if (t >= 0.5f)
			{
				return (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
			}
			return (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * t, 2f))) / 2f;
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000DF190 File Offset: 0x000DD390
		private static float BackIn(float t)
		{
			return 2.70158f * t * t * t - 1.70158f * t * t;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000DF1A7 File Offset: 0x000DD3A7
		private static float BackOut(float t)
		{
			return 1f + 2.70158f * Mathf.Pow(t - 1f, 3f) + 2.70158f * Mathf.Pow(t - 1f, 2f);
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000DF1E0 File Offset: 0x000DD3E0
		private static float BackInOut(float t)
		{
			if (t >= 0.5f)
			{
				return (Mathf.Pow(2f * t - 2f, 2f) * (3.5949094f * (t * 2f - 2f) + 2.5949094f) + 2f) / 2f;
			}
			return Mathf.Pow(2f * t, 2f) * (7.189819f * t - 2.5949094f) / 2f;
		}

		// Token: 0x04001EDB RID: 7899
		private const float c1 = 1.70158f;

		// Token: 0x04001EDC RID: 7900
		private const float c2 = 2.5949094f;

		// Token: 0x04001EDD RID: 7901
		private const float c3 = 2.70158f;
	}
}
