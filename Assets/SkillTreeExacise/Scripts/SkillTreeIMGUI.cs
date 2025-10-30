using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// IMGUIで樹形型スキルツリーを表示
/// </summary>
public class SkillTreeIMGUI : MonoBehaviour
{
    [SerializeField] private SkillTreeManager treeManager;
    [SerializeField] private SkillTreeConfig treeConfig;
    [SerializeField] private bool showWindow = true;
    
    [Header("Layout Settings")]
    [SerializeField] private float nodeWidth = 120f;
    [SerializeField] private float nodeHeight = 80f;
    [SerializeField] private float nodeSpacingX = 50f;
    [SerializeField] private float nodeSpacingY = 30f;
    
    private Vector2 scrollPosition;
    private Rect windowRect = new Rect(50, 50, 1000, 700);
    private Vector2 treeOffset = new Vector2(50, 100);
    private float zoom = 1f;
    
    private GUIStyle nodeUnlockedStyle;
    private GUIStyle nodeLockedStyle;
    private GUIStyle nodeAvailableStyle;
    private GUIStyle headerStyle;
    private GUIStyle labelStyle;
    private bool stylesInitialized = false;
    
    private string selectedSkillId = null;
    
    private void OnGUI()
    {
        if (!showWindow) return;
        
        InitializeStyles();
        windowRect = GUI.Window(0, windowRect, DrawSkillTreeWindow, "Skill Tree - " + treeConfig.treeName);
    }
    
    private void InitializeStyles()
    {
        if (stylesInitialized) return;
        
        // アンロック済みノード
        nodeUnlockedStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(2, 2, new Color(0.2f, 0.7f, 0.2f, 0.8f)) },
            fontSize = 11,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        nodeUnlockedStyle.normal.textColor = Color.white;
        
