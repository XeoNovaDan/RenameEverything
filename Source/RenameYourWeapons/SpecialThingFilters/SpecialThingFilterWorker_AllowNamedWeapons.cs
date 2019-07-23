using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class SpecialThingFilterWorker_AllowNamedWeapons : SpecialThingFilterWorker
    {

        public override bool Matches(Thing t)
        {
            return CanEverMatch(t.def) && t.TryGetComp<CompRenamable>().Named;
        }

        public override bool CanEverMatch(ThingDef def)
        {
            return def.HasComp(typeof(CompRenamable));
        }

    }

}
