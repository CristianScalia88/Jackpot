using UnityEngine;

[CreateAssetMenu(menuName= "Slot Machine/Create New Figure")]
public class FigureDefinition : ScriptableObject {
	[SerializeField] private string name;
	[SerializeField] private Sprite sprite;


	public string Name => name; //TODO Translate a key here.
	public Sprite Sprite => sprite;
}
