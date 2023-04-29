﻿
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace AlphaMechs
{
    public class Hediff_BeamncasterBandNode : Hediff
    {
        private const int BandNodeCheckInterval = 60;

        private int cachedTunedBandNodesCount;

        private HediffStage curStage;

        public int AdditionalBandwidth => cachedTunedBandNodesCount;

        public override bool ShouldRemove => cachedTunedBandNodesCount == 0;

        public override HediffStage CurStage
        {
            get
            {
                if (curStage == null && cachedTunedBandNodesCount > 0)
                {
                    StatModifier statModifier = new StatModifier();
                    statModifier.stat = StatDefOf.MechBandwidth;
                    statModifier.value = cachedTunedBandNodesCount*AlphaMechs_Settings.tier2bandAmount;
                    curStage = new HediffStage();
                    curStage.statOffsets = new List<StatModifier> { statModifier };
                }
                return curStage;
            }
        }

        public override void PostTick()
        {
            base.PostTick();
            if (pawn.IsHashIntervalTick(60))
            {
                RecacheBandNodes();
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            RecacheBandNodes();
        }

        public void RecacheBandNodes()
        {
            int num = cachedTunedBandNodesCount;
            cachedTunedBandNodesCount = 0;
            List<Map> maps = Find.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                foreach (Building item in maps[i].listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_BeamcasterBandNode))
                {
                    if (item.TryGetComp<CompBandNode>().tunedTo == pawn && item.TryGetComp<CompPowerTrader>().PowerOn)
                    {
                        cachedTunedBandNodesCount++;
                    }
                }
            }
            if (num != cachedTunedBandNodesCount)
            {
                curStage = null;
                pawn.mechanitor?.Notify_BandwidthChanged();
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref cachedTunedBandNodesCount, "cachedTunedBandNodesCount", 0);
        }
    }
}
