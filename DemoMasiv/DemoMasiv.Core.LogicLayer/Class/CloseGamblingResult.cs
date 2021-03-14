using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMasiv.Core.LogicLayer.Class
{
    public class CloseGamblingResult
    {
        public List<BetRoulette> BetRoulettes { get; set; }
        public int NumberWinner { get; set; }
        public int IdWinner { get; set; }
        public double ValueWinner { get; set; }
        public string Message { get; set; }
    }
}
