using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

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


    public async Task<Address?> UpdateAddressService(Guid addressId, AddressDto updateAddress)
    {
        var existingAddress = await _addressDbContext.Addresses.FindAsync(addressId);

        if (existingAddress != null)
        {
            existingAddress.Name = updateAddress.Name.IsNullOrEmpty() ? existingAddress.Name : updateAddress.Name;
            existingAddress.AddressLine1 = updateAddress.AddressLine1.IsNullOrEmpty() ? existingAddress.AddressLine1 : updateAddress.AddressLine1;
            existingAddress.AddressLine2 = updateAddress.AddressLine2.IsNullOrEmpty() ? existingAddress.AddressLine2 : updateAddress.AddressLine2;
            existingAddress.Country = updateAddress.Country.IsNullOrEmpty() ? existingAddress.Country : updateAddress.Country;
            existingAddress.Province = updateAddress.Province.IsNullOrEmpty() ? existingAddress.Province : updateAddress.Province;
            existingAddress.City = updateAddress.City.IsNullOrEmpty() ? existingAddress.City : updateAddress.City;
            existingAddress.ZipCode = updateAddress.ZipCode.IsNullOrEmpty() ? existingAddress.ZipCode : updateAddress.ZipCode;
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