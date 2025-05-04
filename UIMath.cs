using System;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001F8 RID: 504
	public static class UIMath
	{
		// Token: 0x06001CE8 RID: 7400 RVA: 0x000D8A6D File Offset: 0x000D6C6D
		public static Quaternion DirectionToRotation(Vector2 dir)
		{
			return Quaternion.Euler(0f, 0f, -UIMath.DirectionToAngleDegrees(dir, true));
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000D8A86 File Offset: 0x000D6C86
		public static float DirectionToAngleDegrees(Vector2 direction, bool positiveRange = true)
		{
			if (!positiveRange)
			{
				return UIMath.DirectionToAngle(direction) * 57.29578f;
			}
			return UIMath.Mod(UIMath.DirectionToAngle(direction) * 57.29578f, 360f);
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000D8AAE File Offset: 0x000D6CAE
		public static float DirectionToAngle(Vector2 direction)
		{
			return Mathf.Atan2(direction.normalized.x, direction.normalized.y);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000D8ACD File Offset: 0x000D6CCD
		public static Quaternion AngleToRotation(float angle)
		{
			if (angle <= 360f)
			{
				return Quaternion.AngleAxis(-angle, Vector3.forward);
			}
			return Quaternion.AngleAxis(-UIMath.Mod(angle, 360f), Vector3.forward);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000D8AFC File Offset: 0x000D6CFC
		public static Vector2 AngleToDirection(float angle)
		{
			return new Vector2((float)Mathf.RoundToInt(Mathf.Sin(angle * 0.017453292f) * 100f) / 100f, (float)Mathf.RoundToInt(Mathf.Cos(angle * 0.017453292f) * 100f) / 100f);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000D8B4A File Offset: 0x000D6D4A
		public static float AngleToPos(float angle, int layer)
		{
			if (!Mathf.Approximately(angle, 360f))
			{
				return 360f * (float)Mathf.Max(layer, 0) + angle;
			}
			return 360f * (float)Mathf.Max(layer, 0);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000D8B78 File Offset: 0x000D6D78
		public static int AngleToRadialPoint(float angle, float segmentSize)
		{
			int num = (int)Math.Round((double)(UIMath.Mod(angle, 360f) / segmentSize), MidpointRounding.AwayFromZero);
			if (!Mathf.Approximately((float)num, 360f / segmentSize))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000D8BB0 File Offset: 0x000D6DB0
		public static int RadialDirection(Vector2 from, Vector2 to)
		{
			if (from == Vector2.zero || to == Vector2.zero)
			{
				return 0;
			}
			if (from.x * to.y - from.y * from.x >= 0f)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000D8C00 File Offset: 0x000D6E00
		public static float Mod(float dividend, float divisor)
		{
			float num = dividend % divisor;
			if (num >= 0f)
			{
				return num;
			}
			return num + Mathf.Abs(divisor);
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000D8C24 File Offset: 0x000D6E24
		public static float RadialDelta(float p1, float p2)
		{
			float num = Mathf.Abs(UIMath.Mod(p1, 360f) - UIMath.Mod(p2, 360f));
			if (num <= 180f)
			{
				return num;
			}
			return 360f - num;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000D8C5F File Offset: 0x000D6E5F
		public static float ClosestSegment(float value, float segmentSize)
		{
			return Mathf.Round(value / segmentSize) * segmentSize;
		}
	}
}
