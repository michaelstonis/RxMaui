using System;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Markup;
using ReactiveUI;

namespace RxMaui
{
    public class RxPage : ReactiveContentPage<ViewModels.RxViewModel>
    {
        private Button _myButton;
        private Button _nextPage;
        private CollectionView _myCollection;

        public RxPage()
        {
            InitializeComponent();
            InitializeBindings();
            ViewModel = new ViewModels.RxViewModel();
        }

        private void InitializeComponent()
        {
            Content =
               new VerticalStackLayout
               {
                   Children =
                   {
                        new LifecycleButton
                        {
                            Text = "Get Count",
                            VerticalOptions = LayoutOptions.Start
                        }
                            .Assign(out _myButton),
                        new Button
                        {
                            Text = "Next Page",
                            VerticalOptions = LayoutOptions.Start
                        }
                            .Assign(out _nextPage),
                        new RxContentView()
                        {
                        },
                        new CollectionView
                        {
                            BackgroundColor = Colors.Fuchsia,
                            VerticalOptions = LayoutOptions.Fill,
                        }
                            .Assign(out _myCollection),
                   },
                   Padding = 8,
                   Spacing = 8,
               };

            //Content =
            //    new Grid
            //    {
            //        RowDefinitions =
            //        {
            //            new RowDefinition(GridLength.Auto),
            //            new RowDefinition(GridLength.Auto),
            //            new RowDefinition(44),
            //            new RowDefinition(GridLength.Star),
            //        },
            //        Children =
            //        {
            //            new LifecycleButton
            //            {
            //                Text = "Get Count",
            //                VerticalOptions = LayoutOptions.Start
            //            }
            //                .Row(0)
            //                .Assign(out _myButton),
            //            new Button
            //            {
            //                Text = "Next Page",
            //                VerticalOptions = LayoutOptions.Start
            //            }
            //                .Row(1)
            //                .Assign(out _nextPage),
            //            new RxContentView()
            //            {
            //            }
            //                .Row(2),
            //            new CollectionView
            //            {
            //                VerticalOptions = LayoutOptions.Fill,
            //            }
            //                .Row(3)
            //                .Assign(out _myCollection),
            //        },
            //        Padding = 8,
            //        RowSpacing = 8,
            //    };
        }

        private void InitializeBindings()
        {
            this.BindCommand(ViewModel, vm => vm.UpdateCalculation, ui => ui._myButton);

            this.WhenAnyValue(x => x.ViewModel.Count)
                .Where(x => x > 0)
                .Select(x => $"The count is {x}")
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(this, ui => ui._myButton.Text);

            this.BindCommand(ViewModel, vm => vm.NextPage, ui => ui._nextPage);

            this.WhenAnyObservable(x => x.ViewModel.NextPage)
                .Select(
                    _ =>
                    {
                        return Observable
                            .FromAsync(() => this.Navigation.PushAsync(new RxPage()))
                            .SubscribeOn(RxApp.MainThreadScheduler);
                    })
                .Merge(1)
                .Subscribe();

            this.OneWayBind(ViewModel, vm => vm.Items, ui => ui._myCollection.ItemsSource);

            Observable
                .Merge(
                    Observable
                        .FromEventPattern(
                            x => this.Loaded += x,
                            x => this.Loaded -= x)
                        .Select(_ => true),
                    Observable
                        .FromEventPattern(
                            x => this.Unloaded += x,
                            x => this.Unloaded -= x)
                        .Select(_ => false))
                .Do(loadState => Console.WriteLine($"RxPage - Loaded: {loadState}"))
                .Subscribe(_ => { }, () => Console.WriteLine($"RxPage - Disposed"));
        }

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

            Console.WriteLine($"RxPage - New Handler: {args.NewHandler != null}");
            Console.WriteLine($"RxPage - Old Handler: {args.OldHandler != null}");
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            Console.WriteLine($"RxPage - Handler Changed: {Handler != null}");
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            Console.WriteLine($"RxPage - Property: {propertyName}");

            if (propertyName == WindowProperty.PropertyName)
            {
                Console.WriteLine($"RxPage - IsLoaded: {this.IsLoaded}");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}

