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

namespace RenameYourWeapons
{

    public static class Patch_Pawn
    {

        [HarmonyPatch(typeof(Pawn))]
        [HarmonyPatch(nameof(Pawn.GetGizmos))]
        public static class Patch_GetGizmos
        {

            public static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
            {
                // Add weapon rename gizmos to pawn if they have a weapon equipped and that weapon has CompRenamable (which should be true 99.9% of the time)
                var equipmentTracker = __instance.equipment;
                if (equipmentTracker != null && equipmentTracker.Primary != null)
                {
                    var renamableComp = equipmentTracker.Primary.GetComp<CompRenamable>();
                    if (renamableComp != null)
                        __result = __result.Concat(RenameWeaponsUtility.RenameWeaponGizmos(renamableComp));
                }
            }

        }

        //[HarmonyPatch(typeof(Pawn))]
        //[HarmonyPatch(nameof(Pawn.GetInspectString))]
        //public static class Patch_GetInspectString
        //{

        //    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //    {
        //        var instructionList = instructions.ToList();

        //        var thingLabelInfo = AccessTools.Property(typeof(Entity), nameof(Entity.Label)).GetGetMethod();
        //        var adjustedShownPrimaryEquipmentLabelInfo = AccessTools.Method(typeof(Patch_GetInspectString), nameof(AdjustedShownPrimaryEquipmentLabel));

        //        for (int i = 0; i < instructionList.Count; i++)
        //        {
        //            var instruction = instructionList[i];

        //            if (instruction.opcode == OpCodes.Callvirt && instruction.operand == thingLabelInfo)
        //            {
        //                yield return instruction;
        //                yield return new CodeInstruction(OpCodes.Ldarg_0);
        //                instruction = new CodeInstruction(OpCodes.Call, adjustedShownPrimaryEquipmentLabelInfo);
        //            }

        //            yield return instruction;
        //        }
        //    }

        //    public static string AdjustedShownPrimaryEquipmentLabel(string original, Pawn instance)
        //    {
        //        Nullchecking isn't necessary but it's just in the case that other mods transpile this methods too
        //        var equipmentTracker = instance.equipment;
        //        if (equipmentTracker != null && equipmentTracker.Primary != null)
        //        {
        //            var renamableComp = equipmentTracker.Primary.GetComp<CompRenamable>();
        //            if (renamableComp != null && renamableComp.Named)
        //            {
        //                return original + $" ({renamableComp.cachedLabel.CapitalizeFirst()})";
        //            }
        //        }
        //        return original;
        //    }

        //}

    }

}
