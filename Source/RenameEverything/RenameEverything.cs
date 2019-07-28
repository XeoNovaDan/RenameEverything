using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;

namespace RenameEverything
{

    public class RenameEverything : Mod
    {

        public RenameEverythingSettings settings;

        public RenameEverything(ModContentPack content) : base(content)
        {
            GetSettings<RenameEverythingSettings>();
            HarmonyInstance = HarmonyInstance.Create("XeoNovaDan.RenameEverything");
        }

        public override string SettingsCategory() => "RenameEverything.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<RenameEverythingSettings>().DoWindowContents(inRect);
        }

        public static HarmonyInstance HarmonyInstance;

    }

}
