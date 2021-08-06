using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static event Action<bool> OnLevelEnd;

    [SerializeField] private Board _board;

    [SerializeField] private GameObject _playerGoalsObject;
    [SerializeField] private GameObject _enemyGoalsObject;

    private List<Goal> _playerGoals;
    private List<Goal> _enemyGoals;

    private List<Goal> _allGoals = new List<Goal>();

    private void Awake() 
    {
        _playerGoals = _playerGoalsObject.GetComponents<Goal>().ToList();
        _enemyGoals = _enemyGoalsObject.GetComponents<Goal>().ToList();

        _playerGoals.ForEach(g => g.Init(_board, PieceSide.Player));
        _enemyGoals.ForEach(g => g.Init(_board, PieceSide.Enemy));

        _allGoals.AddRange(_playerGoals);
        _allGoals.AddRange(_enemyGoals);
    }

    private void Update()
    {
        foreach (Goal goal in _allGoals)
        {
            if (goal.Check)
            {
                if (goal.IsComplete())
                {
                    if (goal.Side == PieceSide.Player)
                    {
                        OnLevelEnd?.Invoke(true);
                    }
                    else if (goal.Side == PieceSide.Enemy)
                    {
                        OnLevelEnd?.Invoke(false);
                    }
                }

                goal.Check = false;
            }
        }
    }
}
