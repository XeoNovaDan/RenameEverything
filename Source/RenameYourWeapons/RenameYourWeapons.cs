using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class RenameYourWeapons : Mod
    {

        public RenameYourWeaponsSettings settings;

        public RenameYourWeapons(ModContentPack content) : base(content)
        {
            GetSettings<RenameYourWeaponsSettings>();
        }

        public override string SettingsCategory() => "RenameYourWeapons.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<RenameYourWeaponsSettings>().DoWindowContents(inRect);
        }

    }

}
