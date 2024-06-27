using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Grid<T>
{
    private int _width;
    private int _height;
    public int Width => _width;
 
    public int Height => _height;
    private float _cellSize;
    public float CellSize => _cellSize;
    private T[,] _gridArray;

    private Vector3 _originPosition;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> factory)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        _gridArray = new T[_width, _height];
        for (int i = 0; i < _gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < _gridArray.GetLength(1); j++)
            {
                _gridArray[i, j] = factory.Invoke(this, i, j);
            }
        }

        for (int i = 0; i < _gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < _gridArray.GetLength(1); j++)
            {
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

   
    public T GetValue(Vector3 worldPosition)
    {
        var v2 = GetCellPosition(worldPosition);
        return GetValue(v2.x, v2.y);
    }

    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _gridArray[x, y];
        }

        return default(T);
    }

    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
        }
    }

    public void SetValue(Vector2Int pos, T value)
    {
        SetValue(pos.x, pos.y, value);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * _cellSize + _originPosition;
    }

    public Vector2Int GetCellPosition(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos - _originPosition).x / _cellSize);
        int y = Mathf.FloorToInt((worldPos - _originPosition).z / _cellSize);

        return new Vector2Int(x, y);
    }

    public bool TryFindPath(Vector2Int form, Vector2Int to, List<T> result)
    {
        result.Clear();

        return true;
    }

    public void CreateText(int x, int y)
    {
        var worldPos = GetWorldPosition(x, y);
        var go = new GameObject();
        go.transform.position = worldPos + new Vector3(0, 0.5f, 0);
        var text = go.AddComponent<TextMeshPro>();
        text.fontSize = 2;
        text.transform.eulerAngles = new Vector3(90, 0, 0);
        text.alignment = TextAlignmentOptions.Center;
        var rect = text.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 0);
        text.text = _gridArray[x, y].ToString();
        //   _gridArray[x, y]._textMeshPro = text;
    }
}