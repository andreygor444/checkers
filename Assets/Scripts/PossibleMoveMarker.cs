using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PossibleMoveMarker : MonoBehaviour, IPointerClickHandler
{
	private const float boardWidth_f = 8.75f, boardHeight_f = 8.75f, cellSize = 1.25f;
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
        transform.position = new Vector3(x * cellSize - boardWidth_f / 2, y * cellSize - boardHeight_f / 2, 0);
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
