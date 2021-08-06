using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelection : Selection
{
    public static event Action<bool> OnSelect;

    [SerializeField] private AudioClip _wrongSound;

    [SerializeField] private LevelTile _levelTile;
    [SerializeField] private GameStateSO _gameState;

    [SerializeField] private List<SelectionPair> _selectionPairs;

    [SerializeField] [Range(0f, 0.25f)] private float _hoverDarkenAmount;
    [SerializeField] private GameObject _selectionBorder;

    private Color _levelStateColor;
    private Color _hoveredColor;

    private static LevelSelection _selectedLevel;

    public bool IsSelected { get; private set; }

    private void Start()
    {
        _selectedLevel = null;

        _levelStateColor = GetLevelStateColor();
        _hoveredColor = GetHoveredColor();

        base.Material.color = _levelStateColor;
    }

    public void Activate(bool toggle, bool playSound = true, bool notify = true)
    {
        if (toggle == this.IsSelected) return;

        if (_levelTile.LevelState == LevelState.Locked)
        {
            base.AudioSource.PlayOneShot(_wrongSound);
            return;
        }

        if (toggle)
        {
            _selectedLevel?.Activate(false, playSound: false, notify: false);
        }

        _selectionBorder.SetActive(toggle);

        this.IsSelected = toggle;
        _selectedLevel = this;
        _gameState.SelectedLevel = _levelTile.Index;

        if (playSound)
        {
            base.AudioSource.PlayOneShot(base.SelectSound);
        }

        if (notify)
        {
            OnSelect?.Invoke(toggle);
        }
    }

    public override void Clicked() => Activate(!this.IsSelected);

    public override void Hovered(bool active, bool playSound = true)
    {
        base.Material.color = active ? _hoveredColor : _levelStateColor;

        if (active)
        {
            base.AudioSource.PlayOneShot(base.HoverSound);
        }
    }

    private Color GetHoveredColor()
    {
        return _levelStateColor * (1f - _hoverDarkenAmount);
    }

    private Color GetLevelStateColor()
    {
        Color color = _selectionPairs.Single(s => s.LevelState == _levelTile.LevelState).Color;

        return color;
    }

    [System.Serializable]
    public class SelectionPair
    {
        [SerializeField] private LevelState _levelState;
        [SerializeField] private Color _color;

        public LevelState LevelState => _levelState;
        public Color Color => _color;
    }
}
