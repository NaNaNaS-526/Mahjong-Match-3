using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    public event Action OnTileMoved;

    [SerializeField] private Color closedTileColor = new(0.0f, 0.0f, 0.0f, 0.5f);
    private bool _isOpened = true;

    private bool IsOpened
    {
        get => _isOpened;
        set
        {
            if (value == false)
            {
                _renderer.color = closedTileColor;
            }

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
        tile.OnTileMoved += () => RemoveClosingTile(tile);
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

    private void MoveTile()
    {
        transform.position = Vector3.zero;
        OnTileMoved?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsOpened) MoveTile();
    }
}