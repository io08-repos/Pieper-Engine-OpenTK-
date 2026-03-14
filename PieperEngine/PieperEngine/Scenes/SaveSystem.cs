using System.Text.Json;
using PieperEngine.Entities;

namespace PieperEngine.Scenes
{
    public static class SaveSystem
    {
        private static readonly string _saveFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PieperEngine"
        );

        private static readonly string _fileExtension = ".json";

        private static readonly JsonSerializerOptions s_writeOptions = new ()
        {
            WriteIndented = true
        };

        public static void Initialize()
        {
            if (!Directory.Exists(_saveFolder))
            {
                Directory.CreateDirectory(_saveFolder);
            }
        }
        public static async Task SaveScene(Scene scene)
        {
            var sceneEntities = scene.Entities;
            EntityDTO[] dtos = new EntityDTO[sceneEntities.Count];
            for (int i = 0; i < dtos.Length; i++)
            {
                EntityDTO dto = new();
                dto.SetID(sceneEntities[i].ID);
                dto.SetPosition(sceneEntities[i].Transform.Position);
                dto.SetScale(sceneEntities[i].Transform.Scale);
                dto.SetRotation(sceneEntities[i].Transform.Rotation);

                dtos[i] = dto;
            }

            string fileName = Path.Combine(_saveFolder, scene.Name);
            if (File.Exists(fileName + _fileExtension))
            {
                int copyNumber = 1;
                string fileCopyName = fileName + $"({copyNumber})";
                while (File.Exists(fileCopyName + _fileExtension))
                {
                    copyNumber++;
                    fileCopyName = fileName + $"({copyNumber})";
                }

                fileName = fileCopyName;
            }

            fileName += _fileExtension;

            await using FileStream fileStream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(fileStream, dtos, s_writeOptions);
        }
    }
}
