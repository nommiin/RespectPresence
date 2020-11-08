﻿using System;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnhollowerBaseLib;
using Discord;

using Unity;
using UnityEngine;

namespace RespectPresence {
    [BepInPlugin("com.nommiin.respectrpc", "RespectPresence", "1.0.0")]
    public class Plugin : BepInEx.IL2CPP.BasePlugin {
        public static Discord.Discord DiscordInstance;
        public static ActivityManager DiscordActivity;
        public static Harmony HarmonyInstance = new Harmony("nommiin.respectrpc.il2cpp");
        public static ManualLogSource Logging;
        public Plugin() {
            Plugin.Logging = this.Log;
        }

        public static void Hook(MethodBase _target, MethodInfo _hook) {
            Plugin.HarmonyInstance.Patch(_target, postfix: new HarmonyMethod(_hook));
        }

        public static void SetActivity(Discord.Activity _activity) {
            Plugin.DiscordActivity.UpdateActivity(_activity, result => {});
        }

        public static string DifficultyMethod = null;
        public override void Load() {
            // Register CoreCompontent
            try {
                ClassInjector.RegisterTypeInIl2Cpp<CoreCompontent>();
                Logging.LogMessage("Successfully registered CoreCompontent in IL2CPP");
            } catch {
                Logging.LogError("Failed to register CoreCompontent in IL2CPP");
            }

            // Hook Events
            try {
                // Ingame
                Hook(AccessTools.Method(typeof(SteamMainGameSceneBase), "Start"), AccessTools.Method(typeof(CoreCompontent), "IngameStart"));
                Hook(AccessTools.Method(typeof(SteamMainGameSceneBase), "Update"), AccessTools.Method(typeof(CoreCompontent), "IngameUpdate"));

                // SongInfoDetailView
                Hook(AccessTools.Method(typeof(SongInfoDetailView), "Awake"), AccessTools.Method(typeof(CoreCompontent), "GetSongInfo"));
                // DifficultChangeController
                Hook(AccessTools.Method(typeof(DifficultChangeController), "Awake"), AccessTools.Method(typeof(CoreCompontent), "GetDifficultySwitcher"));

                // Update & Destroy
                Hook(AccessTools.Method(typeof(SongSelectCoverRotator), "Update"), AccessTools.Method(typeof(CoreCompontent), "SongSelectUpdate"));
                Hook(typeof(SongSelectCoverRotator).GetMethod("OnDestroy"), typeof(CoreCompontent).GetMethod("RemoveReferences"));

                // Difficulty Method
                /*

                Intended to grab the method to return the currently selected difficulty enum from DifficultChangeController
                Only problem is that there's ~6 methods that return an enum, which are likely like... LowestDifficulty/HighestDifficulty
                Not really sure what I can do here, as using obfuscated names will inevitably break backwards compatibility (if names are randomized)

                Could be worth checking other classes to see if the difficulty is stored elsewhere, as of v647 the difficulty enum is named EJLGBABDMIP.NACOBCHIEAO
                */
                
                /*
                MethodInfo[] _difficultyMethods = typeof(DifficultChangeController).GetMethods();
                for(int i = 0; i < _difficultyMethods.Length; i++) {
                    if (_difficultyMethods[i].GetParameters().Length == 0) {
                        if (_difficultyMethods[i].ReturnType.IsEnum == true) {
                            DifficultyMethod = _difficultyMethods[i].Name;
                            break;
                        }
                    }
                }
                */
                // CLKCGPLNLNC
                // Type: EJLGBABDMIP.NACOBCHIEAO

                Logging.LogMessage("Successfully hooked events for CoreCompontent");
            } catch {
                Logging.LogError("Failed to hook events for CoreCompontent");
            }

            // Setup Discord
            try {
                DiscordInstance = new Discord.Discord(Int64.Parse("773332255610372117"), (UInt64)CreateFlags.Default);
                DiscordActivity = DiscordInstance.GetActivityManager();
                DiscordInstance.SetLogHook(Discord.LogLevel.Debug, (level, message) => {
                    Logging.LogMessage("[DISCORD] " + message);
                });
                Logging.LogMessage("Successfully loaded Discord API");
            } catch {
                Logging.LogError("Failed to load Discord");
            }
        }
    }
}
