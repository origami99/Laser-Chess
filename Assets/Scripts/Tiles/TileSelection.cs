using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSelection : Selection
{
    public static event Action<Piece, bool, int> OnTargetAimed;

    public enum ColorState { Normal, Hovered, Selected, PossibleMove, PossibleAttack };

    [SerializeField] private Tile _tile;

    [SerializeField] private Color _hoveredColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _possibleMoveColor;
    [SerializeField] private Color _possibleAttackColor;

    public ColorState CurrentColorState { get; private set; }

    private Color _normalColor;

    public bool Active { get; private set; } = false;

    private static Tile _selectedTile;

    protected override void Awake()
    {
        base.Awake();

        _normalColor = base.Material.color;
    }

    private void OnMouseEnter() => Hovered(true);

    private void OnMouseExit() => Hovered(false);

    private void OnMouseDown() => Clicked();

    public override void Hovered(bool active, bool playSound = true)
    {
        SetColor(active ? ColorState.Hovered : ColorState.Normal);

        if (_selectedTile == null) return;

        if (_tile.Piece != null && _selectedTile != null && _selectedTile.Piece.PossibleTargets.Contains(_tile.Piece))
        {
            HoveredPossibleTarget(active);
        }
        else if (_selectedTile.Piece.PossibleMoves.Contains(_tile))
        {
            HoveredPossibleMove(active);
        }
    }

    private void HoveredPossibleTarget(bool active)
    {
        if (_selectedTile.Piece.Data.AttackType == AttackType.Single)
        {
            if (active)
            {
                base.AudioSource.PlayOneShot(base.HoverSound);
            }

            OnTargetAimed.Invoke(_tile.Piece, active, _selectedTile.Piece.Data.AttackPower);
        }
    }

    private void HoveredPossibleMove(bool active)
    {
        if (_selectedTile.Piece.Data.AttackType == AttackType.All)
        {
            List<Piece> targets = _selectedTile.Piece.GetPossibleTargets(_tile.CurrentY, _tile.CurrentX);

            foreach (Piece target in targets)
            {
                OnTargetAimed.Invoke(target, active, _selectedTile.Piece.Data.AttackPower);
            }
        }
    }

    public override void Clicked()
    {
        if (_tile.Piece != null)
        {
            Activate(!this.Active);

            if (_selectedTile != null &&
                _selectedTile.Piece.Data.AttackType == AttackType.Single &&
                _tile.Piece is EnemyPiece &&
                _selectedTile.Piece.PossibleTargets.Contains(_tile.Piece))
            {
                _selectedTile.Piece.Combat.Attack(targets: _tile.Piece);
            }
        }
        else if (_selectedTile != null)
        {
            Piece piece = _selectedTile.Piece;

            piece.Tile.Selection.Activate(false);

            if (piece.PossibleMoves.Contains(_tile))
            {
                piece.Mobility.Move(_tile);

                if (piece.Data.AttackType == AttackType.All)
                {
                    List<Piece> targets = piece.GetPossibleTargets(_tile.CurrentY, _tile.CurrentX);

                    if (targets.Any())
                    {
                        piece.Combat.Attack(targets: targets.ToArray());
                    }
                }
            }
        }
    }

    public void Activate(bool toggle, bool playSound = true)
    {
        if (toggle == this.Active) return;
        if (toggle && !_tile.Piece.CanSelect) return;

        // Deselect previous
        if (toggle)
        {
            _selectedTile?.Selection.Activate(false);
        }

        // Process the color of piece's tile
        ColorState colorState = toggle ? ColorState.Selected : ColorState.Normal;
        SetColor(colorState, false);

        // Process the color of possible moves tiles
        colorState = toggle ? ColorState.PossibleMove : ColorState.Normal;
        _tile.Piece.PossibleMoves.ForEach(x => x.Selection.SetColor(colorState, rules: false));

        // Process the color of possible atack tiles
        colorState = toggle ? ColorState.PossibleAttack : ColorState.Normal;
        _tile.Piece.PossibleTargets.ForEach(x => x.Tile.Selection.SetColor(colorState, rules: false));

        this.Active = toggle;

        if (playSound)
        {
            base.AudioSource.PlayOneShot(base.SelectSound);
        }

        _selectedTile = toggle ? _tile : null;
    }

    public void SetColor(ColorState colorState, bool rules = true)
    {
        switch (colorState)
        {
            case ColorState.Normal:
                if (!rules || (this.CurrentColorState != ColorState.PossibleMove &&
                               this.CurrentColorState != ColorState.PossibleAttack &&
                               this.CurrentColorState != ColorState.Selected))
                {
                    base.Material.color = _normalColor;
                    this.CurrentColorState = colorState;
                }
                break;
            case ColorState.Hovered:
                if (!rules || (this.CurrentColorState != ColorState.PossibleMove &&
                               this.CurrentColorState != ColorState.PossibleAttack &&
                               this.CurrentColorState != ColorState.Selected))
                {
                    base.Material.color = _hoveredColor;
                    this.CurrentColorState = colorState;
                }
                break;
            case ColorState.Selected:
                base.Material.color = _selectedColor;
                this.CurrentColorState = colorState;
                break;
            case ColorState.PossibleMove:
                if (!rules || this.CurrentColorState != ColorState.PossibleAttack)
                {
                    base.Material.color = _possibleMoveColor;
                    this.CurrentColorState = colorState;
                }
                break;
            case ColorState.PossibleAttack:
                base.Material.color = _possibleAttackColor;
                this.CurrentColorState = colorState;
                break;
        }
    }
}
