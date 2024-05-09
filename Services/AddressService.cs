using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Helpers;

namespace api.Services;


public class AddressService
{
  private readonly AppDbContext _addressDbContext;
  public AddressService(AppDbContext adminDbContext)
  {
    _addressDbContext = adminDbContext;
  }


  public async Task<PaginationResult<Address>> GetAllAddressService(int currentPage, int pageSize)
  {
    var totalAddressCount = await _addressDbContext.Addresses.CountAsync();
    var address = await _addressDbContext.Addresses
    .Skip((currentPage - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    return new PaginationResult<Address>
    {
      Items = address,
      TotalCount = totalAddressCount,
      CurrentPage = currentPage,
      PageSize = pageSize,
    };
  }


  public async Task<Address?> GetAddressById(Guid addressId)
  {
    return await _addressDbContext.Addresses.FindAsync(addressId);
  }


  public async Task<Address> CreateAddressService(Address newAddress)
  {
    newAddress.AddressId = Guid.NewGuid();
    _addressDbContext.Addresses.Add(newAddress);
    await _addressDbContext.SaveChangesAsync();
    return newAddress;
  }


  public async Task<Address?> UpdateAddressService(Guid addressId, Address updateAddress)
  {
    var existingAddress = await _addressDbContext.Addresses.FindAsync(addressId);

    if (existingAddress != null)
    {
      existingAddress.Name = updateAddress.Name ?? existingAddress.Name;
      existingAddress.AddressLine1 = updateAddress.AddressLine1 ?? existingAddress.AddressLine1;
      existingAddress.AddressLine2 = updateAddress.AddressLine2 ?? existingAddress.AddressLine2;
      existingAddress.Country = updateAddress.Country ?? existingAddress.Country;
      existingAddress.Province = updateAddress.Province ?? existingAddress.Province;
      existingAddress.City = updateAddress.City ?? existingAddress.City;
      existingAddress.ZipCode = updateAddress.ZipCode ?? existingAddress.ZipCode;
      await _addressDbContext.SaveChangesAsync();
    }
    return existingAddress;
  }


  public async Task<bool> DeleteAddressService(Guid addressId)
  {
    var addressToRemove = await _addressDbContext.Addresses.FindAsync(addressId);
    if (addressToRemove != null)
    {
      _addressDbContext.Addresses.Remove(addressToRemove);
      await _addressDbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }
}