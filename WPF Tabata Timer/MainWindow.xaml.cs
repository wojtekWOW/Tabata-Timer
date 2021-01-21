using System;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;

namespace WPF_Tabata_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public Consrtuctor
        /// <summary>
        /// Public Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            Tabata.Lanunched = false;
            Tabata.Paused = true;
            Tabata.CurrentStatus = (int)Status.Prepare;
            Tabata.StageNumber = 1;
            Tabata.MaxTime = 30;
            Tabata.RemainingTime = Tabata.MaxTime;
            Tabata.PausedExcerciseTime = 10;

        }
        #endregion

        #region Reset Button method to restore app to initial stage
        /// <summary>
        /// Resets app to initial stage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabata.Lanunched == true)
            {
                Tabata.Paused = true;
                Tabata.Lanunched = false;
                sp1.Background = Brushes.Blue;
                StatusBlok.Text = "Prepare";
                TimerBlock.Text = "10";


                foreach (var stage in CreateStagesList())
                {
                    stage.Background = Brushes.Transparent;
                }
            }
            else
                return;
        }
        #endregion

        #region List of Stages
        /// <summary>
        /// Create a list of Stages 
        /// </summary>
        /// <returns></returns>
        private List<TextBlock> CreateStagesList()
        {
            List<TextBlock> stages = new List<TextBlock>();

            stages.Add(Stage1);
            stages.Add(Stage2);
            stages.Add(Stage3);
            stages.Add(Stage4);
            stages.Add(Stage5);
            stages.Add(Stage6);
            stages.Add(Stage7);
            stages.Add(Stage8);
            stages.Add(Stage9);
            stages.Add(Stage10);
            stages.Add(Stage11);
            stages.Add(Stage12);
            stages.Add(Stage13);
            stages.Add(Stage14); 
            stages.Add(Stage15);
            stages.Add(Stage16);
            stages.Add(Finish);

            return stages;
        }
        #endregion

        #region Start/Pause Button Click Event Method
        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tabata.Lanunched == false)
            {
                Tabata.Lanunched = true;
                
                sp1.Background = Brushes.LimeGreen;                
                    Stage1.Background = Brushes.Green;                
                
            }

            if (Tabata.Paused == true)
            {
                
                StartPauseButton.Content = "Pause";
                Tabata.Paused = false;

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

                TopCountdown(Tabata.MaxTime, TimeSpan.FromSeconds(1), current => StatusBlok.Text = current.ToString());
                Countdown(10, TimeSpan.FromSeconds(1), current => TimerBlock.Text = current.ToString());
            }
            else
            {
                StartPauseButton.Content = "Start";
                Tabata.Paused = true;
            }
        }
        #endregion

        #region Top 4 minutes Countdown
        /// <summary>
        /// Method counting down 4 minutes
        /// </summary>
        /// <param name="count"></param>
        /// <param name="interval"></param>
        /// <param name="tostring"></param>
        private void TopCountdown(int count, TimeSpan interval, Action<int> tostring)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (Tabata.Paused == true)
                {
                    dt.Stop();
                    return;
                }
                if (count-- == 0)
                {
                    dt.Stop();
                    Tabata.CurrentStatus = (int)Status.Finish;
                }
                else
                    tostring(count);
            };
            tostring(count);
            dt.Start();

        }
        #endregion

        #region Excercise/rest countdown
        /// <summary>
        /// Method counting down each excercise and rest
        /// </summary>
        /// <param name="count"></param>
        /// <param name="interval"></param>
        /// <param name="tostring"></param>
        private void Countdown(int count, TimeSpan interval, Action<int> tostring)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (Tabata.Paused == true)
                {
                    dt.Stop();
                    return;
                }
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
                    HighlightCurrentStage(Tabata.StageNumber);
                    Countdown(20, TimeSpan.FromSeconds(1), current => TimerBlock.Text = current.ToString());
                }
                else if ((count == 0) && (Tabata.CurrentStatus == (int)Status.Work))
                {
                    dt.Stop();
                    Tabata.CurrentStatus = (int)Status.Rest;                    
                    sp1.Background = Brushes.Blue;
                    Tabata.StageNumber++;
                    HighlightCurrentStage(Tabata.StageNumber);
                    Countdown(10, TimeSpan.FromSeconds(1), current => TimerBlock.Text = current.ToString());
                }
                else
                {
                    tostring(count);
                }
            };

            tostring(count);
            dt.Start();
        }
        #endregion

        private void StartPauseButton_Click(object sender, RoutedEventArgs e, bool paused)
        {

        }
        private void PauseCountdown(int count, TimeSpan interval, Action<int> tostring)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (count-- == 0)
                    dt.Stop();
                else
                    tostring(count);
            };
            tostring(count);
            dt.Start();
        }

        #region Highlight Current Stage
        /// <summary>
        /// Higlights curent stage block
        /// </summary>
        /// <param name="stageNumber">Number od a current stage</param>
        private void HighlightCurrentStage(int stageNumber)
        {
            if (stageNumber == 2)
            {
                Stage1.Background = Brushes.Transparent;
                Stage2.Background = Brushes.DarkRed;
            }
            if (stageNumber == 3)
            {
                Stage2.Background = Brushes.Transparent;
                Stage3.Background = Brushes.DarkBlue;
            }
            if (stageNumber == 4)
            {
                Stage3.Background = Brushes.Transparent;
                Stage4.Background = Brushes.DarkRed;
            }
            if (stageNumber == 5)
            {
                Stage4.Background = Brushes.Transparent;
                Stage5.Background = Brushes.DarkBlue;
            }
            if (stageNumber == 6)
            {
                Stage5.Background = Brushes.Transparent;
                Stage6.Background = Brushes.DarkRed;
            }
            if (stageNumber == 7)
            {
                Stage6.Background = Brushes.Transparent;
                Stage7.Background = Brushes.DarkBlue;
            }
            if (stageNumber == 8)
            {
                Stage7.Background = Brushes.Transparent;
                Stage8.Background = Brushes.DarkRed;
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
        #endregion
    }
}
