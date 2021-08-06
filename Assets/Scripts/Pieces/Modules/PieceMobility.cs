using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Piece))]
public class PieceMobility : MonoBehaviour
{
    public static event Action OnMoved;

    [Header("Slide animation")]
    [SerializeField] private float _slideAnimationSpeed = 10f;

    [Header("Fall animation")]
    [SerializeField] private float _fallAnimationDelay;
    [SerializeField] private float _fallAnimationSpeed = 10f;
    [SerializeField] private float _fallDistance = 20f;

    private Piece _piece;

    private void Awake()
    {
        _piece = this.GetComponent<Piece>();
    }

    public void Move(Tile tile, float delay = 0f)
    {
        if (_piece.IsMoving) return;

        if (SetTile(tile))
        {
            _piece.transform
                .DOMove(tile.PieceHolder.position, _slideAnimationSpeed)
                .SetDelay(delay)
                .SetEase(Ease.OutSine)
                .SetSpeedBased()
                .OnComplete(() => 
                {
                    _piece.HasMoved = true;
                    _piece.IsMoving = false;

                    OnMoved?.Invoke();
                });

            _piece.IsMoving = true;
        }
    }

    public void Place(Tile tile)
    {
        if (SetTile(tile))
        {
            Vector3 fallPos = tile.PieceHolder.position + new Vector3(0, _fallDistance, 0);
            _piece.transform.position = fallPos;

            _piece.transform
                .DOMove(tile.PieceHolder.position, _fallAnimationSpeed)
                .SetDelay(_fallAnimationDelay)
                .SetEase(Ease.OutBounce)
                .SetSpeedBased();
        }
    }

    private bool SetTile(Tile tile)
    {
        if (tile == null) return false;
        if (!tile.IsEmpty) return false;
        if (tile == _piece.Tile) return false;

        if (_piece.Tile != null)
        {
            _piece.Tile.Piece = null;
        }

        _piece.transform.SetParent(tile.PieceHolder);

        _piece.Tile = tile;
        tile.Piece = _piece;

        return true;
    }
}
