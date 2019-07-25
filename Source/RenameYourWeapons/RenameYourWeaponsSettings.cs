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

        public static bool appendCachedLabel = true;
        public static bool onlyAppendInThingHolder = true;

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
            options.CheckboxLabeled("RenameYourWeapon.AppendCachedLabel".Translate(), ref appendCachedLabel, "RenameYourWeapon.AppendCachedLabel_ToolTip".Translate());

            // Append original label to weapon's name (grey out if appendCachedLabel is false)
            if (!appendCachedLabel)
                GUI.color = Color.grey;
            options.Gap();
            options.CheckboxLabeled("RenameYourWeapon.AppendCachedLabelInThingHolder".Translate(), ref onlyAppendInThingHolder, "RenameYourWeapon.AppendCachedLabelInThingHolder_Tooltip".Translate());
            GUI.color = defaultColor;

            // Finish
            options.End();
            Mod.GetSettings<RenameYourWeaponsSettings>().Write();

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref appendCachedLabel, "appendCachedLabel", true);
            Scribe_Values.Look(ref onlyAppendInThingHolder, "onlyAppendInThingHolder", true);
        }

    }

}
