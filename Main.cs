using System.Collections.Generic;
using System.Linq;
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
				foreach (VerbProperties verb in thing.Verbs)
				{
					if (verb.CausesExplosion)
					{
						verb.minRange = Mathf.Max(verb.minRange, verb.defaultProjectile.projectile.explosionRadius + 0.5f);

#if DEBUG
						Log.Message($"GrenadeFixRearmed :: New '{verb.label}' minRange = {verb.minRange}");
#endif
					}
				}
			}
		}
	}
}
