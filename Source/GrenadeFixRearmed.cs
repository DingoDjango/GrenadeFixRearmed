using System.Collections.Generic;
using Verse;

namespace GrenadeFixRearmed
{
	[StaticConstructorOnStartup]
	public static class GrenadeFixRearmed
	{
		static GrenadeFixRearmed()
		{
			//Find all explosive projectile weapons
			List<ThingDef> explosives = DefDatabase<ThingDef>.AllDefsListForReading.FindAll(def =>
			def.Verbs.Exists(v => v.defaultProjectile?.projectile.explosionRadius > 0f));

#if DEBUG
			Log.Message($"GrenadeFixRearmed :: Found {explosives.Count} explosives.");
#endif

			for (int i = 0; i < explosives.Count; i++)
			{
				ThingDef thing = explosives[i];

				for (int j = 0; j < thing.Verbs.Count; j++)
				{
					VerbProperties verb = thing.Verbs[j];

					float explosionRadius = verb.defaultProjectile?.projectile.explosionRadius ?? 0f;

					if (verb.minRange <= explosionRadius)
					{
						verb.minRange = explosionRadius + 0.5f;

#if DEBUG
						Log.Message($"GrenadeFixRearmed :: Injected to {thing.label}.");
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
