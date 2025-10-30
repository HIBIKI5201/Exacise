using UnityEngine;

/// <summary>
/// スキルツリー上のUIノード表示情報
/// スキルデータとは独立した表示専用のデータ
/// </summary>
[System.Serializable]
public class SkillUINode
{
    [Header("参照")]
    public string skillId;
    
    [Header("表示位置")]
    public Vector2 position;
    
    [Header("ビジュアル設定")]
    public NodeShape shape = NodeShape.Rectangle;
    public Vector2 size = new Vector2(120f, 80f);
    public Color customColor = Color.white;
    public bool useCustomColor = false;
    
    [Header("アニメーション")]
    public bool enablePulse = false;
    public float pulseSpeed = 1f;
    
    public SkillUINode(string id, Vector2 pos)
    {
        skillId = id;
        position = pos;
    }
}

public enum NodeShape
{
    Rectangle,
    Circle,
    Diamond,
    Hexagon
}