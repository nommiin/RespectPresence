# RespectPresence
A BepInEx plugin for DJMAX RESPECT V that displays gameplay information in Discord

# Screenshot~~s~~
![Screenshot](https://i.imgur.com/7jmab9h.png)

# Progress
At the moment, RespectPresence can detect the song name, composer, and selected difficulty when playing Freestyle and Online modes. Both Air and Mission modes have not been tested, and likely do not work due to how RespectPresence works. Additionally, you will notice that the rich presence display in Discord shows "Playing Freestyle" and the 4-button icon regardless of what mode you're playing, this is because the plugin **does not** detect these for the time being.

# Support
RespectPresence has been tested for DJMAX RESPECT V v647. Future versions are likely to fail, as the game employs heavy obfuscation on many classes and their respective members. Ideally, reliance on obfuscated method names will be removed entirely; but for the time being, this is as good as it'll get. This plugin is being developed in my free-time, so it isn't exactly a high priorty for me. I'll be looking into ways to improve this plugin, such as adding in support for all modes, menus, proper RPC display (playmode display, proper mode name display)

# Compiling
1. Install the IL2CPP branch of [BepInEx](https://github.com/BepInEx/BepInEx) for DJMAX RESPECT V ([for further help, see the BepInEx documentation on how to install](https://bepinex.github.io/bepinex_docs/master/articles/user_guide/installation/index.html?tabs=tabid-win))
2. Run the game once to generate unhollowed assemblies
3. Add required BepInEx/HarmonyX/IL2CPP assemblies as references from `BepInEx\core\`
4. Add unhollowed game assemblies from as references from `BepInEx\unhollowed\`
5. Compile the library for x64
6. Copy `RespectPresence.dll` and `discord_game_sdk.dll` from bin folder into `BepInEx\plugins\`

# Thanks
- The BepInEx Discord server for answering my numerous questions
- Thanks to [\u\wh0am15533](https://github.com/wh0am15533) for creating [Il2CPPCSharp-Trainer](https://github.com/wh0am15533/Il2CppCSharp-Trainer) which served as reference when creating IL2CPP plugins
