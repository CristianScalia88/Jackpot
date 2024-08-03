using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Slot Machine/Create Reel Definition")]
public class ReelDefinition : ScriptableObject {
	public List<FigureDefinition> figures;
}