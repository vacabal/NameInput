using NameInput.Enums;
using NameInput.Models.DTO;

namespace NameInput.Services.Interfaces
{
    public interface INameExportService
    {
        public SaveNameStatus AddName(FullNameDto name);
    }
}
