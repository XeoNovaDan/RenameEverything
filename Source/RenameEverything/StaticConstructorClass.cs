using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    [StaticConstructorOnStartup]
    public static class StaticConstructorClass
    {

        static StaticConstructorClass()
        {
            //// Add CompRenamable to all weapon and turret ThingDefs
            //foreach (var tDef in DefDatabase<ThingDef>.AllDefs)
            //{
            //    if (tDef.IsWeapon || tDef.thingClass.IsSubclassOf(typeof(Building_Turret)))
            //    {
            //        if (tDef.comps == null)
            //            tDef.comps = new List<CompProperties>();

            //        if (tDef.IsWeapon)
            //            tDef.comps.Add(new CompProperties_Renamable() { inspectStringTranlationKey = "ShootReportWeapon", renameTranslationKey = "RenameEverythingRenameWeapon" });
            //        else
            //            tDef.comps.Add(new CompProperties_Renamable() { inspectStringTranlationKey = "Turret", renameTranslationKey = "RenameEverythingRenameTurret" });
            //            }
            //}
            foreach (var tDef in DefDatabase<ThingDef>.AllDefs.Where(t => t.thingClass != null && (t.thingClass == typeof(ThingWithComps) || t.thingClass.IsSubclassOf(typeof(ThingWithComps)))))
            {
                if (tDef.comps == null)
                    tDef.comps = new List<CompProperties>();
                tDef.comps.Add(new CompProperties(typeof(CompRenamable)));
            }
        }

    }

}
