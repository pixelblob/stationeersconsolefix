using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using UI.ImGuiUi;

namespace MyFirstPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

        MethodInfo original = AccessTools.Method(typeof(RocketSystemConsole), "SetConsoleOutputCP");

        MethodInfo patch = AccessTools.Method(typeof(MyPatches), "SetConsoleOutputCP_MyPatch");

        harmony.Patch(original, new HarmonyMethod(patch));

        MethodInfo original1 = AccessTools.Method(typeof(RocketSystemConsole), "SetConsoleCP");

        MethodInfo patch1 = AccessTools.Method(typeof(MyPatches), "SetConsoleCP_MyPatch");

        harmony.Patch(original1, new HarmonyMethod(patch1));

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loadedaaaaaaaaa!");
    }
}


public class MyPatches
{
    public static bool SetConsoleOutputCP_MyPatch(uint wCodePageID)
    {
        return false;
    }

    public static bool SetConsoleCP_MyPatch(uint wCodePageID)
    {
        return false;
    }
}