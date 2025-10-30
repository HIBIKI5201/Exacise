using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// IMGUIでスキルツリーを表示・操作
/// </summary>
public class SkillTreeIMGUI : MonoBehaviour
{
    [SerializeField] private SkillTreeManager treeManager;
    [SerializeField] private SkillTreeConfig treeConfig;
    [SerializeField] private bool showDebugWindow = true;
    
    private Vector2 scrollPosition;
    private Rect windowRect = new Rect(20, 20, 800, 600);
    private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
    private GUIStyle headerStyle;
    private GUIStyle buttonStyle;
    private GUIStyle lockedStyle;
    private GUIStyle unlockedStyle;
    private bool stylesInitialized = false;
    
    private void OnGUI()
    {
        if (!showDebugWindow) return;
        
        InitializeStyles();
        windowRect = GUI.Window(0, windowRect, DrawSkillTreeWindow, "Skill Tree");
    }
    
    private void InitializeStyles()
    {
        if (stylesInitialized) return;
        
        headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.cyan }
        };
        
        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold
        };
        
        lockedStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { textColor = Color.gray }
        };
        
        unlockedStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { textColor = Color.green }
        };
        
        stylesInitialized = true;
    }
    
    private void DrawSkillTreeWindow(int windowID)
    {
        GUILayout.BeginVertical();
        
        // ヘッダー情報
        DrawHeader();
        
        GUILayout.Space(10);
        
        // スクロール可能なスキルリスト
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(450));
        
        DrawSkillsByTier();
        
        GUILayout.EndScrollView();
        
        GUILayout.Space(10);
        
        // フッター: リセットボタン
        DrawFooter();
        
        GUILayout.EndVertical();
        
        GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
    }
    
    private void DrawHeader()
    {
        GUILayout.Label($"=== {treeConfig.treeName} ===", headerStyle);
        GUILayout.Label($"Available Points: {treeManager.GetAvailablePoints()}", headerStyle);
        
        GUILayout.Space(5);
        
        // デバッグ用: ポイント追加
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+ 1 Point", GUILayout.Width(100)))
        {
            treeManager.AddSkillPoints(1);
        }
        if (GUILayout.Button("+ 5 Points", GUILayout.Width(100)))
        {
            treeManager.AddSkillPoints(5);
        }
        if (GUILayout.Button("+ 10 Points", GUILayout.Width(100)))
        {
            treeManager.AddSkillPoints(10);
        }
        GUILayout.EndHorizontal();
    }
    
    private void DrawSkillsByTier()
    {
        var skillsByTier = new Dictionary<SkillTier, List<SkillData>>();
        
        // Tierごとにスキルを分類
        foreach (var skill in treeConfig.skills)
        {
            if (!skillsByTier.ContainsKey(skill.tier))
            {
                skillsByTier[skill.tier] = new List<SkillData>();
            }
            skillsByTier[skill.tier].Add(skill);
        }
        
        // Tierごとに表示
        foreach (SkillTier tier in System.Enum.GetValues(typeof(SkillTier)))
        {
            if (!skillsByTier.ContainsKey(tier)) continue;
            
            DrawTierSection(tier, skillsByTier[tier]);
        }
    }
    
    private void DrawTierSection(SkillTier tier, List<SkillData> skills)
    {
        string tierKey = tier.ToString();
        if (!foldouts.ContainsKey(tierKey))
        {
            foldouts[tierKey] = true;
        }
        
        GUILayout.BeginVertical("box");
        
        // Tierヘッダー
        foldouts[tierKey] = GUILayout.Toggle(
            foldouts[tierKey], 
            $"▼ {tier} Tier ({skills.Count} skills)", 
            headerStyle
        );
        
        if (foldouts[tierKey])
        {
            foreach (var skill in skills)
            {
                DrawSkillEntry(skill);
            }
        }
        
        GUILayout.EndVertical();
        GUILayout.Space(5);
    }
    
    private void DrawSkillEntry(SkillData skill)
    {
        var node = treeManager.GetSkillNode(skill.skillId);
        if (node == null) return;
        
        GUIStyle boxStyle = node.isUnlocked ? unlockedStyle : lockedStyle;
        
        GUILayout.BeginVertical(boxStyle);
        
        // スキル名とステータス
        GUILayout.BeginHorizontal();
        
        string statusIcon = node.isUnlocked ? "✓" : "○";
        string levelInfo = node.isUnlocked ? $" [Lv {node.currentLevel}/{skill.maxLevel}]" : "";
        
        GUILayout.Label($"{statusIcon} {skill.skillName}{levelInfo}", GUILayout.Width(300));
        
        // アクションボタン
        DrawSkillButtons(skill, node);
        
        GUILayout.EndHorizontal();
        
        // スキル詳細情報
        if (node.isUnlocked || treeManager.CanUnlockSkill(skill.skillId))
        {
            DrawSkillDetails(skill, node);
        }
        else
        {
            GUILayout.Label("Locked - Prerequisites not met", GUI.skin.GetStyle("label"));
        }
        
        GUILayout.EndVertical();
        GUILayout.Space(3);
    }
    
    private void DrawSkillButtons(SkillData skill, SkillNode node)
    {
        if (!node.isUnlocked)
        {
            bool canUnlock = treeManager.CanUnlockSkill(skill.skillId);
            GUI.enabled = canUnlock;
            
            string buttonText = canUnlock 
                ? $"Unlock ({skill.requiredPoints}pt)" 
                : "Locked";
            
            if (GUILayout.Button(buttonText, buttonStyle, GUILayout.Width(150)))
            {
                treeManager.TryUnlockSkill(skill.skillId);
            }
            
            GUI.enabled = true;
        }
        else if (node.currentLevel < skill.maxLevel)
        {
            bool canLevelUp = treeManager.GetAvailablePoints() >= skill.requiredPoints;
            GUI.enabled = canLevelUp;
            
            if (GUILayout.Button($"Level Up ({skill.requiredPoints}pt)", buttonStyle, GUILayout.Width(150)))
            {
                treeManager.TryLevelUpSkill(skill.skillId);
            }
            
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Label("MAX LEVEL", GUILayout.Width(150));
        }
    }
    
    private void DrawSkillDetails(SkillData skill, SkillNode node)
    {
        GUILayout.BeginVertical("box");
        
        // 説明
        GUILayout.Label($"Description: {skill.description}");
        
        // 効果
        GUILayout.Label($"Effect: {skill.effectType} +{skill.effectValue}");
        
        // 必要レベル
        GUILayout.Label($"Required Level: {skill.requiredLevel}");
        
        // 前提スキル
        if (skill.prerequisiteSkillIds.Count > 0)
        {
            GUILayout.Label("Prerequisites:");
            foreach (var prereqId in skill.prerequisiteSkillIds)
            {
                var prereqSkill = treeConfig.GetSkill(prereqId);
                var prereqNode = treeManager.GetSkillNode(prereqId);
                
                string prereqStatus = prereqNode != null && prereqNode.isUnlocked ? "✓" : "✗";
                string prereqName = prereqSkill != null ? prereqSkill.skillName : prereqId;
                
                GUILayout.Label($"  {prereqStatus} {prereqName}");
            }
        }
        
        GUILayout.EndVertical();
    }
    
    private void DrawFooter()
    {
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Reset All Skills", buttonStyle, GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog(
                "Reset Skill Tree", 
                "Are you sure you want to reset all skills? This will refund all spent points.", 
                "Yes", 
                "No"))
            {
                treeManager.ResetSkillTree();
            }
        }
        
        showDebugWindow = GUILayout.Toggle(showDebugWindow, "Show Window");
        
        GUILayout.EndHorizontal();
    }
}

// EditorUtility互換クラス（ランタイム用）
public static class EditorUtility
{
    public static bool DisplayDialog(string title, string message, string ok, string cancel)
    {
        Debug.Log($"Dialog: {title} - {message}");
        return true; // ランタイムでは常にtrueを返す（実際の実装ではUIを出す）
    }
}