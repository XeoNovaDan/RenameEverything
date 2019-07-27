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

        public static IEnumerable<Gizmo> RenameWeaponGizmos(CompRenamable renamableComp, string renameTranslationKey = null, string removeNameTranslationKey = null)
        {
            yield return new Command_RenameWeapon()
            {
                renamable = renamableComp,
                defaultLabel = (renameTranslationKey ?? renamableComp.Props.renameTranslationKey).Translate(),
                defaultDesc = "RenameYourWeapon.RenameWeapon_Description".Translate(),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1,
            };

            if (renamableComp.Named)
            {
                yield return new Command_Action()
                {
                    defaultLabel = (removeNameTranslationKey ?? "RenameYourWeapon.RemoveName").Translate(),
                    defaultDesc = "RenameYourWeapon.RemoveName_Description".Translate(),
                    icon = TexButton.DeleteX,
                    action = () => renamableComp.Named = false,
                };
            }
        }

    }

}
