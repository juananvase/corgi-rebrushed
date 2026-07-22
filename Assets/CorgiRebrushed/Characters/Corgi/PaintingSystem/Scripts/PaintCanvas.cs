using System.Collections.Generic;
using GameEvents;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PaintCanvas : MonoBehaviour
{
    [SerializeField][FoldoutGroup("References")] private RawImage _rawImage;
    [SerializeField][FoldoutGroup("Data")]       private PaintingDataSO _data;
    [SerializeField][FoldoutGroup("Brush")]      private Material _brushMaterial;
    [SerializeField][FoldoutGroup("Brush")]      private Color _brushColor = Color.white;

    [SerializeField][FoldoutGroup("Brush")]                    private int   _brushSize       = 20;
    [SerializeField][FoldoutGroup("Brush")]                    private int   _bristleCount    = 20;
    [SerializeField][FoldoutGroup("Brush")]                    private int   _bristleRadius   = 3;
    [SerializeField][FoldoutGroup("Brush")][Range(0f,  1f)]   private float _bristleSoftness = 0.12f;
    [SerializeField][FoldoutGroup("Brush")][Range(0f,  1f)]   private float _bristleSpread   = 0.85f;
    [SerializeField][FoldoutGroup("Brush")][Range(0f,  1f)]   private float _opacity         = 0.9f;
    [SerializeField][FoldoutGroup("Brush")][Range(0f,  40f)]  private float _speedFadeMax    = 20f;
    [SerializeField][FoldoutGroup("Brush")][Range(0.01f, 1f)] private float _smoothing       = 0.12f;
    [SerializeField][FoldoutGroup("Brush")][Range(0f, 200f)]  private float _taperLength     = 60f;

    [SerializeField][FoldoutGroup("Fade")] private float _fadeOutDuration = 0.3f;
    [SerializeField][FoldoutGroup("Fade")] private Ease  _fadeOutEase     = Ease.OutQuad;

    [SerializeField] private ECorgiHabilityEventAsset mievento;

    private static readonly int OpacityID = Shader.PropertyToID("_Opacity");

    [ShowInInspector][FoldoutGroup("Testing")] private int _pointCount;

    private Texture2D _texture;
    private Color[]   _pixels;

    private readonly List<Vector2> _stroke = new();

    private Vector2 _lastPos;
    private Vector2 _smoothedPos;
    private float   _currentLength;
    private bool    _isDrawing;

    private Vector2[] _bristleOffsets;
    private float[]   _bristleOpacities;

    private void OnEnable()  => mievento.OnInvoked.AddListener(EscucharEvento);
    private void OnDisable() => mievento.OnInvoked.RemoveListener(EscucharEvento);
    private void EscucharEvento(ECorgiHability r) => Debug.Log(r.ToString());

    private void Awake()
    {
        _texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        _pixels  = new Color[_texture.width * _texture.height];
        ClearTexture();
        _rawImage.texture = _texture;
    }

    public void PrepareSession()
    {
        ClearTexture();
        _stroke.Clear();
        _pointCount = 0;
        _isDrawing  = false;
    }

    public void BeginStroke()
    {
        _smoothedPos   = Mouse.current.position.ReadValue();
        _lastPos       = _smoothedPos;
        _currentLength = 0f;
        _isDrawing     = true;
        RegenerateBristles();
    }

    public void PauseStroke() => _isDrawing = false;

    public List<Vector2> EndSession()
    {
        _isDrawing = false;
        return new List<Vector2>(_stroke);
    }

    public void RecolorStroke(Color color)
    {
        for (int i = 0; i < _pixels.Length; i++)
        {
            if (_pixels[i].a <= 0f) continue;
            _pixels[i] = new Color(color.r, color.g, color.b, _pixels[i].a);
        }

        _texture.SetPixels(_pixels);
        _texture.Apply();
    }

    public void FadeOutAndClear(System.Action onComplete = null)
    {
        var baseline = (Color[])_pixels.Clone();

        Tween.Custom(1f, 0f, _fadeOutDuration, t =>
        {
            for (int i = 0; i < _pixels.Length; i++)
            {
                if (baseline[i].a <= 0f) continue;
                _pixels[i] = new Color(baseline[i].r, baseline[i].g, baseline[i].b, baseline[i].a * t);
            }

            _texture.SetPixels(_pixels);
            _texture.Apply();
        }, ease: _fadeOutEase).OnComplete(() =>
        {
            PrepareSession();
            onComplete?.Invoke();
        });
    }

    private void Update()
    {
        if (!_isDrawing) return;

        Vector2 raw = Mouse.current.position.ReadValue();
        _smoothedPos   = Vector2.Lerp(_smoothedPos, raw, _smoothing);
        float speed    = Vector2.Distance(_lastPos, _smoothedPos);
        _currentLength += speed;

        float startTaper = _taperLength > 0f
            ? Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_currentLength / _taperLength))
            : 1f;

        DrawSegment(_lastPos, _smoothedPos, speed, startTaper);

        _stroke.Add(_smoothedPos);
        _pointCount = _stroke.Count;
        _lastPos = _smoothedPos;
    }

    [Button("Regenerar Cerdas")]
    private void RegenerateBristles()
    {
        _bristleOffsets   = new Vector2[_bristleCount];
        _bristleOpacities = new float[_bristleCount];

        for (int i = 0; i < _bristleCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float r     = Mathf.Sqrt(Random.value) * _brushSize * _bristleSpread;
            _bristleOffsets[i]   = new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
            _bristleOpacities[i] = Random.Range(0.75f, 1f);
        }
    }

    private void DrawSegment(Vector2 from, Vector2 to, float speed, float sizeFactor)
    {
        float matOpacity  = _brushMaterial != null && _brushMaterial.HasProperty(OpacityID)
            ? _brushMaterial.GetFloat(OpacityID) : 1f;
        float speedFactor = 1f - Mathf.Clamp01(speed / Mathf.Max(_speedFadeMax, 0.01f));
        float baseAlpha   = _opacity * matOpacity * Mathf.Lerp(0.4f, 1f, speedFactor);

        int steps = Mathf.Max(1, Mathf.CeilToInt(Vector2.Distance(from, to)));
        for (int i = 0; i <= steps; i++)
        {
            Vector2 p = Vector2.Lerp(from, to, (float)i / steps);
            PaintBristles(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), baseAlpha, sizeFactor);
        }

        _texture.SetPixels(_pixels);
        _texture.Apply();
    }

    private void PaintBristles(int cx, int cy, float baseAlpha, float sizeFactor)
    {
        if (_bristleOffsets == null)
        {
            RegenerateBristles();
            if (_bristleOffsets == null) return;
        }

        int radius = Mathf.Max(1, Mathf.RoundToInt(_bristleRadius * sizeFactor));

        for (int b = 0; b < _bristleCount; b++)
        {
            Vector2 off = _bristleOffsets[b] * sizeFactor;
            int bx = cx + Mathf.RoundToInt(off.x);
            int by = cy + Mathf.RoundToInt(off.y);
            PaintDot(bx, by, baseAlpha * _bristleOpacities[b], radius);
        }
    }

    private void PaintDot(int cx, int cy, float masterAlpha, int r)
    {
        int   w         = _texture.width;
        int   h         = _texture.height;
        float softStart = 1f - _bristleSoftness;

        for (int x = cx - r; x <= cx + r; x++)
        {
            for (int y = cy - r; y <= cy + r; y++)
            {
                if (x < 0 || x >= w || y < 0 || y >= h) continue;
                float dx   = x - cx;
                float dy   = y - cy;
                float dist = Mathf.Sqrt(dx * dx + dy * dy) / Mathf.Max(r, 1);
                if (dist > 1f) continue;

                float alpha = (1f - Mathf.SmoothStep(softStart, 1f, dist)) * masterAlpha;
                if (alpha <= 0.005f) continue;

                int   idx = y * w + x;
                Color dst = _pixels[idx];
                Color src = new Color(_brushColor.r, _brushColor.g, _brushColor.b, alpha);
                float outA = src.a + dst.a * (1f - src.a);
                if (outA > 0f)
                {
                    float sc = src.a;
                    float dc = dst.a * (1f - src.a);
                    _pixels[idx] = new Color(
                        (src.r * sc + dst.r * dc) / outA,
                        (src.g * sc + dst.g * dc) / outA,
                        (src.b * sc + dst.b * dc) / outA,
                        outA
                    );
                }
            }
        }
    }

    private void ClearTexture()
    {
        System.Array.Clear(_pixels, 0, _pixels.Length);
        _texture.SetPixels(_pixels);
        _texture.Apply();
    }
}
