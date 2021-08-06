public class CommandUnit : EnemyPiece
{
    protected override (Tile, Piece[]) FindBestTurn()
    {
        Tile tile = FindSafestMove();

        return (tile, null);
    }
}
