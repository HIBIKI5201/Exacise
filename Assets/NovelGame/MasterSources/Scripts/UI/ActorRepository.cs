using System.Collections.Generic;
using UnityEngine;

namespace NovelGame.Master.Scripts.UI
{
    public class ActorRepository
    {
        public ActorRepository(ActorAssetDataBase actorAssetDataBase)
        {
            _actorAssetDataBase = actorAssetDataBase;
        }

        public ActorPresenter GetActorPresenter(string actorName)
        {
            if (_actorDict.TryGetValue(actorName, out ActorPresenter presenter))
            {
                return presenter;
            }

            ActorPresenter asset = _actorAssetDataBase[actorName];
            if (asset != null)
            {
                asset = Object.Instantiate(asset);
                _actorDict[actorName] = asset;
                return asset;
            }

            Debug.LogError($"ActorPresenter not found: {actorName}");
            return null;
        }

        private readonly ActorAssetDataBase _actorAssetDataBase;
        private Dictionary<string, ActorPresenter> _actorDict = new();
    }
}
