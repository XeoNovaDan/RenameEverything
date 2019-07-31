using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public static class ModCompatibilityCheck
    {

        public static bool DualWield => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Dual Wield");

        public static bool Infused => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Infused");

        public static bool RPGStyleInventory => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "[1.0] RPG Style Inventory");

    }

}
