using TMPro;
using UnityEngine;

public class LevelTile : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private TMP_Text _textIndex;
    [SerializeField] private int _index;

    public int Index => _index;

    public LevelState LevelState { get; private set; }

    private void Awake()
    {
        _textIndex.text = this.Index.ToString();

        if (_gameState.ReachedLevel > this.Index)
        {
            this.LevelState = LevelState.Completed;
        }
        else if (_gameState.ReachedLevel == this.Index)
        {
            this.LevelState = LevelState.Unlocked;
        }
        else
        {
            this.LevelState = LevelState.Locked;
        }
    }
}
