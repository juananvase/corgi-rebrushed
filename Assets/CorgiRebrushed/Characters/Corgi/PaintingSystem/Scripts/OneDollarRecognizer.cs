using System.Collections.Generic;
using UnityEngine;

public static class OneDollarRecognizer
{
    private const int N = 64;
    private const float SquareSize = 250f;

    public static List<Vector2> Normalize(List<Vector2> rawPoints)
    {
        var points = Resample(rawPoints, N);
        points = RotateToZero(points);
        points = ScaleToSquare(points, SquareSize);
        points = TranslateToOrigin(points);
        return points;
    }

    public static List<Vector2> Resample(List<Vector2> points, int n)
    {
        float interval = PathLength(points) / (n - 1);
        float accumulatedDistance = 0f;

        var working = new List<Vector2>(points);
        var resampled = new List<Vector2> { working[0] };

        for (int i = 1; i < working.Count; i++)
        {
            float d = Vector2.Distance(working[i - 1], working[i]);
            if (accumulatedDistance + d >= interval)
            {
                float t = (interval - accumulatedDistance) / d;
                Vector2 newPoint = Vector2.Lerp(working[i - 1], working[i], t);
                resampled.Add(newPoint);
                working.Insert(i, newPoint);
                accumulatedDistance = 0f;
            }
            else
            {
                accumulatedDistance += d;
            }
        }

        // Errores de redondeo a veces dejan el resample un punto corto/largo
        while (resampled.Count < n)
        {
            resampled.Add(working[working.Count - 1]);
        }
        if (resampled.Count > n)
        {
            resampled.RemoveRange(n, resampled.Count - n);
        }

        return resampled;
    }

    public static List<Vector2> RotateToZero(List<Vector2> points)
    {
        Vector2 centroid = Centroid(points);
        float angle = Mathf.Atan2(points[0].y - centroid.y, points[0].x - centroid.x);
        return RotateBy(points, centroid, -angle);
    }

    public static List<Vector2> ScaleToSquare(List<Vector2> points, float size)
    {
        var (min, max) = BoundingBox(points);
        float width = Mathf.Max(max.x - min.x, 0.0001f);
        float height = Mathf.Max(max.y - min.y, 0.0001f);

        var scaled = new List<Vector2>(points.Count);
        foreach (var p in points)
        {
            scaled.Add(new Vector2(
                (p.x - min.x) * (size / width),
                (p.y - min.y) * (size / height)));
        }
        return scaled;
    }

    public static List<Vector2> TranslateToOrigin(List<Vector2> points)
    {
        Vector2 centroid = Centroid(points);
        var translated = new List<Vector2>(points.Count);
        foreach (var p in points)
        {
            translated.Add(p - centroid);
        }
        return translated;
    }

    public static float PathDistance(List<Vector2> a, List<Vector2> b)
    {
        int count = Mathf.Min(a.Count, b.Count);
        float sum = 0f;
        for (int i = 0; i < count; i++)
        {
            sum += Vector2.Distance(a[i], b[i]);
        }
        return sum / count;
    }

    public static (SymbolType type, float score) Recognize(
        List<Vector2> normalizedCandidate,
        IEnumerable<(SymbolType type, List<Vector2> normalizedPoints)> templates)
    {
        SymbolType bestType = SymbolType.Unknown;
        float bestDistance = float.MaxValue;
        bool found = false;

        foreach (var template in templates)
        {
            float distance = PathDistance(normalizedCandidate, template.normalizedPoints);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestType = template.type;
                found = true;
            }
        }

        if (!found) return (SymbolType.Unknown, 0f);

        float halfDiagonal = 0.5f * Mathf.Sqrt(SquareSize * SquareSize + SquareSize * SquareSize);
        float score = 1f - bestDistance / halfDiagonal;
        return (bestType, score);
    }

    private static List<Vector2> RotateBy(List<Vector2> points, Vector2 pivot, float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        var rotated = new List<Vector2>(points.Count);
        foreach (var p in points)
        {
            float dx = p.x - pivot.x;
            float dy = p.y - pivot.y;
            rotated.Add(new Vector2(
                dx * cos - dy * sin + pivot.x,
                dx * sin + dy * cos + pivot.y));
        }
        return rotated;
    }

    private static float PathLength(List<Vector2> points)
    {
        float length = 0f;
        for (int i = 1; i < points.Count; i++)
        {
            length += Vector2.Distance(points[i - 1], points[i]);
        }
        return length;
    }

    private static Vector2 Centroid(List<Vector2> points)
    {
        Vector2 sum = Vector2.zero;
        foreach (var p in points) sum += p;
        return sum / points.Count;
    }

    private static (Vector2 min, Vector2 max) BoundingBox(List<Vector2> points)
    {
        Vector2 min = points[0];
        Vector2 max = points[0];
        foreach (var p in points)
        {
            min = Vector2.Min(min, p);
            max = Vector2.Max(max, p);
        }
        return (min, max);
    }
}
