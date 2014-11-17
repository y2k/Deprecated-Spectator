using GalaSoft.MvvmLight.Messaging;
using System;

namespace Spectator.WP8.ViewModel.Messages
{
    class NavigationMessage : MessageBase
    {
        public static readonly NavigationMessage GoBack = new NavigationMessage();

        public Type Target { get; set; }
    }
}