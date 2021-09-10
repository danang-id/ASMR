using ASMR.Mobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASMR.Mobile.Common.Abstractions;
using ASMR.Mobile.Services.Abstraction;
using Xamarin.Forms;

namespace ASMR.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected static IApplicationState ApplicationState => DependencyService.Get<IApplicationState>();
        protected static ILogging Logging => DependencyService.Get<ILogging>();
        protected static IAlertHandler AlertHandler => DependencyService.Get<IAlertHandler>();
        protected static IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        protected static IBeanService BeanService => DependencyService.Get<IBeanService>();
        protected static IGateService GateService => DependencyService.Get<IGateService>();
        protected static IProductionService ProductionService => DependencyService.Get<IProductionService>();
        protected static IProductService ProductService => DependencyService.Get<IProductService>();
        protected static IStatusService StatusService => DependencyService.Get<IStatusService>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected bool SetProperty<T>(
            ref T backingStore,
            T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
