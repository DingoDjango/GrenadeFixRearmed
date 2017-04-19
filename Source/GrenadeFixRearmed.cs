using System.Collections.Generic;
using Verse;

namespace GrenadeFixRearmed
{
	[StaticConstructorOnStartup]
	public static class GrenadeFixRearmed
	{
		static GrenadeFixRearmed()
		{
			LongEventHandler.QueueLongEvent(InjectDefs, "GrenadeFixRearmed", false, null);
		}

		private static List<ThingDef> explosives = new List<ThingDef>();

		private static void InjectDefs()
		{
			//Generate a list of explosive projectile weapons
			List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < thingDefs.Count; i++)
			{
				if (thingDefs[i].Verbs.Any(verb => verb.projectileDef != null))
				{
					if (thingDefs[i].Verbs.Any(verb => verb.projectileDef.projectile.explosionRadius > 0f))
					{
						if (!explosives.Contains(thingDefs[i]))
						{
							explosives.Add(thingDefs[i]);
						}
					}
				}
			}

#if DEBUG
			Log.Message("GrenadeFixRearmed :: Found " + explosives.Count + " explosive projectile weapons.");
#endif

			//Count the number of ThingDefs we change
			int injectedDefs = 0;

			for (int j = 0; j < explosives.Count; j++)
			{
				var verb = explosives[j].Verbs.Find(v => v.projectileDef.projectile.explosionRadius > 0f);
				float explosionRadius = verb.projectileDef.projectile.explosionRadius;

				//Only change minRange if it's unsafe
				if (verb.minRange < explosionRadius)
				{
					verb.minRange = explosionRadius + 0.1f;

					//Increase modified ThingDefs total count
					injectedDefs++;

#if DEBUG
						Log.Message("GrenadeFixRearmed :: Set minRange for " + thing.label + " (" + thing.defName + ")");
#endif
				}
			}

			//Report the total number of ThingDefs changed to the user
			Log.Message("GFR_FinalMessage_part1".Translate() + " " + injectedDefs + " " + "GFR_FinalMessage_part2".Translate());
		}
	}
}