using System;

[Serializable]
public class LevelData
{
    public int Id { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public string[,] PiecesMap { get; set; }
}
