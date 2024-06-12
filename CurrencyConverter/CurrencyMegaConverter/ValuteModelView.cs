namespace CurrencyMegaConverter;

internal class ValuteModelView : BindableObject
{
    private string _selectedItem;
    private double _entryText;
    private int _selectedIndex;
    private ValuteItem _valuteItem;
    private string[] _valuteNames;
    private ExchangeRates _exchangeRates;

    public double Value => _valuteItem.Value / _valuteItem.Nominal;

    public event Action NominalChanged;

    public ValuteModelView(string[] valuteNames, ExchangeRates exchangeRates, int selectedIndex = 0)
    {
        _valuteNames = valuteNames;
        _exchangeRates = exchangeRates;
        ChangeValuteByIndex(selectedIndex);
    }

    public void UpdateExchangeRates(ExchangeRates exchangeRates)
    {
        _exchangeRates = exchangeRates;
        ChangeValuteByIndex(_selectedIndex);
    }

    public string SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    public double EntryText
    {
        get => _entryText;
        set
        {
            if (_entryText == value) 
                return;

            _entryText = value;
            OnPropertyChanged(nameof(EntryText));
            NominalChanged?.Invoke();
        }
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            OnPropertyChanged(nameof(SelectedIndex));
            ChangeValuteByIndex(SelectedIndex);
            NominalChanged?.Invoke();
        }
    }

    private void ChangeValuteByIndex(int index)
    {
        _selectedIndex = index;
        _selectedItem = _valuteNames[_selectedIndex];
        _valuteItem = _exchangeRates.Valute[_selectedItem];
    }

    public void SetNominal(double value)
    {
        _entryText = value;
        OnPropertyChanged(nameof(EntryText));
    }
}