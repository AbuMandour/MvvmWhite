using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Behaviors.InfiniteScrollCollection
{
    public interface IInfiniteScrollDetector
    {
        bool ShouldLoadMore(object currentItem);
    }
}
