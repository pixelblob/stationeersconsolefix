using Assets.Scripts;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Serialization;
using DLC;
using HarmonyLib;
using System;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using UI.ImGuiUi;
using static System.Net.Mime.MediaTypeNames;

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

        [HarmonyPatch("ConsoleInputThread")]
        [HarmonyPrefix]
        static bool ConsoleInputThread_Patch()
        {
            var RSCType = typeof(RocketSystemConsole);
            var CWType = typeof(ConsoleWindow);
            var bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance;

            var _systemConsoleInput = (RocketSystemConsole)CWType.GetField("_systemConsoleInput", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            var _inputString = (StringBuilder)RSCType.GetField("_inputString", bindingAttr).GetValue(_systemConsoleInput);
            var _inputPrefix = (string)RSCType.GetField("_inputPrefix", bindingAttr).GetValue(_systemConsoleInput);
            var _keepAlive = (bool)RSCType.GetField("_keepAlive", bindingAttr).GetValue(_systemConsoleInput);

            var RedrawInputLine = RSCType.GetMethod("RedrawInputLine", bindingAttr);
            var OnEscape = RSCType.GetMethod("OnEscape", bindingAttr);
            var OnEnter = RSCType.GetMethod("OnEnter", bindingAttr);
            var OnBackspace = RSCType.GetMethod("OnBackspace", bindingAttr);

            _inputString.Append(_inputPrefix);
            while (_keepAlive)
            {
                if (Console.CursorVisible && Console.KeyAvailable)
                {
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                    ConsoleKey key = consoleKeyInfo.Key;
                    if (key == ConsoleKey.Backspace) OnBackspace.Invoke(_systemConsoleInput, []);
                    else if (key == ConsoleKey.Enter) OnEnter.Invoke(_systemConsoleInput, []);
                    else if (key == ConsoleKey.Escape) OnEscape.Invoke(_systemConsoleInput, []);
                    else if (consoleKeyInfo.KeyChar == '\0') RedrawInputLine.Invoke(_systemConsoleInput, []);
                    else _inputString.Append(consoleKeyInfo.KeyChar);
                }
                else
                {
                    string text = Console.ReadLine();
                    _inputString.Clear();
                    _inputString.Append(text);
                    OnEnter.Invoke(_systemConsoleInput, []);
                }
            }
            return false;
        }


    }
}
