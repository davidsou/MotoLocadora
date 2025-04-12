using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.Domain.Enums;


namespace MotoLocadora.Domain.Entities;

public class Rider:BaseEntity
{
    public string Name { get; set; }
    public string CommpanyId { get; set; }//cnpj
    public DateTime BirthDate { get; set; }
    public string LicenseDrive { get; set; }
    public LicenseDriveTypeEnum LicenseDriveType { get; set; }
    public string LicenseDriveImageLink { get; set; }
    public int Type { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string UserId { get; set; }
}
