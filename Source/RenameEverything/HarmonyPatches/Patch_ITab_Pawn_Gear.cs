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

    public static class Patch_ITab_Pawn_Gear
    {

        [HarmonyPatch(typeof(ITab_Pawn_Gear))]
        [HarmonyPatch("DrawThingRow")]
        public static class Patch_DrawThingRow
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var wordWrapInfo = AccessTools.Property(typeof(Text), nameof(Text.WordWrap)).GetSetMethod();
                int wordWraps = 0;

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Call && instruction.operand == wordWrapInfo)
                    {
                        wordWraps++;
                        yield return instruction;
                        if (wordWraps % 2 == 0)
                        {
                            instruction = new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPostLabelDraw_Info); // ChangeGUIColourPostLabelDraw()
                        }
                        else
                        {
                            yield return new CodeInstruction(OpCodes.Ldarg_3); // thing
                            instruction = new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPreLabelDraw_Thing_Info); // ChangeGUIColourPreLabelDraw(thing)
                        }
                    }

                    yield return instruction;
                }
            }

        }

    }

}
