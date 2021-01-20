using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_Tabata_Timer
{
    /// <summary>
    /// Public Tabata class
    /// </summary>
    class Tabata
    {
        static public bool Lanunched { get; set; }
        static public bool Paused { get; set; }
        static public bool Unlocked { get; set; }
        static public int CurrentStatus { get; set; }
        static public int StageNumber { get; set; }
        static public int MaxTime { get; set; }
    }
}
