using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;

public class SaveGameStateDialog : UIView {

	const float MOVE_DIALOG_OUT_DURATION = 0.4f;

	public Region MainFrameRegion;
	public Region MainRegionMount;
	public Region MainRegion;

	public void Start () {
	}

	public void Ok (Button btn) {
		MoveDialogOut ();

		// call the exit menu function of current menu and pass in saveSettings = false
		StartCoroutine(MoveParentMenuOut (false));
	}

	public void Save () {
		MoveDialogOut ();

		// call the exit menu function of current menu and pass in saveSettings = true
		StartCoroutine(MoveParentMenuOut (true));
	}

	IEnumerator MoveParentMenuOut (bool saveSettings) {
		float timeToWait = MOVE_DIALOG_OUT_DURATION + 1;
		yield return new WaitForSeconds(timeToWait);
		GameManager.Instance.ExitMenu (saveSettings);
	}

	private void MoveDialogOut () {
		Tween tween = MainFrameRegion.transform.DOPunchScale(new Vector3(0.8f,0.8f,0), MOVE_DIALOG_OUT_DURATION, 5, 0.5f);
		tween.OnComplete (()=>HideDialog());
	}

	public void ShowDialog () {
		MainRegion.transform.position = MainRegionMount.transform.position;
	}

	public void HideDialog () {
		MainRegion.transform.DOMoveY (-480, 0.1f, true);
	}

}
