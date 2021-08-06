using UnityEngine;

public abstract class Goal : MonoBehaviour
{
    protected Board Board { get; private set; }
    
    public PieceSide Side { get; private set; }

    public bool Check { get; set; }

    public void Init(Board board, PieceSide side)
    {
        this.Board = board;
        this.Side = side;
    }

    public abstract bool IsComplete();
}