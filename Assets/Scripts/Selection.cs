using UnityEngine;

public abstract class Selection : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _selectSound;
    [SerializeField] private AudioClip _hoverSound;

    protected Material Material { get; private set; }

    public AudioSource AudioSource => _audioSource;
    public AudioClip SelectSound => _selectSound;
    public AudioClip HoverSound => _hoverSound;

    protected virtual void Awake()
    {
        this.Material = _meshRenderer.material;
    }

    private void OnMouseEnter() => Hovered(true);

    private void OnMouseExit() => Hovered(false);

    private void OnMouseDown() => Clicked();

    public abstract void Hovered(bool active, bool playSound = true);

    public abstract void Clicked();
}
