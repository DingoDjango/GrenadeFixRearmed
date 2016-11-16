using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace GrenadeFixRearmed
{
	[StaticConstructorOnStartup]
	public static class GrenadeFixRearmed
	{
		static GrenadeFixRearmed()
		{
			while (!PlayDataLoader.Loaded)
			{
				new WaitForSeconds(1f);
			}

			//Generate a list of projectile weapons
			var projectileWeapons = DefDatabase<ThingDef>.AllDefsListForReading.FindAll(v => v.Verbs.Exists(x => x.projectileDef != null));

			//Generate a list of explosive projectile weapons based on the previous list
			var explosives = projectileWeapons.FindAll(v => v.Verbs.Exists(x => x.projectileDef.projectile.explosionRadius > 0f));

#if DEBUG
				Log.Message("GrenadeFixRearmed :: Found " + explosives.Count + " explosive projectile weapons.");
#endif

			//Make a temporary list so we can report how many defs are affected
			List<ThingDef> injectedDefs = new List<ThingDef>();

			foreach (var thing in explosives)
			{
				var thingVerbs = thing.Verbs;
				foreach (var verb in thingVerbs)
				{
					float explosionRadius = verb.projectileDef.projectile.explosionRadius;
					//Only change minRange if it's unsafe
					if (verb.minRange < explosionRadius)
					{
						verb.minRange = explosionRadius + 0.1f;
#if DEBUG
							Log.Message("GrenadeFixRearmed :: Set minRange for " + thing.label + " (" + thing.defName + ")");
#endif
						//Add this ThingDef to our temporary list so we can track that it's been affected
						injectedDefs.Add(thing);
					}
				}
			}

			//Report the total number of ThingDefs changed to the user
			Log.Message("GrenadeFixRearmed :: Defined minRange for " + injectedDefs.Count + " explosive weapons.");
			//Clear our temporary list, just in-case
			injectedDefs.Clear();
		}
	}
}
