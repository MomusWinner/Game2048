using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Value { get; private set; }
    
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);

    public bool IsEmpty => Value == 0;

    public const int MaxValue = 11;
    public bool HasMerged { get; private set; }



    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI point;

    // тест цветов
    public void Update()
    {
        UpdateCell();    
    }

    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;
        GameController.Instance.AddPoints(Value);
        UpdateCell();
    }

    public void ResetFlagsMerged()
    {
        HasMerged = false;
    }

    public void MergedWithCell(Cell otherCell)
    {
        otherCell.IncreaseValue();
        SetValue(X, Y, 0);
    }

    public void SetValue(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;

        UpdateCell();
    }

    public void MoveToCell(Cell target)
    {
        target.SetValue(target.X, target.Y,Value);
        SetValue(X, Y, 0);
    }
    public void UpdateCell()
    {
        point.text = IsEmpty ? string.Empty: Points.ToString();

        point.color = Value <= 2 ? ColorManeger.Instance.PointsDarckColor : 
            ColorManeger.Instance.PointsLightColor;
        image.color = ColorManeger.Instance.CellColors[Value]; 
    }
}