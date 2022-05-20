using System;
using System.Runtime.CompilerServices;

namespace RxMaui
{
    public class LifecycleButton : Button
    {
        public LifecycleButton()
        {
        }

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

            Console.WriteLine($"Lifecycle Button - New Handler: {args.NewHandler != null}");
            Console.WriteLine($"Lifecycle Button - Old Handler: {args.OldHandler != null}");
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == WindowProperty.PropertyName)
            {
                Console.WriteLine($"LifecycleButton - IsLoaded: {this.IsLoaded}");
            }
        }
    }
}