        // ロック中ノード
        nodeLockedStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(2, 2, new Color(0.3f, 0.3f, 0.3f, 0.8f)) },
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        nodeLockedStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
        
        // アンロック可能ノード
        nodeAvailableStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(2, 2, new Color(0.7f, 0.7f, 0.2f, 0.8f)) },
            fontSize = 11,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        nodeAvailableStyle.normal.textColor = Color.white;
        
        headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.cyan }
        };
        
        labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 11
        };
        
        stylesInitialized = true;
    }
    
    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
    
    private void DrawSkillTreeWindow(int windowID)
    {
        GUILayout.BeginVertical();
        
        // ヘッダー
        DrawHeader();
        
        GUILayout.Space(5);
        
        // ツリー表示エリア
        Rect treeArea = GUILayoutUtility.GetRect(windowRect.width - 20, windowRect.height - 150);
        
        GUI.BeginGroup(treeArea);
        DrawTreeBackground(treeArea);
        
        // スクロール可能なツリー
        scrollPosition = GUI.BeginScrollView(
            new Rect(0, 0, treeArea.width, treeArea.height),
            scrollPosition,
            new Rect(0, 0, 1200, 800)
        );
        
        DrawConnections();
        DrawSkillNodes();
        
        GUI.EndScrollView();
        GUI.EndGroup();
        
        GUILayout.Space(5);
        
        // 詳細情報パネル
        DrawDetailPanel();
        
        GUILayout.EndVertical();
        
        GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
    }
    
    private void DrawHeader()
    {
        GUILayout.BeginHorizontal();
        
        GUILayout.Label($"Level: {treeManager.GetPlayerLevel()}", headerStyle, GUILayout.Width(100));
        GUILayout.Label($"Points: {treeManager.GetAvailablePoints()}", headerStyle, GUILayout.Width(100));
        
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Reset", GUILayout.Width(80)))
        {
            treeManager.ResetSkillTree();
            selectedSkillId = null;
        }
        
        if (GUILayout.Button("+1pt", GUILayout.Width(50)))
            treeManager.AddSkillPoints(1);
        
        if (GUILayout.Button("+5pt", GUILayout.Width(50)))
            treeManager.AddSkillPoints(5);
        
        GUILayout.EndHorizontal();
    }
    
    private void DrawTreeBackground(Rect area)
    {
        // 背景グリッド
        Handles.BeginGUI();
        Handles.color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        
        float gridSize = 50f;
        for (float x = 0; x < 1200; x += gridSize)
        {
            Handles.DrawLine(new Vector3(x, 0), new Vector3(x, 800));
        }
        for (float y = 0; y < 800; y += gridSize)
        {
            Handles.DrawLine(new Vector3(0, y), new Vector3(1200, y));
        }
        
        Handles.EndGUI();
    }
    
    private void DrawConnections()
    {
        if (treeConfig.connections.Count == 0)
        {
            // 接続情報がない場合は前提条件から自動生成
            DrawAutoConnections();
            return;
        }
        
        Handles.BeginGUI();
        
        foreach (var connection in treeConfig.connections)
        {
            var fromSkill = treeConfig.GetSkill(connection.fromSkillId);
            var toSkill = treeConfig.GetSkill(connection.toSkillId);
            
            if (fromSkill == null || toSkill == null) continue;
            
            Vector2 fromPos = GetNodeCenter(fromSkill);
            Vector2 toPos = GetNodeCenter(toSkill);
            
            var fromNode = treeManager.GetSkillNode(connection.fromSkillId);
            var toNode = treeManager.GetSkillNode(connection.toSkillId);
            
            // 接続線の色を状態に応じて変更
            if (fromNode != null && fromNode.isUnlocked && toNode != null && toNode.isUnlocked)
                Handles.color = new Color(0.2f, 0.8f, 0.2f, 0.8f); // 両方アンロック
            else if (fromNode != null && fromNode.isUnlocked)
                Handles.color = new Color(0.8f, 0.8f, 0.2f, 0.6f); // 前提条件満たしてる
            else
                Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.4f); // ロック中
            
            Handles.DrawAAPolyLine(3f, fromPos, toPos);
            
            // 矢印を描画
            DrawArrow(fromPos, toPos);
        }
        
        Handles.EndGUI();
    }
    
    private void DrawAutoConnections()
    {
        Handles.BeginGUI();
        
        foreach (var skill in treeConfig.skills)
        {
            if (skill.prerequisiteSkillIds.Count == 0) continue;
            
            Vector2 toPos = GetNodeCenter(skill);
            var toNode = treeManager.GetSkillNode(skill.skillId);
            
            foreach (var prereqId in skill.prerequisiteSkillIds)
            {
                var prereqSkill = treeConfig.GetSkill(prereqId);
                if (prereqSkill == null) continue;
                
                Vector2 fromPos = GetNodeCenter(prereqSkill);
                var fromNode = treeManager.GetSkillNode(prereqId);
                
                // 線の色
                if (fromNode != null && fromNode.isUnlocked && toNode != null && toNode.isUnlocked)
                    Handles.color = new Color(0.2f, 0.8f, 0.2f, 0.8f);
                else if (fromNode != null && fromNode.isUnlocked)
                    Handles.color = new Color(0.8f, 0.8f, 0.2f, 0.6f);
                else
                    Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                
                Handles.DrawAAPolyLine(3f, fromPos, toPos);
                DrawArrow(fromPos, toPos);
            }
        }
        
        Handles.EndGUI();
    }
    
    private void DrawArrow(Vector2 from, Vector2 to)
    {
        Vector2 direction = (to - from).normalized;
        Vector2 arrowPos = to - direction * 40f;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        
        Vector2 arrowPoint1 = arrowPos - direction * 8f + perpendicular * 5f;
        Vector2 arrowPoint2 = arrowPos - direction * 8f - perpendicular * 5f;
        
        Handles.DrawAAPolyLine(3f, arrowPoint1, arrowPos, arrowPoint2);
    }
    
    private void DrawSkillNodes()
    {
        foreach (var skill in treeConfig.skills)
        {
            DrawSkillNode(skill);
        }
    }
    
    private void DrawSkillNode(SkillData skill)
    {
        Vector2 position = GetNodePosition(skill);
        Rect nodeRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
        
        var node = treeManager.GetSkillNode(skill.skillId);
        if (node == null) return;
        
        // ノードのスタイルを選択
        GUIStyle nodeStyle;
        if (node.isUnlocked)
            nodeStyle = nodeUnlockedStyle;
        else if (treeManager.CanUnlockSkill(skill.skillId))
            nodeStyle = nodeAvailableStyle;
        else
            nodeStyle = nodeLockedStyle;
        
        // ノードを描画
        if (GUI.Button(nodeRect, "", nodeStyle))
        {
            selectedSkillId = skill.skillId;
        }
        
        // ノード内容
        GUILayout.BeginArea(nodeRect);
        GUILayout.BeginVertical();
        
        GUILayout.Space(5);
        
        // スキル名
        GUILayout.Label(skill.skillName, nodeStyle);
        
        // レベル表示
        if (node.isUnlocked)
        {
            GUILayout.Label($"Lv {node.currentLevel}/{skill.maxLevel}", nodeStyle);
        }
        else
        {
            GUILayout.Label($"Cost: {skill.requiredPoints}", nodeStyle);
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
        
        // 選択中の枠
        if (selectedSkillId == skill.skillId)
        {
            Handles.BeginGUI();
            Handles.color = Color.yellow;
            Handles.DrawAAPolyLine(4f,
                new Vector3(nodeRect.x, nodeRect.y),
                new Vector3(nodeRect.xMax, nodeRect.y),
                new Vector3(nodeRect.xMax, nodeRect.yMax),
                new Vector3(nodeRect.x, nodeRect.yMax),
                new Vector3(nodeRect.x, nodeRect.y)
            );
            Handles.EndGUI();
        }
    }
    
    private void DrawDetailPanel()
    {
        GUILayout.BeginVertical("box", GUILayout.Height(110));
        
        if (string.IsNullOrEmpty(selectedSkillId))
        {
            GUILayout.Label("Select a skill to view details", labelStyle);
            GUILayout.EndVertical();
            return;
        }
        
        var skill = treeConfig.GetSkill(selectedSkillId);
        var node = treeManager.GetSkillNode(selectedSkillId);
        
        if (skill == null || node == null) 
        {
            GUILayout.EndVertical();
            return;
        }
        
        // スキル名と説明
        GUILayout.BeginHorizontal();
        GUILayout.Label($"[{skill.tier}] {skill.skillName}", headerStyle);
        GUILayout.FlexibleSpace();
        
        // アクションボタン
        if (!node.isUnlocked)
        {
            bool canUnlock = treeManager.CanUnlockSkill(selectedSkillId);
            GUI.enabled = canUnlock;
            
            if (GUILayout.Button($"Unlock ({skill.requiredPoints} points)", GUILayout.Width(200), GUILayout.Height(25)))
            {
                treeManager.TryUnlockSkill(selectedSkillId);
            }
            
            GUI.enabled = true;
        }
        else if (node.currentLevel < skill.maxLevel)
        {
            bool canLevelUp = treeManager.GetAvailablePoints() >= skill.requiredPoints;
            GUI.enabled = canLevelUp;
            
            if (GUILayout.Button($"Level Up ({skill.requiredPoints} points)", GUILayout.Width(200), GUILayout.Height(25)))
            {
                treeManager.TryLevelUpSkill(selectedSkillId);
            }
            
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Label("MAX LEVEL", headerStyle, GUILayout.Width(200));
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.Label(skill.description, labelStyle);
        
        // 効果と必要レベル
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Effect: {skill.effectType} +{skill.effectValue}", labelStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label($"Required Level: {skill.requiredLevel}", labelStyle);
        GUILayout.EndHorizontal();
        
        // 前提条件
        if (skill.prerequisiteSkillIds.Count > 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Prerequisites:", labelStyle);
            foreach (var prereqId in skill.prerequisiteSkillIds)
            {
                var prereqSkill = treeConfig.GetSkill(prereqId);
                var prereqNode = treeManager.GetSkillNode(prereqId);
                string status = prereqNode != null && prereqNode.isUnlocked ? "✓" : "✗";
                string prereqName = prereqSkill != null ? prereqSkill.skillName : prereqId;
                GUILayout.Label($"{status} {prereqName}", labelStyle);
            }
            GUILayout.EndHorizontal();
        }
        
        GUILayout.EndVertical();
    }
    
    private Vector2 GetNodePosition(SkillData skill)
    {
        return treeOffset + skill.treePosition * zoom;
    }
    
    private Vector2 GetNodeCenter(SkillData skill)
    {
        Vector2 pos = GetNodePosition(skill);
        return pos + new Vector2(nodeWidth / 2, nodeHeight / 2);
    }
}

// Handlesクラスの簡易実装（Unity標準のHandlesが使えない場合）
public static class Handles
{
    public static Color color = Color.white;
    
    public static void BeginGUI() { }
    public static void EndGUI() { }
    
    public static void DrawLine(Vector3 p1, Vector3 p2)
    {
        DrawAAPolyLine(1f, p1, p2);
    }
    
    public static void DrawAAPolyLine(float thickness, params Vector3[] points)
    {
        if (points.Length < 2) return;
        
        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawLineSegment(points[i], points[i + 1], thickness);
        }
    }
    
    private static void DrawLineSegment(Vector3 start, Vector3 end, float thickness)
    {
        Vector2 startPos = new Vector2(start.x, start.y);
        Vector2 endPos = new Vector2(end.x, end.y);
        
        Vector2 direction = (endPos - startPos).normalized;
        float distance = Vector2.Distance(startPos, endPos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Matrix4x4 matrix = GUI.matrix;
        GUIUtility.RotateAroundPivot(angle, startPos);
        
        GUI.color = color;
        GUI.DrawTexture(new Rect(startPos.x, startPos.y - thickness / 2, distance, thickness), Texture2D.whiteTexture);
        
        GUI.matrix = matrix;
        GUI.color = Color.white;
    }
}