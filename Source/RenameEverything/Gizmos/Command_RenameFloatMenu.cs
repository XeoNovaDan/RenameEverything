using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class Command_RenameFloatMenu : Command_Renamables
    {

        protected override IEnumerable<FloatMenuOption> DoFloatMenuOptions()
        {
            foreach (var pawnRenamablesPair in allPawnRenamables)
                foreach (var renamable in pawnRenamablesPair.Second)
                    yield return new FloatMenuOption(FloatMenuOptionLabel(pawnRenamablesPair.First, renamable.parent), () => Find.WindowStack.Add(new Dialog_RenameThings(renamable)));
        }

    }

}
