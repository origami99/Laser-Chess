using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PiecesListSO), menuName = "ScriptableObjects/" + nameof(PiecesListSO), order = 0)]
public class PiecesListSO : ScriptableObject
{
    [SerializeField] private List<Piece> _piecesPrefabs;

    public List<Piece> PiecesPrefabs => _piecesPrefabs;
}
