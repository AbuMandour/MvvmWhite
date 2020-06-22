using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WhiteMvvm.Behaviors.InfiniteScrollCollection
{
    public class InfiniteScrollCollection<T> : ObservableCollection<T>, IInfiniteScrollLoader, IInfiniteScrollLoading, INotifyPropertyChanged
    {
        private bool isLoadingMore;

        public InfiniteScrollCollection()
        {
        }

        public InfiniteScrollCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public Action OnBeforeLoadMore { get; set; }

        public Action OnAfterLoadMore { get; set; }

        public Action<Exception> OnError { get; set; }

        public Func<bool> OnCanLoadMore { get; set; }

        public Func<Task<IEnumerable<T>>> OnLoadMore { get; set; }

        public virtual bool CanLoadMore => OnCanLoadMore?.Invoke() ?? true;

        public bool IsLoadingMore
        {
            get => isLoadingMore;
            private set
            {
                if (isLoadingMore != value)
                {
                    isLoadingMore = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLoadingMore)));

                    LoadingMore?.Invoke(this, new LoadingMoreEventArgs(IsLoadingMore));
                }
            }
        }

        public event EventHandler<LoadingMoreEventArgs> LoadingMore;

        public async Task LoadMoreAsync()
        {
            try
            {
                IsLoadingMore = true;

                OnBeforeLoadMore?.Invoke();

                var result = await OnLoadMore();
                AddRange(result);
            }
            catch (Exception ex) when (OnError != null)
            {
                OnError.Invoke(ex);
            }
            finally
            {
                IsLoadingMore = false;
                OnAfterLoadMore?.Invoke();
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                return;
            //throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            var startIndex = Count;
            var changedItems = new List<T>(collection);

            foreach (var i in changedItems)
                Items.Add(i);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
        }
        public new event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Fire to update property
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
