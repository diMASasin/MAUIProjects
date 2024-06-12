using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp2
{
    internal class MainViewModel : BindableObject
    {
        private string _currentText;
        public string CurrentText 
        { 
            get => _currentText;
            set
            {
                _currentText = value;
                OnPropertyChanged(nameof(CurrentText));
            }
        }

        private TaskItem _itemToEdit;

        public MainViewModel()
        {
            AddItemCommand = new Command<string>(x =>
            {
                var item = new TaskItem(x);
                TaskItems.Insert(0, item);
                CurrentText = string.Empty;
            },
                x => string.IsNullOrEmpty(x) == false);

            DeleteItemCommand = new Command<TaskItem>(x =>
            {
                TaskItems.Remove(x);
            },
                x => TaskItems.Contains(x));
        }

        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ObservableCollection<TaskItem> TaskItems { get; } = new();
    }

    internal class TaskItem : BindableObject
    {
        private bool _isEditingModeOn = false;
        public bool IsEditingModeOn
        {
            get => _isEditingModeOn;
            set
            {
                _isEditingModeOn = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditingModeOff = true;
        public bool IsEditingModeOff
        {
            get => _isEditingModeOff;
            set
            {
                _isEditingModeOff = value;
                OnPropertyChanged();
            }

        }

        public ICommand EditItemCommand { get; }
        public ICommand ApplyItemCommand { get; }

        public TaskItem(string text)
        {
            _text = text;


            EditItemCommand = new Command(() =>
            {
                IsEditingModeOn = true;
                IsEditingModeOff = false;
            },
                () => true);

            ApplyItemCommand = new Command(() =>
            {
                IsEditingModeOff = true;
                IsEditingModeOn = false;
            },
                () => true);
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }
}
