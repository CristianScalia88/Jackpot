using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

public class LineScoringTests {
	private ScoringChecker row_0_scoringChecker;
	private ScoringChecker row_1_scoringChecker;
	private ScoringChecker row_2_scoringChecker;
	private FigureDefinition figureDefinition;

	[SetUp]
	public void SetupTests() {
		row_0_scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_Row_0_PatternPattern.asset");
		row_1_scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_Row_1_PatternPattern.asset");
		row_2_scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_Row_2_PatternPattern.asset");
		figureDefinition = AssetDatabase.LoadAssetAtPath<FigureDefinition>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/TestFigure.asset");
	}

	[Test]
	public void CheckAndGetScoring_with0InALLRow_returns_NoScore() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition},
			{figureDefinition, null, null, null, null}
		};
		bool has0Score = row_0_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		bool has1Score = row_1_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult1);
		bool has2Score = row_2_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult2);
		Assert.IsFalse(has0Score || has1Score || has2Score);
	}


	[Test]
	public void CheckAndGetScoring_with2coincidence_InRow0_returns_validScore() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{figureDefinition, figureDefinition, null, null, null},
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition}
		};
		List<MatrixPosition> expectedPositions = new List<MatrixPosition>{new (0, 0),new (0, 1)};
		
		bool hasScore = row_0_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 2;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 2 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}

	[Test]
	public void CheckAndGetScoring_with3coincidence_InRow1_returns_validScore() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{figureDefinition, figureDefinition, figureDefinition, null, null},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition}
		};
		List<MatrixPosition> expectedPositions = new List<MatrixPosition>{new (1, 0),new (1, 1),new (1, 2)};
		
		bool hasScore = row_1_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 5;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 3 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}

	[Test]
	public void CheckAndGetScoring_with5coincidence_InRow2_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition},
			{figureDefinition, figureDefinition, figureDefinition, figureDefinition, figureDefinition}
		};
		List<MatrixPosition> expectedPositions = new List<MatrixPosition>{new (2, 0),new (2, 1),new (2, 2),new (2, 3),new (2, 4)};
		
		bool hasScore = row_2_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
}