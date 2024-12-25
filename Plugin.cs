using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ConsolePatch.Patches;

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
    }
}
