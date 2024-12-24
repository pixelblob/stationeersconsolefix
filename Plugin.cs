using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ConsolePatch.Patches;
using UnityEngine;
using System;
using Assets.Scripts.Serialization;
using System.Reflection;
using System.Threading;
using Assets.Scripts.GridSystem;
using Assets.Scripts;
using System.IO;

namespace ConsolePatch;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    public static Plugin Instance;

    private void Awake()
    {
        Plugin.Instance = this;
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        harmony.PatchAll(typeof(Plugin));
        harmony.PatchAll(typeof(RocketSystemConsolePatch));
        //harmony.PatchAll(typeof(GridManagerPatch));

    }

    public void SaveCurrent()
    {
        MethodInfo writeWorld = typeof(XmlSaveLoad).GetMethod("WriteWorld", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo _validWordData = typeof(XmlSaveLoad).GetField("_validWordData", BindingFlags.NonPublic | BindingFlags.Instance);

        DirectoryInfo saveDir = XmlSaveLoad.Instance.CurrentWorldSave.Directory();
        Console.WriteLine(saveDir.ToString());

        _validWordData.SetValue(XmlSaveLoad.Instance, true);

        writeWorld.Invoke(XmlSaveLoad.Instance, [saveDir.ToString(), false, false]);
    }
}
