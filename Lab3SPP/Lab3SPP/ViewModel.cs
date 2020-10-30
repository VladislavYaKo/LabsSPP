using AssemblyBrowserLibrary;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Lab3SPP
{
    public class ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }


        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ??
                    (_openCommand = new RelayCommand(obj =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Title = "Select Assembly";
                        openFileDialog.Filter = "Assembly|*.dll";
                        if (openFileDialog.ShowDialog() == true)
                        {
                            FilePath = openFileDialog.FileName;
                        }
                        if (FilePath != null)
                        {
                            Model model = new Model();
                            Namespaces = new ObservableCollection<Namespace>(model.LoadAssembly(FilePath));
                        }
                    }));
            }
        }

        private ObservableCollection<Namespace> _namespaces;

        public ObservableCollection<Namespace> Namespaces
        {
            get { return _namespaces; }
            set
            {
                _namespaces = value;
                OnPropertyChanged("Namespaces");
            }
        }
    }
}
