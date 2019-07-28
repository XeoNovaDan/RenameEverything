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
            //            tDef.comps.Add(new CompProperties_Renamable() { inspectStringTranlationKey = "ShootReportWeapon", renameTranslationKey = "RenameEverything.RenameWeapon" });
            //        else
            //            tDef.comps.Add(new CompProperties_Renamable() { inspectStringTranlationKey = "Turret", renameTranslationKey = "RenameEverything.RenameTurret" });
            //            }
            //}
            foreach (var tDef in DefDatabase<ThingDef>.AllDefs.Where(t => t.thingClass != null && t.thingClass.IsClassOrSubclassOf(typeof(ThingWithComps))))
            {
                if (tDef.comps == null)
                    tDef.comps = new List<CompProperties>();

                if (tDef.IsWeapon)
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenameWeapon", inspectStringTranslationKey = "ShootReportWeapon" });
                else if (tDef.IsBuildingArtificial)
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenameBuilding", inspectStringTranslationKey = "RenameEverything.Building" });
                else if (tDef.thingClass.IsClassOrSubclassOf(typeof(Plant)))
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenamePlant", inspectStringTranslationKey = "RenameEverything.Plant" });
                else if (!tDef.thingClass.IsClassOrSubclassOf(typeof(Pawn)))
                    tDef.comps.Add(new CompProperties_Renamable());
            }
        }

    }

}
