using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyMegaConverter 
{
    internal class MainViewModel : BindableObject
    {
        private string[] _currencyCharCodes;
        public string[] CurrencyCharCodes
        {
            get => _currencyCharCodes;
            set
            {
                _currencyCharCodes = value;
                OnPropertyChanged(nameof(CurrencyCharCodes));
            }
        }

        private string[] _currencyNames;
        public string[] CurrencyNames
        {
            get => _currencyNames;
            set
            {
                _currencyNames = value;
                OnPropertyChanged(nameof(CurrencyNames));
            }
        }

        private ValuteModelView _valute1;
        public ValuteModelView Valute1 => _valute1;

        private ValuteModelView _valute2;
        public ValuteModelView Valute2 => _valute2;

        public ExchangeRates ExchangeRates { get; set; }

        private DateTime _latestDate;

        private HttpClient _client;

        private ValuteItem _ruble = new("RUB", 1, "Российский рубль", 1);

        public MainViewModel()
        {
            _client = new HttpClient();
            ChangeDateOnLatest();
            _valute1.EntryText = 1;

        }

        private void ChangeDate()
        {
            var date = ExchangeRates.Date;
            if (date.Date == _latestDate.Date)
            {
                ChangeDateOnLatest();
                return;
            }

            Uri uri;
            HttpResponseMessage response;
            do
            {
                uri = new Uri(GetDateUri(date));
                response = _client.GetAsync(uri).Result;
                date = date.AddDays(-1);
            } 
            while (!response.IsSuccessStatusCode);

            GetData(uri);
        }


        private void ChangeDateOnLatest()
        {
            var uri = new Uri($"https://www.cbr-xml-daily.ru/daily_json.js");
            GetData(uri);
            _latestDate = ExchangeRates.Date;
        }


        private void GetData(Uri uri)
        {
            var response = _client.GetAsync(uri).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            if (ExchangeRates != null)
            {
                ExchangeRates.DatePicked -= ChangeDate;
                ExchangeRates.DatePicked -= OnValute1NominalChanged;
            }

            ExchangeRates = JsonSerializer.Deserialize<ExchangeRates>(result);
            ExchangeRates.Valute.Add(_ruble.CharCode, _ruble);
            ExchangeRates.DatePicked += ChangeDate;
            ExchangeRates.DatePicked += OnValute1NominalChanged;

            if(Valute1 == null || Valute2 == null)
                InitValute();
            else
                UpdateValute();
        }

        private void InitValute()
        {
            if (_valute1 != null) _valute1.NominalChanged -= OnValute1NominalChanged;
            if (_valute2 != null) _valute2.NominalChanged -= OnValute2NominalChanged;

            CurrencyCharCodes = ExchangeRates.Valute.Keys.ToArray();
            _currencyNames = ExchangeRates.Valute.Values
                .Select(value => value.Name)
                .Zip(_currencyCharCodes, (s, s1) => $"[{s1}] {s}").ToArray();

            _valute1 = new ValuteModelView(CurrencyCharCodes, ExchangeRates, 13);
            _valute2 = new ValuteModelView(CurrencyCharCodes, ExchangeRates, 43);
            _valute1.NominalChanged += OnValute1NominalChanged;
            _valute2.NominalChanged += OnValute2NominalChanged;
        }

        private void UpdateValute()
        {
            _valute1.UpdateExchangeRates(ExchangeRates);
            _valute2.UpdateExchangeRates(ExchangeRates);
        }

        private void OnValute1NominalChanged()
        {
            Valute2.SetNominal(Valute1.EntryText * Valute1.Value / Valute2.Value);
        }

        private void OnValute2NominalChanged()
        {
            Valute1.SetNominal(Valute2.EntryText * Valute2.Value / Valute1.Value);
        }

        private string GetDateUri(DateTime date)
        {
            return string.Format("https://www.cbr-xml-daily.ru/archive/{0:D4}/{1:D2}/{2:D2}/daily_json.js", date.Year, date.Month, date.Day);
        }
    }
}
