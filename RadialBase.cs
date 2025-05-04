using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.UI
{
	// Token: 0x0200020F RID: 527
	public class RadialBase : MonoBehaviour
	{
		// Token: 0x06001D7C RID: 7548 RVA: 0x000DB03C File Offset: 0x000D923C
		private void Awake()
		{
			RectTransform rectTransform;
			if (this.m_cursor.transform.GetChild(0).TryGetComponent<RectTransform>(out rectTransform))
			{
				rectTransform.localPosition = Vector3.up * RadialData.SO.CursorDistance;
			}
			this.m_cursorID = this.m_cursor.gameObject.GetInstanceID().ToString();
			this.Active = false;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000DB0A4 File Offset: 0x000D92A4
		public void QueuedOpen(IRadialConfig config, IRadialConfig backConfig = null)
		{
			this.m_queuedOpenConfigs = new ValueTuple<IRadialConfig, IRadialConfig>(config, backConfig);
			if (this.IsRefresh || config is ThrowGroupConfig || this.StartItemIndex >= 0 || !this.m_selected)
			{
				return;
			}
			if (RadialData.SO.DefaultToBackButtonOnNewPage && this.m_previousInput == Vector2.zero)
			{
				return;
			}
			this.StartItemIndex = this.m_elements.IndexOf(this.m_selected) + this.StartOffset;
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000DB122 File Offset: 0x000D9322
		public void Open(IRadialConfig config, IRadialConfig backConfig = null)
		{
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x000DB124 File Offset: 0x000D9324
		public void Refresh()
		{
			if (this.StartItemIndex <= 0 && this.m_selected)
			{
				this.StartItemIndex = this.m_elements.IndexOf(this.m_selected);
			}
			this.IsRefresh = true;
			this.Deselect();
			this.QueuedOpen(this.CurrentConfig, null);
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000DB178 File Offset: 0x000D9378
		private void LoadSettings()
		{
			this.SetXYControls();
			this.SetItemInteractionControls();
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x000DB188 File Offset: 0x000D9388
		public void ConstructRadial(List<RadialMenuElement> elements)
		{
			this.ClearElements();
			List<RadialMenuElement> list = elements ?? new List<RadialMenuElement>();
			if (!this.IsHoverMenu)
			{
				if (this.m_backConfigs.Count <= 0)
				{
					goto IL_5C;
				}
				if (!list.All((RadialMenuElement e) => !(e is BackElement)))
				{
					goto IL_5C;
				}
			}
			this.InsertBackElement(list, 0, this.IsThrowMenu);
			IL_5C:
			int count = list.Count;
			if (!this.IsThrowMenu && (RadialData.SO.ReSizeOnRefresh || !this.IsRefresh))
			{
				this.SetElementsPerLayer(count);
			}
			if (RadialData.SO.UsePersistantBackBtn)
			{
				this.AddPersistentBackButton(list, this.IsThrowMenu);
			}
			this.StartOffset = ((this.StartOffset != 0) ? ((int)UIMath.Mod((float)this.StartOffset, (float)this.MaxElementsPerLayer)) : 0);
			if (count < this.MaxElementsPerLayer)
			{
				this.CenterBackButton(count, this.IsThrowMenu, list);
			}
			if (!this.IsRefresh && this.StartOffset < 0 && this.StartItemIndex >= 0 && !(this.CurrentConfig is ThrowGroupConfig))
			{
				this.StartItemIndex = (int)UIMath.Mod((float)(this.StartItemIndex + -(float)this.StartOffset), (float)this.MaxElementsPerLayer);
			}
			this.m_nrOfLayers = ((count <= this.MaxElementsPerLayer) ? 0 : Mathf.FloorToInt((float)count / (float)this.MaxElementsPerLayer));
			this.m_radialHasItemElements = list.Any((RadialMenuElement e) => e is ItemElement);
			this.m_elements = new RadialArray<RadialMenuElement>(list, this.MaxElementsPerLayer);
			this.SetRadialLayout();
			this.SelectStartElement();
			this.UpdateItemElementVisuals();
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000DB32C File Offset: 0x000D952C
		private void SetElementsPerLayer(int count)
		{
			int num = RadialData.SO.MaxElementsRange[0];
			for (int i = 0; i < RadialData.SO.MaxElementsRange.Length - 1; i++)
			{
				if (count > RadialData.SO.MaxElementsRange[i])
				{
					num = RadialData.SO.MaxElementsRange[i + 1];
				}
			}
			if (num == this.MaxElementsPerLayer)
			{
				return;
			}
			if (this.m_segmentSize == 0f)
			{
				this.m_segmentSize = (float)Mathf.RoundToInt(360f / (float)((this.MaxElementsPerLayer > 0) ? this.MaxElementsPerLayer : num));
			}
			if (this.StartItemIndex > 0 && !this.IsHoverMenu)
			{
				this.StartItemIndex = Mathf.RoundToInt((float)this.StartItemIndex * this.m_segmentSize);
			}
			this.MaxElementsPerLayer = num;
			this.m_segmentSize = (float)Mathf.RoundToInt(360f / (float)this.MaxElementsPerLayer);
			this.m_cursorThreshold = this.m_segmentSize * RadialData.SO.CursorSensitivity;
			this.m_layerFadeDistance = this.m_segmentSize * (float)RadialData.SO.LayerFadeCount;
			this.m_layerShowDistance = this.m_segmentSize * (float)RadialData.SO.LayerShowCount;
			this.m_layerFadeThreshold = (float)((RadialData.SO.LayerShowCount - 1 + RadialData.SO.LayerFadeCount) * 2) * this.m_segmentSize;
			if (this.StartItemIndex > 0 && !this.IsHoverMenu)
			{
				this.StartItemIndex = Mathf.RoundToInt((float)this.StartItemIndex / this.m_segmentSize);
			}
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000DB49C File Offset: 0x000D969C
		private void SetRadialLayout()
		{
			for (int i = 0; i < this.m_elements.Count; i++)
			{
				RadialMenuElement element = this.m_elements.GetElement(i);
				element.transform.SetParent(this.m_elementContainer);
				element.transform.localScale = Vector3.one;
				element.gameObject.SetActive(true);
				Vector2 a = UIMath.AngleToDirection(UIMath.Mod((float)(i + this.StartOffset) * this.m_segmentSize, 360f));
				element.LocalPosition = a * RadialData.SO.ElementsDistance;
				element.Alpha = ((this.m_nrOfLayers > 0) ? 0f : 1f);
				element.SetSegment(this.MaxElementsPerLayer);
				if (element is BackElement)
				{
					element.ElementTransform.GetChild(1).localRotation = UIMath.DirectionToRotation(element.LocalPosition.normalized);
				}
			}
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000DB594 File Offset: 0x000D9794
		private void SelectStartElement()
		{
			if (this.StartItemIndex > 0 && this.m_elements.ViableIndex(this.StartItemIndex) == -1)
			{
				this.StartItemIndex = this.m_elements.MaxIndex;
			}
			else if (!this.IsRefresh && this.StartItemIndex <= 0 && ((RadialData.SO.DefaultToBackButtonOnNewPage && this.m_previousInput == Vector2.zero) || this.CurrentConfig is ThrowGroupConfig))
			{
				this.StartItemIndex = this.m_elements.BackButtonIndex();
			}
			this.m_currentLayer = ((this.StartItemIndex > 0) ? Mathf.FloorToInt((float)this.StartItemIndex / (float)this.MaxElementsPerLayer) : 0);
			this.m_previousClosestPoint = (this.m_closestPoint = ((this.StartItemIndex > 0) ? ((int)UIMath.Mod((float)this.StartItemIndex, (float)this.MaxElementsPerLayer)) : ((this.m_previousInput == Vector2.zero) ? 0 : ((int)UIMath.Mod((float)(UIMath.AngleToRadialPoint(this.m_inputAngle, this.m_segmentSize) - this.StartOffset), (float)this.MaxElementsPerLayer)))));
			this.UpdateSelectionIndex();
			this.m_previousCursorTarget = (this.m_cursorTarget = ((this.m_index != -1) ? UIMath.Mod((float)(this.m_index + this.StartOffset) * this.m_segmentSize, 360f) : ((this.StartItemIndex > 0) ? (UIMath.Mod((float)this.StartItemIndex, (float)this.MaxElementsPerLayer) * this.m_segmentSize) : ((float)this.m_closestPoint * this.m_segmentSize))));
			if (this.m_nrOfLayers > 0 && this.m_index != -1)
			{
				this.SetFadeAnchor((float)this.m_index * this.m_segmentSize, true);
			}
			this.UpdateSelection();
			this.UpdateCursorPosition(true);
			if (!ZInput.IsGamepadActive() && this.IsTopLevel)
			{
				ZInput.SetMousePosition(this.m_selected.ElementTransform.position);
			}
			this.StartItemIndex = -1;
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000DB784 File Offset: 0x000D9984
		private void InsertBackElement(List<RadialMenuElement> elems, int index, bool shouldOffset)
		{
			BackElement backElement = UnityEngine.Object.Instantiate<BackElement>(RadialData.SO.BackElement);
			backElement.Init(this);
			elems.Insert(shouldOffset ? (index + 1) : index, backElement);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000DB7B8 File Offset: 0x000D99B8
		private void AddPersistentBackButton(List<RadialMenuElement> elems, bool shouldOffset)
		{
			int num = Mathf.Max(Mathf.CeilToInt((float)elems.Count / (float)this.MaxElementsPerLayer), 1);
			if (num <= 1)
			{
				return;
			}
			int num2 = elems.Count + num - 1;
			while (Mathf.Max(Mathf.CeilToInt((float)num2 / (float)this.MaxElementsPerLayer), 1) != num)
			{
				num = Mathf.Max(Mathf.CeilToInt((float)num2 / (float)this.MaxElementsPerLayer), 1);
				num2 = elems.Count + num;
			}
			for (int i = 1; i < num; i++)
			{
				this.InsertBackElement(elems, i * this.MaxElementsPerLayer, shouldOffset);
			}
			if (this.m_backConfigs.Count <= 0 && num > 1 && this.StartItemIndex >= this.MaxElementsPerLayer)
			{
				this.StartItemIndex += num - 1;
			}
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000DB878 File Offset: 0x000D9A78
		private void CenterBackButton(int nrOfElements, bool isThrowConfig, List<RadialMenuElement> elems)
		{
			int num = (nrOfElements % 2 == 0) ? (nrOfElements / 2) : Mathf.FloorToInt((float)nrOfElements / 2f);
			RadialMenuElement item = isThrowConfig ? elems[1] : elems[0];
			elems.Remove(item);
			elems.Insert(num, item);
			this.StartOffset -= num;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000DB8D0 File Offset: 0x000D9AD0
		private void Update()
		{
			if (!this.Active)
			{
				return;
			}
			this.m_animationManager.Tick(Time.deltaTime);
			if (this.m_closeQueued)
			{
				this.Close(false);
				return;
			}
			if (this.m_isClosing)
			{
				return;
			}
			this.IsBlockingInput = true;
			RadialArray<RadialMenuElement> elements = this.m_elements;
			if (elements != null && elements.Count > 0)
			{
				this.HandleInput();
				this.HandleVisuals();
			}
			if (this.m_closeQueued)
			{
				this.Close(false);
				return;
			}
			ValueTuple<IRadialConfig, IRadialConfig> queuedOpenConfigs = this.m_queuedOpenConfigs;
			IRadialConfig item = queuedOpenConfigs.Item1;
			IRadialConfig item2 = queuedOpenConfigs.Item2;
			if (item != null || item2 != null)
			{
				this.Open(this.m_queuedOpenConfigs.Item1, this.m_queuedOpenConfigs.Item2);
			}
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000DB980 File Offset: 0x000D9B80
		private void HandleInput()
		{
			if (this.TryClosure() || (this.CanThrow && this.TryThrow()) || this.TryInteract())
			{
				return;
			}
			this.m_previousClosestPoint = this.m_closestPoint;
			Vector2 vector;
			if (!ZInput.IsGamepadActive())
			{
				Func<Vector2> getMouseDirection = this.GetMouseDirection;
				vector = ((getMouseDirection != null) ? getMouseDirection() : Vector2.zero);
			}
			else
			{
				Func<Vector2> getControllerDirection = this.GetControllerDirection;
				vector = ((getControllerDirection != null) ? getControllerDirection() : Vector2.zero);
			}
			Vector2 vector2 = vector;
			bool flag = vector2 != Vector2.zero;
			this.m_inputDirection = (flag ? this.m_inputDirection : 0);
			if (flag)
			{
				if (vector2.normalized != this.m_previousInput.normalized)
				{
					this.OnUpdateCursorInput(vector2);
				}
				if (this.UseHoverSelect && !this.m_animationManager.IsTweenActiveWithEndAction(this.m_selected.ID + "_hov"))
				{
					this.m_selected.StartHoverSelect(this.m_animationManager, RadialData.SO.HoverSelectSpeed, RadialData.SO.HoverSelectEasingType, new Action(this.OnInteract));
				}
			}
			else if (this.UseHoverSelect && this.m_animationManager.IsTweenActiveWithEndAction(this.m_selected.ID + "_hov"))
			{
				this.m_selected.ResetHoverSelect(this.m_animationManager, RadialData.SO.HoverSelectSpeed, RadialData.SO.HoverSelectEasingType);
			}
			if (this.m_reFade && (!this.m_selected || !this.m_animationManager.IsTweenActive(this.m_selected.ID)))
			{
				this.m_reFade = false;
			}
			if (this.CanDoubleClick)
			{
				Func<bool> getDoubleTap = this.GetDoubleTap;
				if (getDoubleTap != null && getDoubleTap())
				{
					goto IL_1C5;
				}
			}
			if (this.CanFlick)
			{
				Func<bool> getFlick = this.GetFlick;
				if (getFlick != null && getFlick())
				{
					goto IL_1C5;
				}
			}
			this.m_previousInput = (flag ? vector2 : Vector2.zero);
			return;
			IL_1C5:
			this.OnInteract();
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000DBB6A File Offset: 0x000D9D6A
		private void HandleVisuals()
		{
			if (this.m_radialHasItemElements)
			{
				this.UpdateItemElementVisuals();
			}
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000DBB7C File Offset: 0x000D9D7C
		private void OnUpdateCursorInput(Vector2 newInput)
		{
			this.m_cursor.gameObject.SetActive(true);
			this.m_inputAngle = UIMath.DirectionToAngleDegrees(newInput.normalized, true);
			float num = Mathf.Floor(UIMath.RadialDelta(this.m_cursorTarget, this.m_inputAngle));
			if (ZInput.IsGamepadActive() && num < ((this.m_index == -1) ? 5f : ((this.m_previousInput == Vector2.zero && this.MaxElementsPerLayer > 6) ? (this.m_segmentSize * 2f) : this.m_cursorThreshold)))
			{
				return;
			}
			this.m_inputDirection = UIMath.RadialDirection(this.m_previousInput.normalized, newInput.normalized);
			this.m_closestPoint = (int)UIMath.Mod((float)(UIMath.AngleToRadialPoint(this.m_inputAngle, this.m_segmentSize) - this.StartOffset), (float)this.MaxElementsPerLayer);
			if (this.m_closestPoint != this.m_previousClosestPoint)
			{
				this.UpdateSelectionIndex();
				this.UpdateSelection();
			}
			this.m_previousCursorTarget = this.m_cursorTarget;
			this.m_cursorTarget = ((this.m_index == -1) ? this.m_inputAngle : UIMath.ClosestSegment(this.m_inputAngle, this.m_segmentSize));
			if (!Mathf.Approximately(this.m_previousCursorTarget, this.m_cursorTarget))
			{
				this.UpdateCursorPosition(false);
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000DBCC0 File Offset: 0x000D9EC0
		private void OnInteract()
		{
			this.IsBlockingInput = !this.IsHoverMenu;
			if (this.IsBlockingInput)
			{
				Action<float> onInteractionDelay = this.OnInteractionDelay;
				if (onInteractionDelay != null)
				{
					onInteractionDelay(RadialData.SO.InteractionDelay);
				}
			}
			if (this.IsHoverMenu && this.m_selected is ItemElement)
			{
				ItemElement itemElement = this.m_selected as ItemElement;
				bool flag;
				if (itemElement == null)
				{
					flag = false;
				}
				else
				{
					Func<GameObject, bool> hoverMenuInteract = itemElement.HoverMenuInteract;
					bool? flag2 = (hoverMenuInteract != null) ? new bool?(hoverMenuInteract(this.HoverObject)) : null;
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					this.<OnInteract>g__OnSuccessfulInteract|16_0();
					return;
				}
			}
			if (this.m_selected)
			{
				Func<bool> interact = this.m_selected.Interact;
				if (interact != null && interact())
				{
					this.<OnInteract>g__OnSuccessfulInteract|16_0();
					return;
				}
			}
			this.QueuedClose();
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000DBDA4 File Offset: 0x000D9FA4
		private bool TryInteract()
		{
			if (!this.m_selected)
			{
				return false;
			}
			Func<bool> getSingleUse = this.GetSingleUse;
			if (getSingleUse != null && getSingleUse())
			{
				if (this.m_previousInput != Vector2.zero)
				{
					this.OnInteract();
				}
				this.QueuedClose();
				return true;
			}
			Func<bool> getConfirm = this.GetConfirm;
			if (getConfirm == null || !getConfirm())
			{
				return false;
			}
			this.OnInteract();
			return true;
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000DBE14 File Offset: 0x000DA014
		private bool TryClosure()
		{
			if (!this.m_localPlayerRef || this.m_localPlayerRef.IsTeleporting() || this.m_localPlayerRef.IsSleeping())
			{
				this.Close(false);
				return true;
			}
			if (this.IsHoverMenu && this.m_localPlayerRef && this.m_localPlayerRef.GetHoverObject() != this.HoverObject)
			{
				this.QueuedClose();
				return true;
			}
			Func<bool> getBack = this.GetBack;
			if (getBack != null && getBack())
			{
				this.Back();
				return true;
			}
			Func<bool> getClose = this.GetClose;
			if (getClose != null && getClose())
			{
				this.QueuedClose();
				return true;
			}
			return false;
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x000DBEC0 File Offset: 0x000DA0C0
		private bool TryThrow()
		{
			if (!this.m_selected)
			{
				return false;
			}
			Func<bool> getOpenThrowMenu = this.GetOpenThrowMenu;
			if (getOpenThrowMenu != null && getOpenThrowMenu() && this.m_selected.TryOpenSubRadial != null)
			{
				return this.m_selected.TryOpenSubRadial(this, this.m_elements.IndexOf(this.m_selected));
			}
			Func<bool> getThrow = this.GetThrow;
			if (getThrow == null || !getThrow())
			{
				return false;
			}
			Action<float> onInteractionDelay = this.OnInteractionDelay;
			if (onInteractionDelay != null)
			{
				onInteractionDelay(RadialData.SO.InteractionDelay);
			}
			Func<bool> secondaryInteract = this.m_selected.SecondaryInteract;
			if (secondaryInteract != null && secondaryInteract())
			{
				ItemElement itemElement = this.LastUsed as ItemElement;
				if (itemElement != null && !this.m_localPlayerRef.GetInventory().ContainsItem(itemElement.m_data))
				{
					this.LastUsed = null;
				}
				this.Refresh();
			}
			return true;
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x000DBFA4 File Offset: 0x000DA1A4
		private void UpdateLayer()
		{
			if (!RadialData.SO.ReFadeAtMidnight && this.m_cursorLocked)
			{
				return;
			}
			if (this.m_inputDirection > 0 && this.m_closestPoint < this.m_previousClosestPoint)
			{
				this.OnNextLayer();
				return;
			}
			if (this.m_inputDirection < 0 && this.m_closestPoint > this.m_previousClosestPoint)
			{
				this.OnPreviousLayer();
				return;
			}
			if (this.m_inputDirection == 0 && this.m_currentLayer == this.m_nrOfLayers && this.m_elements.ViableIndex((int)UIMath.Mod((float)this.m_closestPoint, (float)this.MaxElementsPerLayer) + this.MaxElementsPerLayer * this.m_currentLayer) == -1)
			{
				this.OnPreviousLayer();
			}
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000DC050 File Offset: 0x000DA250
		private void OnNextLayer()
		{
			if (this.m_currentLayer == this.m_nrOfLayers)
			{
				if (RadialData.SO.ReFadeAtMidnight)
				{
					this.m_reFade = true;
				}
				else
				{
					this.m_cursorLocked = true;
				}
			}
			this.m_currentLayer = Mathf.Min(this.m_currentLayer + 1, this.m_nrOfLayers);
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000DC0A0 File Offset: 0x000DA2A0
		private void OnPreviousLayer()
		{
			if (this.m_currentLayer == 0)
			{
				if (RadialData.SO.ReFadeAtMidnight)
				{
					this.m_reFade = true;
				}
				else
				{
					this.m_cursorLocked = true;
				}
			}
			this.m_currentLayer = Mathf.Max(this.m_currentLayer - 1, 0);
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000DC0DC File Offset: 0x000DA2DC
		private void UpdateSelection()
		{
			bool flag = this.m_index == -1 || this.m_index > this.m_elements.MaxIndex;
			if (!flag && this.m_cursorLocked && !this.m_reFade && this.m_elements.GetElement(this.m_index).Alpha == 0f)
			{
				flag = true;
			}
			if ((!flag && this.m_selected == this.m_elements.GetElement(this.m_index)) || (flag && !this.m_selected))
			{
				return;
			}
			this.Selected = (flag ? null : this.m_elements.GetElement(this.m_index));
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000DC18A File Offset: 0x000DA38A
		private void Deselect()
		{
			if (!this.m_selected)
			{
				return;
			}
			this.Selected = null;
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000DC1A4 File Offset: 0x000DA3A4
		private void OnSelectedUpdate(RadialMenuElement oldSelected, RadialMenuElement newSelected)
		{
			this.CanThrow = false;
			if (oldSelected)
			{
				oldSelected.Selected = false;
				this.m_highlighter.SetParent(base.transform);
				GroupElement groupElement = oldSelected as GroupElement;
				if (groupElement != null)
				{
					groupElement.ChangeToDeselectColor();
				}
				if (this.ShouldNudgeSelected)
				{
					oldSelected.ResetNudge(this.m_animationManager, RadialData.SO.ElementsDistance, RadialData.SO.NudgeDuration * 0.75f, RadialData.SO.NudgeEasingType);
				}
				if (this.UseHoverSelect && this.m_animationManager.IsTweenActiveWithEndAction(oldSelected.ID + "_hov"))
				{
					oldSelected.ResetHoverSelect(this.m_animationManager, RadialData.SO.HoverSelectSpeed, RadialData.SO.HoverSelectEasingType);
				}
			}
			if (newSelected)
			{
				newSelected.Selected = true;
				this.m_elementInfo.Set(newSelected, this.m_animationManager);
				this.m_highlighter.SetParent(newSelected.transform);
				Vector3 normalized = newSelected.LocalPosition.normalized;
				float num = UIMath.DirectionToAngleDegrees(normalized, true);
				this.m_highlighter.localRotation = Quaternion.Euler(0f, 0f, -num);
				this.m_highlighter.localPosition = normalized * RadialData.SO.OrnamentOffset;
				GroupElement groupElement2 = newSelected as GroupElement;
				if (groupElement2 != null)
				{
					groupElement2.ChangeToSelectColor();
				}
				if (this.UseHoverSelect && !(newSelected is EmptyElement))
				{
					newSelected.StartHoverSelect(this.m_animationManager, RadialData.SO.HoverSelectSpeed, RadialData.SO.HoverSelectEasingType, new Action(this.OnInteract));
				}
				if (this.ShouldNudgeSelected && !this.m_shouldAnimateIn && !this.m_isAnimating)
				{
					newSelected.StartNudge(this.m_animationManager, RadialData.SO.ElementsDistance + RadialData.SO.NudgeDistance, RadialData.SO.NudgeDuration, RadialData.SO.NudgeEasingType);
				}
				if (this.m_nrOfLayers > 0)
				{
					if (this.m_cursorLocked && !RadialData.SO.ReFadeAtMidnight)
					{
						this.m_reFade = true;
					}
					this.m_cursorLocked = false;
					this.SetFadeAnchor((float)this.m_index * this.m_segmentSize, false);
				}
			}
			if (newSelected)
			{
				this.m_elementInfo.Set(newSelected, this.m_animationManager);
			}
			else
			{
				this.m_elementInfo.Set(this.CurrentConfig, this.m_isAnimating);
			}
			this.m_highlighter.gameObject.SetActive(newSelected);
			this.m_cursor.gameObject.SetActive(this.m_previousInput != Vector2.zero || newSelected);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000DC440 File Offset: 0x000DA640
		private void UpdateSelectionIndex()
		{
			if (this.m_nrOfLayers > 0)
			{
				int num = (int)UIMath.Mod((float)this.m_closestPoint, (float)this.MaxElementsPerLayer) + this.MaxElementsPerLayer * this.m_currentLayer;
				bool cursorLocked = this.m_cursorLocked;
				this.UpdateLayer();
				if (!this.m_cursorLocked && this.m_currentLayer == this.m_nrOfLayers && !this.m_elements.GetElement(num))
				{
					this.m_cursorLocked = (UIMath.AngleToPos((float)this.m_closestPoint * this.m_segmentSize, this.m_currentLayer) > (float)this.m_elements.MaxIndex * this.m_segmentSize);
					if (this.m_cursorLocked && Mathf.Approximately((float)this.m_elements.MaxIndex * this.m_segmentSize, 360f * (float)this.m_nrOfLayers - this.m_segmentSize))
					{
						this.m_currentLayer--;
					}
				}
				if (!cursorLocked && this.m_cursorLocked)
				{
					this.SetFadeAnchor((this.m_currentLayer == 0) ? 0f : ((float)this.m_elements.MaxIndex * this.m_segmentSize), false);
				}
				if (cursorLocked)
				{
					RadialMenuElement radialMenuElement = this.m_elements.GetElement(num);
					if (!radialMenuElement || radialMenuElement.Alpha == 0f)
					{
						radialMenuElement = (this.m_elements.GetElement(num + this.MaxElementsPerLayer) ?? this.m_elements.GetElement(num - this.MaxElementsPerLayer));
						if (radialMenuElement && this.m_elements.GetVisisbleElementsAt(this.m_fadeAnchor, RadialData.SO.LayerFadeCount, RadialData.SO.LayerShowCount, true).Contains(radialMenuElement))
						{
							if (this.m_elements.IndexOf(radialMenuElement) > num)
							{
								this.OnNextLayer();
							}
							else
							{
								this.OnPreviousLayer();
							}
						}
					}
					if (this.m_cursorLocked)
					{
						this.UpdateElementsScale();
					}
				}
			}
			this.m_index = this.m_elements.ViableIndex((int)UIMath.Mod((float)this.m_closestPoint, (float)this.MaxElementsPerLayer) + this.MaxElementsPerLayer * this.m_currentLayer);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000DC64C File Offset: 0x000DA84C
		private void UpdateItemElementVisuals()
		{
			if (!this.m_localPlayerRef || !this.m_radialHasItemElements)
			{
				return;
			}
			string text;
			float progress;
			Player.MinorActionData minorActionData;
			this.m_localPlayerRef.GetActionProgress(out text, out progress, out minorActionData);
			int actionQueueCount = this.m_localPlayerRef.GetActionQueueCount();
			bool flag;
			if (minorActionData != null)
			{
				Player.MinorActionData.ActionType type = minorActionData.m_type;
				flag = (type == Player.MinorActionData.ActionType.Equip || type == Player.MinorActionData.ActionType.Unequip);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			foreach (RadialMenuElement radialMenuElement in this.m_elements.GetArray)
			{
				ItemElement itemElement = radialMenuElement as ItemElement;
				if (itemElement != null)
				{
					itemElement.UpdateDurabilityAndAmount();
					if (flag2 && minorActionData.m_item == itemElement.m_data)
					{
						itemElement.UpdateQueueAndActivation(progress, minorActionData, actionQueueCount);
					}
					else
					{
						itemElement.UpdateQueueAndActivation(this.m_localPlayerRef.IsEquipActionQueued(itemElement.m_data));
					}
					if (radialMenuElement == this.m_selected)
					{
						this.m_elementInfo.UpdateDurabilityAndWeightInfo(radialMenuElement);
					}
				}
			}
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000DC744 File Offset: 0x000DA944
		public void OnAddItem(ItemDrop.ItemData newItemData)
		{
			ItemGroupConfig itemGroupConfig = this.CurrentConfig as ItemGroupConfig;
			if (itemGroupConfig != null && itemGroupConfig.ShouldAddItem(newItemData))
			{
				itemGroupConfig.AddItem(this, newItemData, this.m_elements);
			}
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000DC778 File Offset: 0x000DA978
		private void UpdateElementsScale()
		{
			if (this.m_elements == null)
			{
				return;
			}
			if (this.m_isAnimating || this.m_shouldAnimateIn)
			{
				return;
			}
			RadialMenuElement[] getArray = this.m_elements.GetArray;
			for (int i = 0; i < getArray.Length; i++)
			{
				RadialMenuElement e = getArray[i];
				float scale = this.GetScale(e);
				if (!Mathf.Approximately(e.Scale, scale))
				{
					this.m_animationManager.StartUniqueTween<float>(() => e.Scale, delegate(float val)
					{
						e.Scale = val;
					}, e.ID + "_scale", scale, this.m_reFade ? (RadialData.SO.ElementFadeDuration * RadialData.SO.ReFadeMultiplier) : RadialData.SO.ElementFadeDuration, RadialData.SO.ElementScaleEasingType, null, null);
				}
			}
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x000DC85C File Offset: 0x000DAA5C
		private void UpdateElementsAlpha(bool instant = false)
		{
			if (this.m_elements == null)
			{
				return;
			}
			if (!instant && (this.m_isAnimating || this.m_shouldAnimateIn))
			{
				return;
			}
			List<RadialMenuElement> visisbleElementsAt = this.m_elements.GetVisisbleElementsAt(this.m_fadeAnchor, RadialData.SO.LayerFadeCount, RadialData.SO.LayerShowCount, true);
			RadialMenuElement[] getArray = this.m_elements.GetArray;
			for (int i = 0; i < getArray.Length; i++)
			{
				RadialMenuElement e = getArray[i];
				float num;
				float num2;
				bool flag;
				this.GetFadeAlphaAndScale(e, visisbleElementsAt, out num, out num2, out flag);
				if (instant || flag)
				{
					this.m_animationManager.CancelTweens(e.ID);
					this.m_animationManager.CancelTweens(e.ID + "_scale");
					e.Alpha = num;
				}
				else
				{
					this.m_animationManager.StartUniqueTween<float>(() => e.Alpha, delegate(float val)
					{
						e.Alpha = val;
					}, e.ID, num, this.m_reFade ? (RadialData.SO.ElementFadeDuration * RadialData.SO.ReFadeMultiplier) : RadialData.SO.ElementFadeDuration, RadialData.SO.ElementFadeEasingType, null, null);
				}
				if (!Mathf.Approximately(e.Scale, num2))
				{
					if (instant)
					{
						e.Scale = num2;
					}
					else if (e == this.m_selected)
					{
						this.m_animationManager.StartUniqueTween<float>(() => e.Scale, delegate(float val)
						{
							e.Scale = val;
						}, e.ID + "_scale", num2, this.m_reFade ? (RadialData.SO.ElementFadeDuration * RadialData.SO.ReFadeMultiplier) : RadialData.SO.ElementFadeDuration, RadialData.SO.ElementScaleEasingType, null, delegate()
						{
							this.m_highlighter.localScale = Vector3.one;
						});
					}
					else
					{
						this.m_animationManager.StartUniqueTween<float>(() => e.Scale, delegate(float val)
						{
							e.Scale = val;
						}, e.ID + "_scale", num2, this.m_reFade ? (RadialData.SO.ElementFadeDuration * RadialData.SO.ReFadeMultiplier) : RadialData.SO.ElementFadeDuration, RadialData.SO.ElementScaleEasingType, null, null);
					}
				}
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000DCACC File Offset: 0x000DACCC
		private void GetFadeAlphaAndScale(RadialMenuElement e, List<RadialMenuElement> visibleElements, out float alpha, out float scale, out bool shouldBeInstant)
		{
			alpha = 1f;
			scale = 1f;
			shouldBeInstant = (e == this.m_selected);
			float num = (float)this.m_elements.IndexOf(e) * this.m_segmentSize;
			scale = this.GetScale(e, num);
			if (shouldBeInstant)
			{
				return;
			}
			float num2 = Mathf.Abs(num - this.m_fadeAnchor);
			if (!visibleElements.Contains(e))
			{
				shouldBeInstant = (num2 >= this.m_layerFadeThreshold);
				alpha = 0f;
				return;
			}
			if (num2 <= this.m_layerShowDistance)
			{
				return;
			}
			float num3 = (num > this.m_fadeAnchor) ? (this.m_fadeAnchor + this.m_layerShowDistance) : (this.m_fadeAnchor - this.m_layerShowDistance);
			alpha = 1f - Mathf.Clamp01(Mathf.Abs(num - num3) / (this.m_layerFadeDistance + this.m_segmentSize));
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000DCBA0 File Offset: 0x000DADA0
		private float GetScale(RadialMenuElement e)
		{
			float ePos = (float)this.m_elements.IndexOf(e) * this.m_segmentSize;
			return this.GetScale(e, ePos);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000DCBCC File Offset: 0x000DADCC
		private float GetScale(RadialMenuElement e, float ePos)
		{
			if (RadialData.SO.SpiralEffectInsensity == SpiralEffectIntensitySetting.Off || !RadialData.SO.EnableToggleAnimation)
			{
				return 1f;
			}
			if ((!this.m_selected && !this.IsRefresh) || this.m_index == -1)
			{
				return e.Scale;
			}
			float num = (float)this.m_index * this.m_segmentSize;
			float num2 = Mathf.Abs(ePos - num);
			float num3 = (ePos > num) ? RadialData.SO.ElementScaleFactor : (1f / RadialData.SO.ElementScaleFactor);
			float num4 = Mathf.Lerp(0f, 1f, Mathf.Clamp01(num2 / this.m_layerFadeThreshold));
			float num5 = (ePos > num) ? RadialData.SO.ElementNudgeFactor : (1f / RadialData.SO.ElementNudgeFactor);
			float num6 = (num5 >= 1f) ? (1f + (num5 - 1f) * num4) : (1f - (1f - num5) * num4);
			e.StartNudge(this.m_animationManager, RadialData.SO.ElementsDistance * num6, this.m_reFade ? (RadialData.SO.ElementFadeDuration * RadialData.SO.ReFadeMultiplier) : RadialData.SO.ElementFadeDuration, RadialData.SO.ElementScaleEasingType);
			if (num3 < 1f)
			{
				return 1f - (1f - num3) * num4;
			}
			return 1f + (num3 - 1f) * num4;
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000DCD34 File Offset: 0x000DAF34
		private void SetFadeAnchor(float value, bool instant = false)
		{
			if (Mathf.Approximately(this.m_fadeAnchor, value) && !instant)
			{
				return;
			}
			this.m_fadeAnchor = value;
			this.UpdateElementsAlpha(instant);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000DCD58 File Offset: 0x000DAF58
		private void UpdateCursorPosition(bool instant = false)
		{
			if (instant)
			{
				this.m_cursor.localRotation = UIMath.AngleToRotation(this.m_cursorTarget);
				return;
			}
			this.m_animationManager.StartUniqueAngleTween(delegate(float val)
			{
				this.m_cursor.localRotation = UIMath.AngleToRotation(val);
			}, this.m_cursorID, this.m_previousCursorTarget, this.m_cursorTarget, this.m_reFade ? (RadialData.SO.CursorSpeed * RadialData.SO.ReFadeMultiplier) : RadialData.SO.CursorSpeed, RadialData.SO.CursorEasingType, null, null);
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000DCDE0 File Offset: 0x000DAFE0
		private void StartAnimatingIn()
		{
			this.m_animationManager.EndTweens("_tog");
			this.m_isAnimating = true;
			this.m_elementInfo.OpenAnimation(this.m_animationManager, "_tog", RadialData.SO.ToggleAnimDuration, RadialData.SO.ElementInfoRadius, RadialData.SO.ToggleAnimDistance, RadialData.SO.ToggleAlphaEasingType, RadialData.SO.TogglePosEasingType);
			RadialMenuElement[] getArray = this.m_elements.GetArray;
			for (int i = 0; i < getArray.Length; i++)
			{
				getArray[i].OpenAnimation(this.m_animationManager, "_tog", RadialData.SO.ToggleAnimDuration, RadialData.SO.ElementsDistance, RadialData.SO.ToggleAnimDistance, RadialData.SO.ToggleAlphaEasingType, RadialData.SO.TogglePosEasingType);
			}
			this.m_animationManager.AddEnd("_tog", new Action(this.<StartAnimatingIn>g__OnEnd|36_0));
			this.m_shouldAnimateIn = false;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000DCED0 File Offset: 0x000DB0D0
		private void StartAnimatingOut()
		{
			this.m_animationManager.EndTweens("_tog");
			this.m_elementInfo.CloseAnimation(this.m_animationManager, "_tog", RadialData.SO.ToggleAnimDuration, RadialData.SO.ElementInfoRadius, RadialData.SO.ToggleAnimDistance, RadialData.SO.ToggleAlphaEasingType, RadialData.SO.TogglePosEasingType);
			RadialMenuElement[] getArray = this.m_elements.GetArray;
			for (int i = 0; i < getArray.Length; i++)
			{
				getArray[i].CloseAnimation(this.m_animationManager, "_tog", RadialData.SO.ToggleAnimDuration, RadialData.SO.ElementsDistance, RadialData.SO.ToggleAnimDistance, RadialData.SO.ToggleAlphaEasingType, RadialData.SO.TogglePosEasingType);
			}
			this.m_animationManager.AddEnd("_tog", new Action(this.<StartAnimatingOut>g__OnEnd|37_0));
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000DCFB0 File Offset: 0x000DB1B0
		private void ClearElements()
		{
			if (this.m_elementContainer.childCount <= 0)
			{
				return;
			}
			if (this.LastUsed != null)
			{
				this.LastUsed.transform.SetParent(base.transform);
				this.LastUsed.Alpha = 0f;
			}
			this.m_highlighter.SetParent(base.transform);
			foreach (object obj in this.m_elementContainer)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000DD06C File Offset: 0x000DB26C
		private void ResetExclusiveVariables()
		{
			this.m_cursorLocked = false;
			this.m_closeQueued = false;
			this.m_reFade = false;
			this.m_fadeAnchor = 0f;
			this.StartOffset = 0;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000DD098 File Offset: 0x000DB298
		private void ResetAllVariables()
		{
			this.ResetExclusiveVariables();
			this.m_highlighter.gameObject.SetActive(false);
			this.m_cursor.gameObject.SetActive(false);
			this.m_backConfigs.Clear();
			this.HoverObject = null;
			this.CurrentConfig = null;
			this.m_selected = null;
			this.IsBlockingInput = false;
			this.IsRefresh = false;
			this.m_previousInput = Vector2.zero;
			this.StartItemIndex = -1;
			this.BackIndex = -1;
			this.m_index = -1;
			this.m_cursorTarget = 0f;
			this.m_inputAngle = 0f;
			this.UpdateCursorPosition(true);
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000DD138 File Offset: 0x000DB338
		public void Back()
		{
			this.StartItemIndex = this.BackIndex;
			this.BackIndex = -1;
			if (this.m_backConfigs.Count < 1)
			{
				this.QueuedClose();
				return;
			}
			this.QueuedOpen(this.m_backConfigs.Pop(), null);
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000DD174 File Offset: 0x000DB374
		public void QueuedClose()
		{
			this.m_isClosing = true;
			this.m_closeQueued = true;
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000DD184 File Offset: 0x000DB384
		private void InstantClose()
		{
			this.Close(true);
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000DD190 File Offset: 0x000DB390
		private void Close(bool instant = false)
		{
			if (this.m_localPlayerRef)
			{
				Player localPlayerRef = this.m_localPlayerRef;
				localPlayerRef.m_onDeath = (Action)Delegate.Remove(localPlayerRef.m_onDeath, new Action(this.InstantClose));
			}
			if (this.m_selected)
			{
				this.m_selected.Selected = false;
			}
			Func<Vector2> getControllerDirection = this.GetControllerDirection;
			Vector2 cameraDirectionLock = (getControllerDirection != null) ? getControllerDirection().normalized : Vector2.zero;
			if (ZInput.IsGamepadActive() && !cameraDirectionLock.Equals(Vector2.zero))
			{
				PlayerController.cameraDirectionLock = cameraDirectionLock;
			}
			this.m_closeQueued = false;
			this.ResetAllVariables();
			if (RadialData.SO.EnableToggleAnimation && !instant)
			{
				this.m_isClosing = true;
				this.StartAnimatingOut();
				return;
			}
			this.m_animationManager.EndAll();
			this.CanOpen = false;
			this.Active = false;
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000DD268 File Offset: 0x000DB468
		private void OnDisable()
		{
			this.m_animationManager.EndAll();
			if (this.m_localPlayerRef)
			{
				Player localPlayerRef = this.m_localPlayerRef;
				localPlayerRef.m_onDeath = (Action)Delegate.Remove(localPlayerRef.m_onDeath, new Action(this.InstantClose));
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x000DD2B4 File Offset: 0x000DB4B4
		// (set) Token: 0x06001DAB RID: 7595 RVA: 0x000DD2BC File Offset: 0x000DB4BC
		public Action<float> OnInteractionDelay { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06001DAC RID: 7596 RVA: 0x000DD2C5 File Offset: 0x000DB4C5
		// (set) Token: 0x06001DAD RID: 7597 RVA: 0x000DD2CD File Offset: 0x000DB4CD
		public Func<Vector2> GetControllerDirection { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06001DAE RID: 7598 RVA: 0x000DD2D6 File Offset: 0x000DB4D6
		// (set) Token: 0x06001DAF RID: 7599 RVA: 0x000DD2DE File Offset: 0x000DB4DE
		public Func<Vector2> GetMouseDirection { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x000DD2E7 File Offset: 0x000DB4E7
		// (set) Token: 0x06001DB1 RID: 7601 RVA: 0x000DD2EF File Offset: 0x000DB4EF
		public Func<bool> GetThrow { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x000DD2F8 File Offset: 0x000DB4F8
		// (set) Token: 0x06001DB3 RID: 7603 RVA: 0x000DD300 File Offset: 0x000DB500
		public Func<bool> GetConfirm { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x000DD309 File Offset: 0x000DB509
		// (set) Token: 0x06001DB5 RID: 7605 RVA: 0x000DD311 File Offset: 0x000DB511
		public Func<bool> GetOpenThrowMenu { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x000DD31A File Offset: 0x000DB51A
		// (set) Token: 0x06001DB7 RID: 7607 RVA: 0x000DD322 File Offset: 0x000DB522
		public Func<bool> GetBack { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x000DD32B File Offset: 0x000DB52B
		// (set) Token: 0x06001DB9 RID: 7609 RVA: 0x000DD333 File Offset: 0x000DB533
		public Func<bool> GetClose { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x000DD33C File Offset: 0x000DB53C
		// (set) Token: 0x06001DBB RID: 7611 RVA: 0x000DD344 File Offset: 0x000DB544
		public Func<bool> GetFlick { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x000DD34D File Offset: 0x000DB54D
		// (set) Token: 0x06001DBD RID: 7613 RVA: 0x000DD355 File Offset: 0x000DB555
		public Func<bool> GetDoubleTap { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06001DBE RID: 7614 RVA: 0x000DD35E File Offset: 0x000DB55E
		// (set) Token: 0x06001DBF RID: 7615 RVA: 0x000DD366 File Offset: 0x000DB566
		public Func<bool> GetSingleUse { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x000DD36F File Offset: 0x000DB56F
		// (set) Token: 0x06001DC1 RID: 7617 RVA: 0x000DD377 File Offset: 0x000DB577
		public GameObject HoverObject { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x000DD380 File Offset: 0x000DB580
		// (set) Token: 0x06001DC3 RID: 7619 RVA: 0x000DD388 File Offset: 0x000DB588
		public IRadialConfig CurrentConfig { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x000DD391 File Offset: 0x000DB591
		// (set) Token: 0x06001DC5 RID: 7621 RVA: 0x000DD39C File Offset: 0x000DB59C
		public RadialMenuElement Selected
		{
			get
			{
				return this.m_selected;
			}
			private set
			{
				if (value == this.m_selected)
				{
					return;
				}
				RadialMenuElement selected = this.m_selected;
				this.m_selected = value;
				this.OnSelectedUpdate(selected, this.m_selected);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x000DD3D3 File Offset: 0x000DB5D3
		// (set) Token: 0x06001DC7 RID: 7623 RVA: 0x000DD3DC File Offset: 0x000DB5DC
		public RadialMenuElement LastUsed
		{
			get
			{
				return this.m_lastUsed;
			}
			private set
			{
				if (this.m_lastUsed && this.m_lastUsed != value)
				{
					if (this.m_elements.GetArray.Contains(this.m_lastUsed))
					{
						this.m_lastUsed.transform.SetParent(this.m_elementContainer);
					}
					else
					{
						UnityEngine.Object.Destroy(this.m_lastUsed.gameObject);
					}
				}
				ItemElement itemElement = value as ItemElement;
				if (itemElement != null && !this.m_localPlayerRef.GetInventory().ContainsItem(itemElement.m_data))
				{
					this.m_lastUsed = null;
					return;
				}
				this.m_lastUsed = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x000DD475 File Offset: 0x000DB675
		public Vector2 InfoPosition
		{
			get
			{
				return this.m_elementInfo.InfoTransform.position;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x000DD48C File Offset: 0x000DB68C
		public bool InRadialBlockingPlaceMode
		{
			get
			{
				return this.m_localPlayerRef && this.m_localPlayerRef.InPlaceMode() && !this.m_localPlayerRef.InRepairMode();
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x000DD4B8 File Offset: 0x000DB6B8
		// (set) Token: 0x06001DCB RID: 7627 RVA: 0x000DD4C0 File Offset: 0x000DB6C0
		public bool CanOpen { get; set; } = true;

		// Token: 0x17000137 RID: 311
		// (set) Token: 0x06001DCC RID: 7628 RVA: 0x000DD4C9 File Offset: 0x000DB6C9
		public bool ShouldAnimateIn
		{
			set
			{
				this.m_shouldAnimateIn = (RadialData.SO.EnableToggleAnimation && value);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x000DD4E0 File Offset: 0x000DB6E0
		public bool ShouldNudgeSelected
		{
			get
			{
				return RadialData.SO.EnableToggleAnimation && RadialData.SO.NudgeSelectedElement && (RadialData.SO.SpiralEffectInsensity == SpiralEffectIntensitySetting.Off || this.m_nrOfLayers <= 0 || RadialData.SO.ElementScaleFactor <= 0f);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06001DCE RID: 7630 RVA: 0x000DD534 File Offset: 0x000DB734
		// (set) Token: 0x06001DCF RID: 7631 RVA: 0x000DD53C File Offset: 0x000DB73C
		public bool CanThrow { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x000DD545 File Offset: 0x000DB745
		// (set) Token: 0x06001DD1 RID: 7633 RVA: 0x000DD54D File Offset: 0x000DB74D
		public bool IsBlockingInput { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x000DD556 File Offset: 0x000DB756
		// (set) Token: 0x06001DD3 RID: 7635 RVA: 0x000DD55E File Offset: 0x000DB75E
		public bool IsRefresh { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x000DD567 File Offset: 0x000DB767
		public bool IsThrowMenu
		{
			get
			{
				return this.CurrentConfig is ThrowGroupConfig;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x000DD577 File Offset: 0x000DB777
		public bool IsTopLevel
		{
			get
			{
				return this.m_backConfigs.Count <= 0;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x000DD58A File Offset: 0x000DB78A
		public bool IsHoverMenu
		{
			get
			{
				return this.HoverObject != null;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x000DD598 File Offset: 0x000DB798
		public bool ShowThrowHint
		{
			get
			{
				return (!(this.CurrentConfig is ValheimRadialConfig) || this.LastUsed is ItemElement) && !(this.CurrentConfig is ThrowGroupConfig);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x000DD5C7 File Offset: 0x000DB7C7
		public bool UseHoverSelect
		{
			get
			{
				return RadialData.SO.HoverSelectSpeed > 0f && this.m_selected && !(this.m_selected is EmptyElement);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x000DD5FA File Offset: 0x000DB7FA
		public bool CanDoubleClick
		{
			get
			{
				return RadialData.SO.EnableDoubleClick && !RadialData.SO.EnableFlick && ZInput.IsGamepadActive();
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x000DD61B File Offset: 0x000DB81B
		public bool CanFlick
		{
			get
			{
				return RadialData.SO.EnableFlick && !RadialData.SO.EnableDoubleClick && ZInput.IsGamepadActive();
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x000DD63C File Offset: 0x000DB83C
		// (set) Token: 0x06001DDC RID: 7644 RVA: 0x000DD649 File Offset: 0x000DB849
		public bool Active
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			private set
			{
				this.m_isClosing = false;
				this.m_elementInfo.Clear();
				this.m_cursor.gameObject.SetActive(false);
				base.gameObject.SetActive(value);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x000DD67A File Offset: 0x000DB87A
		// (set) Token: 0x06001DDE RID: 7646 RVA: 0x000DD682 File Offset: 0x000DB882
		public int MaxElementsPerLayer { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x000DD68B File Offset: 0x000DB88B
		// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x000DD693 File Offset: 0x000DB893
		public int StartItemIndex { get; set; } = -1;

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x000DD69C File Offset: 0x000DB89C
		// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x000DD6A4 File Offset: 0x000DB8A4
		public int StartOffset { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x000DD6AD File Offset: 0x000DB8AD
		// (set) Token: 0x06001DE4 RID: 7652 RVA: 0x000DD6B5 File Offset: 0x000DB8B5
		public int BackIndex { get; set; }

		// Token: 0x06001DE6 RID: 7654 RVA: 0x000DD6F8 File Offset: 0x000DB8F8
		[CompilerGenerated]
		private void <OnInteract>g__OnSuccessfulInteract|16_0()
		{
			bool flag;
			if (this.m_selected.AdvancedCloseOnInteract == null)
			{
				Func<bool> closeOnInteract = this.m_selected.CloseOnInteract;
				flag = (closeOnInteract != null && closeOnInteract());
			}
			else
			{
				Func<RadialBase, RadialArray<RadialMenuElement>, bool> advancedCloseOnInteract = this.m_selected.AdvancedCloseOnInteract;
				flag = (advancedCloseOnInteract != null && advancedCloseOnInteract(this, this.m_elements));
			}
			ThrowElement throwElement = this.m_selected as ThrowElement;
			if (throwElement != null && !this.m_localPlayerRef.GetInventory().ContainsItem(throwElement.m_data))
			{
				this.Back();
			}
			RadialMenuElement selected = this.m_selected;
			RadialMenuElement lastUsed;
			if (!(selected is GroupElement))
			{
				if (!(selected is ThrowElement))
				{
					ItemElement itemElement = selected as ItemElement;
					if (itemElement == null)
					{
						if (selected != null)
						{
							lastUsed = this.m_selected;
						}
						else
						{
							lastUsed = null;
						}
					}
					else if (this.m_selected.Equals(this.LastUsed))
					{
						lastUsed = ((!this.m_localPlayerRef.GetInventory().ContainsItem(itemElement.m_data)) ? null : this.LastUsed);
					}
					else
					{
						ItemElement itemElement2 = itemElement;
						lastUsed = ((itemElement2.m_data.m_shared.m_name.Contains("hammer") || !this.m_localPlayerRef.GetInventory().ContainsItem(itemElement2.m_data)) ? this.LastUsed : this.m_selected);
					}
				}
				else
				{
					ItemElement itemElement3 = this.LastUsed as ItemElement;
					if (itemElement3 != null)
					{
						lastUsed = ((!this.m_localPlayerRef.GetInventory().ContainsItem(itemElement3.m_data)) ? null : this.LastUsed);
					}
					else
					{
						lastUsed = this.LastUsed;
					}
				}
			}
			else
			{
				lastUsed = this.LastUsed;
			}
			this.LastUsed = lastUsed;
			if (flag)
			{
				this.QueuedClose();
				return;
			}
			ValueTuple<IRadialConfig, IRadialConfig> queuedOpenConfigs = this.m_queuedOpenConfigs;
			IRadialConfig item = queuedOpenConfigs.Item1;
			IRadialConfig item2 = queuedOpenConfigs.Item2;
			if (item == null && item2 == null)
			{
				this.Refresh();
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000DD8E0 File Offset: 0x000DBAE0
		[CompilerGenerated]
		private void <StartAnimatingIn>g__OnEnd|36_0()
		{
			this.m_isAnimating = false;
			if (!this.m_selected)
			{
				return;
			}
			if (this.ShouldNudgeSelected)
			{
				this.m_selected.StartNudge(this.m_animationManager, RadialData.SO.ElementsDistance + RadialData.SO.NudgeDistance, RadialData.SO.NudgeDuration, RadialData.SO.NudgeEasingType);
				return;
			}
			this.UpdateElementsScale();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000DD94B File Offset: 0x000DBB4B
		[CompilerGenerated]
		private void <StartAnimatingOut>g__OnEnd|37_0()
		{
			this.m_animationManager.EndAll();
			this.CanOpen = false;
			this.Active = false;
		}

		// Token: 0x04001E1E RID: 7710
		[Header("References")]
		[SerializeField]
		private ElementInfo m_elementInfo;

		// Token: 0x04001E1F RID: 7711
		[SerializeField]
		private RectTransform m_elementContainer;

		// Token: 0x04001E20 RID: 7712
		[SerializeField]
		private RectTransform m_cursor;

		// Token: 0x04001E21 RID: 7713
		[SerializeField]
		private RectTransform m_highlighter;

		// Token: 0x04001E22 RID: 7714
		private const string c_TOGGLE_ID = "_tog";

		// Token: 0x04001E23 RID: 7715
		private const string c_HOVER_SUFFIX = "_hov";

		// Token: 0x04001E24 RID: 7716
		private Player m_localPlayerRef;

		// Token: 0x04001E25 RID: 7717
		private Stack<IRadialConfig> m_backConfigs = new Stack<IRadialConfig>();

		// Token: 0x04001E26 RID: 7718
		private RadialArray<RadialMenuElement> m_elements;

		// Token: 0x04001E27 RID: 7719
		private readonly RadialMenuAnimationManager m_animationManager = new RadialMenuAnimationManager();

		// Token: 0x04001E28 RID: 7720
		[TupleElementNames(new string[]
		{
			"openConfig",
			"backConfig"
		})]
		private ValueTuple<IRadialConfig, IRadialConfig> m_queuedOpenConfigs = new ValueTuple<IRadialConfig, IRadialConfig>(null, null);

		// Token: 0x04001E29 RID: 7721
		private Vector2 m_previousInput;

		// Token: 0x04001E2A RID: 7722
		private bool m_radialHasItemElements;

		// Token: 0x04001E2B RID: 7723
		private bool m_reFade;

		// Token: 0x04001E2C RID: 7724
		private bool m_cursorLocked;

		// Token: 0x04001E2D RID: 7725
		private bool m_isAnimating;

		// Token: 0x04001E2E RID: 7726
		private bool m_isClosing;

		// Token: 0x04001E2F RID: 7727
		private bool m_closeQueued;

		// Token: 0x04001E30 RID: 7728
		private int m_nrOfLayers;

		// Token: 0x04001E31 RID: 7729
		private int m_currentLayer;

		// Token: 0x04001E32 RID: 7730
		private int m_index;

		// Token: 0x04001E33 RID: 7731
		private int m_closestPoint;

		// Token: 0x04001E34 RID: 7732
		private int m_previousClosestPoint;

		// Token: 0x04001E35 RID: 7733
		private int m_inputDirection;

		// Token: 0x04001E36 RID: 7734
		private float m_cursorThreshold;

		// Token: 0x04001E37 RID: 7735
		private float m_cursorTarget;

		// Token: 0x04001E38 RID: 7736
		private float m_previousCursorTarget;

		// Token: 0x04001E39 RID: 7737
		private float m_fadeAnchor;

		// Token: 0x04001E3A RID: 7738
		private float m_layerFadeThreshold;

		// Token: 0x04001E3B RID: 7739
		private float m_layerFadeDistance;

		// Token: 0x04001E3C RID: 7740
		private float m_layerShowDistance;

		// Token: 0x04001E3D RID: 7741
		private float m_segmentSize;

		// Token: 0x04001E3E RID: 7742
		private float m_inputAngle;

		// Token: 0x04001E3F RID: 7743
		private string m_cursorID;

		// Token: 0x04001E4D RID: 7757
		private RadialMenuElement m_selected;

		// Token: 0x04001E4E RID: 7758
		private RadialMenuElement m_lastUsed;

		// Token: 0x04001E50 RID: 7760
		private bool m_shouldAnimateIn;
	}
}
