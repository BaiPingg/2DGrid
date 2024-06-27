using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class GridService : ServiceBase
{
    [SerializeField, Required] GameObject _viewer;

    // [SerializeField, Required] GameObject _placementObj;
    // [SerializeField, Required] Material _previewMaterial;
    [SerializeField] private int _width = 2;

    [SerializeField] private int _height = 2;

    [SerializeField] float _CellSize = 1f;

    private Grid<GridItem> _grid;

    public Grid<GridItem> Grid => _grid;

    // private GameObject _preView;
    private List<GridItem> _openList;
    private List<GridItem> _closedList;


    public override void Init()
    {
        base.Init();
        _viewer.transform.localScale = new Vector3(_width * _CellSize / 10, 0, _height * _CellSize / 10);
        _viewer.GetComponent<Renderer>().material.SetVector("_WidthAndHeight", new Vector2(_width, _height));
        _viewer.GetComponent<Renderer>().material.SetFloat("_CellSize", _CellSize);

        _grid = new Grid<GridItem>(_width, _height, _CellSize,
            _viewer.transform.position - new Vector3(_width * _CellSize / 2, _viewer.transform.position.y,
                _height * _CellSize / 2),
            (grid, x, y) =>
            {
                GridItem item = new GridItem(x, y, grid);
                return item;
            });
    }


    // private Vector3 startPos;
    // private Vector3 endPos;

    // private void Update()
    // {
    //     if (_inputManager.TryGetMouseWorldPos(out Vector3 pos))
    //     {
    //         _preView.gameObject.SetActive(true);
    //         if (Mouse.current.leftButton.wasPressedThisFrame)
    //         {
    //             startPos = pos;
    //         }
    //
    //         if (Mouse.current.middleButton.wasPressedThisFrame)
    //         {
    //             endPos = pos;
    //         }
    //
    //         if (Mouse.current.backButton.wasPressedThisFrame)
    //         {
    //             var path = FindPath(startPos, endPos);
    //             var pre = path[0];
    //             for (int i = 1; i < path.Count; i++)
    //             {
    //                 Debug.DrawLine(pre, path[i], Color.magenta, 100f);
    //                 pre = path[i];
    //             }
    //         }
    //
    //         var cell = gridBool.GetCellPosition(pos);
    //         var sss = gridBool.GetWorldPosition(cell.x, cell.y);
    //         _preView.gameObject.transform.position = sss;
    //         if (Mouse.current.rightButton.wasPressedThisFrame)
    //         {
    //             Instantiate(_placementObj, sss, Quaternion.identity);
    //             gridBool.GetValue(pos).Placed = true;
    //         }
    //     }
    //     else
    //     {
    //         _preView.gameObject.SetActive(false);
    //     }
    // }


    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        var from = _grid.GetCellPosition(startWorldPosition);
        var to = _grid.GetCellPosition(endWorldPosition);

        var path = new List<GridItem>();
        if (TryFindPath(from, to, out path))
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (GridItem pathNode in path)
            {
                var point = _grid.GetWorldPosition(pathNode.x, pathNode.y) +
                            new Vector3(_grid.CellSize / 2, 0, _grid.CellSize / 2);
                vectorPath.Add(point);
            }

            return vectorPath;
        }

        return null;
    }

    public bool TryFindPath(Vector2Int form, Vector2Int to, out List<GridItem> result)
    {
        result = new List<GridItem>();
        result.Clear();
        GridItem startItem = _grid.GetValue(form.x, form.y);
        GridItem endItem = _grid.GetValue(to.x, to.y);
        _openList = new List<GridItem>() { startItem };
        _closedList = new List<GridItem>();
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Height; j++)
            {
                var node = _grid.GetValue(i, j);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromItem = null;
            }
        }

        startItem.gCost = 0;
        startItem.hCost = GridItem.CalculateDistanceCost(startItem, endItem);
        startItem.CalculateFCost();
        while (_openList.Count > 0)
        {
            GridItem currentItem = GetLowestFCostNode(_openList);
            if (currentItem == endItem)
            {
                result = CalculatePath(endItem);
                return true;
            }

            _openList.Remove(currentItem);
            _closedList.Add(currentItem);
            foreach (GridItem neighbourNode in GetNeighbourList(currentItem))
            {
                if (_closedList.Contains(neighbourNode)) continue;
                if (neighbourNode.Placed)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentItem.gCost + GridItem.CalculateDistanceCost(currentItem, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromItem = currentItem;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = GridItem.CalculateDistanceCost(neighbourNode, endItem);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        return false;
    }

    private List<GridItem> GetNeighbourList(GridItem currentItem)
    {
        List<GridItem> neighbourList = new List<GridItem>();

        if (currentItem.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(_grid.GetValue(currentItem.x - 1, currentItem.y));
            // Left Down
            //if (currentItem.y - 1 >= 0) neighbourList.Add(gridBool.GetValue(currentItem.x - 1, currentItem.y - 1));
            // Left Up
            // if (currentItem.y + 1 < gridBool.Height)
            //     neighbourList.Add(gridBool.GetValue(currentItem.x - 1, currentItem.y + 1));
        }

        if (currentItem.x + 1 < _grid.Width)
        {
            // Right
            neighbourList.Add(_grid.GetValue(currentItem.x + 1, currentItem.y));
            // Right Down
            //if (currentItem.y - 1 >= 0) neighbourList.Add(gridBool.GetValue(currentItem.x + 1, currentItem.y - 1));
            // Right Up
            // if (currentItem.y + 1 < gridBool.Height)
            //     neighbourList.Add(gridBool.GetValue(currentItem.x + 1, currentItem.y + 1));
        }

        // Down
        if (currentItem.y - 1 >= 0) neighbourList.Add(_grid.GetValue(currentItem.x, currentItem.y - 1));
        // Up
        if (currentItem.y + 1 < _grid.Height) neighbourList.Add(_grid.GetValue(currentItem.x, currentItem.y + 1));

        return neighbourList;
    }

    private List<GridItem> CalculatePath(GridItem endItem)
    {
        List<GridItem> path = new List<GridItem>();
        path.Add(endItem);
        GridItem currentNode = endItem;
        while (currentNode.cameFromItem != null)
        {
            path.Add(currentNode.cameFromItem);
            currentNode = currentNode.cameFromItem;
        }

        path.Reverse();
        return path;
    }

    private GridItem GetLowestFCostNode(List<GridItem> pathNodeList)
    {
        GridItem lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}