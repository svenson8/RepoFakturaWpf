using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace FakturaWpf.Tools
{
    /// <summary>
    /// Logika interakcji dla klasy ProgressBar.xaml
    /// </summary>
    
    public partial class MyProgressBar : Window, INotifyPropertyChanged
    {
        private BackgroundWorker _bgWorker = new BackgroundWorker();
        private int _workerState;

        public event PropertyChangedEventHandler PropertyChanged;

        public int WorkerState
        {
            get { return _workerState; }
            set
            {
                 
                _workerState = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WorkerState"));

            }
        }
        public MyProgressBar(string _name, int max)
        {
            InitializeComponent();

            lab_title.Content = _name;
            my_progcontrol.Maximum = max;

            DataContext = this;

         

        }

        public void UpdateBar(int i)
        {

            _bgWorker.DoWork += (s, e) =>
            {

                    System.Threading.Thread.Sleep(100);
                    WorkerState = i;

            };

            if (i == Convert.ToInt32(my_progcontrol.Maximum))
                Various.InfoOk("Wykonano");


            if (!_bgWorker.IsBusy)
                _bgWorker.RunWorkerAsync();
        }
    }
}
