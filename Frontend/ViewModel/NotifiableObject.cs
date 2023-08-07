﻿using System.ComponentModel;
namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class NotifiableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs
            (property));
        }
    }
}
