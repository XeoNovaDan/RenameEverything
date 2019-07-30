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

    public static class Patch_InspectPaneUtility
    {

        [HarmonyPatch(typeof(InspectPaneUtility))]
        [HarmonyPatch(nameof(InspectPaneUtility.InspectPaneOnGUI))]
        public static class Patch_InspectPaneOnGUI
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var labelInfo = AccessTools.Method(typeof(Widgets), nameof(Widgets.Label), new Type[] { typeof(Rect), typeof(string) });
                var cachedSelectedThingsInfo = AccessTools.Field(typeof(Patch_InspectPaneOnGUI), nameof(cachedSelectedThings));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Ldloc_S && ((LocalBuilder)instruction.operand).LocalIndex == 4)
                    {
                        var firstAhead = instructionList[i + 1];
                        var secondAhead = instructionList[i + 2];
                        if (firstAhead.opcode == OpCodes.Ldloc_S && ((LocalBuilder)firstAhead.operand).LocalIndex == 5 &&
                            secondAhead.opcode == OpCodes.Call && secondAhead.operand == labelInfo)
                        {
                            yield return new CodeInstruction(OpCodes.Ldsfld, cachedSelectedThingsInfo);
                            yield return new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPreLabelDraw_IEnumerableThing_Info);

                            instructionList.Insert(i + 3, new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPostLabelDraw_Info));
                        }
                    }

                    yield return instruction;
                }
            }

            public static List<Thing> cachedSelectedThings = new List<Thing>();

        }

        [HarmonyPatch(typeof(InspectPaneUtility))]
        [HarmonyPatch(nameof(InspectPaneUtility.AdjustedLabelFor))]
        public static class Patch_AdjustedLabelFor
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var clearInfo = AccessTools.Method(typeof(List<Thing>), nameof(List<Thing>.Clear));

                int clearCount = instructionList.Count(i => i.opcode == OpCodes.Callvirt && i.operand == clearInfo);
                int clearCounts = 0;

                var updateCachedSelectedThings = AccessTools.Method(typeof(Patch_AdjustedLabelFor), nameof(UpdateCachedSelectedThings));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Since AdjustedLabelFor clears selectedThings before said label gets drawn on screen, we have to swoop in and cache selectedThings just before it gets cleared
                    if (instruction.opcode == OpCodes.Callvirt && instruction.operand == clearInfo)
                    {
                        clearCounts++;
                        if (clearCounts == clearCount)
                        {
                            yield return new CodeInstruction(OpCodes.Call, updateCachedSelectedThings);
                            yield return instructionList[i - 1].Clone();
                        }
                    }

                    yield return instruction;
                }
            }

            private static void UpdateCachedSelectedThings(List<Thing> selectedThings)
            {
                Patch_InspectPaneOnGUI.cachedSelectedThings.Clear();
                Patch_InspectPaneOnGUI.cachedSelectedThings.AddRange(selectedThings.ToArray());
            }

        }

    }

}
