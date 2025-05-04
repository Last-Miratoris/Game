using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Valheim.SettingsGui
{
	// Token: 0x020001F5 RID: 501
	public abstract class SettingsBase : MonoBehaviour
	{
		// Token: 0x06001CD0 RID: 7376 RVA: 0x000D86DB File Offset: 0x000D68DB
		public void OnOk()
		{
			Settings.instance.OnOk();
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000D86E7 File Offset: 0x000D68E7
		public virtual void OnBack()
		{
			Settings.instance.OnBack();
		}

		// Token: 0x06001CD2 RID: 7378
		public abstract void FixBackButtonNavigation(Button backButton);

		// Token: 0x06001CD3 RID: 7379
		public abstract void FixOkButtonNavigation(Button okButton);

		// Token: 0x06001CD4 RID: 7380
		public abstract void LoadSettings();

		// Token: 0x06001CD5 RID: 7381
		public abstract void SaveSettings();

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000D86F3 File Offset: 0x000D68F3
		public virtual void ResetSettings()
		{
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000D86F8 File Offset: 0x000D68F8
		protected void SetNavigationToFirstActive(Selectable selectable, SettingsBase.NavigationDirection direction, List<Selectable> targets)
		{
			Selectable selectable2 = targets.FirstOrDefault((Selectable t) => t.gameObject.activeSelf);
			if (selectable2 == null)
			{
				return;
			}
			this.SetNavigation(selectable, direction, selectable2);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000D8740 File Offset: 0x000D6940
		protected void SetNavigation(Selectable selectable, SettingsBase.NavigationDirection direction, Selectable target)
		{
			Navigation navigation = selectable.navigation;
			switch (direction)
			{
			case SettingsBase.NavigationDirection.OnLeft:
				navigation.selectOnLeft = target;
				break;
			case SettingsBase.NavigationDirection.OnRight:
				navigation.selectOnRight = target;
				break;
			case SettingsBase.NavigationDirection.OnUp:
				navigation.selectOnUp = target;
				break;
			case SettingsBase.NavigationDirection.OnDown:
				navigation.selectOnDown = target;
				break;
			}
			selectable.navigation = navigation;
		}

		// Token: 0x04001DD9 RID: 7641
		public Action Saved;

		// Token: 0x020003A5 RID: 933
		protected enum NavigationDirection
		{
			// Token: 0x04002704 RID: 9988
			OnLeft,
			// Token: 0x04002705 RID: 9989
			OnRight,
			// Token: 0x04002706 RID: 9990
			OnUp,
			// Token: 0x04002707 RID: 9991
			OnDown
		}
	}
}
