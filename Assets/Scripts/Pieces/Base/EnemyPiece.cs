using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyPiece : Piece
{
    [SerializeField] private int _executionOrder;

    private Tile _bestMove;
    private Piece[] _bestTargets;

    public override bool CanSelect => false;

    public override PieceSide PieceSide => PieceSide.Enemy;

    public int ExecutionOrder => _executionOrder;

    public bool IsExecutingTurn { get; private set; }

    protected abstract (Tile, Piece[]) FindBestTurn();

    private float _actionDelay;

    protected override void Awake()
    {
        base.Awake();

        GoalManager.OnLevelEnd += OnLevelEnd;
        ComputerTurn.OnTurnChange += OnTurnChange;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GoalManager.OnLevelEnd -= OnLevelEnd;
        ComputerTurn.OnTurnChange -= OnTurnChange;
    }

    private void Update()
    {
        if (this.IsExecutingTurn)
        {
            if (!base.HasMoved)
            {
                base.Mobility.Move(_bestMove, delay: _actionDelay);
                base.HasMoved = true;
            }
            else if (!base.IsMoving)
            {
                if (!base.HasAttacked)
                {
                    base.Combat.Attack(targets: _bestTargets, delay: _actionDelay);
                    base.HasAttacked = true;
                }
                else if (!base.IsAttacking)
                {
                    _bestMove = null;
                    _bestTargets = null;

                    this.IsExecutingTurn = false;
                }
            }
        }
    }

    private void OnLevelEnd(bool _)
    {
        this.IsExecutingTurn = false;
    }

    public virtual void ExecuteTurn(float actionDelay)
    {
        _actionDelay = actionDelay;

        (_bestMove, _bestTargets) = FindBestTurn();

        this.IsExecutingTurn = true;
    }

    protected virtual Piece FindBestTarget(int y, int x)
    {
        List<Piece> targets = base.GetPossibleTargets(y, x);

        Piece bestTarget = targets
            .OrderBy(t => t.Health)
            .ThenByDescending(t => t.Data.AttackPower)
            .FirstOrDefault();

        return bestTarget;
    }

    protected virtual Piece FindClosestOpponent(int y, int x)
    {
        Tile tile = base.Board[y, x];

        Piece closestOpponent = base.Board.PlayerPieces
            .OrderBy(o => Vector3.Distance(o.Tile.transform.position, tile.transform.position))
            .ThenBy(o => o.Health)
            .ThenByDescending(o => o.Data.AttackPower)
            .First();

        return closestOpponent;
    }

    protected virtual Tile FindMoveTowardsOpponent(Piece opponent)
    {
        Tile tile = base.PossibleMoves
            .OrderBy(t => Vector3.Distance(t.transform.position, opponent.Tile.transform.position))
            .First();

        return tile;
    }

    protected virtual Tile FindSafestMove()
    {
        Dictionary<Tile, TileSafety> tiles = new Dictionary<Tile, TileSafety>();
        List<Tile> possibleMoves = base.PossibleMoves;

        tiles.Add(base.Tile, new TileSafety());
        possibleMoves.ForEach(m => tiles.Add(m, new TileSafety()));

        foreach (var tile in tiles)
        {
            Tile currTile = tile.Key;
            TileSafety currSafety = tile.Value;

            // Tile endangered
            bool isTileEndangered = false;

            foreach (Piece piece in base.Board.PlayerPieces)
            {
                List<Tile> possibleAttack = piece.GetPossibleAttack(piece.CurrentY, piece.CurrentX);

                if (possibleAttack.Contains(currTile))
                {
                    isTileEndangered = true;
                    break;
                }
            }

            currSafety.IsEndangered = isTileEndangered;

            // Tile closest opponent
            Piece opponent = FindClosestOpponent(currTile.CurrentY, currTile.CurrentX);

            currSafety.ClosestOpponent = Vector3.Distance(currTile.transform.position, opponent.Tile.transform.position);

            // Tile exposability rate
            List<Tile> exposedTiles = new List<Tile>();
            
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (y == 0 && x == 0) continue;

                    AddTilesPerDirection(ref exposedTiles, currTile.CurrentY, currTile.CurrentX, y, x, 999);
                }
            }

            currSafety.ExposabilityRate = exposedTiles.Count;
        }

        Tile safestTile = tiles.OrderBy(x => x.Value.IsEndangered)
             .ThenByDescending(x => x.Value.ClosestOpponent)
             .ThenBy(x => x.Value.ExposabilityRate)
             .FirstOrDefault().Key;

        return safestTile;
    }
}
