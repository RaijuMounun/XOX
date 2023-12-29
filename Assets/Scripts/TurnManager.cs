using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    GameObject tileParent;

    public int Turn { get; set; }
    public Sprite xSprite, oSprite;

    public List<TileCon> Tilecons => tileCons;
    [SerializeField] List<TileCon> tileCons;

    public bool isGameStarted;

    //UI
    [SerializeField] Canvas canvas;
    [SerializeField] Text startText, winnerText;
    [SerializeField] Button replayButton;



    private void Awake()
    {
        if (Instance == null) Instance = this;

        tileParent = GameObject.Find("Tiles");
        foreach (TileCon item in tileParent.transform.GetComponentsInChildren<TileCon>()) tileCons.Add(item);
    }

    public void OnGameStart()
    {
        isGameStarted = true;
        canvas.enabled = false;

        replayButton.gameObject.SetActive(false);
        winnerText.enabled = false;
    }


    public void OnClickReplay()
    {
        OnGameStart();
        foreach (TileCon item in tileCons) item.SetState(TileType.Empty);
    }

    public void OnGameEnd(TileType _type)
    {
        isGameStarted = false;

        canvas.enabled = true;
        startText.enabled = false;
        winnerText.enabled = true;

        string result = "";
        if (_type == TileType.Empty) result = "TIE!";
        else result = _type.ToString() + " WINS!";

        winnerText.text = result;

        replayButton.gameObject.SetActive(true);
    }

    public (bool, TileType) CheckWin()
    {
        foreach (var tile in tileCons) // Tile'larımızı gezdiriyoruz
        {
            if (tile.state == TileType.Empty) continue; // Eğer tile boş ise devam et

            foreach (var dir in Enum.GetValues(typeof(DirectionEnum))) // Her tile için enum'daki tüm değerleri gezdiriyoruz
            {
                TileCon next = tile.GetNextTile((DirectionEnum)dir); // Tile'ımızın şu an baktığımız yöndeki komşu tile'ını alıyoruz
                if (!next) continue; // Eğer komşu tile yoksa devam et

                if (next.state != tile.state) continue; // Eğer komşu tile'ın state'i şu anki tile'ın state'inden farklıysa devam et

                TileCon next2 = next.GetNextTile((DirectionEnum)dir); // Tile'ımızın Komşu tile'ının şu an baktığımız yöndeki komşu tile'ını alıyoruz
                if (!next2) continue; // Eğer komşunun komşusu yoksa devam et

                if (next2.state != tile.state) continue; // Eğer komşunun komşusunun state'i şu anki tile'ın state'inden farklıysa devam et

                return (true, tile.state); // Eğer yukarıdaki koşulların hiçbiri sağlanmıyorsa oyun kazanılmıştır
            }
        }
        return (false, TileType.Empty);
    }

    public bool HasEmptyTile() => return tileCons.Exists(x => x.state == TileType.Empty);
}
