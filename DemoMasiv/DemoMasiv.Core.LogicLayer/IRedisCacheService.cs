using DemoMasiv.Core.LogicLayer.Class;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoMasiv.Core.LogicLayer
{
    public interface IRedisCacheService 
    {        
        Task<long> SetValueAsync(string key, string value);
        Task<long> GetLenValue(string key);
        Task<long> CreateRoulette();
        Task<List<Roulette>> GetAllElementsOfList(string key);
        Task<bool> StartRoulete(int idRoulette);
        Task SetElementOnList(string key, int index, string value);
        Task<string> GamblingAOnNumber(BetRouletteRequest betRouletteRequest, int idUser);
        Task<CloseGamblingResult> CloseGamblingRoulette(int idRoulette);
        Task<List<InventoryRoulettesResult>> InventoryRoulettesResults();
    }
}
