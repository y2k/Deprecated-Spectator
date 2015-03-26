using Autofac;
using System;
using XamarinCommons.Image;

namespace Spectator.Windows
{
    class WPInjectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StubImageDecoder>().As<ImageDecoder>();
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