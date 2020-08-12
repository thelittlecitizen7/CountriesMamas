using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CountriesReflaction
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.LoadFile(@"C:\Users\thelittlecitizen7\source\repos\CountriesReflaction\CountriesReflaction\bin\Debug\netcoreapp3.1\Countries.dll");
            

            var lsCountires = assembly.DefinedTypes.ToList().Where(d => d.ImplementedInterfaces.Any(i => i.Name == "ICountry")).ToList();

            foreach (var countyClass in lsCountires)
            {
                
                Type typeClass = assembly.GetType(countyClass.FullName);

                //MethodInfo [] methodInfos = typeClass.GetMethods(BindingFlags.NonPublic);
                //methodInfos.ToList().ForEach(m => Console.WriteLine(m.Attributes.ToString()));

                var instance = Activator.CreateInstance(typeClass);
                
                MethodInfo methodName = typeClass.GetMethod("get_Name");
                MethodInfo methodPopulation = typeClass.GetMethod("get_Population");
                MethodInfo methodSize = typeClass.GetMethod("CalculateSize");

                string className = countyClass.Name;
                var countyName = methodName.Invoke(instance,null);
                var population = methodPopulation.Invoke(instance, null);
                var size = methodSize.Invoke(instance, null);

               

                Console.WriteLine($"className : {className} , countyName : {countyName},  population : {population} , size : {size}");
                
                
            }


           

        }
    }
}
    