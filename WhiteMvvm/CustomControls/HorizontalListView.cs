using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WhiteMvvm.CustomControls
{
    public class HorizontalListView : Grid
    {
        private ICommand _innerSelectedCommand;
        private readonly ScrollView _scrollView;
        private readonly StackLayout _itemsStackLayout;

        public event EventHandler SelectedItemChanged;

        public StackOrientation ListOrientation { get; set; }

        public double Spacing { get; set; }

        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create("SelectedCommand", typeof(ICommand), typeof(HorizontalListView), null);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(HorizontalListView), default(IEnumerable<object>), BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem", typeof(object), typeof(HorizontalListView), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(HorizontalListView), default(DataTemplate));

        public static readonly BindableProperty ScrollBarVisibilityProperty =
            BindableProperty.Create("ScrollBarVisibility", typeof(bool), typeof(HorizontalListView), false);

        public static readonly BindableProperty ChildrensViewsProperty =
            BindableProperty.Create("ChildrensViews", typeof(IList<View>), typeof(HorizontalListView), default(StackLayout));
        public bool ScrollBarVisibility
        {
            get => (bool)GetValue(ScrollBarVisibilityProperty);
            set => SetValue(ScrollBarVisibilityProperty, value);
        }
        public IList<View> ChildrensViews
        {
            get => (IList<View>)GetValue(ChildrensViewsProperty);
            set => SetValue(ChildrensViewsProperty, value);
        }

        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsLayout = (HorizontalListView)bindable;
            itemsLayout.SetItems();
        }

        public HorizontalListView()
        {
            _itemsStackLayout = new StackLayout
            {
                BackgroundColor = BackgroundColor,
                Padding = Padding,
                Spacing = Spacing,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            ChildrensViews = _itemsStackLayout.Children;
            _scrollView = new ScrollView()
            {
                BackgroundColor = BackgroundColor,
                Content = _itemsStackLayout,
            };
            if (ScrollBarVisibility == false)
            {
                _scrollView.HorizontalScrollBarVisibility = Xamarin.Forms.ScrollBarVisibility.Never;
                _scrollView.VerticalScrollBarVisibility = Xamarin.Forms.ScrollBarVisibility.Never;
            }
            Children.Add(_scrollView);
        }
        public Task ScrollToAsync(double x, double y, bool animated)
        {
            return _scrollView.ScrollToAsync(x, y, animated);
        }
        public Task ScrollToAsync(Element element, ScrollToPosition position, bool animated)
        {
            return _scrollView.ScrollToAsync(element, position, animated);
        }
        protected virtual void SetItems()
        {
            _itemsStackLayout.Children.Clear();
            _itemsStackLayout.Spacing = Spacing;

            _innerSelectedCommand = new Command<View>(view =>
            {
                SelectedItem = view.BindingContext;
                SelectedItem = null; //Allowing item second time selection
                });

            _itemsStackLayout.Orientation = ListOrientation;
            _scrollView.Orientation = ListOrientation == StackOrientation.Horizontal
                ? ScrollOrientation.Horizontal
                : ScrollOrientation.Vertical;

            if (ItemsSource == null)
            {
                return;
            }

            foreach (var item in ItemsSource)
            {
                var view = GetItemView(item);
                _itemsStackLayout.Children.Add(view);
                ChildrensViews.Add(view);
            }
            _itemsStackLayout.BackgroundColor = BackgroundColor;
            SelectedItem = null;
        }

        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();
            var view = content as View;

            if (view == null)
            {
                return null;
            }

            view.BindingContext = item;

            var gesture = new TapGestureRecognizer
            {
                Command = _innerSelectedCommand,
                CommandParameter = view
            };

            AddGesture(view, gesture);

            return view;
        }

        private void AddGesture(View view, TapGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            var layout = view as Layout<View>;

            if (layout == null)
            {
                return;
            }

            foreach (var child in layout.Children)
            {
                AddGesture(child, gesture);
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsView = (HorizontalListView)bindable;
            if (newValue == oldValue && newValue != null)
            {
                return;
            }

            itemsView.SelectedItemChanged?.Invoke(itemsView, EventArgs.Empty);

            if (itemsView.SelectedCommand?.CanExecute(newValue) ?? false)
            {
                itemsView.SelectedCommand?.Execute(newValue);
            }
        }
    }

}
