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

            // Combat Extended
            if (ModCompatibilityCheck.CombatExtended)
            {
                var iTabInventoryCE = GenTypes.GetTypeInAnyAssemblyNew("CombatExtended.ITab_Inventory", "CombatExtended");
                if (iTabInventoryCE != null)
                    RenameEverything.HarmonyInstance.Patch(AccessTools.Method(iTabInventoryCE, "DrawThingRow"), transpiler: new HarmonyMethod(typeof(Patch_ITab_Pawn_Gear.Patch_DrawThingRow), "Transpiler"));
                else
                    Log.Error("Could not find type CombatExtended.ITab_Inventory in Combat Extended");
            }

            // RPG Style Inventory (both have the same namespace and similar enough methods)
            if (ModCompatibilityCheck.RPGStyleInventory || ModCompatibilityCheck.RPGStyleInventoryCE)
            {
                // Apply the same transpiler used on ITab_Pawn_Gear.DrawThingRow to RPG_GearTab's DrawThingRow
                var rpgGearTab = GenTypes.GetTypeInAnyAssemblyNew("Sandy_Detailed_RPG_Inventory.Sandy_Detailed_RPG_GearTab", "Sandy_Detailed_RPG_Inventory");
                if (rpgGearTab != null)
                {
                    RenameEverything.HarmonyInstance.Patch(AccessTools.Method(rpgGearTab, "DrawThingRow"), transpiler: new HarmonyMethod(typeof(Patch_ITab_Pawn_Gear.Patch_DrawThingRow), "Transpiler"));
                    RenameEverything.HarmonyInstance.Patch(AccessTools.Method(rpgGearTab, "DrawThingRow1"), transpiler: new HarmonyMethod(typeof(Patch_RPGStyleInventory_Sandy_Detailed_RPG_GearTab.ManualPatch_DrawThingRow1), "Transpiler"));
                }
                else
                    Log.Error("Could not find type Sandy_Detailed_RPG_Inventory.Sandy_Detailed_RPG_GearTab in RPG Style Inventory");
            }

            // Infused
            if (ModCompatibilityCheck.Infused)
            {
                var infusedThingLabelPatch = GenTypes.GetTypeInAnyAssemblyNew("Infused.GenMapUI_DrawThingLabel_Patch", "Infused");
                if (infusedThingLabelPatch != null)
                    RenameEverything.HarmonyInstance.Patch(AccessTools.Method(infusedThingLabelPatch, "Postfix"), transpiler: new HarmonyMethod(typeof(Patch_Infused_GenMapUI_DrawThingLabel_Patch.ManualPatch_Postfix), "Transpiler"));
                else
                    Log.Error("Could not find type Infused.GenMapUI_DrawThingLabel_Patch in Infused");
            }
        }

    }

}
