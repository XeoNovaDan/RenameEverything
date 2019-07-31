using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class RenameEverythingSettings : ModSettings
    {

        public static bool showNameOnGround = true;
        public static bool appendCachedLabel = false;
        public static bool onlyAppendInThingHolder = true;
        public static bool pawnWeaponRenameGizmos = true;

        public static bool dualWieldInspectString = true;

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);
            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            // Show name when on the ground
            options.Gap();
            options.CheckboxLabeled("RenameEverything.ShowNameOnGround".Translate(), ref showNameOnGround, "RenameEverything.ShowNameOnGround_Tooltip".Translate());

            // Append original label to an object's name
            options.Gap();
            options.CheckboxLabeled("RenameEverything.AppendCachedLabel".Translate(), ref appendCachedLabel, "RenameEverything.AppendCachedLabel_Tooltip".Translate());

            // Only append when equipped (grey out if appendCachedLabel is false)
            if (!appendCachedLabel)
                GUI.color = Color.grey;
            options.Gap();
            options.CheckboxLabeled("RenameEverything.AppendCachedLabelInThingHolder".Translate(), ref onlyAppendInThingHolder, "RenameEverything.AppendCachedLabelInThingHolder_Tooltip".Translate());
            GUI.color = defaultColor;

            // Show equipment renaming buttons on pawns
            options.Gap();
            options.CheckboxLabeled("RenameEverything.ShowRenameGizmosOnPawns".Translate(), ref pawnWeaponRenameGizmos, "RenameEverything.ShowRenameGizmosOnPawns_Tooltip".Translate());

            #region Dual Wield
            // Dual Wield integration
            Text.Font = GameFont.Medium;
            options.Gap(24);
            options.Label("RenameEverything.DualWieldIntegration".Translate());
            Text.Font = GameFont.Small;

            // Dual Wield not active
            if (!ModCompatibilityCheck.DualWield)
            {
                GUI.color = Color.grey;
                options.Label("RenameEverything.DualWieldNotActive".Translate());
                GUI.color = defaultColor;
            }

            else
            {
                // Show both weapon names when dual wielding
                options.Gap();
                options.CheckboxLabeled("RenameEverything.ShowBothWeaponNamesDualWield".Translate(), ref dualWieldInspectString, "RenameEverything.ShowBothWeaponNamesDualWield_Tooltip".Translate());
            }
            #endregion

            // Finish
            options.End();
            Mod.GetSettings<RenameEverythingSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref showNameOnGround, "showNameOnGround", true);
            Scribe_Values.Look(ref appendCachedLabel, "appendCachedLabel", false);
            Scribe_Values.Look(ref onlyAppendInThingHolder, "onlyAppendInThingHolder", true);
            Scribe_Values.Look(ref pawnWeaponRenameGizmos, "pawnWeaponRenameGizmos", true);

            Scribe_Values.Look(ref dualWieldInspectString, "dualWieldInspectString", true);
        }

    }

}
