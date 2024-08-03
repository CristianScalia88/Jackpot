using UnityEngine;
using UnityEngine.UI;

public class FigureUI : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    
    private FigureDefinition figureDefinition;

    public float GetHeight => rectTransform.rect.height;
    public void Bind(FigureDefinition definition) {
        figureDefinition = definition;
        image.sprite = definition.Sprite;
    }
}