using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// スキルの基本データを定義するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillTree/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("基本情報")]
    public string skillId;
    public string skillName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    
    [Header("習得条件")]
    public int requiredLevel = 1;
    public int requiredPoints = 1;
    public List<string> prerequisiteSkillIds = new List<string>();
    
    [Header("スキル効果")]
    public SkillEffectType effectType;
    public float effectValue;
    public int maxLevel = 1;
    
    [Header("ビジュアル")]
    public Vector2 treePosition; // ツリー上の配置座標
    public SkillTier tier = SkillTier.Basic;
}

public enum SkillEffectType
{
    StatBonus,      // ステータス増加
    UnlockAbility,  // 新能力解放
    PassiveEffect,  // パッシブ効果
    ActiveSkill     // アクティブスキル
}

public enum SkillTier
{
    Basic,
    Intermediate,
    Advanced,
    Master
}