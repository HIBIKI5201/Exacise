using System;

/// <summary>
/// スキルツリー上の個別スキルの状態を管理
/// </summary>
[Serializable]
public class SkillNode
{
    public string skillId;
    public int currentLevel;
    public bool isUnlocked;
    public DateTime unlockedDate;
    
    public SkillNode(string id)
    {
        skillId = id;
        currentLevel = 0;
        isUnlocked = false;
    }
    
    /// <summary>
    /// スキルをアンロック
    /// </summary>
    public bool Unlock()
    {
        if (isUnlocked) return false;
        
        isUnlocked = true;
        currentLevel = 1;
        unlockedDate = DateTime.Now;
        return true;
    }
    
    /// <summary>
    /// スキルレベルアップ
    /// </summary>
    public bool LevelUp(int maxLevel)
    {
        if (!isUnlocked || currentLevel >= maxLevel)
            return false;
        
        currentLevel++;
        return true;
    }
    
    /// <summary>
    /// リセット
    /// </summary>
    public void Reset()
    {
        currentLevel = 0;
        isUnlocked = false;
    }
}