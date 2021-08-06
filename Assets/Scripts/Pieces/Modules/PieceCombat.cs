using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PieceCombat : MonoBehaviour
{
    public static event Action<Piece> OnTakeDamage;

    [SerializeField] Piece _piece;
    [SerializeField] private AudioSource _audioSource;

    public bool Attack(float delay = 0f, params Piece[] targets)
    {
        if (targets == null || !targets.Any(t => t != null)) return false;
        if (!_piece.CanAttack) return false;

        _piece.Tile.Selection.Activate(false, playSound: false);

        StartCoroutine(AttackWithDelay(delay, targets));

        return true;
    }

    private IEnumerator AttackWithDelay(float delay, Piece[] targets)
    {
        _piece.IsAttacking = true;

        yield return new WaitForSeconds(delay);

        foreach (Piece target in targets)
        {
            target.Health -= _piece.Data.AttackPower;
            OnTakeDamage.Invoke(target);
        }

        _audioSource.Play();

        _piece.IsAttacking = false;
        _piece.HasAttacked = true;
    }
}
