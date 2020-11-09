using System;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using HarmonyLib;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace RespectPresence {
    public enum GameMode {
        NONE,
        AIR,
        FREESTYLE,
        ONLINE,
        MISSION
    }

    class CoreCompontent : MonoBehaviour {
        public CoreCompontent(IntPtr ptr) : base(ptr) {
            Plugin.Logging.LogMessage("CoreCompontent constructor entered");
        }

        public static GameMode CurrentMode = GameMode.NONE;

        #region Helpers
        public static string GetDifficultyName(int _difficulty) {
            switch (_difficulty) {
                case 1: return "Normal";
                case 2: return "Hard";
                case 3: return "Maximum";
                case 5: return "SC";
            }
            return "Unknown";
        }

        public static string GetModeName(GameMode _mode) {
            switch (_mode) {
                case GameMode.AIR: return "AIR";
                case GameMode.FREESTYLE: return "Freestyle";
                case GameMode.ONLINE: return "Online";
                case GameMode.MISSION: return "Mission";
            }
            return "Unknown";
        }

        public static string GetKeymodeName(int _mode) {
            return "4B";
            switch (_mode) {
                case 4: return "4B";
                case 5: return "5B";
                case 6: return "6B";
                case 8: return "8B";
            }
            return "unknown";
        }

        public static void RunCallbacks() {
            Plugin.DiscordInstance.RunCallbacks();
        }
        #endregion

        #region Song Select
        public static SongInfoDetailView SongInfo = null;
        public static DifficultChangeController SongDifficultySwitcher = null;
        public static BaseKeymodeChangeController KeymodeSwitcher = null;
        public static string SongName = "";
        public static string SongComposer = "";
        public static int SongDifficulty = -1;
        public static int CurrentKeyMode = -1;

        // Grabs SongInfoDetailView and stores reference
        public static void GetSongInfo(SongInfoDetailView __instance) {
            SongInfo = __instance;
        }

        // Grabs DifficultChangeController and stores reference
        public static void GetDifficultySwitcher(DifficultChangeController __instance) {
            //BepInExLoader.Logging.LogMessage("ref difficulty");
            SongDifficultySwitcher = __instance;
        }

        // Grabs BaseKeymodeChangeController and stores reference
        public static void GetKeymodeSwitcher(BaseKeymodeChangeController __instance) {
            Console.WriteLine("GOT BaseKeymodeChangeController");
            KeymodeSwitcher = __instance;
        }

        // Clear references
        public static void RemoveReferences() {
            SongInfo = null;
            SongDifficultySwitcher = null;
            KeymodeSwitcher = null;
        }

        // Cheap update event in song select
        public static void SongSelectUpdate() {
            if (SongInfo != null) {
                SongName = SongInfo.m_songTitleLabel.mText;
                SongComposer = SongInfo.m_songComposerLabel.mText;
                if (SongDifficultySwitcher != null) SongDifficulty = (int)SongDifficultySwitcher.CLKCGPLNLNC();
                if (KeymodeSwitcher != null) CurrentKeyMode = (int)KeymodeSwitcher.CADLDNCJNJK;
                //Console.WriteLine(CurrentKeyMode);
            }
        }

        /*
            1 = Normal
            2 = Hard
            3 = Maximum
            4 = ???
            5 = SC
        */
        #endregion

        #region Ingame
        public static void IngameStart(object __instance) {
            Plugin.SetActivity(new Discord.Activity {
                State = "Playing " + GetModeName(CurrentMode) + (CurrentMode == GameMode.MISSION ? "" : " (" + GetDifficultyName(SongDifficulty) + ")"),
                Details = SongComposer + " - " + SongName,
                Assets = {
                    LargeImage = "asset_" + GetModeName(CurrentMode).ToLower(),
                    LargeText = GetModeName(CurrentMode),
                    SmallImage = "asset_" + GetKeymodeName(CurrentKeyMode).ToLower() + "utton",
                    SmallText = GetKeymodeName(CurrentKeyMode)
                },
                Timestamps = {
                    Start = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
                }
            });
        }
        #endregion

        #region Mode Catches
        public static void FreestyleCatch(FreestylePanelController __instance) {
            CurrentMode = GameMode.FREESTYLE;
        }

        public static void AirCatch(AirModeNextTuneView __instance) {
            CurrentMode = GameMode.AIR;
        }

        public static void MissionCatch(MissionDetailRotator __instance) {
            CurrentMode = GameMode.MISSION;
        }

        public static void ResultCatch() {
            Plugin.SetActivity(new Discord.Activity {
                State = "Viewing Results",
                Details = SongComposer + " - " + SongName,
                Assets = {
                    LargeImage = "asset_" + GetModeName(CurrentMode).ToLower(),
                    LargeText = GetModeName(CurrentMode),
                    SmallImage = "asset_" + GetKeymodeName(CurrentKeyMode),
                    SmallText = GetKeymodeName(CurrentKeyMode)
                }
            });
        }
        #endregion
    }
}
