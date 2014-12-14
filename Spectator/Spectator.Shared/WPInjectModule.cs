using Autofac;
using SQLite.Net.Interop;
using System;
using XamarinCommons.Image;

namespace Spectator
{
    class WPInjectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StubImageDecoder>().As<ImageDecoder>();
            builder.RegisterType<SQLite.Net.Platform.WinRT.SQLitePlatformWinRT>().As<ISQLitePlatform>();
        }

        public class StubImageDecoder : ImageDecoder
        {
            public override int GetImageSize(object commonImage)
            {
                throw new NotImplementedException();
            }
        }
    }
}