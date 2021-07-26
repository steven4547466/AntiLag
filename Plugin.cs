using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Reflection.Emit;

namespace AntiLag
{
    public class Plugin : Plugin<Config>
    {
		public static Plugin Instance;

		public override PluginPriority Priority { get; } = PluginPriority.Last;
		public override string Name { get; } = "AntiLag";
		public override string Author { get; } = "Steven4547466";
		public override Version Version { get; } = new Version(1, 0, 0);
		public override Version RequiredExiledVersion { get; } = new Version(2, 1, 22);
		public override string Prefix { get; } = "AntiLag";

		public Handlers.Player player { get; private set; }
		//public Handlers.Server server { get; set; }
		//public Handlers.Map map { get; set; }

		int harmonyPatches = 0;
		private Harmony HarmonyInstance { get; set; }

		public override void OnEnabled()
		{
			base.OnEnabled();
			Instance = this;
			RegisterEvents();
			HarmonyInstance = new Harmony($"steven4547466.antilag-{++harmonyPatches}");
			HarmonyInstance.PatchAll();
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			UnregisterEvents();
			HarmonyInstance.UnpatchAll();
			Instance = null;
		}

		public void RegisterEvents()
		{
			player = new Handlers.Player();
			if (Plugin.Instance.Config.EnableAmmoStacking) Player.ItemDropped += player.OnItemDropped;
		}

		public void UnregisterEvents()
		{
			if (Plugin.Instance.Config.EnableAmmoStacking)  Player.ItemDropped -= player.OnItemDropped;
			player = null;
		}
	}
}
