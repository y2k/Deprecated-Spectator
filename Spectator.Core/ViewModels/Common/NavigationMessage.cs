﻿namespace Spectator.Core.ViewModels.Common
{
    public class NavigationMessage
    {
        protected NavigationMessage PutInt(int arg)
        {
            return this;
        }

        protected T Get<T>()
        {
            throw new System.NotImplementedException();
        }

        protected void Set(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}