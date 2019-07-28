using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public static class RenameUtility
    {

        public static IEnumerable<Gizmo> RenameGizmos(CompRenamable renamableComp, string renameTranslationKey = null, string removeNameTranslationKey = null)
        {
            yield return new Command_Rename()
            {
                renamable = renamableComp,
                defaultLabel = (renameTranslationKey ?? "RenameEverything.RenameObject").Translate(),
                defaultDesc = "RenameEverything.RenameGizmo_Description".Translate(renamableComp.Props.inspectStringTranslationKey.Translate()),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1,
            };

            if (renamableComp.Named)
            {
                yield return new Command_Action()
                {
                    defaultLabel = (removeNameTranslationKey ?? "RenameEverything.RemoveName").Translate(),
                    defaultDesc = "RenameEverything.RemoveName_Description".Translate(),
                    icon = TexButton.DeleteX,
                    action = () => renamableComp.Named = false,
                };
            }
        }

        public static bool IsClassOrSubclassOf(this Type type, Type other) => type == other || type.IsSubclassOf(other);

    }

}
