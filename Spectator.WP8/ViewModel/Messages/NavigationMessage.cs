using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectator.WP8.ViewModel.Messages
{
    class NavigationMessage : MessageBase
    {
        public Type Target { get; set; }
    }
}