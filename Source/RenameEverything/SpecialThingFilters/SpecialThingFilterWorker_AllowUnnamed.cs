using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class SpecialThingFilterWorker_AllowUnnamed : SpecialThingFilterWorker
    {

        public override bool Matches(Thing t)
        {
            return AlwaysMatches(t.def) || !t.TryGetComp<CompRenamable>().Named;
        }

        public override bool AlwaysMatches(ThingDef def)
        {
            return !def.HasComp(typeof(CompRenamable));
        }

    }

}
