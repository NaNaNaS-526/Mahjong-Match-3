using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoardGenerator : MonoBehaviour
{
    public event Action OnTilesOver;

    [SerializeField] private Tile[] tilesPrefabs;
    [SerializeField] private SpriteRenderer gameBoard;

    [SerializeField] private int layersAmount;
    [SerializeField] private int bottomLayerHeight;
    [SerializeField] private int bottomLayerWidth;

    private List<Tile[,]> _createdTiles;

    private float _leftBoardBound;
    private float _bottomBoardBound;

    private const float BaseOffset = 0.5f;

    private int _tilesOnBoardAmount;
    private int _currentTileNumber;

    private void Awake()
    {
        InitializeGameBoardBounds();
        InitializeCreatedTileArray();
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < _createdTiles.Count; i++)
        {
            int rows = _createdTiles[i].GetUpperBound(0) + 1;
            int columns = _createdTiles[i].Length / rows;
            float offset = i * BaseOffset;

            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; k++)
                {
                    float leftPoint = _leftBoardBound + j + BaseOffset + offset;
                    float bottomPoint = _bottomBoardBound + k + BaseOffset + offset;
                    Vector3 newTilePosition = new Vector3(leftPoint, bottomPoint, i * -0.01f);

                    CreateTile(newTilePosition, i, j, k);
                }
            }
        }
    }

    private void CreateTile(Vector3 spawnPosition, int layer, int x, int y)
    {
        Tile randomTile = tilesPrefabs[_currentTileNumber];
        var newTile = Instantiate(randomTile, spawnPosition, Quaternion.identity);
        _createdTiles[layer][x, y] = newTile;
        if (layer != 0)
        {
            CloseTiles(newTile, layer, x, y);
        }

        _currentTileNumber++;
    }

    private void CloseTiles(Tile tile, int layer, int x, int y)
    {
        List<Tile> newClosedTiles = new List<Tile>
        {
            _createdTiles[layer - 1][x, y],
            _createdTiles[layer - 1][x + 1, y],
            _createdTiles[layer - 1][x, y + 1],
            _createdTiles[layer - 1][x + 1, y + 1]
        };
        foreach (var closedTile in newClosedTiles.Where(closedTile => closedTile != null))
        {
            closedTile.AddClosingTile(tile);
        }
    }

    private void InitializeCreatedTileArray()
    {
        _createdTiles = new List<Tile[,]>();
        for (int i = 0; i < layersAmount; i++)
        {
            var layer = new Tile[bottomLayerWidth - i, bottomLayerHeight - i];
            _createdTiles.Add(layer);
            _tilesOnBoardAmount += layer.Length;
        }

        tilesPrefabs = CorrectTileArrayCreator.CreateCorrectTileArray(tilesPrefabs, _tilesOnBoardAmount);
    }

    private void InitializeGameBoardBounds()
    {
        gameBoard.transform.localScale = new Vector2(bottomLayerWidth, bottomLayerHeight);
        var bounds = gameBoard.bounds;
        _leftBoardBound = bounds.min.x;
        _bottomBoardBound = bounds.min.y;
    }

    public List<Tile[,]> GetCreatedTiles()
    {
        return _createdTiles;
    }

    public (int width, int height) GetGameBoardSizes()
    {
        return (bottomLayerWidth, bottomLayerHeight);
    }

    public void ChangeTilesOnBoardAmount(int addedValue)
    {
        _tilesOnBoardAmount += addedValue;
        if (_tilesOnBoardAmount <= 0)
        {
            OnTilesOver?.Invoke();
        }
    }
}