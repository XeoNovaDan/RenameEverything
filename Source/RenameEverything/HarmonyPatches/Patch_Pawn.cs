﻿using System;
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

    public static class Patch_Pawn
    {

        [HarmonyPatch(typeof(Pawn))]
        [HarmonyPatch(nameof(Pawn.GetInspectString))]
        public static class Patch_GetInspectString
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var thingLabelInfo = AccessTools.Property(typeof(Entity), nameof(Entity.Label)).GetGetMethod();
                var adjustedEquippedInspectStringInfo = AccessTools.Method(typeof(Patch_GetInspectString), nameof(AdjustedEquippedInspectString));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Callvirt && instruction.operand == thingLabelInfo)
                    {
                        yield return instruction; // this.equipment.Primary.Label
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        instruction = new CodeInstruction(OpCodes.Call, adjustedEquippedInspectStringInfo); // AdjustedEquippedInspectString(this.equipment.Primary.Label, this)
                    }

                    yield return instruction;
                }
            }

            public static string AdjustedEquippedInspectString(string original, Pawn instance)
            {
                // Integration with Dual Wield
                if (ModCompatibilityCheck.DualWield && RenameEverythingSettings.dualWieldInspectString)
                {
                    var equipmentTracker = instance.equipment;
                    if (equipmentTracker != null)
                    {
                        var primary = equipmentTracker.Primary;
                        if (primary != null && ReflectedMethods.TryGetOffHandEquipment(equipmentTracker, out ThingWithComps secondary))
                            return $"{original} {"AndLower".Translate()} {secondary.LabelCap}";
                    }
                }
                return original;
            }

        }

    }

}
