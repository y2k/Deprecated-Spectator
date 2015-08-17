using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using Spectator.Core.ViewModels.Common;

namespace Spectator.Core.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        PropertyHolder properties = new PropertyHolder();

        protected bool Set<T>(T newValue, bool broadcast = false, [CallerMemberName] string propertyName = null)
        {
            return properties.Set<T>(this, newValue, broadcast, propertyName);
        }

        protected T Get<T>([CallerMemberName] string propertyName = null)
        {
            return properties.Get<T>(propertyName);
        }

//        public virtual void Initialize(NavigationMessage argument)
//        {
//        }

        class PropertyHolder
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();

            internal bool Set<T>(ViewModel parent, T newValue, bool broadcast = false, [CallerMemberName] string propertyName = null)
            {
                var oldValue = Get<T>(propertyName);
                if (!EqualityComparer<T>.Default.Equals(newValue, oldValue))
                {
                    properties[propertyName] = newValue;
                    parent.RaisePropertyChanged(propertyName, oldValue, newValue, broadcast);
                    return true;
                }
                return false;
            }

            internal T Get<T>([CallerMemberName] string propertyName = null)
            {
                return properties
                    .Where(s => s.Key == propertyName)
                    .Select(s => (T)s.Value)
                    .FirstOrDefault();
            }
        }
    }
}