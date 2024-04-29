using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Services;

public class AddressService
{

  private readonly AppDbContext _dbContext;
  public AddressService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }


  public async Task<IEnumerable<Address>> GetAllAddressService()
  {
    return await _dbContext.Addresses.ToListAsync();
  }


  public async Task<Address?> GetAddressById(Guid addressId)
  {
    return await _dbContext.Addresses.FindAsync(addressId);
  }


  public async Task<Address> CreateAddressService(Address newAddress)
  {
    newAddress.AddressId = Guid.NewGuid();
    _dbContext.Addresses.Add(newAddress);
    await _dbContext.SaveChangesAsync();
    return newAddress;
  }


  public async Task<Address> UpdateAddressService(Guid addressId, Address updateAddress)
  {
    var existingAddress = await _dbContext.Addresses.FindAsync(addressId);

    if (existingAddress != null)
    {
      existingAddress.Name = updateAddress.Name;
      existingAddress.AddressLine1 = updateAddress.AddressLine1;
      existingAddress.AddressLine2 = updateAddress.AddressLine2;
      existingAddress.Country = updateAddress.Country;
      existingAddress.Province = updateAddress.Province;
      existingAddress.City = updateAddress.City;
      existingAddress.ZipCode = updateAddress.ZipCode;
      await _dbContext.SaveChangesAsync();
    }
    return existingAddress;
  }


  public async Task<bool> DeleteAddressService(Guid addressId)
  {
    var addressToRemove = await _dbContext.Addresses.FindAsync(addressId);
    if (addressToRemove != null)
    {
      _dbContext.Addresses.Remove(addressToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

}