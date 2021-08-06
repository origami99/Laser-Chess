using System.Linq;

public class KillAllOpponentsGoal : KillGoal
{
    public override bool IsComplete()
    {
        return !base.Board.Pieces.Any(p => p.PieceSide != base.Side);
    }
}
