using System.Linq;

public abstract class PlayerPiece : Piece
{
    public override bool CanSelect => base.CanSelect && !(base.HasAttacked || base.IsMoving || (base.HasMoved && !PossibleTargets.Any()));

    public override PieceSide PieceSide => PieceSide.Player;

    protected override void Awake()
    {
        base.Awake();

        ComputerTurn.OnTurnChange += active => OnTurnChange(!active);
    }

    protected override void OnTurnChange(bool active)
    {
        base.OnTurnChange(active);

        if (!active)
        {
            base.Tile.Selection.Activate(false, playSound: false);
        }
    }
}
