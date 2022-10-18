using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static int Points { get; private set; }
    public static bool GameStart { get; private set; }

    [SerializeField]
    private TextMeshProUGUI gameResult;
    [SerializeField]
    private TextMeshProUGUI pointText;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void Win()
    {
        GameStart = false;
        gameResult.text = "You win!!!";
    }
    public void Lose()
    {
        GameStart = false;
        gameResult.text = "You lose:(";
    }

    public void StartGame()
    {
        Field.Instance.GenerateField();
        gameResult.text = "";
        GameStart = true;
        SetPoints(0);
        
    }

    public void AddPoints(int point)
    {
        SetPoints(Points + point);
    }

    private void SetPoints(int point)
    {
        Points = point;
        pointText.text = Points.ToString();
    }

}
