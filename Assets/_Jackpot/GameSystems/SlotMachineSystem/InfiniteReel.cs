using UnityEngine;
using UnityEngine.UI;

public class InfiniteReel : MonoBehaviour {
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private VerticalLayoutGroup verticalLayout;
	[SerializeField] private ContentSizeFitter contentSizeFitter;
	[SerializeField] private FigureUI figureUIPrefab;
	[SerializeField] private ReelDefinition reelDefinition;

	//reelDefinition dependency should be assigned, by something else.
	private void Start() {
		Bind(reelDefinition);
	}

	public void Bind(ReelDefinition reelDefinition) {
		reelDefinition.figures.ForEach(figureDefinition => {
			FigureUI figureUI = Instantiate(figureUIPrefab, scrollRect.content);
			figureUI.Bind(figureDefinition);
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
		contentSizeFitter.enabled = false;
		verticalLayout.enabled = false;
	}

	private void Update() {
		Transform firstChild = scrollRect.content.GetChild(0);
		float figureUIHeight = figureUIPrefab.GetHeight;
		var firstChildPos = scrollRect.transform.InverseTransformPoint(firstChild.transform.position);
		if (firstChildPos.y > figureUIHeight) {
			Transform lastChild = scrollRect.content.GetChild(scrollRect.content.childCount - 1);
			firstChild.transform.localPosition = lastChild.localPosition - Vector3.up * figureUIHeight;
			Debug.LogError(" Increase the scrollRect Content child Count");
			firstChild.SetAsLastSibling();
		}
	}
}