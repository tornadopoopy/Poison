/*
 * Tornado Menu compatibility layer for Poison.
 * Copyright (C) 2026 contributors.
 * Distributed under GPL-3.0-or-later; see root LICENSE.
 * Original project: https://github.com/heycanihavethis/Poison
 */
using BepInEx;
using Poison.Menu;
using Poison.Mods;
using System;
using UnityEngine;
using Utilla;
using Utilla.Attributes;

namespace Poison.Tornado
{
    [ModdedGamemode]
    public sealed class TornadoLobbyGate : MonoBehaviour
    {
        public static bool InModdedRoom { get; private set; }
        public static bool Resetting { get; private set; }
        public static bool ForceYToggle => true;

        private bool themed;
        private float nextCheck;

        private void Awake()
        {
            InModdedRoom = false; // Fail closed unless Utilla confirms a modded gamemode.
            ApplyControls();
        }

        [ModdedGamemodeJoin]
        private void OnModdedGamemodeJoined(string gamemode)
        {
            InModdedRoom = true;
            ApplyControls();
            Main.toggleButtonActive = false; // Y opens it; don't open automatically.
            Main.toggleButtonHeld = false;
            themed = false;
            LoggerMessage("Enabled in modded gamemode: " + gamemode);
        }

        [ModdedGamemodeLeave]
        private void OnModdedGamemodeLeft(string gamemode)
        {
            InModdedRoom = false;
            ResetMenu();
            LoggerMessage("Disabled after leaving modded gamemode.");
        }

        private void Update()
        {
            if (!InModdedRoom)
                return;

            // Keep the requested controls even when an old Poison preference is loaded.
            ApplyControls();

            if (!themed && Main.HasLoaded)
            {
                Main.themeType = 0; // Our modified first theme is Tornado Blue.
                Settings.ApplyMenuTheme(0);
                themed = true;
            }
        }

        private static void ApplyControls()
        {
            Main.rightHand = false;
            Main.bothHands = false;
            Main.oneHand = false;
            Main.wristMenu = false; // Main's normal menu already follows the left hand.
            Main.watchMenu = false;
            Main.joystickMenu = false;
            Main.physicalMenu = false;
            Main.barkMenu = false;
            Main.menuButtonIndex = 1; // Y / leftSecondary on Quest Touch.
            Main.toggleButton = true; // Edge-trigger toggle rather than hold.
        }

        public static void EnsureClosed()
        {
            Main.toggleButtonActive = false;
            Main.toggleButtonHeld = false;
            if (Main.menu != null)
                Main.CloseMenu();
        }

        private static void ResetMenu()
        {
            try
            {
                // Disable previously enabled mods, including callbacks that otherwise
                // persist after leaving a modded room. Do not persist their new states.
                Resetting = true;
                if (Main.HasLoaded)
                    Settings.Panic();
            }
            catch (Exception ex)
            {
                LoggerMessage("Error disabling mods: " + ex.Message);
            }
            finally
            {
                Resetting = false;
                EnsureClosed();
            }
        }

        private void OnDestroy()
        {
            InModdedRoom = false;
            ResetMenu();
        }

        private static void LoggerMessage(string message) => Debug.Log("[Tornado Menu] " + message);
    }
}
