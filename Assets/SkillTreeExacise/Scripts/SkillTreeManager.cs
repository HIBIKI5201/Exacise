using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// スキルツリーの状態管理とロジック処理
/// </summary>
public class SkillTreeManager : MonoBehaviour
{
    [SerializeField] private SkillTreeConfig treeConfig;
    [Header("Initial Settings")]
    [SerializeField] private int initialPlayerLevel = 1;
    [SerializeField] private int initialSkillPoints = 0;
    
    private Dictionary<string, SkillNode> skillNodes = new Dictionary<string, SkillNode>();
    private int availablePoints = 0;
    private int playerLevel = 1; // デフォルトレベル1
    
    public event System.Action<SkillNode> OnSkillUnlocked;
    public event System.Action<SkillNode> OnSkillLevelUp;
    public event System.Action<int> OnPointsChanged;
    
    private void Awake()
    {
        InitializeTree();
        
        // 初期設定を適用
        playerLevel = initialPlayerLevel;
        availablePoints = initialSkillPoints;
    }
    
    /// <summary>
    /// スキルツリーの初期化
    /// </summary>
    private void InitializeTree()
    {
        skillNodes.Clear();
        
        foreach (var skill in treeConfig.skills)
        {
            skillNodes[skill.skillId] = new SkillNode(skill.skillId);
        }
    }
    
    /// <summary>
    /// スキルをアンロック
    /// </summary>
    public bool TryUnlockSkill(string skillId)
    {
        if (!skillNodes.ContainsKey(skillId))
        {
            Debug.LogError($"Skill {skillId} not found");
            return false;
        }
        
        var node = skillNodes[skillId];
        var data = treeConfig.GetSkill(skillId);
        
        if (node.isUnlocked)
        {
            Debug.Log("Skill already unlocked");
            return false;
        }
        
        // 前提条件チェック
        if (!ValidateUnlock(skillId, data))
            return false;
        
        // ポイント消費
        if (availablePoints < data.requiredPoints)
        {
            Debug.Log("Not enough skill points");
            return false;
        }
        
        // アンロック実行
        node.Unlock();
        availablePoints -= data.requiredPoints;
        
        OnSkillUnlocked?.Invoke(node);
        OnPointsChanged?.Invoke(availablePoints);
        
        Debug.Log($"Unlocked skill: {data.skillName}");
        return true;
    }
    
    /// <summary>
    /// スキルレベルアップ
    /// </summary>
    public bool TryLevelUpSkill(string skillId)
    {
        if (!skillNodes.ContainsKey(skillId))
            return false;
        
        var node = skillNodes[skillId];
        var data = treeConfig.GetSkill(skillId);
        
        if (!node.isUnlocked || node.currentLevel >= data.maxLevel)
            return false;
        
        if (availablePoints < data.requiredPoints)
        {
            Debug.Log("Not enough skill points");
            return false;
        }
        
        node.LevelUp(data.maxLevel);
        availablePoints -= data.requiredPoints;
        
        OnSkillLevelUp?.Invoke(node);
        OnPointsChanged?.Invoke(availablePoints);
        
        return true;
    }
    
    /// <summary>
    /// アンロック条件の検証
    /// </summary>
    private bool ValidateUnlock(string skillId, SkillData data)
    {
        // レベル要件
        if (playerLevel < data.requiredLevel)
        {
            Debug.Log($"Player level {playerLevel} is less than required {data.requiredLevel}");
            return false;
        }
        
        // 前提スキルチェック
        var unlockedSkills = new HashSet<string>(
            skillNodes.Where(kv => kv.Value.isUnlocked).Select(kv => kv.Key)
        );
        
        if (!treeConfig.ValidatePrerequisites(skillId, unlockedSkills))
        {
            Debug.Log("Prerequisites not met");
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// スキルポイントを追加
    /// </summary>
    public void AddSkillPoints(int points)
    {
        availablePoints += points;
        OnPointsChanged?.Invoke(availablePoints);
    }
    
    /// <summary>
    /// プレイヤーレベルを設定
    /// </summary>
    public void SetPlayerLevel(int level)
    {
        playerLevel = level;
    }
    
    /// <summary>
    /// スキルツリーをリセット
    /// </summary>
    public void ResetSkillTree()
    {
        int refundedPoints = 0;
        
        foreach (var node in skillNodes.Values)
        {
            if (node.isUnlocked)
            {
                var data = treeConfig.GetSkill(node.skillId);
                refundedPoints += data.requiredPoints * node.currentLevel;
                node.Reset();
            }
        }
        
        availablePoints += refundedPoints;
        OnPointsChanged?.Invoke(availablePoints);
        
        Debug.Log($"Skill tree reset. Refunded {refundedPoints} points");
    }
    
    /// <summary>
    /// スキルノードを取得
    /// </summary>
    public SkillNode GetSkillNode(string skillId)
    {
        return skillNodes.TryGetValue(skillId, out var node) ? node : null;
    }
    
    /// <summary>
    /// 利用可能ポイント取得
    /// </summary>
    public int GetAvailablePoints() => availablePoints;
    
    /// <summary>
    /// プレイヤーレベル取得
    /// </summary>
    public int GetPlayerLevel() => playerLevel;
    
    /// <summary>
    /// スキルがアンロック可能か確認
    /// </summary>
    public bool CanUnlockSkill(string skillId)
    {
        if (!skillNodes.ContainsKey(skillId))
            return false;
        
        var node = skillNodes[skillId];
        var data = treeConfig.GetSkill(skillId);
        
        if (node.isUnlocked || availablePoints < data.requiredPoints)
            return false;
        
        return ValidateUnlock(skillId, data);
    }
}