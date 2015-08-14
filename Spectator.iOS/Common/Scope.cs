using System;
using Spectator.Core.ViewModels;

namespace Spectator.iOS.Common
{
    public class Scope
    {
        BindingFactory bindins = new BindingFactory();

        public T New<T>() where T : ViewModel
        {
            var vm = Activator.CreateInstance<T>();
            bindins.BeginScope(vm);
            return vm;
        }

        public void EndScope()
        {
            bindins.EndScope();
        }
    }
}