using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace BlindMonke
{
    [ModdedGamemode("blindI", "BLIND INFECTION", Utilla.Models.BaseGamemode.Infection)]
    [ModdedGamemode("blindH", "BLIND HUNT", Utilla.Models.BaseGamemode.Hunt)]
    [ModdedGamemode("blindC", "BLIND CASUAL", Utilla.Models.BaseGamemode.Casual)]
    [ModdedGamemode("blindP", "BLIND PAINTBRAWL", Utilla.Models.BaseGamemode.Paintbrawl)]
    [BepInDependency("org.legoandmars.gorillatag.utilla")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        GameObject asset;

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            var Blind = LoadAssetBundle("BlindMonke.Assets.blindmonke");
            var tempAsset = Blind.LoadAsset<GameObject>("Blindness");
            asset = Instantiate(tempAsset);
            asset.transform.parent = GorillaTagger.Instance.mainCamera.transform;
            asset.transform.localPosition = new Vector3(0, 0, 0);
            asset.SetActive(false);
        }

        AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            asset.SetActive(true);
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            asset.SetActive(false);
        }
    }
}