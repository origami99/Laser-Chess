using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake() => GoalManager.OnLevelEnd += OnLevelEnd;

    private void OnDestroy() => GoalManager.OnLevelEnd -= OnLevelEnd;

    private void OnLevelEnd(bool _) => _button.interactable = false;
}
