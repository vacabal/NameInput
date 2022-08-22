using NameInput.Enums;
using NameInput.Models.DTO;
using NameInput.Services.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace NameInput.Services
{
    public class NameExportService: INameExportService
    {
        protected readonly IConfiguration _config;

        public NameExportService(IConfiguration config)
        {
            _config = config;
        }
       
        public SaveNameStatus AddName(FullNameDto newName)
        {
            string path = _config.GetValue<string>("JsonFilePath");
            FullNameDto[] fileContents = ReadJsonFile();
            List<FullNameDto> names = new List<FullNameDto>();
            
            if (fileContents != null && fileContents.Length > 0)
            {
                names.AddRange(fileContents);
                if (names.Exists(x => x.FirstName == newName.FirstName && x.LastName == newName.LastName))
                {
                    return SaveNameStatus.Duplicate;
                }
            }

            using (StreamWriter sw = new StreamWriter(path))
            {                    
                JsonWriter jtw = new JsonTextWriter(sw);
                try
                {
                    names.Add(newName);
                    jtw.WriteRawValue(JsonConvert.SerializeObject(names));
                }
                catch
                {
                    if (fileContents != null)
                    {
                        jtw.WriteRawValue(JsonConvert.SerializeObject(fileContents));  //write back original contents when error occurs
                    }
                    return SaveNameStatus.Failed;
                }
            }
            return SaveNameStatus.Success;
        }

        private FullNameDto[] ReadJsonFile()
        {
            string path = _config.GetValue<string>("JsonFilePath");
            if (String.IsNullOrEmpty(path))
            {
                throw new Exception("Json path is invalid");
            }
            if (File.Exists(path))
            {
                FullNameDto[] existingNames = JsonConvert.DeserializeObject<FullNameDto[]>(File.ReadAllText(path));
                return existingNames;
            }
            return null;
        }
    }
}
