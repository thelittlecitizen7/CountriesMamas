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
            Type att = assembly.GetType("Countries.Attributes.TopSecretAttribute");
            var methods = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(att, false).Length > 0)
                      .ToArray();

            var lsCountires = assembly.DefinedTypes.ToList().Where(d => d.ImplementedInterfaces.Any(i => i.Name == "ICountry")).ToList();

            var rr = assembly.DefinedTypes.ToList().Where(d => d.Attributes.ToString().Contains("Private")).ToList();

            

            foreach (var countyClass in lsCountires)
            {
                foreach (var item in countyClass.DeclaredMethods)
                {

                    Console.WriteLine(item.Attributes);
                    Console.WriteLine(item.GetCustomAttribute(att));
                }
                
                //countyClass.GetDeclaredMethods().Where(e => e.Attributes.ToString().Contains("Private"));
                
                Type typeClass = assembly.GetType(countyClass.FullName);

                

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


            List<string> methosPrivate = new List<string>();
            List<string> names = new List<string>();
            foreach (var countyClass in lsCountires)
            {
                
                foreach (var item in countyClass.DeclaredMethods)
                {

                    if (item.Attributes.ToString().Contains("Private"))
                    {
                        Console.WriteLine(item.GetCustomAttribute(att));
                        methosPrivate.Add(item.Name);
                        names.Add(countyClass.Name);
                    }
                    else {
                        Console.WriteLine("no");
                    }
                }
            }
            methosPrivate.ForEach(m => Console.WriteLine(m));
            names.ForEach(m => Console.WriteLine(m));





        }
    }
}
    