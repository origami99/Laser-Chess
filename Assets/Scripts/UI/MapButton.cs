using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour
{
    public void BackToMap() => SceneManager.LoadScene("Level Map");
}
