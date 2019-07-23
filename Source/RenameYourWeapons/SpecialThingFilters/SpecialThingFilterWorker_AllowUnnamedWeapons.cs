using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class SpecialThingFilterWorker_AllowUnnamedWeapons : SpecialThingFilterWorker
    {

        public override bool Matches(Thing t)
        {
            return !t.TryGetComp<CompRenamable>().Named;
        }

        public override bool AlwaysMatches(ThingDef def)
        {
            return !def.HasComp(typeof(CompRenamable));
        }

    }

}
