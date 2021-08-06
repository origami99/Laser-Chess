using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndPanel : MonoBehaviour
{
    private const string LEVEL_FAILED = "Level Failed!";
    private const string LEVEL_COMPLETE = "Level Complete!";

    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private Image _rectangle;
    [SerializeField] private TMP_Text _levelEndText;

    private void Awake() => GoalManager.OnLevelEnd += OnLevelEnd;
    private void OnDestroy() => GoalManager.OnLevelEnd -= OnLevelEnd;

    private void OnLevelEnd(bool success)
    {
        _rectangle.enabled = true;
        _levelEndText.enabled = true;

        _levelEndText.text = success ? LEVEL_COMPLETE : LEVEL_FAILED;

        if (success && _gameState.SelectedLevel == _gameState.ReachedLevel)
        {
            _gameState.ReachedLevel++;
        }

        StartCoroutine(ReturnToLevelMap(2f));
    }

    private IEnumerator ReturnToLevelMap(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Level Map");
    }
}