using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    const int boardSize = 8, white = 1, black = -1, none = 0, possibleMove = 2;
    Checker[] checkerPrefabs;
    Checker[] checkers;
    List<PossibleMoveMarker> moveMarkerPrefabs;
    List<PossibleMoveMarker> moveMarkers;
    int currentTurnColor = white;
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
                    checkers[currentCheckerInd++].Init(i, j, white, this);
                    checkers[currentCheckerInd] = Instantiate(checkerPrefabs[currentCheckerInd], new Vector3(0, 0, 0), Quaternion.identity) as Checker;
                    checkers[currentCheckerInd++].Init(i, boardSize-j-1, black, this);
                    field[i][j] = white;
                    field[i][boardSize-j-1] = black;
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

    void showPossibleMoves(Checker checker)
    {
        Pair<int, int> pos = checker.GetPos();
        int x = pos.first, y = pos.second, x_, y_;
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                x_ = x + i;
                y_ = y + j;
                if (x_ < 0 || x_ >= boardSize || y_ < 0 || y_ >= boardSize || field[x_][y_] == checker.GetColor())
                    continue;
                if (field[x_][y_] != 0)
                {
                    x_ += i;
                    y_ += j;
                    if (field[x_][y_] != none)
                        continue;
                }

                field[x_][y_] = possibleMove;
                moveMarkerPrefabs.Add(new PossibleMoveMarker());
                moveMarkers.Add(new PossibleMoveMarker());
                moveMarkers[moveMarkers.Count - 1] = Instantiate(moveMarkerPrefabs[moveMarkerPrefabs.Count - 1], new Vector3(0, 0, 0), Quaternion.identity) as PossibleMoveMarker;
                moveMarkers[moveMarkers.Count - 1].Init(x_, y_);
            }
        }
        if (checker.IsKing())
        {
            
        }
    }
}
