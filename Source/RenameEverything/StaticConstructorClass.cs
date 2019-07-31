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
            foreach (var tDef in DefDatabase<ThingDef>.AllDefs.Where(t => typeof(ThingWithComps).IsAssignableFrom(t.thingClass)))
            {
                if (tDef.comps == null)
                    tDef.comps = new List<CompProperties>();

                // Weapon
                if (tDef.IsWeapon)
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenameWeapon", inspectStringTranslationKey = "ShootReportWeapon" });
                
                // Apparel
                else if (tDef.IsApparel)
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenameApparel", inspectStringTranslationKey = "Apparel" });
                
                // Building
                else if (tDef.IsBuildingArtificial)
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenameBuilding", inspectStringTranslationKey = "RenameEverything.Building" });
                
                // Plant
                else if (typeof(Plant).IsAssignableFrom(tDef.thingClass))
                    tDef.comps.Add(new CompProperties_Renamable() { renameTranslationKey = "RenameEverything.RenamePlant", inspectStringTranslationKey = "RenameEverything.Plant" });

                // Not a pawn, corpse or minified building
                else if (!typeof(Pawn).IsAssignableFrom(tDef.thingClass) && !typeof(Corpse).IsAssignableFrom(tDef.thingClass) && !typeof(MinifiedThing).IsAssignableFrom(tDef.thingClass))
                    tDef.comps.Add(new CompProperties_Renamable());
            }
        }

    }

}
