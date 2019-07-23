using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class Dialog_RenameThings : Dialog_Rename
    {

        public Dialog_RenameThings(List<CompRenamable> renamableComps)
        {
            this.renamableComps = renamableComps;
            curName = renamableComps.Count == 1 ? renamableComps[0].name : String.Empty;
        }

        protected override AcceptanceReport NameIsValid(string name)
        {
            return true;
        }


        protected override void SetName(string name)
        {
            foreach (var renamableComp in renamableComps)
                renamableComp.name = name;
        }

        private List<CompRenamable> renamableComps = new List<CompRenamable>();

    }

}
