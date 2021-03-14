using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DemoMasiv.Core.LogicLayer.Class
{
    public class Roulette
    {
        public int Id { get; set; }
        public int State { get; set; }
        public List<UserRoulette> Users { get; set; }
        public List<BetRoulette> Bets { get; set; }
    }
}
