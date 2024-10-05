using PhoneNumbers;

namespace Banking.Service.Helpers;

public class PhoneNumberValidator
{
    public bool IsValidInternationalPhoneNumber(string phoneNumber)
    {
        var phoneUtil = PhoneNumberUtil.GetInstance();
        try
        {
            // Parse the number, assuming it has the correct country code
            var parsedNumber = phoneUtil.Parse(phoneNumber, null); // null lets it accept international numbers

            // Check if the number is valid
            return phoneUtil.IsValidNumber(parsedNumber);
        }
        catch (NumberParseException)
        {
            return false; // Invalid number
        }
    }
}
