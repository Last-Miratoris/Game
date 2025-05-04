using System;
using TMPro;
using UnityEngine;

namespace Valheim.SettingsGui
{
	// Token: 0x020001E8 RID: 488
	public class GamepadMap : MonoBehaviour
	{
		// Token: 0x06001C4B RID: 7243 RVA: 0x000D43C8 File Offset: 0x000D25C8
		public void UpdateMap(InputLayout layout)
		{
			this.joyButton0.Label.text = GamepadMap.GetText(GamepadInput.FaceButtonA, null);
			this.joyButton1.Label.text = GamepadMap.GetText(GamepadInput.FaceButtonB, null);
			this.joyButton2.Label.text = GamepadMap.GetText(GamepadInput.FaceButtonX, null);
			this.joyButton3.Label.text = GamepadMap.GetText(GamepadInput.FaceButtonY, null);
			this.joyButton4.Label.text = GamepadMap.GetText(GamepadInput.BumperL, null);
			this.joyButton5.Label.text = GamepadMap.GetText(GamepadInput.BumperR, null);
			this.joyButton6.Label.text = GamepadMap.GetText(GamepadInput.Select, null);
			this.joyButton7.Label.text = GamepadMap.GetText(GamepadInput.Start, null);
			this.joyAxis9.Label.text = GamepadMap.GetText(GamepadInput.TriggerL, null);
			this.joyAxis10.Label.text = GamepadMap.GetText(GamepadInput.TriggerR, null);
			this.joyAxis9And10.gameObject.SetActive(layout == InputLayout.Alternative1 || layout == InputLayout.Alternative2);
			this.joyAxis9And10.Label.text = Localization.instance.Localize("$settings_gp");
			this.joyAxis1And2.Label.text = Localization.instance.Localize("$settings_move");
			this.joyAxis4And5.Label.text = Localization.instance.Localize("$settings_look");
			this.joyButton8.Label.text = GamepadMap.GetText(GamepadInput.StickLButton, null);
			this.joyButton9.Label.text = GamepadMap.GetText(GamepadInput.StickRButton, null);
			this.joyAxis6LeftRight.Label.text = GamepadMap.GetText(GamepadInput.DPadRight, null);
			this.joyAxis7Up.Label.text = GamepadMap.GetText(GamepadInput.DPadUp, null);
			this.joyAxis7Down.Label.text = GamepadMap.GetText(GamepadInput.DPadDown, null);
			this.alternateButtonLabel.text = Localization.instance.Localize("$alternate_key_label ") + ZInput.instance.GetBoundKeyString("JoyAltKeys", false);
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000D4658 File Offset: 0x000D2858
		private static string GetText(GamepadInput gamepadInput, FloatRange? floatRange = null)
		{
			string boundActionString = ZInput.instance.GetBoundActionString(gamepadInput, floatRange);
			return Localization.instance.Localize(boundActionString);
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000D4680 File Offset: 0x000D2880
		private static string GetText(KeyCode keyboardKey)
		{
			string boundActionString = ZInput.instance.GetBoundActionString(keyboardKey);
			return Localization.instance.Localize(boundActionString);
		}

		// Token: 0x04001D29 RID: 7465
		[Header("Face Buttons")]
		[SerializeField]
		private GamepadMapLabel joyButton0;

		// Token: 0x04001D2A RID: 7466
		[SerializeField]
		private GamepadMapLabel joyButton1;

		// Token: 0x04001D2B RID: 7467
		[SerializeField]
		private GamepadMapLabel joyButton2;

		// Token: 0x04001D2C RID: 7468
		[SerializeField]
		private GamepadMapLabel joyButton3;

		// Token: 0x04001D2D RID: 7469
		[Header("Bumpers")]
		[SerializeField]
		private GamepadMapLabel joyButton4;

		// Token: 0x04001D2E RID: 7470
		[SerializeField]
		private GamepadMapLabel joyButton5;

		// Token: 0x04001D2F RID: 7471
		[Header("Center")]
		[SerializeField]
		private GamepadMapLabel joyButton6;

		// Token: 0x04001D30 RID: 7472
		[SerializeField]
		private GamepadMapLabel joyButton7;

		// Token: 0x04001D31 RID: 7473
		[Header("Triggers")]
		[SerializeField]
		private GamepadMapLabel joyAxis9;

		// Token: 0x04001D32 RID: 7474
		[SerializeField]
		private GamepadMapLabel joyAxis10;

		// Token: 0x04001D33 RID: 7475
		[SerializeField]
		private GamepadMapLabel joyAxis9And10;

		// Token: 0x04001D34 RID: 7476
		[Header("Sticks")]
		[SerializeField]
		private GamepadMapLabel joyButton8;

		// Token: 0x04001D35 RID: 7477
		[SerializeField]
		private GamepadMapLabel joyButton9;

		// Token: 0x04001D36 RID: 7478
		[SerializeField]
		private GamepadMapLabel joyAxis1And2;

		// Token: 0x04001D37 RID: 7479
		[SerializeField]
		private GamepadMapLabel joyAxis4And5;

		// Token: 0x04001D38 RID: 7480
		[Header("Dpad")]
		[SerializeField]
		private GamepadMapLabel joyAxis6And7;

		// Token: 0x04001D39 RID: 7481
		[SerializeField]
		private GamepadMapLabel joyAxis6Left;

		// Token: 0x04001D3A RID: 7482
		[SerializeField]
		private GamepadMapLabel joyAxis6Right;

		// Token: 0x04001D3B RID: 7483
		[SerializeField]
		private GamepadMapLabel joyAxis6LeftRight;

		// Token: 0x04001D3C RID: 7484
		[SerializeField]
		private GamepadMapLabel joyAxis7Up;

		// Token: 0x04001D3D RID: 7485
		[SerializeField]
		private GamepadMapLabel joyAxis7Down;

		// Token: 0x04001D3E RID: 7486
		[SerializeField]
		private TextMeshProUGUI alternateButtonLabel;
	}
}
