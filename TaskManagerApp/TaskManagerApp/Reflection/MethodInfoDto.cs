namespace TaskManagerApp.Reflection
{
    public class MethodInfoDto
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<ParameterDto> Parameters { get; set; }
    }
}
