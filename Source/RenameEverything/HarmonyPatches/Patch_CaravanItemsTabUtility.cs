using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using ColourPicker;

namespace RenameEverything
{

    public static class Patch_CaravanItemsTabUtility
    {

        [HarmonyPatch(typeof(CaravanItemsTabUtility))]
        [HarmonyPatch("DoRow")]
        [HarmonyPatch(new Type[] { typeof(Rect), typeof(TransferableImmutable), typeof(Caravan) })]
        public static class Patch_DrawThingRow
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var wordWrapInfo = AccessTools.Property(typeof(Text), nameof(Text.WordWrap)).GetSetMethod();
                int wordWraps = 0;

                var infoCardButtonInfo = AccessTools.Method(typeof(Widgets), nameof(Widgets.InfoCardButton), new Type[] { typeof(float), typeof(float), typeof(Thing) });
                var anyThingInfo = AccessTools.Property(typeof(Transferable), nameof(Transferable.AnyThing)).GetGetMethod();

                var doRenameFloatMenuButtonInfo = AccessTools.Method(typeof(Patch_DrawThingRow), nameof(DoRenameFloatMenuButton));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Do our 'renamable gizmos substitute' button after the info card button
                    if (instruction.opcode == OpCodes.Call && instruction.operand == infoCardButtonInfo)
                    {
                        yield return instruction;
                        yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        instruction = new CodeInstruction(OpCodes.Call, doRenameFloatMenuButtonInfo);
                    }

                    // Label recolouring
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
                            yield return new CodeInstruction(OpCodes.Ldarg_1); // thing
                            yield return new CodeInstruction(OpCodes.Callvirt, anyThingInfo); // thing.AnyThing
                            instruction = new CodeInstruction(OpCodes.Call, RenameUtility.ChangeGUIColourPreLabelDraw_Thing_Info); // ChangeGUIColourPreLabelDraw(thing.AnyThing)
                        }
                    }

                    yield return instruction;
                }
            }

            private static void DoRenameFloatMenuButton(ref Rect rect2, Rect rect, TransferableImmutable thing)
            {
                rect2.width -= 24;
                var renamableComp = thing.AnyThing.TryGetComp<CompRenamable>();
                if (renamableComp != null && Widgets.ButtonImage(new Rect(rect2.width - 24, rect.height - 24, 24, 24), TexButton.RenameTex))
                {
                    Find.WindowStack.Add(new FloatMenu(FloatMenuOptions(renamableComp).ToList()));
                }
            }

            //public static void Postfix(Rect rect, TransferableImmutable thing)
            //{
            //    // Try and squeeze in a rename/recolour/remove name button between the mass and info card rects
            //    var renamableComp = thing.AnyThing.TryGetComp<CompRenamable>();
            //    if (renamableComp != null)
            //    {
            //        var rowRectZeroed = rect.AtZero();
            //        if (Widgets.ButtonImage(new Rect(rowRectZeroed.width - 96, rowRectZeroed.height, 24, 24), TexButton.RenameTex))
            //        {
            //            Find.WindowStack.Add(new FloatMenu(FloatMenuOptions(renamableComp).ToList()));
            //        }
            //    }
            //}

            private static IEnumerable<FloatMenuOption> FloatMenuOptions(CompRenamable renamableComp)
            {
                // Rename
                yield return new FloatMenuOption(renamableComp.Props.renameTranslationKey.Translate(), () => Find.WindowStack.Add(new Dialog_RenameThings(renamableComp)));

                // Recolour
                yield return new FloatMenuOption("RenameEverything.RecolourLabel".Translate(), () => Find.WindowStack.Add(new Dialog_ColourPicker(renamableComp.labelColour, c => renamableComp.labelColour = c)));

                // Remove name
                if (renamableComp.Named)
                    yield return new FloatMenuOption("RenameEverything.RemoveName".Translate(), () => renamableComp.Named = false);
            }

        }

    }

}
