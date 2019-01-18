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
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

namespace FakturaWpf.Tools
{
    /// <summary>
    /// Logika interakcji dla klasy ProgressBar.xaml
    /// </summary>

    public partial class MyProgressBar : Window
    {

        public MyProgressBar(string _name, int max)
        {
            InitializeComponent();

            lab_title.Content = _name;
            my_progcontrol.Maximum = max;

        }

        public void Step(int[] arr, Boolean message = false) // few steps
        {
            foreach (int i in arr)
            {
                my_progcontrol.Value = i;
                DoAndDelay();
            }

            CheckMessageAndClose(arr.Last(), message);

        }

        public void Step(int i, Boolean message = false) // one step
        {

            my_progcontrol.Value = i;
            DoAndDelay();

            CheckMessageAndClose(i, message);

        }

        private void DoAndDelay()
        {
            Various.DoEvents();
            System.Threading.Thread.Sleep(100);
        }

        private void CheckMessageAndClose(int last, Boolean mess)
        {
            if (last== Convert.ToInt32(my_progcontrol.Maximum))
            {
                if (mess == true)
                    Various.InfoOk("Wykonano");
                this.Close();
            }
        }
    }
}
