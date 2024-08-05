using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteReel : MonoBehaviour {
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private VerticalLayoutGroup verticalLayout;
	[SerializeField] private ContentSizeFitter contentSizeFitter;
	[SerializeField] private FigureUI figureUIPrefab;
	[SerializeField] private ReelDefinition reelDefinition;
	[SerializeField] private float velocity = 10000;

	//reelDefinition dependency should be assigned, by something else.
	private void Start() {
		Bind(reelDefinition);
	}

	public void Bind(ReelDefinition reelDefinition) {
		scrollRect.content.sizeDelta += Vector2.up * reelDefinition.figures.Count * FigureHeight;

		for (var i = 0; i < reelDefinition.figures.Count; i++) {
			FigureDefinition figureDefinition = reelDefinition.figures[i];
			FigureUI figureUI = Instantiate(figureUIPrefab, scrollRect.content);
			figureUI.transform.localPosition = Vector3.up * i * FigureHeight; 
			figureUI.Bind(figureDefinition);
		}
	}

	private void Update() {
		RepositionsUIElements();
	}

	public void Spin() {
		scrollRect.inertia = true;
		scrollRect.decelerationRate = 1;
		DOTween.To(()=> scrollRect.velocity, x=> scrollRect.velocity = x, new Vector2(0,-velocity), .25f).SetEase(Ease.InElastic);
	}

	public void Stop() {
		scrollRect.inertia = false;
		scrollRect.decelerationRate = 0;
		float pos = Mathf.FloorToInt(scrollRect.content.localPosition.y / figureUIPrefab.GetHeight) * figureUIPrefab.GetHeight;
		scrollRect.content.DOLocalMoveY(pos, .15f).SetEase(Ease.OutElastic);
	}

	private void RepositionsUIElements() {
		Transform childToCheck = GetFirstSibling;
		Vector3 childPos = scrollRect.transform.InverseTransformPoint(childToCheck.transform.position);
		if (childPos.y <= -FigureHeight) {
			RectTransform content = scrollRect.content;
			Transform firstChild = GetLastSibling;
			childToCheck.transform.localPosition = firstChild.localPosition + Vector3.up * FigureHeight;
			childToCheck.SetAsLastSibling();
			content.sizeDelta += new Vector2(0,  FigureHeight);
		}
	}

	private Transform GetFirstSibling => scrollRect.content.GetChild(0);
	private Transform GetLastSibling => scrollRect.content.GetChild(scrollRect.content.childCount - 1);
	private float FigureHeight => figureUIPrefab.GetHeight;

}