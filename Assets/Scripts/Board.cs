using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private PiecesListSO _piecesSO;

    [Header("Generation info")]
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private float _tilesOffset = 3f;

    private LevelData _level;

    private Tile[,] _tiles;

    public Tile this[int y, int x]
    {
        get
        {
            if (MatrixUtils.IsInBound(_tiles, y, x))
            {
                return _tiles[y, x];
            }

            return null;
        }
        private set 
        {
            if (MatrixUtils.IsInBound(_tiles, y, x))
            {
                _tiles[y, x] = value;
            }
        }
    }

    public int Height => _level.Height;
    public int Width => _level.Width;

    public ICollection<Piece> Pieces { get; } = new List<Piece>();
    public IEnumerable<PlayerPiece> PlayerPieces => Pieces.OfType<PlayerPiece>();
    public IEnumerable<EnemyPiece> EnemyPieces => Pieces.OfType<EnemyPiece>();

    private void Awake()
    {
        _level = JsonSerializer.Load<LevelData>(Paths.GetLevelPath(_gameState.SelectedLevel));

        GenerateBoard();
    }

    private void GenerateBoard()
    {
        _tiles = new Tile[this.Height, this.Width];

        for (int y = 0; y < this.Height; y++)
        {
            for (int x = 0; x < this.Width; x++)
            {
                float xPos = x * _tilesOffset;
                float zPos = (this.Height - y - 1) * _tilesOffset;
                Vector3 tilePos = new Vector3(xPos, 0, zPos);

                Tile tile = Instantiate(_tilePrefab, tilePos, Quaternion.identity, this.transform);

                tile.Board = this;
                tile.CurrentY = y;
                tile.CurrentX = x;

                string pieceName = _level.PiecesMap[y, x];
                Piece piecePrefab = _piecesSO.PiecesPrefabs.SingleOrDefault(p => p.GetType().Name == pieceName);

                if (piecePrefab != null)
                {
                    Piece piece = Instantiate(piecePrefab, tile.PieceHolder.position, Quaternion.identity, parent: tile.PieceHolder);

                    piece.Board = this;
                    piece.Mobility.Place(tile);

                    this.Pieces.Add(piece);
                }

                this[y, x] = tile;
            }
        }
    }
}
