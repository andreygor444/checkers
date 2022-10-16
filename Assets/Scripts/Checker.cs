using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Checker : MonoBehaviour, IPointerClickHandler
{
    private const int white = 1, black = -1;
    private int x, y;
    private int color;
    private bool isKing = false;
    private GameManager gm;
    public void Init(int x, int y, int color, GameManager gm)
    {
        SetPos(x, y);
        SetColor(color);
        this.gm = gm;
    }

    public void SetPos(int x, int y)
    {
        this.x = x;
        this.y = y;
        transform.position = new Vector3(x * Constants.cellSize - Constants.boardWidth_f / 2, y * Constants.cellSize - Constants.boardHeight_f / 2, 0);
    }

    public Pair<int, int> GetPos()
    {
        return new Pair<int, int>(x, y);
    }

    private void SetColor(int color)
    {
        this.color = color;
        Color color_ = color == white ? Color.white : Color.black;
        this.GetComponent<Renderer>().material.SetColor("_Color", color_);
    }

    public int GetColor()
    {
        return color;
    }

    public bool IsKing()
    {
        return isKing;
    }
    public void OnPointerClick(PointerEventData data)
    {
        List<Pair<int, int>> possibleMoves = new List<Pair<int, int>>();
        if (x + 1 >= 0 && x + 1 < 8 && y + 1 >= 0 && y + 1 < 8)
        {
            //SetColor(Color.red);
            gm.SelectChecker(this);
        }
    }

    void Update()
    {
        
    }
}
