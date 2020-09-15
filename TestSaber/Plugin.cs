using System;
using IPA;
using IPALogger = IPA.Logging.Logger;
using BS_Utils.Utilities;
using UnityEngine;

namespace TestSaber
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "TestSaber";

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            instance = this;
            Logger.log = logger;
            Logger.log.Debug("Logger initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Logger.log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            if (Array.Exists<string>(System.Environment.GetCommandLineArgs(), argument => argument == "fpfc"))
            {
                PluginUI.instance.Setup();
                this.AddEvents();
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");

        }

        private void AddEvents()
        {
            BSEvents.gameSceneLoaded += TestSaberController.Load;
            // BSEvents.levelSelected += PrintLevelID;
            BSEvents.levelFailed += TestSaberController.DestroyInstance;
            BSEvents.levelCleared += TestSaberController.DestroyInstance;
        }

        private void PrintLevelID(LevelCollectionViewController levelCollectionViewController, IPreviewBeatmapLevel previewBeatmapLevel)
        {
            Logger.log.Critical(previewBeatmapLevel.levelID.ToString());
        }
    }
}
