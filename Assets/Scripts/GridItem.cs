using TMPro;
using UnityEngine;

public class GridItem
{
    public const int MOVE_DIAGONAL_COST = 14; //对角线
    public const int MOVE_STRAIGHT_COST = 10; //直线

    public Grid<GridItem> grid;
    public int x, y;
    public int gCost; //从开始节点的值总和
    public int hCost; // 到终点的值总和
    public int fCost; //最终值
    public TextMeshPro _textMeshPro;
    private bool placed;

    public GridItem cameFromItem;

    public bool Placed
    {
        get => placed;
        set
        {
            placed = value;
            _textMeshPro.text = ToString();
        }
    }

    public GridItem(int x, int y, Grid<GridItem> grid)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        var worldPos = grid.GetWorldPosition(x, y);
        var go = new GameObject();
        go.transform.position = worldPos + new Vector3(0, 0.08f, 0);
        go.transform.SetParent(SL.Get<GridService>().gameObject.transform);
        var text = go.AddComponent<TextMeshPro>();
        text.fontSize = 2;
        text.transform.eulerAngles = new Vector3(90, 0, 0);
        text.alignment = TextAlignmentOptions.Center;
        var rect = text.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 0);
        text.text = ToString();
        _textMeshPro = text;
    }

    public override string ToString()
    {
        return $"({x},{y})  {placed}";
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public static int CalculateDistanceCost(GridItem a, GridItem b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remain = Mathf.Abs(xDistance - yDistance);

        //斜线然后直线
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remain;
    }
}