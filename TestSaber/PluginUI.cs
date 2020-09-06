using BeatSaberMarkupLanguage.MenuButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SongCore;

namespace TestSaber
{
    public class PluginUI : PersistentSingleton<PluginUI>
    {
        public MenuButton _environmentButton;
        internal void Setup()
        {
            _environmentButton = new MenuButton("TestSaber", "Saber Testing environment", EnvironmentButtonPressed, true);
            MenuButtons.instance.RegisterButton(_environmentButton);
        }

        internal void EnvironmentButtonPressed()
        {
        }
    }
}