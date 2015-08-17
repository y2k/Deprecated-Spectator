using System;
using Spectator.Core.ViewModels;
using Spectator.Core.ViewModels.Common;

namespace Spectator.iOS.Common
{
    public class Scope
    {
        readonly BindingFactory bindins = new BindingFactory();

        NavigationMessage argument;

        public Scope(NavigationMessage argument)
        {
            this.argument = argument;
        }

        public T New<T>() where T : ViewModel
        {
            var vm = Activator.CreateInstance<T>();
            if (argument != null)
            {
                var init = vm.GetType().GetMethod("Initialize");
                init.Invoke(vm, new object[] { argument });
            }

            bindins.BeginScope(vm);
            return vm;
        }

        public void EndScope()
        {
            bindins.EndScope();
        }
    }
}