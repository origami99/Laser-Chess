using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public static event Action OnDeath;

    [SerializeField] private PieceSO _data;

    [SerializeField] private PieceMobility _pieceMobility;
    [SerializeField] private PieceCombat _pieceCombat;

    public PieceSO Data => _data;
    public PieceMobility Mobility => _pieceMobility;
    public PieceCombat Combat => _pieceCombat;

    public int Health { get; set; }

    public List<Tile> PossibleMoves
    {
        get
        {
            if (!this.HasMoved)
            {
                return GetPossibleMoves(this.CurrentY, this.CurrentX);
            }

            return new List<Tile>();
        }
    }

    public List<Piece> PossibleTargets
    {
        get
        {
            if (!this.HasAttacked)
            {
                return GetPossibleTargets(this.CurrentY, this.CurrentX);
            }

            return new List<Piece>();
        }
    }

    public Board Board { get; set; }
    public Tile Tile { get; set; }

    public int CurrentY => this.Tile.CurrentY;
    public int CurrentX => this.Tile.CurrentX;

    public bool HasMoved { get; set; } = false;
    public bool IsMoving { get; set; } = false;

    public bool HasAttacked { get; set; } = false;
    public bool IsAttacking { get; set; } = false;
    public bool CanAttack => this.Data.AttackType != AttackType.None;

    public abstract PieceSide PieceSide { get; }

    public virtual bool CanSelect { get; private set; } = true;

    public bool IsOpponent(Piece piece) => piece.PieceSide != this.PieceSide;

    protected virtual void Awake()
    {
        PieceCombat.OnTakeDamage += OnTakeDamage;

        this.Health = this.Data.MaxHealth;
    }

    protected virtual void OnDestroy()
    {
        PieceCombat.OnTakeDamage -= OnTakeDamage;
    }

    public List<Piece> GetPossibleTargets(int y, int x)
    {
        List<Tile> possibleAttack = GetPossibleAttack(y, x);

        List<Piece> possibleTargets = possibleAttack
            .Where(x => x.Piece != null && this.IsOpponent(x.Piece))
            .Select(x => x.Piece)
            .ToList();

        return possibleTargets;
    }

    public List<Tile> GetPossibleAttack(int y, int x)
    {
        if (_data.OverrideAttackScript)
        {
            return GetTilesPerDirections(this.Data.AttackDirections, y, x, true);
        }
        else
        {
            return GetCustomAttack(y, x);
        }
    }

    public List<Tile> GetPossibleMoves(int y, int x)
    {
        if (_data.OverrideMovementScript)
        {
            return GetTilesPerDirections(this.Data.MovementDirections, y, x);
        }
        else
        {
            return GetCustomMovement(y, x);
        }
    }

    protected virtual List<Tile> GetCustomMovement(int y, int x) => new List<Tile>();
    protected virtual List<Tile> GetCustomAttack(int y, int x) => new List<Tile>();

    protected List<Tile> GetTilesPerDirections(PieceSO.Directions directions, int origY, int origX, bool getBlocked = false)
    {
        List<Tile> tiles = new List<Tile>();

        // Orthogonal tiles
        AddTilesPerDirection(ref tiles, origY, origX, 1, 0, directions.UpRange, getBlocked);
        AddTilesPerDirection(ref tiles, origY, origX, -1, 0, directions.DownRange, getBlocked);
        AddTilesPerDirection(ref tiles, origY, origX, 0, 1, directions.RightRange, getBlocked);
        AddTilesPerDirection(ref tiles, origY, origX, 0, -1, directions.LeftRange, getBlocked);

        // Diagonal tiles 
        AddTilesPerDirection(ref tiles, origY, origX, 1, 1, directions.UpRightRange, getBlocked);
        AddTilesPerDirection(ref tiles, origY, origX, -1, -1, directions.DownLeftRange, getBlocked);
        AddTilesPerDirection(ref tiles, origY, origX, 1, -1, directions.UpLeftRange, getBlocked);
        AddTilesPerDirection(ref tiles, origY, origX, -1, 1, directions.DownRightRange, getBlocked);

        return tiles;
    }

    protected void AddTilesPerDirection(ref List<Tile> tiles, int origY, int origX, int yDir, int xDir, int range, bool addBlocked = false)
    {
        int reachedRange = 0;
        while (true)
        {
            reachedRange++;
            int y = origY + reachedRange * yDir;
            int x = origX + reachedRange * xDir;

            Tile tile = this.Board[y, x];

            if (reachedRange > range || tile == null)
            {
                break;
            }
            else if (tile.IsBlocked(out Piece piece) && piece != this)
            {
                if (addBlocked)
                {
                    tiles.Add(tile);
                }

                break;
            }
            else
            {
                tiles.Add(tile);
            }
        }
    }

    protected virtual void Die()
    {
        this.Tile.Selection.Activate(false);
        this.Board.Pieces.Remove(this);
        this.Tile.Piece = null;
        this.gameObject.SetActive(false);

        OnDeath?.Invoke();
    }

    #region Events
    protected virtual void OnTurnChange(bool active)
    {
        if (active)
        {
            this.HasMoved = false;
            this.HasAttacked = false;
        }

        this.CanSelect = active;
    }

    private void OnTakeDamage(Piece piece)
    {
        if (this == piece && this.Health <= 0)
        {
            this.Die();
        }
    }
    #endregion
}
