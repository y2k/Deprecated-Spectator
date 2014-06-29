using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Spectator.Core.Model;

namespace Spectator.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.MainViewModel>();

            Mvx.RegisterType<ISubscriptionModel, SubscriptionModel>();
            Mvx.RegisterType<IProfileModel, ProfileModel>();
        }
    }
}