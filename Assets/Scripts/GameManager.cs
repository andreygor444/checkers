using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Checker checker { get; set; }
    public int x { get; set; }
    public int y { get; set; }
#nullable enable
    public Checker? victim { get; set; }
#nullable disable
}

public class GameManager : MonoBehaviour
{
    const int boardSize = 8, none = -1, possibleMove = -2;
    const bool white = true, black = false;
    public Checker checkerPrefab;
    public Checker[] checkers;
    public PossibleMoveMarker moveMarkerPrefab;
    public List<PossibleMoveMarker> moveMarkers;
    bool currentTurnColor = white, canAnyoneKill = false;
#nullable enable
    private Checker? attacker = null;
#nullable disable
    int[][] field;

    void Start()
    {
        checkers = new Checker[24];
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
            for (int j = 0; j < 3; ++j)
            {
                if (i % 2 == 0 && j % 2 == 0 || i % 2 == 1 && j % 2 == 1)
                {
                    checkers[currentCheckerInd] = Instantiate(checkerPrefab, new Vector3(0, 0, 0),
                        Quaternion.identity) as Checker;
                    checkers[currentCheckerInd].Init(i, j, white, currentCheckerInd, this);
                    field[i][j] = currentCheckerInd++;
                    checkers[currentCheckerInd] = Instantiate(checkerPrefab, new Vector3(0, 0, 0),
                        Quaternion.identity) as Checker;
                    checkers[currentCheckerInd]
                        .Init(boardSize - i - 1, boardSize - j - 1, black, currentCheckerInd, this);
                    field[boardSize - i - 1][boardSize - j - 1] = currentCheckerInd++;
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

    List<Move> GetPossibleMoves(Checker checker)
    {
        List<Move> moves = new List<Move>();
        Pair<int, int> pos = checker.GetPos();
        int x = pos.first, y = pos.second, x_, y_;
#nullable enable
        Checker? victim;
#nullable disable
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                victim = null;
                x_ = x + i;
                y_ = y + j;
                if (x_ < 0 || x_ >= boardSize || y_ < 0 || y_ >= boardSize ||
                    (field[x_][y_] >= 0 && checkers[field[x_][y_]].GetColor() == checker.GetColor()))
                    continue;
                if (field[x_][y_] != none && field[x_][y_] != possibleMove)
                {
                    victim = checkers[field[x_][y_]];
                    x_ += i;
                    y_ += j;
                    if (x_ < 0 || x_ >= boardSize || y_ < 0 || y_ >= boardSize || field[x_][y_] != none)
                        continue;
                }

                if (victim == null)
                {
                    if (checker.GetColor() == white && j == -1 || checker.GetColor() == black && j == 1)
                        continue;
                }

                moves.Add(new Move{checker=checker, x=x_, y=y_, victim=victim});
            }
        }

        return moves;
    }

    void showPossibleMoves(Checker checker)
    {
        if (attacker != null && attacker != checker)
            return;
        List<Move> moves = GetPossibleMoves(checker);
        if (canAnyoneKill && !CanKill(moves)) {
            return;
        }

        DestroyPossibleMoveMarkers();
        foreach (var move in moves)
        {
            if (canAnyoneKill && move.victim == null)
                continue;
            field[move.x][move.y] = possibleMove;
            moveMarkers.Add(
                Instantiate(moveMarkerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as PossibleMoveMarker);
            moveMarkers[moveMarkers.Count - 1].Init(move.x, move.y, checker, this, move.victim);
        }


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
            canAnyoneKill = CanAnyoneKill();
            return;
        }

        Pair<int, int> victimPos = victim.GetPos();
        victim.Kill();
        x = victimPos.first;
        y = victimPos.second;
        Destroy(victim.gameObject);
        field[x][y] = none;
        if (CanKill(GetPossibleMoves(checker)))
        {
            SelectChecker(checker);
            attacker = checker;
            return;
        }

        currentTurnColor = !currentTurnColor;
        canAnyoneKill = CanAnyoneKill();
        attacker = null;
    }
#nullable disable

    bool CanKill(List<Move> moves)
    {
        foreach (var move in moves)
        {
            if (move.victim != null)
                return true;
        }

        return false;
    }

    bool CanAnyoneKill()
    {
        bool ans = false;
        foreach (var checker in checkers)
        {
            if (checker.IsAlive() && checker.GetColor() == currentTurnColor && CanKill(GetPossibleMoves(checker)))
            {
                ans = true;
                break;
            }
        }

        return ans;
    }
}