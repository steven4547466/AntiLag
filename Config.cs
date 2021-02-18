using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;

namespace AntiLag
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Enables ammo stacking to prevent tracking many entities.")]
		public bool EnableAmmoStacking { get; set; } = true;
		public int MaxAmmoStackSize { get; set; } = 300;

		[Description("Blood is client side, but the RPC call does take time to execute server side as well.")]
		public bool Disable173BloodSerpentsHand { get; set; } = false;
		public bool Disable173BloodScp035 { get; set; } = false;
	}
}
