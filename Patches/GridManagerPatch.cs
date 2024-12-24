using Assets.Scripts.GridSystem;
using Assets.Scripts;
using HarmonyLib;
using System;
using Assets.Scripts.Serialization;

namespace ConsolePatch.Patches
{
    [HarmonyPatch(typeof(GridManager))]
    internal class GridManagerPatch
    {
        [HarmonyPatch("OnApplicationQuit")]
        [HarmonyPrefix]
        static bool OnApplicationQuit_Patch()
        {
            Console.WriteLine("--------------------- GridManager.OnApplicationQuit ---------------------");

            Console.WriteLine(!GameManager.IsNewTutorial);
            Console.WriteLine(!GameManager.IsTutorial);

            GameState gameState = GameManager.GameState;

            Console.WriteLine(gameState != GameState.None);
            Console.WriteLine(gameState != GameState.Loading);
            Console.WriteLine(gameState);
            Console.WriteLine(XmlSaveLoad.Instance.SavingWorld);

            Console.WriteLine("SAVING");

            Plugin.Instance.SaveCurrent();

            Console.WriteLine("DONE");
            GameManager.GameState = GameState.None;
            //Application.Quit();
            return false;
        }



    }
}
