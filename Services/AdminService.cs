public class AdminService
{
  public static List<Admin> _admins = new List<Admin>() {
    new Admin{
        AdminId = Guid.Parse("75424b9b-cbd4-49b9-901b-056dd1c6a020"),
        FirstName = "John",
        LastName = "Doe",
        Email = "john@example.com",
        Password = "password123",
        Address = "123 Main St",
        IsBanned = false,
        CreatedAt = DateTime.Now
    },
    new Admin{
        AdminId = Guid.Parse("24508f7e-94ec-4f0b-b8d6-e8e16a9a3b29"),
        FirstName = "Alice",
        LastName = "Smith",
        Email = "alice@example.com",
        Password = "password456",
        Address = "456 Elm St",
        IsBanned = false,
        CreatedAt = DateTime.Now
    },
    new Admin{
        AdminId = Guid.Parse("87e5c4f3-d3e5-4e16-88b5-809b2b08b773"),
        FirstName = "Bob",
        LastName = "Johnson",
        Email = "bob@example.com",
        Password = "password789",
        Address = "789 Oak St",
        IsBanned = false,
        CreatedAt = DateTime.Now
    }
};

  public IEnumerable<Admin> GetAllAdminsService()
  {
    return _admins;
  }


  public Admin? GetAdminById(Guid adminId)
  {
    return _admins.Find(admin => admin.AdminId == adminId);
  }


  public Admin CreateAdminService(Admin newAdmin)
  {
    newAdmin.AdminId = Guid.NewGuid();
    newAdmin.CreatedAt = DateTime.Now;
    _admins.Add(newAdmin);
    return newAdmin;
  }


  public Admin UpdateAdminService(Guid adminId, Admin updateAdmin)
  {
    var existingAdmin = _admins.FirstOrDefault(u => u.AdminId == adminId);
    if (existingAdmin != null)
    {
      existingAdmin.FirstName = updateAdmin.FirstName;
      existingAdmin.LastName = updateAdmin.LastName;
      existingAdmin.Email = updateAdmin.Email;
      existingAdmin.Password = updateAdmin.Password;
      existingAdmin.Address = updateAdmin.Address;
      existingAdmin.Image = updateAdmin.Image;
      existingAdmin.IsBanned = updateAdmin.IsBanned;
    }
    return existingAdmin;
  }


  public bool DeleteAdminService(Guid adminId)
  {
    var adminToRemove = _admins.FirstOrDefault(u => u.AdminId == adminId);
    if (adminToRemove != null)
    {
      _admins.Remove(adminToRemove);
      return true;
    }
    return false;
  }

}