public abstract class KillGoal : Goal
{
    private void Awake() => Piece.OnDeath += OnPieceDeath;
    private void OnDestroy() => Piece.OnDeath -= OnPieceDeath;

    private void OnPieceDeath() => base.Check = true;
}