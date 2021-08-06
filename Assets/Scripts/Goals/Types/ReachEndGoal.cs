using System;
using System.Linq;
using UnityEngine;

public class ReachEndGoal : Goal
{
    [SerializeField] private Piece _piecePrefab;

    private void Awake() => PieceMobility.OnMoved += OnPieceMoved;
    private void OnDestroy() => PieceMobility.OnMoved -= OnPieceMoved;

    private void OnPieceMoved() => base.Check = true;

    public override bool IsComplete()
    {
        Type pieceType = _piecePrefab.GetType();

        int boardEnd = base.Side == PieceSide.Player ? 0 : Board.Height - 1;

        bool complete = base.Board.Pieces
            .Where(p => p.GetType() == pieceType)
            .Any(p => p.Tile.CurrentY == boardEnd);

        return complete;
    }
}
