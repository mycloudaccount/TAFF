using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;

public class GameTitle : UIView {

	public Image TitleAfricansImage;
	public Image TitleFreedomImage;
	public Image Title3ForImage;

	public void Start(){
	
	}

	public void TitleMoveIn () {

		Vector3 newLoc;

		newLoc = new Vector3(
			TitleAfricansImage.transform.position.x,
			TitleAfricansImage.transform.position.y + TitleAfricansImage.Offset.Value.Top.Value - 7,
			TitleAfricansImage.transform.position.z
		);
		TitleAfricansImage.transform.DOMove(newLoc,1, true).SetDelay(0.5f).SetEase(Ease.OutBounce);

		newLoc = new Vector3(
			TitleFreedomImage.transform.position.x,
			TitleFreedomImage.transform.position.y + TitleFreedomImage.Offset.Value.Top.Value + 262,
			TitleFreedomImage.transform.position.z
		);
		TitleFreedomImage.transform.DOMove(newLoc,1, true).SetDelay(0.5f).SetEase(Ease.OutBounce);

		newLoc = new Vector3(
			Title3ForImage.transform.position.x,
			Title3ForImage.transform.position.y + Title3ForImage.Offset.Value.Top.Value - 95,
			Title3ForImage.transform.position.z
		);
		Title3ForImage.transform.DOMove(newLoc, 0.5f, true).SetDelay(1.0f).SetEase(Ease.OutQuad);
		Title3ForImage.transform.DOScale(new Vector3(1.0f,1.0f,1.0f), 0.5f).SetDelay(1.0f).SetEase(Ease.OutQuad).OnComplete(Loop3For);

	}

	public void TitleMoveOut () {

		Vector3 newLoc;

		newLoc = new Vector3(
			TitleAfricansImage.transform.position.x,
			TitleAfricansImage.transform.position.y - TitleAfricansImage.Offset.Value.Top.Value + 7,
			TitleAfricansImage.transform.position.z
		);
		TitleAfricansImage.transform.DOMove(newLoc,0.5f, true).SetDelay(0.0f).SetEase(Ease.Linear);

		newLoc = new Vector3(
			TitleFreedomImage.transform.position.x,
			TitleFreedomImage.transform.position.y - TitleFreedomImage.Offset.Value.Top.Value - 262,
			TitleFreedomImage.transform.position.z
		);
		TitleFreedomImage.transform.DOMove(newLoc,0.5f, true).SetDelay(0.0f).SetEase(Ease.Linear);

		newLoc = new Vector3(
			Title3ForImage.transform.position.x,
			Title3ForImage.transform.position.y - Title3ForImage.Offset.Value.Top.Value + 95,
			Title3ForImage.transform.position.z
		);
		Title3ForImage.transform.DOMove(newLoc, 0.3f, true).SetDelay(0.0f).SetEase(Ease.Linear);
		Title3ForImage.transform.DOScale(new Vector3(0.0f,0.0f,0.0f), 0.3f).SetDelay(0.0f).SetEase(Ease.Linear);

	}

	public void Loop3For () {

		//Title3ForImage.transform.DOScale (new Vector3 (1.1f, 1.1f, 1f), 0.7f).SetDelay(0.0f).SetLoops (-1, LoopType.Restart); 

	}

}
