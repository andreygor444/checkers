using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMoveMarker : MonoBehaviour
{
    public void Init(int x, int y)
    {
        transform.position = new Vector3(x * Constants.cellSize - Constants.boardWidth_f / 2, y * Constants.cellSize - Constants.boardHeight_f / 2, 0);
    }
}
