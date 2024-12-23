using HarmonyLib;
using UI.ImGuiUi;

namespace ConsolePatch.Patches
{
    [HarmonyPatch(typeof(RocketSystemConsole))]
    internal class RocketSystemConsolePatch
    {
        [HarmonyPatch("SetConsoleOutputCP")]
        [HarmonyPrefix]
        static bool SetConsoleOutputCP_Patch()
        {
            return false;
        }

        [HarmonyPatch("SetConsoleCP")]
        [HarmonyPrefix]
        static bool SetConsoleCP_Patch()
        {
            return false;
        }


    }
}
