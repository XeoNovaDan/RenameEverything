using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class Command_RenameWeapon : Command
    {

        public CompRenamable renamable;
        private List<CompRenamable> renamables;

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            if (renamables == null)
                renamables = new List<CompRenamable>();
            renamables.Add(renamable);
            Find.WindowStack.Add(new Dialog_RenameThings(renamables));
        }

        public override bool InheritInteractionsFrom(Gizmo other)
        {
            if (renamables == null)
                renamables = new List<CompRenamable>();
            renamables.Add(((Command_RenameWeapon)other).renamable);
            return false;
        }

    }

}
