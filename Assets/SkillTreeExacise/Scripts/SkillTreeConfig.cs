using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// スキルツリー全体の構成を定義するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "NewSkillTree", menuName = "SkillTree/Tree Config")]
public class SkillTreeConfig : ScriptableObject
{
    [Header("ツリー情報")]
    public string treeId;
    public string treeName;
    public Sprite treeIcon;
    
    [Header("スキル構成")]
    public List<SkillData> skills = new List<SkillData>();
    
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