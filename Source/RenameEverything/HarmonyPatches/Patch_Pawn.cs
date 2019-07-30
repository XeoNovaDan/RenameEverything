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

    public static class Patch_Pawn
    {

        [HarmonyPatch(typeof(Pawn))]
        [HarmonyPatch(nameof(Pawn.GetGizmos))]
        public static class Patch_GetGizmos
        {

            public static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
            {
                // Add weapon rename gizmos to pawn if they have any equipment that has CompRenamable
                if (RenameEverythingSettings.pawnWeaponRenameGizmos)
                {
                    var renamables = RenameUtility.GetRenamableEquipmentComps(__instance);
                    if (renamables.Any())
                    {
                        var renameGizmo = new Command_RenameFloatMenu()
                        {
                            pawnRenamables = new Pair<Pawn, List<CompRenamable>>(__instance, renamables.ToList()),
                            defaultLabel = "RenameEverything.RenameEquipment".Translate(),
                            defaultDesc = "RenameEverything.RenameEquipment_Description".Translate(),
                            icon = TexButton.RenameTex,
                        };
                        __result = __result.Add(renameGizmo);

                        var recolourGizmo = new Command_RecolourLabelFloatMenu()
                        {
                            pawnRenamables = new Pair<Pawn, List<CompRenamable>>(__instance, renamables.ToList()),
                            defaultLabel = "RenameEverything.RecolourLabel".Translate(),
                            defaultDesc = "RenameEverything.RecolourEquipmentLabel_Description".Translate(),
                            icon = TexButton.RenameTex,
                        };
                        __result = __result.Add(recolourGizmo);

                        if (renamables.Any(r => r.Named))
                        {
                            var removeNameGizmo = new Command_RemoveNameFloatMenu()
                            {
                                pawnRenamables = new Pair<Pawn, List<CompRenamable>>(__instance, renamables.ToList()),
                                defaultLabel = "RenameEverything.RemoveName".Translate(),
                                defaultDesc = "RenameEverything.RemoveEquipmentName_Description".Translate(),
                                icon = TexButton.DeleteX,
                            };
                            __result = __result.Add(removeNameGizmo);
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
