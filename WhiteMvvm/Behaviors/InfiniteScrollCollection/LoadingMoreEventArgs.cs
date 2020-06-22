using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Behaviors.InfiniteScrollCollection
{

    public class LoadingMoreEventArgs : EventArgs
    {
        public LoadingMoreEventArgs(bool isLoadingMore)
        {
            IsLoadingMore = isLoadingMore;
        }

        public bool IsLoadingMore { get; }
    }
}
