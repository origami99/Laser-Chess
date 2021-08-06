public static class MatrixUtils
{
    public static bool IsInBound<T>(T[,] matrix, int y, int x)
    {
        bool isInBound = y >= 0 && y < matrix.GetLength(0) &&
                         x >= 0 && x < matrix.GetLength(1);

        return isInBound;
    }
}
