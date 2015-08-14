using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace Spectator.iOS.Common
{
    public class BindingFactory : ICleanup
    {
        readonly List<Binding> bindings = new List<Binding>();
        readonly List<Action> actions = new List<Action>();

        public static BindingFactory Scope { get; private set; }

        public object DataContext { get; set; }

        public void BeginScope(object dataContext)
        {
            DataContext = dataContext;
            Scope = this;
        }

        public void EndScope()
        {
            Scope = null;
        }

        internal Binding<bool, bool> Add(object source, Expression<Func<bool>> sourceExpression, UISwitch target)
        {
            var prop = (PropertyInfo)((MemberExpression)sourceExpression.Body).Member;
            var binding = Add(source, sourceExpression)
                .WhenSourceChanges(() => target.On = (bool)prop.GetValue(source));
            target.ValueChanged += (sender, e) => prop.SetValue(source, target.On);
            return binding;
        }

        internal Binding<string, string> Add(object source, Expression<Func<string>> sourceExpression, UITextField target)
        {
            var prop = (PropertyInfo)((MemberExpression)sourceExpression.Body).Member;
            var binding = Add(source, sourceExpression)
                .WhenSourceChanges(() => target.Text = (string)prop.GetValue(source));
            target.EditingChanged += (sender, e) => prop.SetValue(source, target.Text);
            return binding;
        }

        internal Binding<TS, TS> Add<TS>(object source, Expression<Func<TS>> sourceExpression, BindingMode mode = BindingMode.Default)
        {
            return SaveAndReturn(source.SetBinding(sourceExpression, mode));
        }


        internal Binding<TS, TT> Add<TS, TT>(object source, Expression<Func<TS>> sourceExpression, object target, Expression<Func<TT>> targetExpression, BindingMode mode)
        {
            return SaveAndReturn(source.SetBinding(sourceExpression, target, targetExpression, mode));
        }

        Binding<TS, TT> SaveAndReturn<TS, TT>(Binding<TS, TT> binding)
        {
            bindings.Add(binding);
            return binding;
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        public void Cleanup()
        {
            bindings.Clear();
            actions.Clear();
        }
    }
}