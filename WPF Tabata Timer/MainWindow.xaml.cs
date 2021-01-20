using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Tabata_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Public Tabata class
        /// </summary>
        static class Tabata
        {
            #region Public Properties

            static public bool Lanunched { get; set; }
            static public bool Paused { get; set; }
            static public bool Unlocked { get; set; }
            static public int CurrentStatus { get; set; }
            static public int StageNumber { get; set; }

            #endregion
        }

        public MainWindow()
        {
            InitializeComponent();
            Tabata.Lanunched = false;
        }

        #region Lock and Unlock Buttons method
        /// <summary>
        /// Lock and unlock buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LockelementsButton_Click(object sender, RoutedEventArgs e)
        {
            if (LockelementsButton.Content.Equals("Locked"))
            {
                LockelementsButton.Content = "Unlocked";
                Tabata.Unlocked = true;
            }
            else
            {
                LockelementsButton.Content = "Locked";
                Tabata.Unlocked = false;
            }
        }
        #endregion

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabata.Lanunched == false)
            {
                Tabata.Lanunched = true;
                Tabata.StageNumber = 1;
                sp1.Background = Brushes.LimeGreen;
            }

            if(Tabata.Paused==false)
            {
                Tabata.CurrentStatus = (int)Status.Prepare;
                StartPauseButton.Content = "Start";
                Tabata.Paused = true;

                #region Status Block not used jet
                /*
                if(Tabata.StageNumber==1)
                StatusBlok.Text = "Prepare";
                else if(Tabata.StageNumber%2==0)
                    StatusBlok.Text = "Work";
                else if(Tabata.StageNumber==17)
                    StatusBlok.Text = "Finish";
                */
                #endregion

                TopCountdown(240, TimeSpan.FromSeconds(1), current => StatusBlok.Text = current.ToString());
                Countdown(10, TimeSpan.FromSeconds(1), current => TimerBlock.Text = current.ToString());
            }
            else
            {                
                StartPauseButton.Content = "Pause";
                Tabata.Paused = false;
            }
            
        }

        /// <summary>
        /// Method counting down 4 minutes
        /// </summary>
        /// <param name="count"></param>
        /// <param name="interval"></param>
        /// <param name="ts"></param>
        private void TopCountdown( int count, TimeSpan interval, Action<int> ts)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (count-- == 0)
                { 
                    dt.Stop();
                    Tabata.CurrentStatus = (int)Status.Finish;
                }
                else
                    ts(count);
            };
            ts(count);
            dt.Start();            
        }

        /// <summary>
        /// Method counting down each excercise and rest
        /// </summary>
        /// <param name="count"></param>
        /// <param name="interval"></param>
        /// <param name="ts"></param>
        private void Countdown(int count, TimeSpan interval, Action<int> ts)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                HighlightCurrentStage(Tabata.StageNumber);

                if (Tabata.CurrentStatus == (int)Status.Finish)
                {
                    TimerBlock.Text = "0";
                    return; 
                }
                count--;
                if ((count == 0) && (Tabata.CurrentStatus == (int)Status.Prepare || Tabata.CurrentStatus == (int)Status.Rest))
                {
                    dt.Stop();
                    Tabata.CurrentStatus = (int)Status.Work;
                    sp1.Background = Brushes.Red;
                    Tabata.StageNumber++;

                    Countdown(20, TimeSpan.FromSeconds(1), current => TimerBlock.Text = current.ToString());
                }
                else if ((count == 0) && (Tabata.CurrentStatus == (int)Status.Work))
                {
                    dt.Stop();
                    Tabata.CurrentStatus = (int)Status.Rest;
                    sp1.Background = Brushes.Blue;
                    Tabata.StageNumber++;
                    Countdown(10, TimeSpan.FromSeconds(1), current => TimerBlock.Text = current.ToString());
                }
                else
                {
                    
                    ts(count);
                }
            };
            
            ts(count);
            dt.Start();
            
        }
        private void PauseCountdown(int count, TimeSpan interval, Action<int> ts)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (count-- == 0)
                    dt.Stop();
                else
                    ts(count);
            };
            ts(count);
            dt.Start();
        }
        private void HighlightCurrentStage(int stageNumber)
        {
            if (stageNumber == 1)
                Stage1.Background = Brushes.Green;
            if (stageNumber == 2)
            {
                Stage2.Background = Brushes.DarkRed;
                Stage1.Background = Brushes.Transparent;
            }
            if (stageNumber == 3)
            {
                Stage3.Background = Brushes.DarkBlue;
                Stage2.Background = Brushes.Transparent;
            }
            if (stageNumber == 4)
            {
                Stage4.Background = Brushes.DarkRed;
                Stage3.Background = Brushes.Transparent;
            }
            if (stageNumber == 5)
            {
                Stage5.Background = Brushes.DarkBlue;
                Stage4.Background = Brushes.Transparent;
            }
            if (stageNumber == 6)
            {
                Stage6.Background = Brushes.DarkRed;
                Stage5.Background = Brushes.Transparent;
            }
            if (stageNumber == 7)
            {
                Stage7.Background = Brushes.DarkBlue;
                Stage6.Background = Brushes.Transparent;
            }
            if (stageNumber == 8)
            {
                Stage8.Background = Brushes.DarkRed;
                Stage7.Background = Brushes.Transparent;
            }
            if (stageNumber == 9)
            {
                Stage9.Background = Brushes.DarkBlue;
                Stage8.Background = Brushes.Transparent;
            }
            if (stageNumber == 10)
            {
                Stage10.Background = Brushes.DarkRed;
                Stage9.Background = Brushes.Transparent;
            }
            if (stageNumber == 11)
            {
                Stage11.Background = Brushes.DarkBlue;
                Stage10.Background = Brushes.Transparent;
            }
            if (stageNumber == 12)
            {
                Stage12.Background = Brushes.DarkRed;
                Stage11.Background = Brushes.Transparent;
            }
            if (stageNumber == 13)
            {
                Stage13.Background = Brushes.DarkBlue;
                Stage12.Background = Brushes.Transparent;
            }
            if (stageNumber == 14)
            {
                Stage14.Background = Brushes.DarkRed;
                Stage13.Background = Brushes.Transparent;
            }
            if (stageNumber == 15)
            {
                Stage15.Background = Brushes.DarkBlue;
                Stage14.Background = Brushes.Transparent;
            }
            if (stageNumber == 16)
            {
                Stage16.Background = Brushes.DarkRed;
                Stage15.Background = Brushes.Transparent;
            }
            if (stageNumber == 17)
            {
                Finish.Background = Brushes.DarkBlue;
                Stage16.Background = Brushes.Transparent;
            }
        }
    }
}
