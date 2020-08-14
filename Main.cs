using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace Grenade_Fix_Rearmed
{
	[StaticConstructorOnStartup]
	public static class Main
	{
		static Main()
		{
			//Find all explosive projectile weapons
			IEnumerable<ThingDef> explosives = DefDatabase<ThingDef>.AllDefs.Where(def => def.Verbs.Exists(verb => verb.CausesExplosion));

#if DEBUG
			Log.Message($"GrenadeFixRearmed :: Found {explosives.Count()} explosives.");
#endif

			foreach (ThingDef thing in explosives)
			{
#if DEBUG
				Log.Message($"GrenadeFixRearmed :: Working on '{thing.label}' (defName = '{thing.defName}')");
#endif

				foreach (VerbProperties verb in thing.Verbs)
				{
					if (verb.CausesExplosion)
					{
#if DEBUG
						Log.Message($"GrenadeFixRearmed :: Injecting for '{verb.label}' with minRange {verb.minRange}");
#endif

						verb.minRange = Mathf.Max(verb.minRange, verb.defaultProjectile.projectile.explosionRadius + 0.5f);

#if DEBUG
						Log.Message($"GrenadeFixRearmed :: New '{verb.label}' minRange = {verb.minRange}");
#endif
					}
				}
			}

#if DEBUG
			Log.Message("GrenadeFixRearmed :: Finished injecting minimum safe ranges.");
#endif
		}
	}
}
