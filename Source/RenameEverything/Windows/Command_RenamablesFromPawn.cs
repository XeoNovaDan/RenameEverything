using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public abstract class Command_RenamablesFromPawn : Command
    {

        public Pair<Pawn, List<CompRenamable>> pawnRenamables;
        protected List<Pair<Pawn, List<CompRenamable>>> allPawnRenamables;

        protected abstract IEnumerable<FloatMenuOption> DoFloatMenuOptions();

        protected string FloatMenuOptionLabel(Pawn pawn, Thing renamableThing)
        {
            if (allPawnRenamables.Count > 1)
                return $"{pawn.LabelShort}: {renamableThing.LabelCap}";
            return renamableThing.LabelCap;
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);

            if (allPawnRenamables == null)
                allPawnRenamables = new List<Pair<Pawn, List<CompRenamable>>>();
            allPawnRenamables.Add(pawnRenamables);

            // Do the float menu
            var floatMenuOptions = DoFloatMenuOptions().ToList();
            Find.WindowStack.Add(new FloatMenu(floatMenuOptions));
        }

        public override bool InheritInteractionsFrom(Gizmo other)
        {
            var otherGizmo = (Command_RenamablesFromPawn)other;

            if (allPawnRenamables == null)
                allPawnRenamables = new List<Pair<Pawn, List<CompRenamable>>>();
            allPawnRenamables.Add(otherGizmo.pawnRenamables);

            return false;
        }

    }

}
