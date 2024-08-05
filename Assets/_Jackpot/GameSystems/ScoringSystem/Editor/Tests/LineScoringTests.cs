using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

public class LineScoringTests {
	private ScoringChecker scoringChecker;
	private FigureDefinition figureDefinition;

	[SetUp]
	public void SetupTests() {
		scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Definitions/CustomPattern/Row_0_PatternPattern.asset");
		figureDefinition = AssetDatabase.LoadAssetAtPath<FigureDefinition>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/TestFigure.asset");
	}

	[Test]
	public void CheckAndGetScoring_with1InLastRow_returns_NoScore() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition},
			{figureDefinition, null, null, null, null}
		};
		bool hasScore = scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		Assert.IsFalse(hasScore);
	}


	[Test]
	public void CheckAndGetScoring_with1InLastRow_returns_validScore() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{figureDefinition, figureDefinition, null, null, null},
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition}
		};
		List<MatrixPosition> expectedPositions = new List<MatrixPosition>{new (0, 0),new (0, 1)};
		
		bool hasScore = scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 2;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 2 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}

	[Test]
	public void CheckAndGetScoring_with2InLastRow_returns_validScore() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{figureDefinition, figureDefinition, figureDefinition, null, null},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition}
		};
		List<MatrixPosition> expectedPositions = new List<MatrixPosition>{new (1, 0),new (1, 1),new (1, 2)};
		
		bool hasScore = scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 5;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 3 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}

	[Test]
	public void CheckAndGetScoring_with5InLastRow_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null, figureDefinition, null, figureDefinition, figureDefinition},
			{null, figureDefinition, figureDefinition, figureDefinition, figureDefinition},
			{figureDefinition, figureDefinition, figureDefinition, figureDefinition, figureDefinition}
		};
		List<MatrixPosition> expectedPositions = new List<MatrixPosition>{new (2, 0),new (2, 1),new (2, 2),new (2, 3),new (2, 4)};
		
		bool hasScore = scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
}