using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    public TileType type;

    public event Action<Tile> OnTileClicked;

    [SerializeField] private Color closedTileColor = new(0.0f, 0.0f, 0.0f, 0.5f);
    private bool _isOpened = true;
    private bool _isClicked;

    private static float lastClickTime;

    private bool IsOpened
    {
        get => _isOpened;
        set
        {
            if (value == false) _renderer.color = closedTileColor;
            _isOpened = value;
        }
    }


    public List<Tile> closingTiles;

    private SpriteRenderer _renderer;
    private Color _baseColor;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _baseColor = _renderer.color;
    }

    public void AddClosingTile(Tile tile)
    {
        closingTiles.Add(tile);
        tile.OnTileClicked += RemoveClosingTile;
        IsOpened = false;
    }

    private void RemoveClosingTile(Tile tile)
    {
        closingTiles.Remove(tile);
        CheckClosingTiles();
    }

    private void CheckClosingTiles()
    {
        if (closingTiles.Count != 0) return;
        OpenTile();
    }

    private void OpenTile()
    {
        _renderer.color = _baseColor;
        IsOpened = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsOpened && !_isClicked && Time.time - lastClickTime > 1f)
        {
            OnTileClicked?.Invoke(this);
            _isClicked = true;
            lastClickTime = Time.time;
        }
    }
}