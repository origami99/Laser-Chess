using System.Collections.Generic;

public class Dreadnought : EnemyPiece
{
    protected override (Tile, Piece[]) FindBestTurn()
    {
        Piece closestOpponent = FindClosestOpponent(base.CurrentY, base.CurrentX);
        Tile tile = FindMoveTowardsOpponent(closestOpponent);

        List<Piece> targets = GetPossibleTargets(tile.CurrentY, tile.CurrentX);

        return (tile, targets.ToArray());
    }
}
