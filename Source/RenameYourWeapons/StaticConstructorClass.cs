using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    [StaticConstructorOnStartup]
    public static class StaticConstructorClass
    {

        static StaticConstructorClass()
        {
            foreach (var tDef in DefDatabase<ThingDef>.AllDefs)
                if (tDef.IsWeapon)
                {
                    if (tDef.comps == null)
                        tDef.comps = new List<CompProperties>();
                    tDef.comps.Add(new CompProperties(typeof(CompRenamable)));
                }
        }

    }

}
