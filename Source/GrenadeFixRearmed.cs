using System.Collections.Generic;
using Verse;

namespace GrenadeFixRearmed
{
	public class GrenadeFixRearmed : GameComponent
	{
		public GrenadeFixRearmed()
		{
		}

		public GrenadeFixRearmed(Game game)
		{
		}

		public override void StartedNewGame()
		{
			InjectDefs();
		}

		public override void LoadedGame()
		{
			InjectDefs();
		}

		private void InjectDefs()
		{
			//Generate a list of explosive projectile weapons
			List<ThingDef> explosives = DefDatabase<ThingDef>.AllDefsListForReading.FindAll(def => def.Verbs.Exists(v => v.projectileDef != null && v.projectileDef.projectile.explosionRadius > 0f));

#if DEBUG
			Log.Message("GrenadeFixRearmed :: Found " + explosives.Count + " explosive projectile weapons.");
#endif

			//Count the number of verbs we change
			int injectedVerbs = 0;

			for (int i = 0; i < explosives.Count; i++)
			{
				var verbList = explosives[i].Verbs;

				for (int j = 0; j < verbList.Count; j++)
				{
					var ev = verbList[j];

					if (ev.projectileDef != null)
					{
						if (ev.projectileDef.projectile.explosionRadius > 0f)
						{
							float explosionRad = ev.projectileDef.projectile.explosionRadius;

							//Added check for safe minrange
							if (ev.minRange < explosionRad)
							{
								ev.minRange = explosionRad + 0.5f;

								//Increase modified verbs count
								injectedVerbs++;
							}
						}
					}
				}
			}

			//Report the total number of ThingDefs changed to the user
			Log.Message("GFR_FinalMessage_part1".Translate() + " " + injectedVerbs + " " + "GFR_FinalMessage_part2".Translate());
		}
	}
}