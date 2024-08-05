using System.Collections.Generic;
using UnityEngine;


public abstract class ScoringChecker : ScriptableObject {
	public abstract bool CheckAndGetScoring(FigureDefinition[,] matrix, out ScoringResult scoringResult);
}

public class ScoringResult {
	public List<MatrixPosition> positions = new List<MatrixPosition>();
	public int score = 0;
}

[System.Serializable]
public class MatrixPosition {
	public int row;
	public int column;

	public MatrixPosition(int row, int column) {
		this.row = row;
		this.column = column;
	}

	public override bool Equals(object obj) {
		if (obj != null && obj is MatrixPosition matrixPosition) {
			return matrixPosition.row == row && matrixPosition.column == column;
		}
		return false;
	}

	public override string ToString() {
		return $"({row},{column})";
	}
}