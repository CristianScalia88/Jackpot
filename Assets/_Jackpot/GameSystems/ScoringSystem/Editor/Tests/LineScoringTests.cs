using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

public class LineScoringTests {
	private ScoringChecker row_0_scoringChecker;
	private ScoringChecker row_1_scoringChecker;
	private ScoringChecker row_2_scoringChecker;
	private CustomScoringChecker custom_1_scoringChecker;
	private CustomScoringChecker custom_2_scoringChecker;
	private CustomScoringChecker custom_3_scoringChecker;
	private CustomScoringChecker custom_4_scoringChecker;
	private CustomScoringChecker custom_5_scoringChecker;
	private CustomScoringChecker custom_6_scoringChecker;
	private FigureDefinition figureDefinition;

	[SetUp]
	public void SetupTests() {
		row_0_scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_Row_0_PatternPattern.asset");
		row_1_scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_Row_1_PatternPattern.asset");
		row_2_scoringChecker = AssetDatabase.LoadAssetAtPath<ScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_Row_2_PatternPattern.asset");
		custom_1_scoringChecker = AssetDatabase.LoadAssetAtPath<CustomScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_CustomPattern 1.asset");
		custom_2_scoringChecker = AssetDatabase.LoadAssetAtPath<CustomScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_CustomPattern 2.asset");
		custom_3_scoringChecker = AssetDatabase.LoadAssetAtPath<CustomScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_CustomPattern 3.asset");
		custom_4_scoringChecker = AssetDatabase.LoadAssetAtPath<CustomScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_CustomPattern 4.asset");
		custom_5_scoringChecker = AssetDatabase.LoadAssetAtPath<CustomScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_CustomPattern 5.asset");
		custom_6_scoringChecker = AssetDatabase.LoadAssetAtPath<CustomScoringChecker>("Assets/_Jackpot/GameSystems/ScoringSystem/Editor/Tests/Test_CustomPattern 6.asset");
		
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
	
	[Test]
	public void CheckAndGetScoring_pattern_1_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null           , figureDefinition, null             , figureDefinition, null            },
			{null            , null            , null            , null            , null            },
			{figureDefinition, null            , figureDefinition, null            , figureDefinition}
		};
		List<MatrixPosition> expectedPositions = custom_1_scoringChecker.positions;
		
		bool hasScore = custom_1_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
	
	[Test]
	public void CheckAndGetScoring_pattern_2_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{figureDefinition ,null             , figureDefinition, null            , figureDefinition},
			{null            , null            ,  null            , null            , null            },
			{null            , figureDefinition , null            , figureDefinition, null            }
		};
		List<MatrixPosition> expectedPositions = custom_2_scoringChecker.positions;
		
		bool hasScore = custom_2_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
	
	[Test]
	public void CheckAndGetScoring_pattern_3_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{figureDefinition, null           , null              , null             , figureDefinition },
			{null            , figureDefinition ,null              , figureDefinition , null            },
			{null            , null            , figureDefinition , null             , null            }
		};
		List<MatrixPosition> expectedPositions = custom_3_scoringChecker.positions;
		
		bool hasScore = custom_3_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
	
	[Test]
	public void CheckAndGetScoring_pattern_4_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null             , null           , figureDefinition , null             , null             },
			{null            , figureDefinition ,null              , figureDefinition  , null            },
			{figureDefinition , null            , null             , null             , figureDefinition  }
		};
		List<MatrixPosition> expectedPositions = custom_4_scoringChecker.positions;
		
		bool hasScore = custom_4_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
	
	[Test]
	public void CheckAndGetScoring_pattern_5_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{figureDefinition , figureDefinition , null              , null             , null             },
			{null            , null             ,figureDefinition  , null             , null            },
			{null            , null            , null             ,figureDefinition  , figureDefinition }
		};
		List<MatrixPosition> expectedPositions = custom_5_scoringChecker.positions;
		
		bool hasScore = custom_5_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
	
	[Test]
	public void CheckAndGetScoring_pattern_6_returns_validScoreAndPositions() {
		FigureDefinition[,] figures = new FigureDefinition[,] {
			{null             , null           , null              ,figureDefinition  , figureDefinition  },
			{null            , null             ,figureDefinition  , null             , null            },
			{figureDefinition  , figureDefinition  , null             , null             , null            }
		};
		List<MatrixPosition> expectedPositions = custom_6_scoringChecker.positions;
		
		bool hasScore = custom_6_scoringChecker.CheckAndGetScoring(figures, out ScoringResult scoringResult);
		
		Assert.IsTrue(hasScore);
		bool validScore = scoringResult.score == 20;
		Assert.IsTrue(validScore);
		bool validPositions = scoringResult.positions.Count == 5 && expectedPositions.All(mp => scoringResult.positions.Contains(mp));
		Assert.IsTrue(validPositions);
	}
}