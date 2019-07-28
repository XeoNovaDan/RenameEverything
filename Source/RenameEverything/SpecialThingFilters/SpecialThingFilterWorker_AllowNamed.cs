using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class SpecialThingFilterWorker_AllowNamed : SpecialThingFilterWorker
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
