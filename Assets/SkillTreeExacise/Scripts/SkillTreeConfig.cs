using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// スキルツリー全体の構成を定義するScriptableObject
/// スキルデータとUIレイアウトを分離して管理
/// </summary>
[CreateAssetMenu(fileName = "NewSkillTree", menuName = "SkillTree/Tree Config")]
public class SkillTreeConfig : ScriptableObject
{
    [Header("ツリー情報")]
    public string treeId;
    public string treeName;
    public Sprite treeIcon;
    
    [Header("スキルデータ")]
    public List<SkillData> skills = new List<SkillData>();
    
    [Header("UIレイアウト")]
    public List<SkillUINode> uiNodes = new List<SkillUINode>();
    
    [Header("接続情報")]
    public List<SkillConnection> connections = new List<SkillConnection>();
    
    /// <summary>
    /// スキルIDから依存関係を検証
    /// </summary>
    public bool ValidatePrerequisites(string skillId, HashSet<string> unlockedSkills)
    {
        var skill = skills.Find(s => s.skillId == skillId);
        if (skill == null) return false;
        
        foreach (var prereqId in skill.prerequisiteSkillIds)
        {
            if (!unlockedSkills.Contains(prereqId))
                return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// スキルデータを取得
    /// </summary>
    public SkillData GetSkill(string skillId)
    {
        return skills.Find(s => s.skillId == skillId);
    }
    
    /// <summary>
    /// UIノードを取得
    /// </summary>
    public SkillUINode GetUINode(string skillId)
    {
        return uiNodes.Find(n => n.skillId == skillId);
    }
    
    /// <summary>
    /// スキルデータとUIノードを自動的に同期
    /// </summary>
    public void SyncUINodes()
    {
        // 不要なUIノードを削除
        uiNodes.RemoveAll(node => !skills.Exists(s => s.skillId == node.skillId));
        
        // 新しいスキルにUIノードを追加
        foreach (var skill in skills)
        {
            if (!uiNodes.Exists(n => n.skillId == skill.skillId))
            {
                uiNodes.Add(new SkillUINode(skill.skillId, Vector2.zero));
            }
        }
    }
}

/// <summary>
/// スキル間の接続情報
/// </summary>
[System.Serializable]
public class SkillConnection
{
    public string fromSkillId;
    public string toSkillId;
    public ConnectionType connectionType = ConnectionType.Requirement;
}

public enum ConnectionType
{
    Requirement,    // 必須前提条件
    Recommended,    // 推奨
    Alternative     // 選択肢の一つ
}