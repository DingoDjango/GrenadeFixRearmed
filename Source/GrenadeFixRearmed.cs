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

		private static void InjectDefs()
		{
			//Generate a list of explosive projectile weapons
			var explosives = DefDatabase<ThingDef>.AllDefsListForReading.FindAll(v => v.Verbs.Any(x => x.projectileDef != null && x.projectileDef.projectile.explosionRadius > 0f));

#if DEBUG
			Log.Message("GrenadeFixRearmed :: Found " + explosives.Count + " explosive projectile weapons.");
#endif

			//Count the number of ThingDefs we change
			int injectedDefs = 0;

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

						//Increase modified ThingDefs total count
						injectedDefs++;

#if DEBUG
						Log.Message("GrenadeFixRearmed :: Set minRange for " + thing.label + " (" + thing.defName + ")");
#endif
					}
				}
			}

			//Report the total number of ThingDefs changed to the user
			Log.Message("GrenadeFixRearmed :: Defined minRange for " + injectedDefs + " explosive weapons.");
		}
	}
}
