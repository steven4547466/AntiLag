using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using NorthwoodLib.Pools;
using static HarmonyLib.AccessTools;

namespace AntiLag.Patches
{
	[HarmonyPatch(typeof(AmmoBox), nameof(AmmoBox.CallCmdDrop))]
	class AmmoBoxPatches
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

			if (!Plugin.Instance.Config.EnableAmmoStacking)
			{
				foreach (CodeInstruction code in newInstructions)
					yield return code;

				ListPool<CodeInstruction>.Shared.Return(newInstructions);
				yield break;
			}

			var pickup = generator.DeclareLocal(typeof(Pickup));

			newInstructions.RemoveRange(newInstructions.Count - 2, 2);

			newInstructions.AddRange(new CodeInstruction[] 
			{
				new CodeInstruction(OpCodes.Stloc_0, pickup.LocalIndex),
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(Plugin), nameof(Plugin.Instance))),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.player))),
				new CodeInstruction(OpCodes.Ldloc_0, pickup.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, Method(typeof(Handlers.Player), nameof(Handlers.Player.AmmoDropped))),
				new CodeInstruction(OpCodes.Ret)
			});

			foreach (CodeInstruction code in newInstructions)
				yield return code;

			ListPool<CodeInstruction>.Shared.Return(newInstructions);
		}
	}
}
