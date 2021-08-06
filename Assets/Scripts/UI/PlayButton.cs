using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake() => LevelSelection.OnSelect += OnLevelSelect;

    private void OnDestroy() => LevelSelection.OnSelect -= OnLevelSelect;

    public void PlayLevel() => SceneManager.LoadScene("Level");

    private void OnLevelSelect(bool toggle)
    {
        _button.interactable = toggle;
    }
}
