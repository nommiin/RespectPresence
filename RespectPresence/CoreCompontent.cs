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
    class CoreCompontent : MonoBehaviour {
        public CoreCompontent(IntPtr ptr) : base(ptr) {
            Plugin.Logging.LogMessage("CoreCompontent constructor entered");
        }

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
        #endregion

        #region Song Select
        public static SongInfoDetailView SongInfo = null;
        public static DifficultChangeController SongDifficultySwitcher = null;
        public static string SongName = "";
        public static string SongComposer = "";
        public static int SongDifficulty = -1;

        // Grabs SongInfoDetailView and stores reference
        public static void GetSongInfo(SongInfoDetailView __instance) {
            SongInfo = __instance;
        }

        // Grabs DifficultChangeController and stores reference
        public static void GetDifficultySwitcher(DifficultChangeController __instance) {
            //BepInExLoader.Logging.LogMessage("ref difficulty");
            SongDifficultySwitcher = __instance;
        }

        // Clear references
        public static void RemoveReferences() {
            SongInfo = null;
            SongDifficultySwitcher = null;
        }

        // Cheap update event in song select
        public static void SongSelectUpdate() {
            if (SongInfo != null) {
                SongName = SongInfo.m_songTitleLabel.mText;
                SongComposer = SongInfo.m_songComposerLabel.mText;
                if (SongDifficultySwitcher != null) {
                    SongDifficulty = (int)SongDifficultySwitcher.CLKCGPLNLNC();
                    //Console.WriteLine(SongDifficulty.GetType().GetMethod("").Invoke(BepInExLoader.DifficultyMethod, null));
                    //Console.WriteLine((int)SongDifficultySwitcher.CLKCGPLNLNC());
                }
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
        public static void IngameStart() {
            Plugin.DiscordActivity.UpdateActivity(new Discord.Activity {
                State = "Playing Freestyle (" + GetDifficultyName(SongDifficulty) + ")",
                Details = SongComposer + " - " + SongName,
                Assets = {
                    LargeImage = "asset_freestyle",
                    SmallImage = "asset_4button"
                },
                Timestamps = {
                    Start = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds
                }
            }, result => {
                Console.WriteLine(result);
            });
        }

        public static void IngameUpdate() {
            Plugin.DiscordInstance.RunCallbacks();
        }
        #endregion
    }
}
