using System;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Markup;
using ReactiveUI;

namespace RxMaui
{
    public class RxContentView : ReactiveContentView<ViewModels.RxViewModel>
    {

        public RxContentView()
        {
            InitializeComponent();
            InitializeBindings();
            ViewModel = new ViewModels.RxViewModel();
        }

        private void InitializeComponent()
        {
            var rng = new Random(Guid.NewGuid().GetHashCode());
            var bytes = new byte[3];
            rng.NextBytes(bytes);
            BackgroundColor = Color.FromRgb(bytes[0], bytes[1], bytes[2]);
        }

        private void InitializeBindings()
        {
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
                .Do(loadState => Console.WriteLine($"RxContentView - Loaded: {loadState}"))
                .Subscribe(_ => { }, () => Console.WriteLine($"RxContentView - Disposed"));
        }

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

            Console.WriteLine($"RxContentView - New Handler: {args.NewHandler != null}");
            Console.WriteLine($"RxContentView - Old Handler: {args.OldHandler != null}");
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            Console.WriteLine($"RxContentView - Handler Changed: {Handler != null}");
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            Console.WriteLine($"RxContentView - Property: {propertyName}");

            if (propertyName == WindowProperty.PropertyName)
            {
                Console.WriteLine($"RxContentView - IsLoaded: {this.IsLoaded}");
            }
        }
    }
}

