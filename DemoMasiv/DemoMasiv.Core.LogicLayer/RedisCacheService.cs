using DemoMasiv.Core.LogicLayer.Class;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoMasiv.Core.LogicLayer
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService (IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<long> GetLenValue(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return await db.ListLengthAsync(key);
        }        

        public async Task<long> SetValueAsync(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return await db.ListRightPushAsync(key, value);
        }

        public async Task<List<Roulette>> GetAllElementsOfList(string key)
        {

            IDatabase db = _connectionMultiplexer.GetDatabase();
            List<Roulette> listRoulette = new List<Roulette>();
            var values = await db.ListRangeAsync(key, 0, -1);
            if(values.Count() > 0)
            {
                foreach (var value in values)
                {
                    var roulette = JsonSerializer.Deserialize<Roulette>(value.ToString());
                    listRoulette.Add(roulette);
                }                
            }

            return listRoulette;            
        }

        public async Task SetElementOnList(string key, int index, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            await db.ListSetByIndexAsync(key, index, value);
        }

        public async Task<long> CreateRoulette()
        {
            var lenArrayRoulette = await GetLenValue("Roulette");
            var idRoulette = lenArrayRoulette + 1;
            var roulette = new Roulette() { 
                Id = Convert.ToInt32(idRoulette),
                State = 0,
                Users = new List<UserRoulette>(),
                Bets = new List<BetRoulette>()
            };
            var jsonRoulette = JsonSerializer.Serialize(roulette);
            var resultOperation = await SetValueAsync("Roulette", jsonRoulette);

            return resultOperation > 0 ? resultOperation : 0;            
        }       

        public async Task<bool> StartRoulete(int idRoulette)
        {
            var allElements = await GetAllElementsOfList("Roulette");
            var rouletteExist = allElements.Where(x => x.Id == idRoulette).FirstOrDefault();
            if(rouletteExist != null)
            {
                rouletteExist.State = 1;
                var index = rouletteExist.Id - 1;
                var value = JsonSerializer.Serialize(rouletteExist);
                await SetElementOnList("Roulette", index, value);
                return true;
            }
            else { return false; }
        }

        public async Task<string> GamblingAOnNumber(BetRouletteRequest betRouletteRequest,
                                                    int idUser)
        {
            if (betRouletteRequest.NumberBet > 36 || betRouletteRequest.NumberBet < 0)
                return "Numero no autorizado en la ruleta";
            if (betRouletteRequest.ValueBet > 10000 || betRouletteRequest.ValueBet == 0)
                return "Valor de apuesta no autorizado";
            if (betRouletteRequest.ColorBet > 2 || betRouletteRequest.ColorBet < 0)
                return "Color no autorizado para apuesta";            
            var allElements = await GetAllElementsOfList("Roulette");
            var rouletteExist = allElements
                .Where(x => x.Id == betRouletteRequest.IdRoulette).FirstOrDefault();
            if (rouletteExist != null)
            {
                if (rouletteExist.State == 1)                
                    return await ValidateGamblig(rouletteExist, betRouletteRequest, idUser);
                 else  
                    return "Ruleta no activa para recibir apuestas";                
            } else { return "No se realizo la apuesta"; }
        }

        public async Task<string> ValidateGamblig(Roulette rouletteExist,
                                                  BetRouletteRequest betRouletteRequest,
                                                  int idUser)
        {
            var index = rouletteExist.Id - 1;
            var betRoulette = new BetRoulette()
            {
                IdUser = idUser,
                IdRoulette = betRouletteRequest.IdRoulette,
                ColorBet = betRouletteRequest.ColorBet,
                NumberBet = betRouletteRequest.NumberBet,
                ValueBet = betRouletteRequest.ValueBet
            };
            rouletteExist.Bets.Add(betRoulette);
            var value = JsonSerializer.Serialize(rouletteExist);
            await SetElementOnList("Roulette", index, value);

            return "Apuesta recibida con exito";
        }

        public async Task<CloseGamblingResult> CloseGamblingRoulette(int idRoulette)
        {
            var allElements = await GetAllElementsOfList("Roulette");
            var rouletteExist = allElements.Where(x => x.Id == idRoulette).FirstOrDefault();
            var arrayWinner = new CloseGamblingResult() { };
            if (rouletteExist != null)
            {
                if (rouletteExist.State == 1)
                {
                    return ChooseWinner(rouletteExist, arrayWinner);
                }
                else {
                    arrayWinner.Message = "Ruleta no activa para recibir apuestas";
                    return arrayWinner;
                }
            }
            else {
                arrayWinner.Message = "Id de ruleta errado";
                return arrayWinner;
            }
        }

        public CloseGamblingResult ChooseWinner(Roulette rouletteExist,
                                                CloseGamblingResult arrayWinner)
        {
            arrayWinner.NumberWinner = new Random().Next(1, 37);
            arrayWinner.BetRoulettes = rouletteExist.Bets;
            var gamblingWinnerNumber = rouletteExist.Bets.Where(x =>
                            x.NumberBet == arrayWinner.NumberWinner)
                .FirstOrDefault();
            if (gamblingWinnerNumber != null)
            {
                arrayWinner.IdWinner = gamblingWinnerNumber.IdUser;
                arrayWinner.ValueWinner = 5 * gamblingWinnerNumber.ValueBet;
                arrayWinner.Message = $"Felicidades Usuario {arrayWinner.IdWinner}" +
                    $" Has escogido el numero ganardor {arrayWinner.NumberWinner}" +
                    $" Tu premio es de {arrayWinner.ValueWinner}";
            }
            else
            {
                var par = arrayWinner.NumberWinner % 2 == 0 ? 1 : 2;
                var gamblingWinnerColor = rouletteExist.Bets.Where(x =>
                            x.ColorBet == par).FirstOrDefault();
                if (gamblingWinnerColor != null)
                {
                    arrayWinner.IdWinner = gamblingWinnerColor.IdUser;
                    arrayWinner.ValueWinner = 1.8 * gamblingWinnerColor.ValueBet;
                    arrayWinner.Message = $"Felicidades Usuario {arrayWinner.IdWinner}" +
                        $" Has escogido el color ganardor {arrayWinner.NumberWinner}" +
                        $" Tu premio es de {arrayWinner.ValueWinner}";
                }
                else
                {
                    arrayWinner.IdWinner = 0;
                    arrayWinner.ValueWinner = 0;
                    arrayWinner.Message = $"No hay ganadores en esta ronda";
                }
            }
            return arrayWinner;
        }

        public async Task<List<InventoryRoulettesResult>> InventoryRoulettesResults()
        {
            var allElements = await GetAllElementsOfList("Roulette");
            var roulettesResult = new List<InventoryRoulettesResult>();
            foreach(var element in allElements)
            {
                var roulette = new InventoryRoulettesResult() 
                {
                    idRoulette = element.Id,
                    state = element.State == 1 ? "Activada":"Desactivada"
                };
                roulettesResult.Add(roulette);
            }

            return roulettesResult;
        }
    }
}
