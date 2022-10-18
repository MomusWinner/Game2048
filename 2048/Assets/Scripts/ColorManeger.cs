using UnityEngine;



public class ColorManeger : MonoBehaviour
{
    public static ColorManeger Instance;

    public Color[] CellColors;
    [Space(5)]

    public Color PointsDarckColor;
    public Color PointsLightColor;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
