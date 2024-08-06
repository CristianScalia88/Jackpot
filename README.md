# Slot Machine Game Documentation

## Overview

This template consists of various ScriptableObjects to define figures, scoring rules, reel patterns and the main slot machine logic, which handles spinning reels, checking scores, and animating results.

## Classes and ScriptableObjects

### SlotMachine
The main MonoBehaviour class that controls the slot machine logic, including spinning reels, checking scores, and animating results.
#### Properties:
    reels (List<InfiniteReel>): The list of reels in the slot machine.
    delayBetweenReels (float): The delay between starting each reel spin.
    spinTimeMin (float): The minimum spin time.
    spinTimeMax (float): The maximum spin time.
    spinButton (Button): The button to start the spin.
    scoringCheckers (List<ScoringChecker>): The list of scoring checkers.

scoreText (TMP_Text): The text displaying the score.
#### Methods:
    Spin(): Starts the coroutine to spin the reels.
    SpinCoroutine(): Handles the reel spinning sequence and collects the score.
    CollectScoreSequence(): Converts the reel figures to definitions and checks the score using the scoring checkers.
    AnimateScoringResult(FigureUI[,] figures, ScoringResult scoringResult): Animates the scoring result.

### FigureDefinition
defines a slot machine figure.
#### Properties:
    name (string): The name of the figure.
    sprite (Sprite): The visual representation of the figure.
#### Methods:
    Name: Returns the name of the figure.
    Sprite: Returns the sprite of the figure.

### FigureScoringDefinition
This ScriptableObject defines the scoring rules for a specific figure.
#### Properties:
    figure (FigureDefinition): The figure associated with this scoring rule.
    coincidencesScoring (List<int>): The scoring values based on the number of matches. Array index represents coincidences, the value represents scoring.
#### Methods:
    GetScoreFor(int coincidences): Returns the score for the given number of matches. => coincidencesScoring[coincidences]
    FigureDefinition: Returns the figure associated with this scoring rule.

### ScoringChecker
An abstract base class for implementing different scoring checkers.
#### Methods:
    CheckAndGetScoring(FigureDefinition[,] matrix, out ScoringResult scoringResult): An abstract method that checks the matrix for scoring and returns the result.

### CustomScoringChecker
This ScriptableObject extends ScoringChecker and provides custom scoring logic based on specific positions in the slot machine matrix.
#### Properties:
    positions (List<MatrixPosition>): The positions to check for matching figures.
    allScoring (List<FigureScoringDefinition>): The scoring definitions for all figures.
#### Methods:
    CheckAndGetScoring(FigureDefinition[,] matrix, out ScoringResult scoringResult): Checks the matrix for matching figures and returns the scoring result.

### ScoringResult
A class that holds the result of a scoring check.
#### Properties:
    positions (List<MatrixPosition>): The positions that contributed to the score.
    score (int): The total score.
    scoringChecker (ScoringChecker): The scoring checker used.
    figureDefinition (FigureDefinition): The figure definition associated with the score.

### MatrixPosition
A serializable class representing a position in the slot machine matrix.
#### Properties:
    row (int): The row index.
    column (int): The column index.