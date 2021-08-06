using UnityEngine;

[CreateAssetMenu(fileName = nameof(PieceSO), menuName = "ScriptableObjects/" + nameof(PieceSO), order = 0)]
public class PieceSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _attackPower;
    [SerializeField] private AttackType _attackType;

    [Header("Movement")]
    [SerializeField] private bool _overrideMovementScript;
    [SerializeField] private Directions _movementDirections; // TODO: [ConditionalHide(nameof(_overrideMovementScript))] 

    [Header("Attack")]
    [SerializeField] private bool _overrideAttackScript;
    [SerializeField] private Directions _attackDirections; // TODO: [ConditionalHide(nameof(_overrideAttackScript))] 

    [Header("Editor")]
    [SerializeField] private Texture _graphicIcon;

    public int MaxHealth => _maxHealth;
    public int AttackPower => _attackPower;
    public AttackType AttackType => _attackType;

    public bool OverrideMovementScript => _overrideMovementScript;
    public bool OverrideAttackScript => _overrideAttackScript;

    public Directions MovementDirections => _movementDirections;
    public Directions AttackDirections => _attackDirections;

    public Texture GraphicIcon => _graphicIcon;

    [System.Serializable]
    public class Directions
    {
        [Header("Ortogonally")]
        [SerializeField] private int _upRange;
        [SerializeField] private int _downRange;
        [SerializeField] private int _rightRange;
        [SerializeField] private int _leftRange;

        [Header("Diagonally")]
        [SerializeField] private int _upLeftRange;
        [SerializeField] private int _upRightRange;
        [SerializeField] private int _downRightRange;
        [SerializeField] private int _downLeftRange;

        public int UpRange => _upRange;
        public int DownRange => _downRange;
        public int RightRange => _rightRange;
        public int LeftRange => _leftRange;

        public int UpLeftRange => _upLeftRange;
        public int UpRightRange => _upRightRange;
        public int DownRightRange => _downRightRange;
        public int DownLeftRange => _downLeftRange;
    }
}
