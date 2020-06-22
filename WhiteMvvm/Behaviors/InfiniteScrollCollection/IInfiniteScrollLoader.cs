using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhiteMvvm.Behaviors.InfiniteScrollCollection
{
    public interface IInfiniteScrollLoader
    {
        bool CanLoadMore { get; }

        Task LoadMoreAsync();
    }
}
