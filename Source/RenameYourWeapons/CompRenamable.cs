using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class CompRenamable : ThingComp
    {

        private string cachedLabel;
        public string name;

        public bool Named
        {
            get => !name.NullOrEmpty();
            set
            {
                if (!value)
                    name = String.Empty;
            }
        }

        public override string TransformLabel(string label)
        {
            cachedLabel = label;
            return Named ? name : label;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_RenameWeapon()
            {
                renamable = this,
                defaultLabel = "Rename".Translate(),
                defaultDesc = "RenameYourWeapon.RenameWeapon_Description".Translate(),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1
            };

            if (Named)
            {
                yield return new Command_Action()
                {
                    defaultLabel = "RenameYourWeapon.RemoveName".Translate(),
                    defaultDesc = "RenameYourWeapon.RemoveName_Description".Translate(),
                    icon = TexButton.DeleteX,
                    action = () => Named = false
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref cachedLabel, "cachedLabel");
            Scribe_Values.Look(ref name, "name");
            base.PostExposeData();
        }

        public override string CompInspectStringExtra()
        {
            return Named ? $"{"ShootReportWeapon".Translate()}: {cachedLabel.CapitalizeFirst()}" : null;
        }

    }

}
