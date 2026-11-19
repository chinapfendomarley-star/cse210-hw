using System;

// Address and country in formatted-address form.
class Address
{
    private string _street;
    private string _city;
    private string _stateProvince;
    private string _country;

    public Address(string street, string city, string stateProvince, string country)
    {
        _street = street;
        _city = city;
        _stateProvince = stateProvince;
        _country = country;
    }

    public bool IsInUSA()
    {
        string normalized = _country.Trim();
        return normalized.Equals("USA", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("UNITED STATES", StringComparison.OrdinalIgnoreCase)
            || normalized.Equals("UNITED STATES OF AMERICA", StringComparison.OrdinalIgnoreCase);
    }

    public string GetFormattedAddress()
    {
        return $"{_street}\n{_city}, {_stateProvince}\n{_country}";
    }
}