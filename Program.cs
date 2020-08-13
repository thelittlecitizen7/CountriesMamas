using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CountriesReflaction
{
    class Program
    {
        static void Main(string[] args)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            Assembly assembly = Assembly.LoadFile(Path.Combine(executableLocation, "Countries.dll"));
            
            Console.WriteLine("------------------------------------------------------------");
            // ex1
            PrintAllCOuntriesDetails(assembly);
            Console.WriteLine("------------------------------------------------------------");
            // ex2.b
            PrintAllPrivateMethosWithTopSecretAttributes(assembly);
            Console.WriteLine("------------------------------------------------------------");

            // ex2.c
            PrintMamasContinent(assembly);
            Console.WriteLine("------------------------------------------------------------");



        }


        public static List<TypeInfo> GetAllCountries(Assembly assembly) {
            return assembly.DefinedTypes.ToList().Where(d => d.ImplementedInterfaces.Any(i => i.Name == "ICountry")).ToList();
        }


        // ex 1
        public static void PrintAllCOuntriesDetails(Assembly assembly) {
            foreach (TypeInfo countyClass in GetAllCountries(assembly))
            {
                Type typeClass = assembly.GetType(countyClass.FullName);

                var instance = Activator.CreateInstance(typeClass);

                MethodInfo methodName = typeClass.GetMethod("get_Name");
                MethodInfo methodPopulation = typeClass.GetMethod("get_Population");
                MethodInfo methodSize = typeClass.GetMethod("CalculateSize");

                string className = countyClass.Name;
                var countyName = methodName.Invoke(instance, null);
                var population = methodPopulation.Invoke(instance, null);
                var size = methodSize.Invoke(instance, null);

                Console.WriteLine($"className : {className} , countyName : {countyName},  population : {population} , size : {size}");

            }
        }

        // ex 2.b
        public static void PrintAllPrivateMethosWithTopSecretAttributes(Assembly assembly) {
            foreach (TypeInfo countyClass in GetAllCountries(assembly))
            {
                foreach (var method in countyClass.DeclaredMethods)
                {

                    if (method.IsPrivate)
                    {

                        var allMethodAttributes = method.GetCustomAttributes().Select(p => p).ToList();

                        bool hasTopSecretAttribute = allMethodAttributes.Any(f => f.GetType().Name == "TopSecretAttribute");
                        if (hasTopSecretAttribute)
                        {
                            Console.WriteLine($"county : {countyClass.Name} , method : {method.Name}");
                        }
                    }
                }
            }
        }

        // ex 2.c
        public static void PrintMamasContinent(Assembly assembly) {
            foreach (TypeInfo countyClass in GetAllCountries(assembly))
            {

                if (countyClass.Name == "MamasEmpire")
                {
                    foreach (var item in countyClass.CustomAttributes)
                    {
                        item.ConstructorArguments.ToList().ForEach(t => Console.WriteLine(t.Value));
                    }
                }

            }
        }
    }
}
    