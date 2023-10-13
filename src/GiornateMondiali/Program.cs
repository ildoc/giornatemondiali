using System.ComponentModel.DataAnnotations;
using HtmlAgilityPack;

namespace GiornateMondiali
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //foreach (var d in GmCore.GetSpecialDays(2025))
            //    Console.WriteLine(d);

            var web = new HtmlWeb();

            var today = DateOnly.FromDateTime(DateTime.Today);

            var forecastDays = new Dictionary<DateOnly, string>
            {
                {today, "https://www.3bmeteo.com/meteo/genova" }
            };

            for (var i = 1; i <= 5; i++)
                forecastDays.Add(today.AddDays(i), $"https://www.3bmeteo.com/meteo/genova/dettagli_orari/{i}");

            var forecasts = forecastDays.Select(x => ScrapeDayData(web, x.Key, x.Value));

            Console.ReadLine();
        }

        private static DailyForecast ScrapeDayData(HtmlWeb web, DateOnly date, string url)
        {
            var document = web.Load(url);
            var dailyForecast = new DailyForecast(date);

            var hourlyForecastElements = document.DocumentNode.QuerySelectorAll("div.row-table.noPad:not(.row-segnala)").Where(x => !x.InnerHtml.Contains("Invia la tua segnalazione"));

            foreach (var hf in hourlyForecastElements)
            {
                var header = hf.QuerySelector("div.col-xs-2-3.col-sm-2-5>div.row-table.special_campaign");

                var ora = header.QuerySelector("div.col-xs-1-4.big.zoom_prv").InnerText.Split(':');
                var desc = header.QuerySelector("div.col-xs-2-4.zoom_prv").InnerText;
                var img = header.QuerySelector("div.col-xs-1-4.text-center.no-padding.zoom_prv>img").NextSibling.Attributes["src"].Value;

                var body = hf.QuerySelector("div.col-xs-1-3.col-sm-3-5.table-striped-inverse-h.text-center>div.row-table");

                var temp = body.QuerySelector("div.col-xs-1-2.col-sm-1-5.big>span.switchcelsius").InnerText.Trim();
                var prec = body.QuerySelector("div.altriDati-precipitazioni>span.gray").InnerText;
                var umidita = body.QuerySelector("div.altriDati-umidita").InnerText;
                var mare = body.QuerySelector("div.altriDati-mare").ChildNodes["small"].InnerText;
                var mareImg = body.QuerySelector("div.altriDati-mare").ChildNodes["img"].Attributes["src"].Value;
                var tempPerc = body.QuerySelector("div.altriDati-percepita>span.switchcelsius").InnerText;
                var pressione = body.QuerySelector("div.altriDati-pressione").InnerText;


                var hour = date.ToDateTime(new TimeOnly(Convert.ToInt32(ora[0]), Convert.ToInt32(ora[1]));

                dailyForecast.AddForecast(new HourlyForecast(hour, desc,img,Convert.ToDouble(temp),prec, ));

                Console.WriteLine(hf);
            }

            return dailyForecast;
        }

        public record HourlyForecast(DateTime Hour, string Desc, string Img, double Temp, string Rain, int Humidity, string SeaState, string SeaStateImg, double TempPerc, double Pressure);
        public class DailyForecast
        {
            public List<HourlyForecast> HourlyForecasts { get; } = new();
            public DateOnly Date { get; private set; }
            public double TempMax { get; private set; } = double.MinValue;
            public double TempMin { get; private set; } = double.MaxValue;

            public DailyForecast(DateOnly dateOnly) { Date =  dateOnly; }

            public void AddForecast(HourlyForecast hourlyForecast) {

                if (hourlyForecast.Temp > TempMax)
                {
                    TempMax = hourlyForecast.Temp;
                }
                if(hourlyForecast.Temp< TempMin)
                {
                    TempMin = hourlyForecast.Temp;
                }

                HourlyForecasts.Add(hourlyForecast);
            }
        }
    }
}
