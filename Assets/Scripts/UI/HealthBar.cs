using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Piece _piece;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _damageSlider;

    [SerializeField] private float _animationDuration = 1f;

    private Camera _camera;

    private void Start()
    {
        PieceCombat.OnTakeDamage += OnTakeDamage;
        TileSelection.OnTargetAimed += OnTargetAimed;

        _camera = Camera.main;
    }

    private void Update()
    {
        this.transform.LookAt(
            transform.position + _camera.transform.rotation * -Vector3.back,
            _camera.transform.rotation * -Vector3.down);
    }

    private void OnDestroy()
    {
        PieceCombat.OnTakeDamage -= OnTakeDamage;
        TileSelection.OnTargetAimed -= OnTargetAimed;
    }

    private void OnTakeDamage(Piece piece)
    {
        if (piece == _piece)
        {
            UpdateHealthBar(piece);
        }
    }

    private void OnTargetAimed(Piece piece, bool active, int damage)
    {
        if (piece == _piece)
        {
            PreviewDamage(piece, active, damage);
        }
    }

    private void UpdateHealthBar(Piece piece)
    {
        if (piece != _piece) return;

        float percent = GetPercent(_piece.Health);

        _damageSlider.DOValue(percent, _animationDuration).SetEase(Ease.OutQuart);

        if (_healthSlider.value > percent)
        {
            _healthSlider.value = percent;
        }
    }

    private void PreviewDamage(Piece piece, bool toggle, int damagePreview = 0)
    {
        if (piece != _piece) return;

        if (toggle)
        {
            _healthSlider.value = GetPercent(_piece.Health - damagePreview);
        }
        else
        {
            _healthSlider.value = _damageSlider.value;
        }
    }

    private float GetPercent(int health) => ((float)health / _piece.Data.MaxHealth) * 100;
}
