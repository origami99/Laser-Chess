using System.Linq;

public class Drone : EnemyPiece
{
    protected override (Tile, Piece[]) FindBestTurn()
    {
        Tile tile = base.PossibleMoves.SingleOrDefault();
        Piece target = tile != null ? FindBestTarget(tile.CurrentY, tile.CurrentX) : null;

        return (tile, new Piece[] { target });
    }
}
