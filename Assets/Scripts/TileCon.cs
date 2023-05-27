using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileCon : MonoBehaviour, IPointerDownHandler
{
    public TileType state;
    [SerializeField] SpriteRenderer sprRdr;
    TurnManager turnManager;
    public Vector2 coords;

    private void Awake()
    {
        turnManager = TurnManager.Instance;
        sprRdr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (state != TileType.Empty) return;

        if (!turnManager.isGameStarted)
        {
            turnManager.OnGameStart();
        }


        TileType _type = turnManager.Turn % 2 == 0 ? TileType.X : TileType.O;
        SetState(_type);
        TurnManager.Instance.Turn++;

        var result = turnManager.CheckWin();
        var hasWinner = result.Item1;
        if (hasWinner) turnManager.OnGameEnd(result.Item2);
        else
        {
            if (!turnManager.HasEmptyTile())
            {
                turnManager.OnGameEnd(TileType.Empty);
            }

        }
    }

    public void SetState(TileType _state)
    {
        state = _state;
        //sprRdr.color = state == TileType.X ? turnManager.xColor : turnManager.oColor;
        sprRdr.sprite = state == TileType.X ? turnManager.xSprite : turnManager.oSprite;
        if (state == TileType.Empty) sprRdr.sprite = null;

    }

    public TileCon GetNextTile(DirectionEnum _direction)
    {
        Vector2 _nextTile = coords;
        switch (_direction)
        {
            case DirectionEnum.Up:
                _nextTile.y++;
                break;
            case DirectionEnum.UpRight:
                _nextTile.x++;
                _nextTile.y++;
                break;
            case DirectionEnum.Right:
                _nextTile.x++;
                break;
            case DirectionEnum.DownRight:
                _nextTile.x++;
                _nextTile.y--;
                break;
            case DirectionEnum.Down:
                _nextTile.y--;
                break;
            case DirectionEnum.DownLeft:
                _nextTile.x--;
                _nextTile.y--;
                break;
            case DirectionEnum.Left:
                _nextTile.x--;
                break;
            case DirectionEnum.UpLeft:
                _nextTile.x--;
                _nextTile.y++;
                break;
        }

        return turnManager.Tilecons.Find(tile => tile.coords == _nextTile);
    }


}

public enum TileType
{
    Empty,
    X,
    O
}

public enum DirectionEnum
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}
