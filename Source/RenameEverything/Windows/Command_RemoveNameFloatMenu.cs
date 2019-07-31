using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Multiplayer.API;

namespace RenameEverything
{

    public class Command_RemoveNameFloatMenu : Command_RenamablesFromPawn
    {

        protected override IEnumerable<FloatMenuOption> DoFloatMenuOptions()
        {
            foreach (var pawnRenamablesPair in allPawnRenamables)
                foreach (var renamable in pawnRenamablesPair.Second)
                    if (renamable.Named)
                        yield return new FloatMenuOption(FloatMenuOptionLabel(pawnRenamablesPair.First, renamable.parent), () => RemoveNameAction(renamable));
        }

        [SyncMethod]
        private void RemoveNameAction(CompRenamable renamableComp)
        {
            renamableComp.Named = false;
        }

    }

}
