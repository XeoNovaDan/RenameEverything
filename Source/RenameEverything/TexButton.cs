using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    [StaticConstructorOnStartup]
    static class TexButton
    {

        public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
        public static readonly Texture2D RenameTex = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);
        public static readonly Texture2D ColourPreviewTex = ContentFinder<Texture2D>.Get("UI/ColourPreview", true);

    }

}
