using Fusion;
using UnityEngine;

namespace PhotonExacise.Scripts
{
    /*
    public class CharacterUseCase : NetworkBehaviour
    {
        private static CharacterUseCase _instance;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            
        }

        // ---- 攻撃要求を送る（クライアントから呼ばれる）----
        public static void Attack(CharacterEntity entity, int baseDamage)
        {
            // クライアントがホストにRPCで攻撃を要求
            _instance.RPC_RequestAttack(entity, baseDamage);
        }

        // ---- クライアント→ホスト ----
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_RequestAttack(CharacterEntity entity, int baseDamage)
        {
            int finalDamage = CalcDamage(baseDamage);

            // ダメージを適用（ホストで計算・反映）
            entity.TakeDamage(finalDamage);

            // 全員に「攻撃結果」を通知
            RPC_NotifyAttackResult(entity, finalDamage);
        }

        // ---- ホスト→全クライアント ----
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyAttackResult(CharacterEntity entity, int finalDamage)
        {
            Debug.Log($"[Notify] {entity} に {finalDamage} のダメージが適用されました");

            // クライアント側も表示・エフェクトなどを再生可能
            entity.TakeDamage(finalDamage);
        }

        // ---- ダメージ計算 ----
        private int CalcDamage(int baseDamage)
        {
            int result = baseDamage + Random.Range(-3, 3);
            Debug.Log($"[ServerCalc] 結果ダメージ: {result}");
            return result;
        }
    }
    */
}
