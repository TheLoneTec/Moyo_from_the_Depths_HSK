
using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace HPF_Moyo
{
    [StaticConstructorOnStartup]
    public static class HPF_Moyo
    {
        static HPF_Moyo()
        {
            Harmony harmonyInstance = new Harmony("ChangeLook_Harmony");
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnGraphicSet), "ResolveAllGraphics", null, null), null, new HarmonyMethod(patchType, "ResolveAllGraphicsPostfix", null), null, null);
        }

        [HarmonyPostfix]
        public static void ResolveAllGraphicsPostfix(PawnGraphicSet __instance)
        {
            if (__instance.pawn.def.defName == "Alien_Moyo")
            {
                if (__instance.pawn.gender == Gender.Female && (__instance.pawn.ageTracker.CurLifeStage == LifeStageDefOf.HumanlikeAdult || __instance.pawn.ageTracker.CurLifeStage.defName == "MoyoTeenager"))
                {
                    __instance.pawn.story.bodyType = BodyTypeDefOf.Female;
                }

                __instance.swaddledBabyGraphic = GraphicDatabase.Get<Graphic_Multi>("Moyo/Dummy/Dummy_south", ShaderDatabase.Cutout, Vector2.one, Color.white);
            }
        }
        private static readonly Type patchType = typeof(HPF_Moyo);
    }
}
