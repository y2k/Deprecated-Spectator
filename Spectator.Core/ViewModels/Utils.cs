﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spectator.Core.ViewModels
{
    public static class Utils
    {
        public static void ReplaceAll<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
        {
            collection.Clear();
            foreach (var s in newItems)
                collection.Add(s);
        }
    }
}