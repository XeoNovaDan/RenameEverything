using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;
using ColourPicker;

namespace RenameEverything
{

    public static class RenameUtility
    {

        private const int MaxTextWidth = 65;

        private static Color cachedGUIColour;

        public static IEnumerable<Gizmo> GetRenamableCompGizmos(CompRenamable renamableComp)
        {
            string filler = renamableComp.Props.inspectStringTranslationKey.Translate().UncapitalizeFirst();
            // Rename
            yield return new Command_Rename()
            {
                renamable = renamableComp,
                defaultLabel = renamableComp.Props.renameTranslationKey.Translate(),
                defaultDesc = "RenameEverything.RenameGizmo_Description".Translate(filler),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1,
            };

            // Recolour label
            yield return new Command_RecolourLabel()
            {
                renamable = renamableComp,
                defaultLabel = "RenameEverything.RecolourLabel".Translate(),
                defaultDesc = "RenameEverything.RecolourLabel_Description".Translate(filler),
                icon = TexButton.RecolourTex
            };

            if (renamableComp.Named || renamableComp.Coloured)
            {
                // Allow merging
                if (renamableComp.parent.def.stackLimit > 1)
                    yield return new Command_Toggle()
                    {
                        defaultLabel = "RenameEverything.AllowMerging".Translate(),
                        defaultDesc = "RenameEverything.AllowMerging_Description".Translate(),
                        icon = TexButton.AllowMergingTex,
                        isActive = () => renamableComp.allowMerge,
                        toggleAction = () => renamableComp.allowMerge = !renamableComp.allowMerge
                    };

                // Remove name
                if (renamableComp.Named)
                    yield return new Command_Action()
                    {
                        defaultLabel = "RenameEverything.RemoveName".Translate(),
                        defaultDesc = "RenameEverything.RemoveName_Description".Translate(filler),
                        icon = TexButton.DeleteX,
                        action = () => renamableComp.Named = false,
                    };
            }
        }

        public static IEnumerable<CompRenamable> GetRenamableEquipmentComps(Pawn pawn)
        {
            // Equipment
            if (pawn.equipment != null)
                foreach (var eq in pawn.equipment.AllEquipmentListForReading)
                    if (eq.GetComp<CompRenamable>() is CompRenamable renamableComp)
                        yield return renamableComp;

            // Apparel
            if (pawn.apparel != null)
                foreach (var ap in pawn.apparel.WornApparel)
                    if (ap.GetComp<CompRenamable>() is CompRenamable renamableComp)
                        yield return renamableComp;
        }

        public static void ChangeGUIColourPreLabelDraw(IEnumerable<Thing> things)
        {
            if (things.Count() == 1)
                ChangeGUIColourPreLabelDraw(things.First());
            else
                cachedGUIColour = GUI.color;
        }

        public static MethodInfo ChangeGUIColourPreLabelDraw_IEnumerableThing_Info => AccessTools.Method(typeof(RenameUtility), nameof(ChangeGUIColourPreLabelDraw), new Type[] { typeof(IEnumerable<Thing>) });

        public static void ChangeGUIColourPreLabelDraw(Thing thing)
        {
            // Store the current GUI labelColour and change the GUI labelColour to what's defined in renamableComp
            cachedGUIColour = GUI.color;
            if (thing.GetInnerIfMinified().TryGetComp<CompRenamable>() is CompRenamable renamableComp)
            {
                GUI.color = renamableComp.labelColour;
            }
        }

        public static MethodInfo ChangeGUIColourPreLabelDraw_Thing_Info => AccessTools.Method(typeof(RenameUtility), nameof(ChangeGUIColourPreLabelDraw), new Type[] { typeof(Thing) });

        public static void ChangeGUIColourPostLabelDraw()
        {
            // After the label has been drawn, change the labelColour back to the previous one
            GUI.color = cachedGUIColour;
        }

        public static MethodInfo ChangeGUIColourPostLabelDraw_Info => AccessTools.Method(typeof(RenameUtility), nameof(ChangeGUIColourPostLabelDraw));

        public static IEnumerable<FloatMenuOption> CaravanRenameThingButtonFloatMenuOptions(CompRenamable renamableComp)
        {
            // Rename
            yield return new FloatMenuOption(renamableComp.Props.renameTranslationKey.Translate(), () => Find.WindowStack.Add(new Dialog_RenameThings(renamableComp)));

            // Recolour
            yield return new FloatMenuOption("RenameEverything.RecolourLabel".Translate(), () => Find.WindowStack.Add(new Dialog_ColourPicker(renamableComp.labelColour, c => renamableComp.labelColour = c)));

            // Remove name
            if (renamableComp.Named)
                yield return new FloatMenuOption("RenameEverything.RemoveName".Translate(), () => renamableComp.Named = false);
        }

        public static void DrawThingName(Thing thing)
        {
            if (RenameEverythingSettings.showNameOnGround && thing.TryGetComp<CompRenamable>() is CompRenamable renamableComp && renamableComp.Named)
            {
                // Do background
                Text.Font = GameFont.Tiny;
                var screenPos = GenMapUI.LabelDrawPosFor(thing, -0.4f);
                string text = Text.CalcSize(renamableComp.Name).x <= MaxTextWidth ? renamableComp.Name : renamableComp.Name.Shorten().Truncate(MaxTextWidth);
                float x = Text.CalcSize(text).x;
                var backgroundRect = new Rect(screenPos.x - x / 2 - 4, screenPos.y, x + 8, 12);
                GUI.DrawTexture(backgroundRect, TexUI.GrayTextBG);

                // Do label
                Text.Anchor = TextAnchor.UpperCenter;
                ChangeGUIColourPreLabelDraw(thing);
                var textRect = new Rect(screenPos.x - x / 2, screenPos.y - 3, x, 999);
                Widgets.Label(textRect, text);
                ChangeGUIColourPostLabelDraw();

                // Finish off
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
        }

    }

}
