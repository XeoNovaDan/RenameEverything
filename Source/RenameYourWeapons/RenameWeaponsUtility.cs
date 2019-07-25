using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public static class RenameWeaponsUtility
    {

        public static IEnumerable<Gizmo> RenameWeaponGizmos(CompRenamable renamableComp)
        {
            yield return new Command_RenameWeapon()
            {
                renamable = renamableComp,
                defaultLabel = "RenameYourWeapon.RenameWeapon".Translate(),
                defaultDesc = "RenameYourWeapon.RenameWeapon_Description".Translate(),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1
            };

            if (renamableComp.Named)
            {
                yield return new Command_Action()
                {
                    defaultLabel = "RenameYourWeapon.RemoveName".Translate(),
                    defaultDesc = "RenameYourWeapon.RemoveName_Description".Translate(),
                    icon = TexButton.DeleteX,
                    action = () => renamableComp.Named = false
                };
            }
        }

    }

}
