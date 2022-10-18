using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Field Priperties")]
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int BeginningInitCellsCount;

    [Space(10)]
    [SerializeField]
    private Cell cellPref;
    [SerializeField]
    private RectTransform rect;

    private Cell[,] field;

    public bool anyCellMoved;



    private void Update()
    {     
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
    }
    public void OnInput(Vector2 direction)
    {
        if (!GameController.GameStart)
            return;
        anyCellMoved = false;
        ResetCellsFlagsMerged();

        Move(direction);
        if (anyCellMoved)
        {
            GenerateRandomCell();
            CheckGameResult();
        }
    }
    
    public void Move(Vector2 direction)
    {
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -= dir)
            {
                var cell = direction.x != 0 ? field[k, i] : field[i, k];
                if (cell.IsEmpty)
                    continue;
                var cellToMerge = FindCellToMegrge(cell, direction);
                if(cellToMerge != null)
                {
                    cell.MergedWithCell(cellToMerge);
                    anyCellMoved = true;

                    continue;
                }

                var emtyCell = FindEmptyCell(cell, direction);
                if (emtyCell != null)
                {
                    cell.MoveToCell(emtyCell);
                    anyCellMoved = true;
                }
            }
        }
    }
    
    private Cell FindCellToMegrge(Cell cell, Vector2 directoin)
    {
        int startX = cell.X + (int)directoin.x;
        int startY = cell.Y - (int)directoin.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)directoin.x, y -= (int)directoin.y)
        {
            if (field[x, y].IsEmpty)
                continue;
            if(field[x, y].Value == cell.Value && !field[x,y].HasMerged)
            {
                return field[x, y];
            }
            break;
        }
        return null;
    }

    private Cell FindEmptyCell(Cell cell, Vector2 directoin)
    {
        Cell emptyCell = null;

        int startX = cell.X + (int)directoin.x;
        int startY = cell.Y - (int)directoin.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)directoin.x, y -= (int)directoin.y)
        {
            if (field[x, y].IsEmpty)
                emptyCell = field[x, y];
            else
                break;
        }
        return emptyCell;
    }

    public void CheckGameResult()
    {
        bool lose = true;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (field[x,y].Value == Cell.MaxValue)
                {
                    GameController.Instance.Win();
                    return;
                }

                if (lose &&
                    field[x,y].IsEmpty ||
                    FindCellToMegrge(field[x, y], Vector2.left) ||
                    FindCellToMegrge(field[x, y], Vector2.right) ||
                    FindCellToMegrge(field[x, y], Vector2.up) ||
                    FindCellToMegrge(field[x, y], Vector2.down))
                {
                    lose = false;
                }
            }
        }

        if (lose)
            GameController.Instance.Lose();
    }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void CreateField()
    {
        field = new Cell[FieldSize, FieldSize];

        float fieldWidwh = FieldSize * (CellSize + Spacing) + Spacing;
        rect.sizeDelta = new Vector2(fieldWidwh, fieldWidwh);

        float startX = -(fieldWidwh / 2) + (CellSize/2) + Spacing;
        float startY = (fieldWidwh / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;

                field[x, y] = cell;

                cell.SetValue(x, y, 0);
            }
        }
    }
    
    public void GenerateField()
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < BeginningInitCellsCount; i++)
        {
            GenerateRandomCell();
        }
    }

    public void GenerateRandomCell()
    {
        var emptyCells = new List<Cell>();

        for (int x = 0; x < FieldSize; x++)
        
            for (int y = 0; y < FieldSize; y++)
                if(field[x, y].IsEmpty)
                    emptyCells.Add(field[x, y]);

        if (emptyCells.Count == 0)
            throw new System.Exception("Theris no any emty cell");

        int value = Random.Range(0,10) == 0 ? 2 : 1;

        var cell = emptyCells[Random.Range(0, emptyCells.Count)];

        cell.SetValue(cell.X,cell.Y, value);
    }

    private void ResetCellsFlagsMerged()
    {
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                field[x, y].ResetFlagsMerged();
            }
        }
    }
}
