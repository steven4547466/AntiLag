using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using HarmonyLib;
using NorthwoodLib.Pools;
using UnityEngine;
using static HarmonyLib.AccessTools;

namespace AntiLag.Patches
{
	[HarmonyPatch(typeof(Scp173PlayerScript), nameof(Scp173PlayerScript.CallCmdHurtPlayer))]
	class Scp173CallCmdHurtPlayerPatch
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

			if (!Plugin.Instance.Config.Disable173BloodScp035 && !Plugin.Instance.Config.Disable173BloodSerpentsHand)
			{
				foreach (CodeInstruction code in newInstructions)
					yield return code;

				ListPool<CodeInstruction>.Shared.Return(newInstructions);
				yield break;
			}

			var continueLabel = generator.DefineLabel();
			var skipLabel = generator.DefineLabel();
			var serpentsHandLabel = generator.DefineLabel();

			var playerLocal = generator.DeclareLocal(typeof(Player));

			var offset = newInstructions.FindLastIndex(code => code.opcode == OpCodes.Ldc_R4) - 10;

			newInstructions[offset + 8].labels.Add(skipLabel);

			CodeInstruction[] codeInstructions = new CodeInstruction[]
			{
				new CodeInstruction(OpCodes.Ldarg_1),
				new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(GameObject) })),
				new CodeInstruction(OpCodes.Stloc_S, playerLocal.LocalIndex),
				new CodeInstruction(OpCodes.Ldloc_S, playerLocal.LocalIndex),
				new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Plugin.Instance.Config.Disable173BloodScp035))),
				new CodeInstruction(OpCodes.Brfalse_S, serpentsHandLabel),
				new CodeInstruction(OpCodes.Ldloc_S, playerLocal.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, Method(typeof(Extensions), nameof(Extensions.IsScp035))),
				new CodeInstruction(OpCodes.Brtrue_S, skipLabel),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Plugin.Instance.Config.Disable173BloodSerpentsHand))).WithLabels(serpentsHandLabel),
				new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
				new CodeInstruction(OpCodes.Ldloc_S, playerLocal.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, Method(typeof(Extensions), nameof(Extensions.IsSerpentsHand))),
				new CodeInstruction(OpCodes.Brtrue_S, skipLabel)
			};

			newInstructions[offset + codeInstructions.Length].labels.Add(continueLabel);

			newInstructions.InsertRange(offset, codeInstructions);

			foreach (CodeInstruction code in newInstructions)
				yield return code;

			ListPool<CodeInstruction>.Shared.Return(newInstructions);
		}
	}
}
