using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameStateSO), menuName = "ScriptableObjects/" + nameof(GameStateSO), order = 0)]
public class GameStateSO : ScriptableObject
{
    [SerializeField] private int _reachedLevel;

    [HideInInspector]
    [SerializeField] private int _selectedLevel;

    public int ReachedLevel 
    {
        get => _reachedLevel; 
        set => _reachedLevel = value; 
    }

    public int SelectedLevel 
    { 
        get => _selectedLevel; 
        set => _selectedLevel = value;
    }
}
