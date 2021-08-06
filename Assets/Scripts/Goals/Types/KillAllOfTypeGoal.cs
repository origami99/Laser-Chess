using System;
using System.Linq;
using UnityEngine;

public class KillAllOfTypeGoal : KillGoal
{
    [SerializeField] private Piece _piecePrefab;

    public override bool IsComplete()
    {
        Type pieceType = _piecePrefab.GetType();

        return !base.Board.Pieces.Any(p => p.GetType() == pieceType);
    }
}
