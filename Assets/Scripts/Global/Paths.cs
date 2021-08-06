using UnityEngine;

public static class Paths
{
    public static string GetLevelPath(int index) => $"{Application.streamingAssetsPath}/Levels/{index}.json";
}
