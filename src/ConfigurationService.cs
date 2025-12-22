using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
namespace ARM9Editor;

public sealed class ConfigurationService
{
    private static readonly Lazy<ConfigurationService> _instance = new(valueFactory: static () => new ConfigurationService());
    public static ConfigurationService Instance => _instance.Value;
    private readonly Dictionary<EditorTab, List<EditorConfig>> _configs = [];
    private readonly Dictionary<EditorTab, TabConfig> _tabConfigs = new()
    {
        [EditorTab.Music] = new("Music IDs", EditorTab.Music, "Music Track", "Music ID"),
        [EditorTab.Courses] = new("Course Filenames", EditorTab.Courses, "Course", "Filename"),
        [EditorTab.Weather] = new("Weather Slots", EditorTab.Weather, "Weather Slot", "Course ID"),
        [EditorTab.Emblems] = new("Emblem Filenames", EditorTab.Emblems, "Emblem", "Filename"),
        [EditorTab.Karts] = new("Kart Filenames", EditorTab.Karts, "Kart", "Filename"),
        [EditorTab.Characters] = new("Character Filenames", EditorTab.Characters, "Character", "Filename")
    };
    [RequiresUnreferencedCode("Calls ARM9Editor.ConfigurationService.LoadConfigurations()")]
    private ConfigurationService()
    {
        LoadConfigurations();
    }
    public TabConfig GetTabConfig(EditorTab tab)
    {
        return _tabConfigs[tab];
    }
    public IReadOnlyList<EditorConfig> GetEditorConfigs(EditorTab tab)
    {
        return _configs.TryGetValue(tab, out List<EditorConfig>? configs) ? configs : Array.Empty<EditorConfig>();
    }
    [RequiresUnreferencedCode("Calls ARM9Editor.ConfigurationService.LoadByteConfigs(String, Int32, Int32)")]
    private void LoadConfigurations()
    {
        _configs[EditorTab.Music] = LoadByteConfigs("music_offsets.json", 0, 75);
        _configs[EditorTab.Courses] = LoadStringRangeConfigs("course_offsets.json");
        _configs[EditorTab.Weather] = LoadByteConfigs("slot_offsets.json", 1, 54);
        _configs[EditorTab.Emblems] = LoadFixedStringConfigs("emblem_offsets.json", 2);
        _configs[EditorTab.Karts] = LoadFixedStringConfigs("kart_offsets.json", 4);
        _configs[EditorTab.Characters] = LoadFixedStringConfigs("character_offsets.json", 4);
    }
    [RequiresUnreferencedCode("Calls ARM9Editor.ConfigurationService.LoadResource<T>(String)")]
    private List<EditorConfig> LoadByteConfigs(string fileName, int min, int max)
    {
        Dictionary<string, int>? dict = LoadResource<Dictionary<string, int>>(fileName);
        return dict?.Select(kvp => new EditorConfig(kvp.Key, EditorType.Byte, kvp.Value, MinValue: min, MaxValue: max)).ToList() ?? [];
    }
    [RequiresUnreferencedCode("Calls ARM9Editor.ConfigurationService.LoadResource<T>(String)")]
    private List<EditorConfig> LoadFixedStringConfigs(string fileName, int length)
    {
        Dictionary<string, int>? dict = LoadResource<Dictionary<string, int>>(fileName);
        return dict?.Select(kvp => new EditorConfig(kvp.Key, EditorType.FixedString, kvp.Value, MaxLength: length)).ToList() ?? [];
    }
    [RequiresUnreferencedCode("Calls ARM9Editor.ConfigurationService.LoadResource<T>(String)")]
    private List<EditorConfig> LoadStringRangeConfigs(string fileName)
    {
        Dictionary<string, int[]>? dict = LoadResource<Dictionary<string, int[]>>(fileName);
        return dict?.Where(kvp => kvp.Value?.Length == 2).Select(kvp => new EditorConfig(kvp.Key, EditorType.VariableString, kvp.Value[0], MaxLength: kvp.Value[1] - kvp.Value[0])).ToList() ?? [];
    }
    [RequiresUnreferencedCode("Calls Newtonsoft.Json.JsonConvert.DeserializeObject<T>(String)")]
    private T? LoadResource<T>(string fileName)
    {
        try
        {
            string resourceName = $"ARM9Editor.assets.json.{fileName}";
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return default;
            }
            using StreamReader reader = new(stream);
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }
        catch
        {
            return default;
        }
    }
}