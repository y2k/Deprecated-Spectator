using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace Spectator.iOS.Common
{
    public static class BindingExtensions
    {
        public static InnerBinding<T, TS> SetBinding<T, TS>(this T target, Action whenSourceChanged, Expression<Func<TS>> sourceExpression)
        {
            return SetBinding<T, TS>(target, (_, _2) => whenSourceChanged(), sourceExpression);
        }

        public static InnerBinding<T, TS> SetBinding<T, TS>(this T target, Action<T, TS> whenSourceChanged, Expression<Func<TS>> sourceExpression)
        {
            var source = BindingFactory.Scope.DataContext;
            var sourceProperty = (PropertyInfo)((MemberExpression)sourceExpression.Body).Member;

            Action action = () => whenSourceChanged(target, (TS)sourceProperty.GetValue(source));
            BindingFactory.Scope.AddAction(action);
            var binding = BindingFactory.Scope
                .Add(source, sourceExpression, BindingMode.OneWay)
                .WhenSourceChanges(action);

            return new InnerBinding<T, TS>
            {
                binding = binding,
                target = target,
                source = source,
                sourceProperty = sourceProperty,
            };
        }

        public class InnerBinding<T, TS>
        {
            internal object target;
            internal object source;
            internal PropertyInfo sourceProperty;
            internal Binding<TS, TS> binding;
            Func<T,TS> converter;

            public void SetTwoWay()
            {
                SetTwoWay<TS>(null, converter);
            }

            public void SetTwoWay<TT>(Expression<Func<T, TT>> targetExpression, Func<T,TS> converter = null)
            {
                this.converter = converter;

                if (target is INotifyPropertyChanged)
                {
                    var targetProperty = (PropertyInfo)((MemberExpression)targetExpression.Body).Member;
                    var observable = (INotifyPropertyChanged)target;

                    observable.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName == targetProperty.Name)
                            SetSource(targetProperty.GetValue(target));
                    };
                }
                else if (target is UISwitch)
                {
                    var view = (UISwitch)target;
                    view.ValueChanged += (sender, e) => SetSource(view.On);
                }
                else if (target is UITextField)
                {
                    var view = (UITextField)target;
                    view.EditingChanged += (sender, e) => SetSource(view.Text);
                }
                else if (target is UIStepper)
                {
                    var view = (UIStepper)target;
                    view.ValueChanged += (sender, e) => SetSource(view.Value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            void SetSource(object value)
            {
                if (converter == null)
                    sourceProperty.SetValue(source, value);
                else
                    sourceProperty.SetValue(source, converter((T)target));
            }
        }
    }
}