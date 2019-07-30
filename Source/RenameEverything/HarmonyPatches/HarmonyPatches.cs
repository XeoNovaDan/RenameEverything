using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace RenameEverything
{

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            //HarmonyInstance.DEBUG = true;
            RenameEverything.HarmonyInstance.PatchAll();

            // RPG Style Inventory
            if (ModCompatibilityCheck.RPGStyleInventory)
            {
                // Apply the same transpiler used on ITab_Pawn_Gear.DrawThingRow to RPG_GearTab's DrawThingRow
                var rpgGearTab = GenTypes.GetTypeInAnyAssemblyNew("Sandy_Detailed_RPG_Inventory.Sandy_Detailed_RPG_GearTab", "Sandy_Detailed_RPG_Inventory");
                if (rpgGearTab != null)
                    RenameEverything.HarmonyInstance.Patch(AccessTools.Method(rpgGearTab, "DrawThingRow"), transpiler: new HarmonyMethod(typeof(Patch_ITab_Pawn_Gear.Patch_DrawThingRow), "Transpiler"));
                else
                    Log.Error("Could not find type Sandy_Detailed_RPG_Inventory.Sandy_Detailed_RPG_GearTab in RPG Style Inventory");
            }
        }

    }

}
