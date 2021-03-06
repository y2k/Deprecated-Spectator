﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectator.Core.Model
{
    public interface IProfileModel
    {
        Uri LoginStartUrl { get; }

		bool IsValid(string url);

		Task LoginViaCodeAsync (string url);

		bool IsAccessDenied (string url);
    }
}