using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonExacise.Scripts
{
    /// <summary>
    /// 各クライアントが持つCharacterUseCase
    /// </summary>
    public class CharacterUseCase : NetworkBehaviour
    {
        private static CharacterUseCase _instance;
        private readonly Dictionary<string, CharacterEntity> _entities = new();

        public static CharacterUseCase Instance => _instance;

        [RuntimeInitializeOnLoadMethod]
        private static void Init() => _instance = null;

        public override void Spawned()
        {
            base.Spawned();

            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            Debug.Log("[CharacterUseCase] Spawned");
        }

        // ---- キャラクター登録 ----
        public static void Register(CharacterEntity entity)
        {
            _instance._entities[entity.Id] = entity;
        }

        public CharacterEntity GetCharacter(string id)
        {
            _instance._entities.TryGetValue(id, out var entity);
            return entity;
        }

        // ---- 攻撃要求を送る（クライアント側から呼ぶ）----
        public void Attack(CharacterEntity entity, int baseDamage)
        {
            Debug.Log(nameof(Attack));
            _instance.RPC_RequestAttack(entity.Id, baseDamage);
        }

        // ---- クライアント → ホスト ----
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_RequestAttack(string id, int baseDamage, RpcInfo info = default)
        {
            Debug.Log($"{nameof(RPC_RequestAttack)} requested");

            var entity = GetCharacter(id);
            if (entity == null)
            {
                Debug.LogWarning($"[Server] Entity not found: {id}");
                return;
            }

            int finalDamage = CalcDamage(baseDamage);

            // ホストで計算後、全員に通知
            RPC_NotifyAttackResult(id, finalDamage);
        }

        // ---- ホスト → 全クライアント ----
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyAttackResult(string id, int finalDamage)
        {
            var entity = GetCharacter(id);
            if (entity == null) return;

            Debug.Log($"{nameof(RPC_NotifyAttackResult)} start");

            entity.TakeDamage(finalDamage);
            Debug.Log($"[Notify] {id} に {finalDamage} ダメージ");
        }

        // ---- ダメージ計算 ----
        private int CalcDamage(int baseDamage)
        {
            int result = baseDamage + Random.Range(-3, 3);
            Debug.Log($"[ServerCalc] ダメージ: {result}");
            return result;
        }
    }
}