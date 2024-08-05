using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteReel : MonoBehaviour {
	[SerializeField] private RectTransform content;
	[SerializeField] private FigureUI figureUIPrefab;
	[SerializeField] private ReelDefinition reelDefinition;
	[SerializeField] private float spinVelocity = 10000;
	
	private List<FigureUI> figuresUI;
	private Vector3 velocity;


	private int targetIndex;
#if UNITY_EDITOR
	public Stack<string> logs = new Stack<string>();
	public int forceResult = -1;
#endif
	//reelDefinition dependency should be assigned, by something else.
	private void Start() {
		Bind(reelDefinition);
	}

	public void Bind(ReelDefinition reelDefinition) {
		figuresUI = new List<FigureUI>();
		for (var i = 0; i < reelDefinition.figures.Count; i++) {
			FigureDefinition figureDefinition = reelDefinition.figures[i];
			FigureUI figureUI = Instantiate(figureUIPrefab, content);
			figureUI.name += $" ({i})";
			figureUI.transform.localPosition = Vector3.up * i * FigureHeight; 
			figureUI.Bind(figureDefinition);
			figuresUI.Add(figureUI);
		}
	}

	private void Update() {
		MoveFigures();
		RepositionsUIElements();
	}

	private void MoveFigures() {
		for (int i = 0; i < figuresUI.Count; i++) {
			Transform figure = figuresUI[i].transform;
			figure.transform.position += velocity * Time.deltaTime;
		}
	}

	public void Spin() {
		stopped = false;
		targetIndex =Random.Range(0, figuresUI.Count);
#if UNITY_EDITOR
		logs.Clear();
		if (forceResult != -1) {
			targetIndex = forceResult;
		} 
#endif
		DOTween.To(()=> velocity, x=> velocity = x, new Vector3(0,-spinVelocity, 0), .25f).SetEase(Ease.InElastic);
	}

	private bool stopped;
	public IEnumerator Stop() {
		stopped = true;
		yield return new WaitUntil(() => GetFirstSibling == TargetFigure);
		velocity = Vector3.zero;
		for (int i = 0; i < content.childCount; i++) {
			content.GetChild(i).transform.DOLocalMoveY(i * FigureHeight, .25f);
		}
#if UNITY_EDITOR
		yield return new WaitForSeconds(.25f);
		if (GetFirstSibling != TargetFigure) {
			AddLog($"ERROR, {name} Expected Figure {figuresUI[targetIndex].FigureDefinition.name}, Received {GetThirdFigure.FigureDefinition.name}");
			Debug.LogError($"ERROR", gameObject);
			PrintLogs();
		}
#endif
	}

	private void RepositionsUIElements() {
		Transform firstSibling = GetFirstSibling;
		if (stopped && TargetFigure == firstSibling)
			return;
		if (firstSibling.localPosition.y <= -FigureHeight) {
			Transform firstChild = GetLastSibling;
			firstSibling.transform.localPosition = firstChild.localPosition + Vector3.up * FigureHeight;
			firstSibling.SetAsLastSibling();
		}
	}

	private Transform GetFirstSibling => content.GetChild(0);
	public FigureUI GetFirstFigure => content.GetChild(2).GetComponent<FigureUI>();
	public FigureUI GetSecondFigure => content.GetChild(1).GetComponent<FigureUI>();
	public FigureUI GetThirdFigure => content.GetChild(0).GetComponent<FigureUI>();
	private Transform GetLastSibling => content.GetChild(content.childCount - 1);
	private float FigureHeight => figureUIPrefab.GetHeight;

	private Transform TargetFigure => figuresUI[targetIndex].transform;

#if UNITY_EDITOR
	private void AddLog(string log) {
		logs.Push(log);
		if (logs.Count > 20) {
			logs.Pop();
		}
	}

	private void PrintLogs() {
		Debug.LogError(string.Join("\n", logs));
	}
#endif
}