using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

namespace GrenadeFixRearmed
{
	public class AssignMinRanges : MonoBehaviour
	{
		public virtual void Start()
		{
			this.enabled = true;
		}

		public virtual void Update()
		{
			if (PlayDataLoader.Loaded)
			{
				GrenadeFixer();
				this.enabled = false;
			}
			else new WaitForSeconds(1);
		}

		internal void GrenadeFixer()
		{
			List<ThingDef> projectileWeapons = DefDatabase<ThingDef>.AllDefs.Where(w => w.Verbs.Exists(v => v.projectileDef != null)).ToList();
			List<ThingDef> explosives = projectileWeapons.FindAll(v => v.Verbs.Exists(x => x.projectileDef.projectile.explosionRadius > 0f));

#if DEBUG
			Log.Message("GrenadeFixRearmed :: Found " + explosives.Count + " explosives.");
#endif

			List<ThingDef> injectedDefs = new List<ThingDef>();

			foreach (var thing in explosives)
			{
				var thingVerbs = thing.Verbs;
				foreach (var verb in thingVerbs)
				{
					float explosionRadius = verb.projectileDef.projectile.explosionRadius;
					if (verb.minRange < explosionRadius)
					{
						verb.minRange = explosionRadius + 0.1f;
#if DEBUG
						Log.Message("GrenadeFixRearmed :: Set minRange for " + thing.label + " (" + thing.defName + ")");
#endif
						injectedDefs.Add(thing);
					}
				}
			}

			Log.Message("GrenadeFixRearmed :: Defined minRange for " + injectedDefs.Count + " explosives.");
			injectedDefs.Clear();
		}
	}

	public class ModInitializer : ITab
	{
		public ModInitializer()
		{
			var gameObject = new GameObject("Grenade Fix: Rearmed");
			gameObject.AddComponent<AssignMinRanges>();
		}

		protected override void FillTab()
		{
		}
	}
}
