using TaskManagerApp.Services;

namespace TaskManagerApp.Reflection
{
    public class PluginInspector
    {
        public ReflectionInfo GetMetaInfo()
        {
            var info = typeof(TaskService);

            var methods = info.GetMethods()
                .Where(m => m.IsPublic && !m.IsStatic)
                .Select(m => new MethodInfoDto
                {
                    Name = m.Name,
                    ReturnType = m.ReturnType.Name,
                    Parameters = m.GetParameters().Select(p => new ParameterDto
                    {
                        Name = p.Name,
                        Type = p.ParameterType.Name
                    }
                    ).ToList()
                }).ToList();

            var Properties = info.GetProperties()
                .Where(p => p.CanWrite && p.CanRead)
                .Select(p => new PropertyDto
                {
                    name = p.Name,
                    type = p.PropertyType.Name
                }).ToList();

            var reflectioninfo = new ReflectionInfo 
            {
                methods = methods,
                Properties = Properties
            };

            return reflectioninfo;
        }
    }
}
