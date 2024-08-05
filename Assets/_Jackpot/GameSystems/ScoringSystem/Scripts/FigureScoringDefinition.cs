using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Slot Machine/Scoring/Create Figure Scoring")]
public class FigureScoringDefinition : ScriptableObject {
	[SerializeField] private FigureDefinition figure;
	// Scoring based on matches the index in the array represents the number of matches, 
	// and the value represents the score associated with that number of matches.
	// Example: {0, 0, 5, 8} means 0 points for 0 repetitions, 0 points for 1 repetition, 5 points for 2 repetitions,
	// and 8 points for 3 repetitions.
	[SerializeField] private List<int> coincidencesScoring;
	
	public int GetScoreFor(int coincidences) => coincidencesScoring[coincidences];
	public FigureDefinition FigureDefinition => figure;
}