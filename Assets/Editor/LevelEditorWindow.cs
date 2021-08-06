using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private enum BrushType
    {
        Player,
        Enemy,
        Clear
    }

    private enum BrushMode
    {
        Tile,
        Row,
        Column,
        Fill
    }

    private PiecesListSO _pieces;

    private int _prevWidth = -1;
    private int _prevHeight = -1;

    private BrushType _currentBrushType;
    private BrushMode _currentBrushMode = BrushMode.Tile;

    private string[] _playerPieces;
    private string[] _enemyPieces;

    private string _currentPlayerPiece;
    private string _currentEnemyPiece;

    private LevelData _currentLevel;

    private Vector2 _scrollPos;

    [MenuItem("Window/Laser Chess/Level Editor", false, 0)]
    private static void Init()
    {
        var window = GetWindow(typeof(LevelEditorWindow));
        window.titleContent = new GUIContent("Level Editor");
    }

    public void OnEnable()
    {
        _pieces = Resources.Load<PiecesListSO>("PiecesListSO");

        _playerPieces = _pieces.PiecesPrefabs.OfType<PlayerPiece>().Select(p => p.GetType().Name).ToArray();
        _enemyPieces = _pieces.PiecesPrefabs.OfType<EnemyPiece>().Select(p => p.GetType().Name).ToArray();

        _currentPlayerPiece = _playerPieces[0];
        _currentEnemyPiece = _enemyPieces[0];
    }

    private void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        var oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 90;

        GUILayout.Space(15);

        DrawMenu();

        if (_currentLevel != null)
        {
            var level = _currentLevel;
            _prevWidth = level.Width;

            GUILayout.Space(15);

            DrawGeneralSettings();

            GUILayout.Space(15);

            DrawLevelEditor();
        }

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndScrollView();
    }

    private void DrawMenu()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("New", GUILayout.Width(100), GUILayout.Height(50)))
        {
            _currentLevel = new LevelData();
        }

        if (GUILayout.Button("Open", GUILayout.Width(100), GUILayout.Height(50)))
        {
            var path = EditorUtility.OpenFilePanel("Open level", Application.streamingAssetsPath + "/Levels", "json");

            if (!string.IsNullOrEmpty(path))
            {
                _currentLevel = JsonSerializer.Load<LevelData>(path);
            }
        }

        if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(50)))
        {
            SaveLevel();
        }

        GUILayout.EndHorizontal();
    }

    private void DrawGeneralSettings()
    {
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        EditorGUILayout.HelpBox("The general settings of this level.", MessageType.Info);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Level number", "The number of this level."),
        GUILayout.Width(EditorGUIUtility.labelWidth));
        _currentLevel.Id = EditorGUILayout.IntField(_currentLevel.Id, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void DrawLevelEditor()
    {
        EditorGUILayout.LabelField("Level", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        EditorGUILayout.HelpBox("The layout settings of this level.", MessageType.Info);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Width", "The width of this level."),
                                       GUILayout.Width(EditorGUIUtility.labelWidth));
        _currentLevel.Width = EditorGUILayout.IntField(_currentLevel.Width, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        _prevHeight = _currentLevel.Height;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Height", "The height of this level."),
                                       GUILayout.Width(EditorGUIUtility.labelWidth));
        _currentLevel.Height = EditorGUILayout.IntField(_currentLevel.Height, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Brush type", "The current type of brush."),
                                       GUILayout.Width(EditorGUIUtility.labelWidth));
        _currentBrushType = (BrushType)EditorGUILayout.EnumPopup(_currentBrushType, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        if (_currentBrushType == BrushType.Player)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Player", "The current type of unit."),
                                           GUILayout.Width(EditorGUIUtility.labelWidth));
            int selectedIndex = Array.IndexOf(_playerPieces, _currentPlayerPiece);
            selectedIndex = EditorGUILayout.Popup(selectedIndex, _playerPieces, GUILayout.Width(100));
            _currentPlayerPiece = _playerPieces[selectedIndex];

            GUILayout.EndHorizontal();
        }
        else if (_currentBrushType == BrushType.Enemy)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Enemy", "The current type of unit."),
                                           GUILayout.Width(EditorGUIUtility.labelWidth));
            int selectedIndex = Array.IndexOf(_enemyPieces, _currentEnemyPiece);
            selectedIndex = EditorGUILayout.Popup(selectedIndex, _enemyPieces, GUILayout.Width(100));
            _currentEnemyPiece = _enemyPieces[selectedIndex];

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Brush mode", "The current brush mode."),
                                       GUILayout.Width(EditorGUIUtility.labelWidth));
        _currentBrushMode = (BrushMode)EditorGUILayout.EnumPopup(_currentBrushMode, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (_prevWidth != _currentLevel.Width || _prevHeight != _currentLevel.Height)
        {
            _currentLevel.PiecesMap = new string[_currentLevel.Height, _currentLevel.Width];
        }

        for (var y = 0; y < _currentLevel.Height; y++)
        {
            GUILayout.BeginHorizontal();

            for (var x = 0; x < _currentLevel.Width; x++)
            {
                CreateButton(y, x);
            }

            GUILayout.EndHorizontal();
        }
    }

    private void CreateButton(int tileY, int tileX)
    {
        string pieceName = _currentLevel.PiecesMap[tileY, tileX];

        Piece piece = _pieces.PiecesPrefabs.SingleOrDefault(p => p.GetType().Name == pieceName);

        if (piece != null)
        {
            if (GUILayout.Button(piece.Data.GraphicIcon, GUILayout.Width(60), GUILayout.Height(60)))
            {
                DrawTile(tileY, tileX);
            }
        }
        else
        {
            if (GUILayout.Button("", GUILayout.Width(60), GUILayout.Height(60)))
            {
                DrawTile(tileY, tileX);
            }
        }
    }

    private void DrawTile(int tileY, int tileX)
    {
        string piece = null;

        switch (_currentBrushType)
        {
            case BrushType.Player:
                piece = _currentPlayerPiece;
                break;
            case BrushType.Enemy:
                piece = _currentEnemyPiece;
                break;
            case BrushType.Clear:
                piece = null;
                break;
        }

        switch (_currentBrushMode)
        {
            case BrushMode.Tile:
                _currentLevel.PiecesMap[tileY, tileX] = piece;
                break;

            case BrushMode.Row:
                for (int x = 0; x < _currentLevel.Width; x++)
                {
                    _currentLevel.PiecesMap[tileY, x] = piece;
                }
                break;

            case BrushMode.Column:
                for (int y = 0; y < _currentLevel.Height; y++)
                {
                    _currentLevel.PiecesMap[y, tileX] = piece;
                }
                break;
            
            case BrushMode.Fill:
                for (int y = 0; y < _currentLevel.Height; y++)
                {
                    for (int x = 0; x < _currentLevel.Width; x++)
                    {
                        _currentLevel.PiecesMap[y, x] = piece;
                    }
                }
                break;
        }
    }

    private void SaveLevel()
    {
#if UNITY_EDITOR
        JsonSerializer.Save(Paths.GetLevelPath(_currentLevel.Id), _currentLevel);
        AssetDatabase.Refresh();
#endif
    }
}
