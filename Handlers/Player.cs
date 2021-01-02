using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using Exiled.API.Features;
using UnityEngine;

namespace AntiLag.Handlers
{
	public class Player
	{
		public void OnItemDropped(ItemDroppedEventArgs ev)
		{
			AmmoDropped(ev.Pickup);
		}

		public void AmmoDropped(Pickup pickup)
		{
			if (!pickup.itemId.IsAmmo())
				return;

			Collider[] colliders = Physics.OverlapSphere(pickup.Networkposition, 3, LayerMask.GetMask("Pickup"));

			foreach (Collider c in colliders)
			{
				Pickup p = c.gameObject.GetComponent<Pickup>();
				if (p != null && p.Rb != pickup.Rb && p.itemId == pickup.itemId && p.durability < Plugin.Instance.Config.MaxAmmoStackSize)
				{
					if (p.durability + pickup.durability <= Plugin.Instance.Config.MaxAmmoStackSize)
					{
						p.durability += pickup.durability;
						UnityEngine.Object.Destroy(pickup.gameObject);
					} 
					else
					{
						pickup.durability = Mathf.Abs(pickup.durability - p.durability);
						p.durability = Plugin.Instance.Config.MaxAmmoStackSize;
					}
					break;
				}
			}
		}
	}
}
