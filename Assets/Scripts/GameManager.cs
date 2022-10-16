using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    const int boardSize = 8, none = -1, possibleMove = -2;
    const bool white = true, black = false;
    public Checker[] checkerPrefabs;
    public Checker[] checkers;
    public PossibleMoveMarker moveMarkerPrefab;
    public List<PossibleMoveMarker> moveMarkers;
    bool currentTurnColor = white;
    int[][] field;
    void Start()
    {
        checkers = new Checker[checkerPrefabs.Length];
        int currentCheckerInd = 0;
        field = new int[boardSize][];
        for (int i = 0; i < boardSize; ++i)
        {
            field[i] = new int[boardSize];
            for (int j = 0; j < boardSize; ++j)
            {
                field[i][j] = none;
            }
        }
        for (int i = 0; i < boardSize; ++i)
        {
            for (int j = 0;j < 3;++j)
            {
                if (i % 2 == 0 && j % 2 == 0 || i % 2 == 1 && j % 2 == 1)
                {
                    checkers[currentCheckerInd] = Instantiate(checkerPrefabs[currentCheckerInd], new Vector3(0, 0, 0), Quaternion.identity) as Checker;
                    checkers[currentCheckerInd].Init(i, j, white, currentCheckerInd, this);
                    field[i][j] = currentCheckerInd++;
                    checkers[currentCheckerInd] = Instantiate(checkerPrefabs[currentCheckerInd], new Vector3(0, 0, 0), Quaternion.identity) as Checker;
                    checkers[currentCheckerInd].Init(boardSize-i-1, boardSize-j-1, black, currentCheckerInd, this);
                    field[boardSize-i-1][boardSize-j-1] = currentCheckerInd++;
                }
            }
        }

        currentTurnColor = white;
    }

    public void SelectChecker(Checker checker)
    {
        if (checker.GetColor() != currentTurnColor)
        {
            return;
        }

        showPossibleMoves(checker);
    }
    
    void DestroyPossibleMoveMarkers()
    {
        foreach (var moveMarker in moveMarkers)
        {
            Pair<int, int> pos = moveMarker.GetPos();
            field[pos.first][pos.second] = none;
            Destroy(moveMarker.gameObject);
        }
        moveMarkers.Clear();
    }

    void showPossibleMoves(Checker checker)
    {
        DestroyPossibleMoveMarkers();
        Pair<int, int> pos = checker.GetPos();
        int x = pos.first, y = pos.second, x_, y_;
#nullable enable
        Checker? victim;
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                victim = null;
                x_ = x + i;
                y_ = y + j;
                if (x_ < 0 || x_ >= boardSize || y_ < 0 || y_ >= boardSize || (field[x_][y_] >= 0 && checkers[field[x_][y_]].GetColor() == checker.GetColor()))
                    continue;
                if (field[x_][y_] != none)
                {
                    victim = checkers[field[x_][y_]];
                    x_ += i;
                    y_ += j;
                    if (field[x_][y_] != none)
                        continue;
                }

                field[x_][y_] = possibleMove;
                moveMarkers.Add(Instantiate(moveMarkerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as PossibleMoveMarker);
                moveMarkers[moveMarkers.Count - 1].Init(x_, y_, checker, this, victim);
            }
        }
#nullable disable
        if (checker.IsKing())
        {
            
        }
    }

#nullable enable
    public void Move(Checker checker, PossibleMoveMarker moveMarker, Checker? victim = null)
    {
        DestroyPossibleMoveMarkers();
        Pair<int, int> checkerPos = checker.GetPos();
        Pair<int, int> movePos = moveMarker.GetPos();
        int x = checkerPos.first, y = checkerPos.second, x_ = movePos.first, y_ = movePos.second;
        checker.SetPos(x_, y_);
        field[x][y] = none;
        field[x_][y_] = checker.GetId();
        if (victim == null)
        {
            currentTurnColor = !currentTurnColor;
            return;
        }
        Pair<int, int> victimPos = victim.GetPos();
        x = victimPos.first;
        y = victimPos.second;
        Destroy(victim.gameObject);
        field[x][y] = none;
        SelectChecker(checker);
    }
#nullable disable
}
