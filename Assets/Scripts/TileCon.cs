using UnityEngine;
using UnityEngine.EventSystems;

public class TileCon : MonoBehaviour, IPointerDownHandler
{
    SpriteRenderer sprRdr;
    TurnManager turnManager;
    public TileType state;
    public Vector2 coords;

    private void Awake()
    {
        turnManager = TurnManager.Instance;
        sprRdr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state != TileType.Empty) return; //tile boş değilse dön

        if (!turnManager.isGameStarted) turnManager.OnGameStart(); //oyun başlamamışsa başlat

        TileType _type = turnManager.Turn % 2 == 0 ? TileType.X : TileType.O;
        SetState(_type);
        TurnManager.Instance.Turn++;

        var result = turnManager.CheckWin();
        bool hasWinner = result.Item1;

        if (hasWinner)
        {
            turnManager.OnGameEnd(result.Item2);
            return;
        }

        //hasWinner false ise;
        if (turnManager.HasEmptyTile()) return; //ve boş tile yoksa:
        turnManager.OnGameEnd(TileType.Empty); //oyunu berabere bitir
    }



    public void SetState(TileType _state)
    {
        state = _state;
        sprRdr.sprite = state == TileType.X ? turnManager.xSprite : turnManager.oSprite;
        if (state == TileType.Empty) sprRdr.sprite = null;
    }



    public TileCon GetNextTile(DirectionEnum _direction)
    {
        Vector2 _nextTile = coords;

        if (_direction.ToString().Contains("Up")) _nextTile.y++;
        if (_direction.ToString().Contains("Right")) _nextTile.x++;
        if (_direction.ToString().Contains("Down")) _nextTile.y--;
        if (_direction.ToString().Contains("Left")) _nextTile.x--;

        return turnManager.Tilecons.Find(tile => tile.coords == _nextTile);
    }
}

public enum TileType
{
    Empty, X, O
}

public enum DirectionEnum
{
    Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
}
