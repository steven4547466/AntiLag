﻿using Exiled.API.Features;
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

		public static bool IsScp035(this Player player)
		{
			Assembly assembly = Loader.Plugins.FirstOrDefault(p => p.Name == "scp035")?.Assembly;
			if (assembly == null) return false;
			return ((Player)assembly.GetType("scp035.API.Scp035Data").GetMethod("GetScp035").Invoke(null, null)) == player;
		}

		public static bool IsSerpentsHand(this Player player)
		{
			Assembly assembly = Loader.Plugins.FirstOrDefault(p => p.Name == "SerpentsHand")?.Assembly;
			if (assembly == null) return false;
			return ((List<Player>)assembly.GetType("SerpentsHand.API.SerpentsHand").GetMethod("GetSHPlayers").Invoke(null, null)).Contains(player);
		}
	}
}
