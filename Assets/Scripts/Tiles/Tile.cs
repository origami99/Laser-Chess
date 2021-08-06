using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileSelection _tileSelection;

    [SerializeField] private Transform _pieceHolder;

    public TileSelection Selection => _tileSelection;

    public Transform PieceHolder => _pieceHolder;

    public Piece Piece { get; set; }
    public Board Board { get; set; }

    public bool IsEmpty => Piece == null;

    public int CurrentY { set; get; }
    public int CurrentX { set; get; }

    public bool IsBlocked(out Piece piece)
    {
        piece = this.Piece;
        return !this.IsEmpty;
    }
}
