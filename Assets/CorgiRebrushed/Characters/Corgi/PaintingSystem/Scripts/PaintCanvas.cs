using System;
using System.Collections.Generic;
using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PaintCanvas : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private RawImage _rawImage;
    [SerializeField][FoldoutGroup("Data")] private PaintingDataSO _data;
    [SerializeField] private ECorgiHabilityEventAsset mievento;

    [ShowInInspector][FoldoutGroup("Testing")] private int _pointCount;

    private Texture2D _texture;
    private readonly List<Vector2> _stroke = new();
    private Vector2 _lastMousePos;
    private bool _isDrawing;

    private void OnEnable()
    {
        mievento.OnInvoked.AddListener(EscucharEvento);
    }

    private void OnDisable()
    {
        mievento.OnInvoked.RemoveListener(EscucharEvento);
    }

    private void EscucharEvento(ECorgiHability resultado)
    {
        Debug.Log(resultado.ToString());
    }

    private void Awake()
    {
        _texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        ClearTexture();
        _rawImage.texture = _texture;
    }

    public void PrepareSession()
    {
        ClearTexture();
        _stroke.Clear();
        _pointCount = 0;
        _isDrawing = false;
    }

    public void BeginStroke()
    {
        _lastMousePos = Mouse.current.position.ReadValue();
        _isDrawing = true;
    }

    public void PauseStroke()
    {
        _isDrawing = false;
    }

    public List<Vector2> EndSession()
    {
        _isDrawing = false;
        return new List<Vector2>(_stroke);
    }

    private void Update()
    {
        if (!_isDrawing) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        DrawLine(_lastMousePos, mousePos);
        _stroke.Add(mousePos);
        _pointCount = _stroke.Count;
        _lastMousePos = mousePos;
    }

    private void DrawLine(Vector2 from, Vector2 to)
    {
        int steps = Mathf.Max(1, Mathf.CeilToInt(Vector2.Distance(from, to)));
        for (int i = 0; i <= steps; i++)
        {
            Vector2 point = Vector2.Lerp(from, to, (float)i / steps);
            DrawCircle(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
        }
        _texture.Apply();
    }

    private void DrawCircle(int cx, int cy)
    {
        int r = _data.BrushSize;
        Color color = _data.BrushColor;
        for (int x = cx - r; x <= cx + r; x++)
        {
            for (int y = cy - r; y <= cy + r; y++)
            {
                if ((x - cx) * (x - cx) + (y - cy) * (y - cy) > r * r) continue;
                if (x < 0 || x >= _texture.width || y < 0 || y >= _texture.height) continue;
                _texture.SetPixel(x, y, color);
            }
        }
    }

    private void ClearTexture()
    {
        var pixels = new Color32[_texture.width * _texture.height];
        _texture.SetPixels32(pixels);
        _texture.Apply();
    }
}
