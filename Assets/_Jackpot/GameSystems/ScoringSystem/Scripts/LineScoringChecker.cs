using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jackpot/Scoring/Create Line Scoring")]
public class LineScoringChecker : ScoringChecker {
	[SerializeField] private FigureDefinition figure;
	// Scoring based on matches the index in the array represents the number of matches, 
	// and the value represents the score associated with that number of matches.
	// Example: {0, 0, 5, 8} means 0 points for 0 repetitions, 0 points for 1 repetition, 5 points for 2 repetitions,
	// and 8 points for 3 repetitions.
	[SerializeField] private List<int> coincidencesScoring;

	public override bool CheckAndGetScoring(FigureDefinition[,] matrix, out ScoringResult scoringResult) {
		int rowsCount = matrix.GetLength(0);
		int columnsCount = matrix.GetLength(1);
		scoringResult = new ScoringResult();
		for (int row = 0; row < rowsCount; row++) {
			List<MatrixPosition> validPositions = new List<MatrixPosition>();
			for (int column = 0; column < columnsCount; column++) {
				FigureDefinition resultFigure = matrix[row, column];
				if (resultFigure != figure) {
					break;
				}
				if (resultFigure == figure) {
					validPositions.Add(new MatrixPosition(row, column));
				}
			}

			if (coincidencesScoring.Count > validPositions.Count) {
				int score = coincidencesScoring[validPositions.Count];
				if (score > 0) {
					scoringResult.score += score;
					scoringResult.positions.AddRange(validPositions);
				}
			}
		}

		return scoringResult.score > 0;
	}
}