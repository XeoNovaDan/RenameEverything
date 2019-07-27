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
            var renamableComp = t.TryGetComp<CompRenamable>();
            return renamableComp == null || !renamableComp.Named;
        }

        public override bool AlwaysMatches(ThingDef def)
        {
            return !def.HasComp(typeof(CompRenamable));
        }

    }

}
