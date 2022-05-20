using System;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace RxMaui.ViewModels
{
    public class RxViewModel : ReactiveObject
    {
        private SourceList<DataTest> _itemsSource;

        [Reactive]
        public IEnumerable<DataTest> Items { get; private set; }

        [Reactive]
        public int Count { get; set; }

        [Reactive]
        public ReactiveCommand<Unit, Unit> UpdateCalculation { get; private set; }

        [Reactive]
        public ReactiveCommand<Unit, Unit> NextPage { get; private set; }

        public RxViewModel()
        {
            _itemsSource = new SourceList<DataTest>();

            _itemsSource
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out var itemsCollection)
                .Subscribe();

            Items = itemsCollection;

            UpdateCalculation =
                ReactiveCommand
                    .Create(
                        () =>
                        {
                            ++Count;
                            _itemsSource.Add(new DataTest { Name = $"Item {Count}", Value = Count });
                        });

            NextPage =
                ReactiveCommand
                    .Create(
                        () =>
                        {
                        });
        }
    }

    public class DataTest
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}