using Newtonsoft.Json;
using Refit;

namespace Diploma.Common.Utils;

public static class RefitFunctions
{
    public static T GetRefitService<T>(HttpClient httpClient)
    {
        try
        {
            return RestService.For<T>(httpClient, new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(),
            });
        }
        catch (Exception ex)
        {
            // Обработка исключения
            // Например, можно вывести сообщение об ошибке или записать исключение в лог
            Console.WriteLine("An error occurred while creating the Refit service:");
            Console.WriteLine(ex.Message);
            return default(T); // Возвращаем значение по умолчанию или null
        }
    }
}