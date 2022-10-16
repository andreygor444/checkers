using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Checker : MonoBehaviour, IPointerClickHandler
{
    private const bool white = true, black = false;
    private int x, y, id;
    private bool color;
    private bool isKing = false;
    private GameManager gm;
    public void Init(int x, int y, bool color, int id, GameManager gm)
    {
        SetPos(x, y);
        SetColor(color);
        this.id = id;
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

    private void SetColor(bool color)
    {
        this.color = color;
        Color color_ = color == white ? Color.white : Color.grey;
        this.GetComponent<Renderer>().material.SetColor("_Color", color_);
    }

    public bool GetColor()
    {
        return color;
    }

    public int GetId()
    {
        return id;
    }

    public bool IsKing()
    {
        return isKing;
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (x + 1 >= 0 && x + 1 < 8 && y + 1 >= 0 && y + 1 < 8)
        {
            gm.SelectChecker(this);
        }
    }

    void Update()
    {
        
    }
}
