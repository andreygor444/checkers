using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PossibleMoveMarker : MonoBehaviour, IPointerClickHandler
{
    private int x, y;
    private GameManager gm;
    private Checker checker;
#nullable enable
    private Checker? victim;
    public void Init(int x, int y, Checker checker, GameManager gm, Checker? victim = null)
    {
        this.x = x;
        this.y = y;
        this.checker = checker;
        this.gm = gm;
        this.victim = victim;
        transform.position = new Vector3(x * Constants.cellSize - Constants.boardWidth_f / 2, y * Constants.cellSize - Constants.boardHeight_f / 2, 0);
    }
#nullable disable
    
    public Pair<int, int> GetPos()
    {
        return new Pair<int, int>(x, y);
    }
    
    public void OnPointerClick(PointerEventData data)
    {
        this.gm.Move(checker, this, victim);
    }
}
