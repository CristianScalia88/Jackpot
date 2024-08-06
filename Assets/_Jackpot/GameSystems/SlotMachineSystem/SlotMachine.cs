using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Jackpot.GameSystems.SlotMachineSystem {
	public class SlotMachine : MonoBehaviour {
		[SerializeField] private List<InfiniteReel> reels;
		[SerializeField] private float delayBetweenReels = .15f;
		[SerializeField] private float spinTimeMin = 2f;
		[SerializeField] private float spinTimeMax = 4f;
		[SerializeField] private Button spinButton;
		[SerializeField] private List<ScoringChecker> scoringCheckers;
		[SerializeField] private TMP_Text scoreText;

		private void Start() {
			spinButton.onClick.AddListener(Spin);
		}

		private void Spin() {
			StartCoroutine(SpinCoroutine());
		}

		private IEnumerator SpinCoroutine() {
			spinButton.interactable = false;
			foreach (var reel in reels) {
				reel.Spin();
				yield return new WaitForSeconds(delayBetweenReels);
			}
			yield return new WaitForSeconds(Random.Range(spinTimeMin, spinTimeMax));
			foreach (var reel in reels) {
				float stopTimeDelta = Time.time;
				yield return reel.Stop();
				stopTimeDelta = Time.time - stopTimeDelta;
				if(stopTimeDelta < delayBetweenReels)
					yield return new WaitForSeconds(delayBetweenReels - stopTimeDelta);
			}

			yield return CollectScoreSequence();
			spinButton.interactable = true;
		}

		private IEnumerator CollectScoreSequence() {
			FigureUI[,] figures = new FigureUI[,] {
				{reels[0].GetFirstFigure , reels[1].GetFirstFigure , reels[2].GetFirstFigure , reels[3].GetFirstFigure , reels[4].GetFirstFigure },
				{reels[0].GetSecondFigure, reels[1].GetSecondFigure, reels[2].GetSecondFigure, reels[3].GetSecondFigure, reels[4].GetSecondFigure},
				{reels[0].GetThirdFigure , reels[1].GetThirdFigure , reels[2].GetThirdFigure , reels[3].GetThirdFigure , reels[4].GetThirdFigure }
			};
			//Convert Figures UI to Figures Definitions Matrix
			int rowsCount = figures.GetLength(0);
			int columnsCount = figures.GetLength(1);
			FigureDefinition[,] figuresDefinitions = new FigureDefinition[3, 5];
			for (int row = 0; row < rowsCount; row++) {
				for (int column = 0; column < columnsCount; column++) {
					figuresDefinitions[row, column] = figures[row, column].FigureDefinition;
				}
			}
			//Checking Score
			foreach (ScoringChecker scoringChecker in scoringCheckers) {
				if (scoringChecker.CheckAndGetScoring(figuresDefinitions, out ScoringResult scoringResult)) {
#if UNITY_EDITOR
					Debug.LogWarning($"Pattern:{scoringResult.scoringChecker.name} Figure:{scoringResult.figureDefinition.name} Score:{scoringResult.score}");
#endif
					yield return AnimateScoringResult(figures,scoringResult);
				}
			}
		}

		private IEnumerator AnimateScoringResult(FigureUI[,] figures, ScoringResult scoringResult) {
			scoreText.text = scoringResult.score.ToString();
			scoringResult.positions.ForEach(pos => {
				Sequence s = DOTween.Sequence();
				FigureUI figureUI = figures[pos.row, pos.column];
				figureUI.OverrideSortingLayer(10,true);
				s.Append(figureUI.transform.DOScale(1.25f, .1f).SetEase(Ease.OutBounce));
				s.Join(scoreText.transform.DOScale(1, .15f).SetEase(Ease.OutBounce));
				s.Append(figureUI.transform.DOShakeRotation(1.25f, 3f).SetEase(Ease.Linear));
				s.Append(figureUI.transform.DOScale(1, .1f).SetEase(Ease.OutElastic));
				s.Join(scoreText.transform.DOScale(0, .15f).SetEase(Ease.Linear));
				s.AppendCallback(()=> figureUI.OverrideSortingLayer(0,false));
			});
			yield return new WaitForSeconds(2.27f);
		}
	}
}