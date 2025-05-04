using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x020001F9 RID: 505
	[Serializable]
	public class RadialArray<T>
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x000D8C6B File Offset: 0x000D6E6B
		public T[] GetArray { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000D8C73 File Offset: 0x000D6E73
		public List<T> GetAsList
		{
			get
			{
				return this.GetArray.ToList<T>();
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000D8C80 File Offset: 0x000D6E80
		public int Count { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x000D8C88 File Offset: 0x000D6E88
		public int MaxIndex { get; }

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000D8C90 File Offset: 0x000D6E90
		public RadialArray(List<T> elements, int elementsPerLayer) : this(elements.ToArray(), elementsPerLayer)
		{
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000D8C9F File Offset: 0x000D6E9F
		public RadialArray(T[] elements, int elementsPerLayer)
		{
			this.GetArray = elements;
			this.Count = elements.Length;
			this.MaxIndex = this.Count - 1;
			this._segmentSize = Mathf.Round(360f / (float)elementsPerLayer);
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000D8CD8 File Offset: 0x000D6ED8
		public T GetElement(float cursorPos)
		{
			return this.GetElement(this.IndexOf(cursorPos));
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000D8CE8 File Offset: 0x000D6EE8
		public T GetElement(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.GetArray[index];
			}
			return default(T);
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000D8D18 File Offset: 0x000D6F18
		public List<T> GetVisisbleElementsAt(float cursorPos, int fadeCount, int showCount, bool doubleSided = true)
		{
			return this.GetVisisbleElementsAt(this.IndexOf(cursorPos), fadeCount, showCount, doubleSided);
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000D8D2C File Offset: 0x000D6F2C
		public List<T> GetVisisbleElementsAt(int index, int fadeCount, int showCount, bool doubleSided = true)
		{
			List<T> list = new List<T>();
			for (int i = doubleSided ? (index - fadeCount - showCount) : (index - fadeCount); i <= index + showCount + fadeCount; i++)
			{
				if (i >= 0)
				{
					if (i >= this.Count)
					{
						break;
					}
					list.Add(this.GetArray[i]);
				}
			}
			return list;
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000D8D7C File Offset: 0x000D6F7C
		public bool IsVisible(float pos, int fadeIndex, int fadeCount, int showCount, bool doubleSided = true)
		{
			return this.IsVisible(this.GetElement(pos), fadeIndex, fadeCount, showCount, doubleSided);
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000D8D91 File Offset: 0x000D6F91
		public bool IsVisible(int index, float fadePos, int fadeCount, int showCount, bool doubleSided = true)
		{
			return this.IsVisible(index, this.IndexOf(fadePos), fadeCount, showCount, doubleSided);
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000D8DA6 File Offset: 0x000D6FA6
		public bool IsVisible(int index, int fadeIndex, int fadeCount, int showCount, bool doubleSided = true)
		{
			return this.IsVisible(this.GetElement(index), fadeIndex, fadeCount, showCount, doubleSided);
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000D8DBB File Offset: 0x000D6FBB
		public bool IsVisible(T element, int fadeIndex, int fadeCount, int showCount, bool doubleSided = true)
		{
			return this.GetVisisbleElementsAt(fadeIndex, fadeCount, showCount, doubleSided).Contains(element);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000D8DCF File Offset: 0x000D6FCF
		public int ViableIndex(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return index;
			}
			return -1;
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000D8DE1 File Offset: 0x000D6FE1
		public int IndexOf(float position)
		{
			return Mathf.FloorToInt(UIMath.ClosestSegment(position, this._segmentSize) / this._segmentSize);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000D8DFB File Offset: 0x000D6FFB
		public int IndexOf(T element)
		{
			return Array.IndexOf<T>(this.GetArray, element);
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000D8E0C File Offset: 0x000D700C
		public int BackButtonIndex()
		{
			if (!this.GetArray.Any((T e) => e is BackElement))
			{
				return -1;
			}
			return Array.IndexOf<T>(this.GetArray, this.GetArray.First((T e) => e is BackElement));
		}

		// Token: 0x04001DEC RID: 7660
		private float _segmentSize;
	}
}
