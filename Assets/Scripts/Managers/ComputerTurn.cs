using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerTurn : MonoBehaviour
{
    public static event Action<bool> OnTurnChange;

    [SerializeField] private Board _board;
    [SerializeField] private float _actionDelay = 0.75f;

    private List<EnemyPiece> _enemyPieces = new List<EnemyPiece>();
    private int _currPieceIndex = 0;

    private bool _isTurnActive = false;

    private void Awake() => GoalManager.OnLevelEnd += OnLevelEnd;

    private void OnDestroy() => GoalManager.OnLevelEnd -= OnLevelEnd;

    private void Update()
    {
        if (_isTurnActive)
        {
            if (_currPieceIndex >= _enemyPieces.Count)
            {
                _isTurnActive = false;
                _currPieceIndex = 0;

                OnTurnChange?.Invoke(false);
            }
            else
            {
                EnemyPiece currPiece = _enemyPieces[_currPieceIndex];

                if (!currPiece.IsExecutingTurn)
                {
                    if (!currPiece.HasMoved && !currPiece.HasAttacked)
                    {
                        currPiece.ExecuteTurn(_actionDelay);
                    }
                    else
                    {
                        _currPieceIndex++;
                    }
                }
            }
        }
    }

    public void PlayTurn()
    {
        if (!_isTurnActive)
        {
            _enemyPieces = _board.EnemyPieces.OrderBy(e => e.ExecutionOrder).ToList();
            _isTurnActive = true;

            OnTurnChange?.Invoke(true);
        }
    }

    private void OnLevelEnd(bool _) => _isTurnActive = false;
}
