using BeatSaberMarkupLanguage.MenuButtons;

namespace TestSaber
{
    public class PluginUI : PersistentSingleton<PluginUI>
    {
        public MenuButton _environmentButton;
        private const string LEVEL_HASH = "5427FBB9C36F6EC96EF58A11489AC5F89C1D2EC8";

        internal void Setup()
        {
            _environmentButton = new MenuButton("TestSaber", "Saber Testing environment", EnvironmentButtonPressed, true);
            MenuButtons.instance.RegisterButton(_environmentButton);
        }

        internal void EnvironmentButtonPressed()
        {
            CustomPreviewBeatmapLevel level = SongCore.Loader.GetLevelByHash(LEVEL_HASH);
            BeatmapDifficulty beatmapDifficulty = level.previewDifficultyBeatmapSets[0].beatmapDifficulties[0];
            BeatmapCharacteristicSO beatmapCharacteristic = level.previewDifficultyBeatmapSets[0].beatmapCharacteristic;
            Utils.PlaySong(level, beatmapCharacteristic, beatmapDifficulty);
        }
    }
}