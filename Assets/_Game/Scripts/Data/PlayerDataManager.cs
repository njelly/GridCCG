using PlayFab;
using PlayFab.ClientModels;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.GridCCG.Data
{
    public class PlayerDataManager : SingletonBehaviour<PlayerDataManager>
    {
        protected override bool DestroyGameObjectWhenInstanceExists => true;
        protected override bool SetDontDestroyOnLoad => true;
        protected override bool SuppressError => true;

        private void Start()
        {
            var loginRequest = new LoginWithCustomIDRequest
            {
                CreateAccount = true,
                CustomId = SystemInfo.deviceUniqueIdentifier,
            };

            PlayFabClientAPI.LoginWithCustomID(loginRequest, result =>
            {
                
            }, OnPlayFabError);
        }

        private void OnPlayFabError(PlayFabError error)
        {
            Debug.LogError($"PlayFabError {error.Error}: {error.ErrorMessage}");
        }
    }
}