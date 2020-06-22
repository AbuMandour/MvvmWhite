using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Behaviors.InfiniteScrollCollection
{
    public interface IInfiniteScrollLoading
    {
        bool IsLoadingMore { get; }

        event EventHandler<LoadingMoreEventArgs> LoadingMore;
    }
}
