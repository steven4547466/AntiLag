using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Loader;
using System.Reflection;

namespace AntiLag
{
	public static class Extensions
	{
		public static bool IsAmmo(this ItemType itemType)
		{
			return itemType == ItemType.Ammo556 || itemType == ItemType.Ammo762 || itemType == ItemType.Ammo9mm;
		}
	}
}
