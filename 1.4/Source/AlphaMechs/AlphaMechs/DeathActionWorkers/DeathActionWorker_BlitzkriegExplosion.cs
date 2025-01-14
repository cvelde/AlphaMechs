﻿using RimWorld;
using Verse;
namespace AlphaMechs
{
	public class DeathActionWorker_BlitzkriegExplosion : DeathActionWorker
	{
		public override RulePackDef DeathRules => RulePackDefOf.Transition_DiedExplosive;

		public override bool DangerousInMelee => true;

		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(radius: 3.9f, center: corpse.Position, map: corpse.Map, damType: DamageDefOf.Bomb, damAmount:30,chanceToStartFire:0.5f,instigator: corpse.InnerPawn);
		}
	}
}
