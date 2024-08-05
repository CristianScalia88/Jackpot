using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReelDefinition))]
public class ReelDefinitionEditor : Editor {
	private const string BellPath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Bell.asset";
	private const string CherriesPath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Cherries.asset";
	private const string GrapesPath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Grapes.asset";
	private const string LemonPath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Lemon.asset";
	private const string OrangePath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Orange.asset";
	private const string PlumPath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Plum.asset";
	private const string WatermelonPath = "Assets/_Jackpot/GameSystems/FiguresSystem/Definitions/Watermelon.asset";

	private ReelDefinition reelDefinition => (ReelDefinition)target;

	private bool listeningKeyboard;
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		listeningKeyboard = EditorGUILayout.Toggle("Listening Keyboard", listeningKeyboard);
		if (listeningKeyboard) {
			CheckForKey("B", BellPath);
			CheckForKey("C", CherriesPath);
			CheckForKey("G", GrapesPath);
			CheckForKey("L", LemonPath);
			CheckForKey("O", OrangePath);
			CheckForKey("P", PlumPath);
			CheckForKey("W", WatermelonPath);
		}
	}

	private void CheckForKey(string key, string figureDefinitionPath) {
		if (Event.current.Equals(Event.KeyboardEvent(key))) {
			AddFigureDefinition(figureDefinitionPath);
			Event.current.Use();
		}
	}

	private void AddFigureDefinition(string assetPath) {
		FigureDefinition figure = AssetDatabase.LoadAssetAtPath<FigureDefinition>(assetPath);
		if (figure != null) {
			Undo.RecordObject(reelDefinition, "Add FigureDefinition");
			reelDefinition.figures.Add(figure);
			EditorUtility.SetDirty(reelDefinition);
		} else {
			Debug.LogWarning("FigureDefinition not found at path: " + assetPath);
		}
	}
}