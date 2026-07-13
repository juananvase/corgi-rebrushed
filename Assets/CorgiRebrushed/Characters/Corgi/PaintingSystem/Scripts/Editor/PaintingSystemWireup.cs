using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// One-time Editor bootstrap: wires all PaintingSystem references after domain reload.
/// Safe to leave in the project — it only acts when references are missing.
/// </summary>
[InitializeOnLoad]
public static class PaintingSystemWireup
{
    static PaintingSystemWireup()
    {
        EditorApplication.delayCall += TryWire;
    }

    static void TryWire()
    {
        var scene = SceneManager.GetActiveScene();
        if (!scene.name.Contains("Painting")) return;

        var paintSystemGO = GameObject.Find("PaintingSystem");
        if (paintSystemGO == null) return;

        var overlay = paintSystemGO.GetComponent<PaintOverlay>();
        if (overlay == null) return;

        // Already wired check
        var soCheck = new SerializedObject(overlay);
        if (soCheck.FindProperty("_overlayImage")?.objectReferenceValue != null) return;

        Debug.Log("[PaintingSystemWireup] References missing — wiring now.");

        var canvasGO      = GameObject.Find("Canvas");
        var overlayGO     = GameObject.Find("Overlay");
        var drawingGO     = GameObject.Find("DrawingSurface");

        if (canvasGO == null || overlayGO == null || drawingGO == null)
        {
            Debug.LogError("[PaintingSystemWireup] Missing scene GOs, skipping.");
            return;
        }

        // Load PaintingDataSO
        string soPath = "Assets/CorgiRebrushed/Characters/Corgi/PaintingSystem/Data/SOs/PaintingData.asset";
        var soAsset   = AssetDatabase.LoadAssetAtPath<PaintingDataSO>(soPath);
        if (soAsset == null)
        {
            soAsset = ScriptableObject.CreateInstance<PaintingDataSO>();
            if (!AssetDatabase.IsValidFolder("Assets/CorgiRebrushed/Characters/Corgi/PaintingSystem/Data/SOs"))
                AssetDatabase.CreateFolder("Assets/CorgiRebrushed/Characters/Corgi/PaintingSystem/Data", "SOs");
            AssetDatabase.CreateAsset(soAsset, soPath);
            AssetDatabase.SaveAssets();
        }

        Wire<PaintOverlay>(paintSystemGO, "_overlayImage", overlayGO.GetComponent<Image>());
        Wire<PaintOverlay>(paintSystemGO, "_data",         soAsset);
        Wire<PaintCanvas>(paintSystemGO,  "_rawImage",     drawingGO.GetComponent<RawImage>());
        Wire<PaintCanvas>(paintSystemGO,  "_data",         soAsset);
        Wire<PaintingModeController>(paintSystemGO, "_overlay",    paintSystemGO.GetComponent<PaintOverlay>());
        Wire<PaintingModeController>(paintSystemGO, "_canvas",     paintSystemGO.GetComponent<PaintCanvas>());
        Wire<PaintingModeController>(paintSystemGO, "_recognizer", paintSystemGO.GetComponent<SymbolRecognizer>());

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[PaintingSystemWireup] Done — scene saved.");
    }

    static void Wire<T>(GameObject go, string prop, Object val) where T : Component
    {
        var comp = go.GetComponent<T>();
        if (comp == null || val == null) return;
        var so = new SerializedObject(comp);
        var p  = so.FindProperty(prop);
        if (p == null) { Debug.LogWarning($"[PaintingSystemWireup] Property '{prop}' not found on {typeof(T).Name}"); return; }
        p.objectReferenceValue = val;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
