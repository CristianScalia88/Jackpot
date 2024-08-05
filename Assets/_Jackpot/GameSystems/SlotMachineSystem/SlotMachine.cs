using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
				reel.Stop();
				yield return new WaitForSeconds(delayBetweenReels);
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
			int rowsCount = figures.GetLength(0);
			int columnsCount = figures.GetLength(1);
			FigureDefinition[,] figuresDefinitions = new FigureDefinition[3, 5];
			for (int row = 0; row < rowsCount; row++) {
				for (int column = 0; column < columnsCount; column++) {
					figuresDefinitions[row, column] = figures[row, column].FigureDefinition;
				}
			}

			foreach (ScoringChecker scoringChecker in scoringCheckers) {
				if (scoringChecker.CheckAndGetScoring(figuresDefinitions, out ScoringResult scoringResult)) {
					yield return AnimateScoringResult(scoringResult);
				}
			}
		}

		private IEnumerator AnimateScoringResult(ScoringResult scoringResult) {
			Debug.LogError(string.Join(" , ", scoringResult.positions));
			yield return new WaitForSeconds(1);
		}
	}
}