using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonExacise.Scripts
{
    public class CharacterUseCase : NetworkBehaviour
    {
        private static CharacterUseCase _instance;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _instance = null;
        }

        public static void Register(CharacterEntity entity)
        {
            _instance._entities.TryAdd(entity.Id, entity);
        }

        // ---- 攻撃要求を送る（クライアントから呼ばれる）----
        public static void Attack(CharacterEntity entity, int baseDamage)
        {
            // クライアントがホストにRPCで攻撃を要求
            _instance.RPC_RequestAttack(entity.Id, baseDamage);
        }

        // ---- クライアント→ホスト ----
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_RequestAttack(string id, int baseDamage)
        {
            int finalDamage = CalcDamage(baseDamage);

            // 全員に「攻撃結果」を通知
            RPC_NotifyAttackResult(id, finalDamage);
        }

        // ---- ホスト→全クライアント ----
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyAttackResult(string id, int finalDamage)
        {
            if (!_instance._entities.TryGetValue(id, out CharacterEntity entity)) return;

            Debug.Log($"[Notify] {entity} に {finalDamage} のダメージが適用されました");

            // クライアント側も表示・エフェクトなどを再生可能
            entity.TakeDamage(finalDamage);
        }

        // ---- ダメージ計算 ----
        private int CalcDamage(int baseDamage)
        {
            int result = baseDamage + 3;
            Debug.Log($"[ServerCalc] 結果ダメージ: {result}");
            return result;
        }

        private Dictionary<string, CharacterEntity> _entities = new();

        public override void Spawned()
        {
            base.Spawned();

            if (_instance == null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }
    }
}
