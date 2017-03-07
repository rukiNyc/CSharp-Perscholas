using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
	public abstract class ArgsBase
	{
		protected ArgsBase(string[] args)
		{
		}

		protected abstract bool ParseArg(string prefix, out string name, out string value);
	}
}
