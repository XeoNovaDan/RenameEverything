using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class Dialog_RenameThings : Dialog_Rename
    {

        public Dialog_RenameThings(List<CompRenamable> renamableComps)
        {
            this.renamableComps = renamableComps;

            if (renamableComps.Count == 1)
            {
                var renamable = renamableComps[0];
                if (renamable.Named)
                    curName = renamable.name;
                else
                    curName = renamable.parent.LabelCapNoCount;
            }
            else
                curName = String.Empty;
        }

        public Dialog_RenameThings(CompRenamable renamableComp)
        {
            renamableComps = new List<CompRenamable>() { renamableComp };
            curName = renamableComp.Named ? renamableComp.name : renamableComp.parent.LabelCapNoCount;
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
