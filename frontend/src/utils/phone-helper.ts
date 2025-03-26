  import { CountryCode, getCountries, getCountryCallingCode, parsePhoneNumberWithError } from "libphonenumber-js";
  
  export const countriesPhoneFormat = getCountries().map((country) => {  //get country code and format
      const phoneCode = getCountryCallingCode(country); 
      return {
        key: country,
        name: country,
        format: `+${phoneCode}`,
      };
    });
  
  export const getOriginPhoneNumber = (phoneNumber: string) => {
    if(!phoneNumber) return null;
    try {
      const parsedNumber = phoneNumber.startsWith("+")
        ? phoneNumber
        : `+${phoneNumber}`;
      const parsed = parsePhoneNumberWithError(parsedNumber);
      if (parsed && parsed.isValid()) {
        return {
          originNumber: parsed.nationalNumber,
          countryCode: parsed.country as string
        } 
      }
      return null;
    } catch (error) {
      console.log('error', error);
    }
    return null;
  }
  export const getPhoneNumberFormat = (phoneNumber: string, countryCode: string) => {
    try {
      const parsed = parsePhoneNumberWithError(phoneNumber, { defaultCountry: countryCode as CountryCode });
      if (parsed && parsed.isValid()) {
        return parsed.number;
      }
      return phoneNumber;
    } catch (error) {
      console.error("Error parsing phone number:", error);
      return phoneNumber;
    }
  }
