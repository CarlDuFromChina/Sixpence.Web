﻿using Sixpence.Common.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Auth
{
    [ServiceRegister]
    public interface IThirdPartyBindStrategy
    {
        string GetName();
        void Bind(string code, string userid);
    }
}
