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
                if (RenameYourWeaponsSettings.pawnWeaponRenameGizmos)
                {
                    var equipmentTracker = __instance.equipment;
                    if (equipmentTracker != null)
                    {
                        if (equipmentTracker.Primary != null)
                        {
                            var renamableComp = equipmentTracker.Primary.GetComp<CompRenamable>();
                            if (renamableComp != null)
                                __result = __result.Concat(RenameWeaponsUtility.RenameWeaponGizmos(renamableComp));
                        }

                        // Integration with Dual Wield
                        if (ModCompatibilityCheck.DualWield && ReflectedMethods.TryGetOffHandEquipment(equipmentTracker, out ThingWithComps secondary))
                        {
                            var secondaryRenamableComp = secondary.GetComp<CompRenamable>();
                            if (secondaryRenamableComp != null)
                            {
                                if (RenameYourWeaponsSettings.offHandRenameGizmos)
                                    __result = __result.Concat(RenameWeaponsUtility.RenameWeaponGizmos(secondaryRenamableComp, "RenameYourWeapon.RenameOffHandWeapon", "RenameYourWeapon.RemoveOffHandName"));
                                else
                                    __result = __result.Concat(RenameWeaponsUtility.RenameWeaponGizmos(secondaryRenamableComp));
                            }
                        }
                    }
                }
            }

        }

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
                if (ModCompatibilityCheck.DualWield && RenameYourWeaponsSettings.dualWieldInspectString)
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
