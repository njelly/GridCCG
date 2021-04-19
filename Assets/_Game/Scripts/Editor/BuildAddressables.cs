using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Tofunaut.GridCCG.Editor
{
    public class BuildAddressablesUtil
    {
        [MenuItem("Tofunaut/GridCCG/Build Addressables")]
        public static void BuildAddressables()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }
    }
}