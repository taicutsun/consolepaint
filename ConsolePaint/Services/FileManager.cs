using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsolePaint.Services
{
    public static class FileManager
    {
        public static void SaveShapesToFile(List<Shape> shapes, string filename)
        {
            ArgumentNullException.ThrowIfNull(shapes);
            ArgumentNullException.ThrowIfNull(filename);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

            var wrappers = new List<ShapeWrapper>();
            foreach (var shape in shapes)
            {
                var typeName = shape.GetType().AssemblyQualifiedName ?? shape.GetType().FullName ?? "UnknownType";

                var shapeJson = JsonSerializer.Serialize(shape, shape.GetType(), options);

                var wrapper = new ShapeWrapper
                {
                    Type = typeName,
                    Json = shapeJson
                };
                wrappers.Add(wrapper);
            }

            var json = JsonSerializer.Serialize(wrappers, options);
            File.WriteAllText(filename, json);
        }

        public static List<Shape> LoadShapesFromFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                return [];
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            var json = File.ReadAllText(filename);

            var wrappers = JsonSerializer.Deserialize<List<ShapeWrapper>>(json, options);
            if (wrappers == null)
            {
                return [];
            }

            var shapes = new List<Shape>();
            foreach (var wrapper in wrappers)
            {
                if (string.IsNullOrWhiteSpace(wrapper.Type) || string.IsNullOrWhiteSpace(wrapper.Json))
                {
                    continue;
                }

                var type = Type.GetType(wrapper.Type);
                if (type == null)
                {
                    continue;
                }

                var deserialized = JsonSerializer.Deserialize(wrapper.Json, type, options);
                if (deserialized is Shape shape)
                {
                    shapes.Add(shape);
                }
            }
            return shapes;
        }
    }
    file class ShapeWrapper
    {
        public string Type { get; init; } = string.Empty;
        public string Json { get; init; } = string.Empty;
    }
}
