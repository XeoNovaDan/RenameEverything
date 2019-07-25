using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class RenameYourWeaponsSettings : ModSettings
    {

        public static bool appendCachedLabel = false;
        public static bool onlyAppendInThingHolder = true;
        public static bool pawnWeaponRenameGizmos = true;

        public static bool offHandRenameGizmos = true;
        public static bool dualWieldInspectString = true;

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);
            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            // Append original label to weapon's name
            options.Gap();
            options.CheckboxLabeled("RenameYourWeapon.AppendCachedLabel".Translate(), ref appendCachedLabel, "RenameYourWeapon.AppendCachedLabel_Tooltip".Translate());

            // Append original label to weapon's name (grey out if appendCachedLabel is false)
            if (!appendCachedLabel)
                GUI.color = Color.grey;
            options.Gap();
            options.CheckboxLabeled("RenameYourWeapon.AppendCachedLabelInThingHolder".Translate(), ref onlyAppendInThingHolder, "RenameYourWeapon.AppendCachedLabelInThingHolder_Tooltip".Translate());
            GUI.color = defaultColor;

            // Show weapon renaming buttons on pawns
            options.Gap();
            options.CheckboxLabeled("RenameYourWeapon.ShowRenameGizmosOnPawns".Translate(), ref pawnWeaponRenameGizmos, "RenameYourWeapon.ShowRenameGizmosOnPawns_Tooltip".Translate());

            #region Dual Wield
            // Dual Wield integration
            Text.Font = GameFont.Medium;
            options.Gap(24);
            options.Label("RenameYourWeapon.DualWieldIntegration".Translate());
            Text.Font = GameFont.Small;

            // Dual Wield not active
            if (!ModCompatibilityCheck.DualWield)
            {
                GUI.color = Color.grey;
                options.Label("RenameYourWeapon.DualWieldNotActive".Translate());
                GUI.color = defaultColor;
            }

            else
            {
                // Show off-hand weapon renaming buttons on pawns
                if (!pawnWeaponRenameGizmos)
                    GUI.color = Color.grey;
                options.Gap();
                options.CheckboxLabeled("RenameYourWeapon.ShowOffHandRenameGizmosOnPawns".Translate(), ref offHandRenameGizmos, "RenameYourWeapon.ShowOffHandRenameGizmosOnPawns_Tooltip".Translate());
                GUI.color = defaultColor;

                // Show both weapon names when dual wielding
                options.Gap();
                options.CheckboxLabeled("RenameYourWeapon.ShowBothWeaponNamesDualWield".Translate(), ref dualWieldInspectString, "RenameYourWeapon.ShowBothWeaponNamesDualWield_Tooltip".Translate());
            }
            #endregion
            // Finish
            options.End();
            Mod.GetSettings<RenameYourWeaponsSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref appendCachedLabel, "appendCachedLabel", false);
            Scribe_Values.Look(ref onlyAppendInThingHolder, "onlyAppendInThingHolder", true);
            Scribe_Values.Look(ref pawnWeaponRenameGizmos, "pawnWeaponRenameGizmos", true);

            Scribe_Values.Look(ref offHandRenameGizmos, "offHandRenameGizmos", true);
            Scribe_Values.Look(ref dualWieldInspectString, "dualWieldInspectString", true);
        }

    }

}
