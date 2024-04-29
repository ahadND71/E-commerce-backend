public class AddressService
{
  public static List<Address> _address = new List<Address>() {
    new Address{
        AddressId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        CustomerId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        Name = "Work",
        AddressLine1 = "Street name",
        AddressLine2 = "Eastern Khobar",
        Country = "Saudi Arabia",
        Province = "Eastern",
        City = "Khobar",
        ZipCode = "31311"
    },
    new Address{
        AddressId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        CustomerId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        Name = "Home",
        AddressLine1 = "Road name",
        AddressLine2 = "More details",
        Country = "Saudi Arabia",
        Province = "Central Region",
        City = "Riyadh",
        ZipCode = "34771"
    },
    new Address{
        AddressId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        CustomerId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        Name = "Friend",
        AddressLine1 = "Street name",
        AddressLine2 = "more details",
        Country = "Saudi Arabia",
        Province = "West",
        City = "Jeddah",
        ZipCode = "39884"
    }
};

  public IEnumerable<Address> GetAllAddressService()
  {
    return _address;
  }


  public Address? GetAddressById(Guid addressId)
  {
    return _address.Find(address => address.AddressId == addressId);
  }


  public Address CreateAddressService(Address newAddress)
  {
    newAddress.AddressId = Guid.NewGuid();
    _address.Add(newAddress);
    return newAddress;
  }


  public Address UpdateAddressService(Guid addressId, Address updateAddress)
  {
    var existingAddress = _address.FirstOrDefault(u => u.AddressId == addressId);
    if (existingAddress != null)
    {
      existingAddress.Name = updateAddress.Name;
      existingAddress.AddressLine1 = updateAddress.AddressLine1;
      existingAddress.AddressLine2 = updateAddress.AddressLine2;
      existingAddress.Country = updateAddress.Country;
      existingAddress.Province = updateAddress.Province;
      existingAddress.City = updateAddress.City;
      existingAddress.ZipCode = updateAddress.ZipCode;
    }
    return existingAddress;
  }


  public bool DeleteAddressService(Guid addressId)
  {
    var addressToRemove = _address.FirstOrDefault(u => u.AddressId == addressId);
    if (addressToRemove != null)
    {
      _address.Remove(addressToRemove);
      return true;
    }
    return false;
  }

}