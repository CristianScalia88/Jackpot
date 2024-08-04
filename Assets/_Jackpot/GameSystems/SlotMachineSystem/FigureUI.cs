using UnityEngine;
using UnityEngine.UI;

public class FigureUI : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    
    public FigureDefinition FigureDefinition { get; private set; }

    public float GetHeight => rectTransform.rect.height;
    public void Bind(FigureDefinition definition) {
        FigureDefinition = definition;
        image.sprite = definition.Sprite;
    }
}