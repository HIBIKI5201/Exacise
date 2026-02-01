using NovelGame.Master.Scripts.UI;
using NovelGame.Master.Scripts.Utility;
using System;
using UnityEngine;

namespace NovelGame.Master
{
    [CreateAssetMenu(fileName = nameof(ActorAssetDataBase),
        menuName = InfraContraint.ASSET_PATH + nameof(ActorAssetDataBase))]
    public class ActorAssetDataBase : ScriptableObject
    {
        public ActorPresenter this[string name] => 
            Array.Find(_actorAssets, a => a.Name == name);
        public int Length => _actorAssets.Length;

        [SerializeField]
        private ActorPresenter[] _actorAssets;
    }
}
