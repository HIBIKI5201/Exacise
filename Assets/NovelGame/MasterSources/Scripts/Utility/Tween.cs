using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NovelGame.Master.Scripts.Utility
{
    public static class Tween
    {
        /// <summary>
        ///     指定した時間の間、AnimationCurveかLerpな曲線で指定した範囲を毎フレーム実行する
        ///     curveを指定した場合はCurveで、指定しないかnullの場合はLerpで実行される
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s">スタートの値</param>
        /// <param name="action">実行内容</param>
        /// <param name="e">エンドの値</param>
        /// <param name="d">長さ</param>
        /// <param name="curve">曲線を決める（xの大きさで正規化される）</param>
        /// <param name="token"></param>
        public static async ValueTask Tweening<T>(T s, Action<T> action, T e, float d,
            AnimationCurve curve = null,
            IPauseHandler ph = null,
            CancellationToken token = default) where T : struct
        {
            if (d <= 0f)
            {
                action?.Invoke(e);
                return;
            }

            curve = NormalizeCurve(curve);
            var timer = Time.time;

            //時間終了までループ
            while (Time.time <= timer + d)
            {
                var elapsed = Time.time - timer;

                var t = Mathf.Clamp01(elapsed / d); //正規化された値

                var result = curve != null ? CurveValue((s, e), t, curve) : LerpValue((s, e), t);

                if (result == null)
                {
                    Debug.LogWarning($"{typeof(T).Name}型は{nameof(Tweening)}に対応していません");
                    return;
                }

                action?.Invoke(result.Value);

                await Awaitable.NextFrameAsync(token);

                if (ph != null) { await ph.WaitResumeAsync(token); }
            }

            //最後に最終値にする。
            action?.Invoke(e);
        }

        /// <summary>
        ///     対応した型のLerpした値を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static T? LerpValue<T>((T s, T e) value, float t) where T : struct
        {
            //それぞれの型でLerpを実行
            T? result = value switch
            {
                (int s, int e) => (T)Convert.ChangeType(Mathf.Lerp(s, e, t), typeof(T)),
                (float s, float e) => (T)Convert.ChangeType(Mathf.Lerp(s, e, t), typeof(T)),
                (Vector2 s, Vector2 e) => (T)Convert.ChangeType(Vector2.Lerp(s, e, t), typeof(T)),
                (Vector3 s, Vector3 e) => (T)Convert.ChangeType(Vector3.Lerp(s, e, t), typeof(T)),
                (Quaternion s, Quaternion e) => (T)Convert.ChangeType(Quaternion.Lerp(s, e, t), typeof(T)),
                (Color s, Color e) => (T)Convert.ChangeType(Color.Lerp(s, e, t), typeof(T)),
                _ => null
            };

            return result;
        }

        /// <summary>
        ///     対応した型のCurveのEvaluateした値を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        private static T? CurveValue<T>((T s, T e) value, float t, AnimationCurve curve) where T : struct
        {
            //対応する型でカーブの量を掛ける
            T? result = value switch
            {
                (int s, int e) => (T)Convert.ChangeType((e - s) * curve.Evaluate(t), typeof(T)),
                (float s, float e) => (T)Convert.ChangeType((e - s) * curve.Evaluate(t), typeof(T)),
                (Vector2 s, Vector2 e) => (T)Convert.ChangeType((e - s) * curve.Evaluate(t), typeof(T)),
                (Vector3 s, Vector3 e) => (T)Convert.ChangeType((e - s) * curve.Evaluate(t), typeof(T)),
                (Color s, Color e) => (T)Convert.ChangeType((e - s) * curve.Evaluate(t), typeof(T)),
                _ => null
            };

            return result;
        }

        /// <summary>
        ///     カーブのキーをxが0~1になるように正規化する
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        private static AnimationCurve NormalizeCurve(AnimationCurve curve)
        {
            if (curve == null || curve.length == 0)
                return null;

            // 最後のキーの時間（x軸の最大値）を取得
            var maxTime = curve.keys[curve.length - 1].time;

            // 新しいカーブを作成
            var normalizedCurve = new AnimationCurve();

            // 各キーを正規化して新しいカーブに追加
            foreach (var key in curve.keys)
            {
                var normalizedTime = key.time / maxTime;
                var normalizedKey = new Keyframe(normalizedTime, key.value, key.inTangent, key.outTangent);
                normalizedCurve.AddKey(normalizedKey);
            }

            return normalizedCurve;
        }
    }
}
