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
	}
}
