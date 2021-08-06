using System.Collections.Generic;

public class Jumpship : PlayerPiece
{
    protected override List<Tile> GetCustomMovement(int y, int x)
    {
        List<Tile> moves = new List<Tile>();

        TryAddTile(ref moves, y + 2, x - 1); // Up left
        TryAddTile(ref moves, y + 2, x + 1); // Up right

        TryAddTile(ref moves, y - 2, x - 1); // Down left
        TryAddTile(ref moves, y - 2, x + 1); // Down right

        TryAddTile(ref moves, y + 1, x - 2); // Left up
        TryAddTile(ref moves, y - 1, x - 2); // Left down

        TryAddTile(ref moves, y + 1, x + 2); // Right up
        TryAddTile(ref moves, y - 1, x + 2); // Right down

        return moves;
    }

    private void TryAddTile(ref List<Tile> tiles, int y, int x)
    {
        Tile tile = base.Board[y, x];

        if (tile != null && !tile.IsBlocked(out Piece _))
        {
            tiles.Add(tile);
        }
    }
}
