using MotoLocadora.BuildingBlocks.Entities;


namespace MotoLocadora.Domain.Entities;

public class Rider:BaseEntity
{
    public string Name { get; set; }
    public string CommpanyId { get; set; }//cnpj
    public DateTime BirthDate { get; set; }
    public string LicenseDrive { get; set; }
    public string LicenseDriveType { get; set; }
    public string LicenseDriveImageLink { get; set; }
    public int Type { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
