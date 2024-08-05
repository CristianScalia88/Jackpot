using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Slot Machine/Scoring/Create Custom Scoring")]
public class CustomScoringChecker : ScoringChecker {
	public List<MatrixPosition> positions;
	public List<LineScoringChecker> allLineScoring;

	public override bool CheckAndGetScoring(FigureDefinition[,] matrix, out ScoringResult scoringResult) {
		scoringResult = new ScoringResult();
		FigureDefinition figureToCheck = matrix[positions[0].row, positions[0].column];
		for (int i = 0; i < positions.Count; i++) {
			FigureDefinition resultFigure = matrix[positions[i].row, positions[i].column];
			if (figureToCheck != resultFigure) {
				break;
			}
			else {
				scoringResult.positions.Add(positions[i]);
			}
		}

		int coincidences = scoringResult.positions.Count;
		LineScoringChecker lineScoringForFigure = allLineScoring.FirstOrDefault(s => s.FigureDefinition == figureToCheck);
		scoringResult.score = lineScoringForFigure?.GetScoreFor(coincidences) ?? 0;
		return scoringResult.score > 0;
	}
}