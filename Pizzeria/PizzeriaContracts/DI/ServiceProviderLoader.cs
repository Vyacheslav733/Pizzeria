﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.DI
{
    public class ServiceProviderLoader
    {
        /// <summary>
        /// Загрузка всех классов-реализаций IImplementationExtension
        /// </summary>
        /// <returns></returns>
        public static IImplementationExtension? GetImplementationExtensions()
        {
            IImplementationExtension? source = null;
            var files = Directory.GetFiles(TryGetImplementationExtensionsFolder(), "*.dll", SearchOption.AllDirectories);
            foreach (var file in files.Distinct())
            {
                Assembly asm = Assembly.LoadFrom(file);
                foreach (var t in asm.GetExportedTypes())
                {
                    if (t.IsClass && typeof(IImplementationExtension).IsAssignableFrom(t))
                    {
                        if (source == null)
                        {
                            source = (IImplementationExtension)Activator.CreateInstance(t)!;
                        }
                        else
                        {
                            var newSource = (IImplementationExtension)Activator.CreateInstance(t)!;
                            if (newSource.Priority > source.Priority)
                            {
                                source = newSource;
                            }
                        }
                    }
                }
            }
            return source;
        }

        public static IBusinessLogicExtension? GetBusinessLogicExtensions()
        {
            IBusinessLogicExtension? source = null;
            var files = Directory.GetFiles(TryGetBusinessLogicExtensionsFolder(), "*.dll", SearchOption.AllDirectories);
            foreach (var file in files.Distinct())
            {
                Assembly asm = Assembly.LoadFrom(file);
                foreach (var t in asm.GetExportedTypes())
                {
                    if (t.IsClass && typeof(IBusinessLogicExtension).IsAssignableFrom(t))
                    {
                        if (source == null)
                        {
                            source = (IBusinessLogicExtension)Activator.CreateInstance(t)!;
                        }
                        else
                        {
                            var newSource = (IBusinessLogicExtension)Activator.CreateInstance(t)!;
                            if (newSource.Priority > source.Priority)
                            {
                                source = newSource;
                            }
                        }
                    }
                }
            }
            return source;
        }

        private static string TryGetImplementationExtensionsFolder()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetDirectories("ImplementationExtensions", SearchOption.AllDirectories).Any(x => x.Name == "ImplementationExtensions"))
            {
                directory = directory.Parent;
            }
            return $"{directory?.FullName}\\ImplementationExtensions";
        }

        private static string TryGetBusinessLogicExtensionsFolder()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetDirectories("BusinessLogicExtensions", SearchOption.AllDirectories).Any(x => x.Name == "BusinessLogicExtensions"))
            {
                directory = directory.Parent;
            }
            return $"{directory?.FullName}\\BusinessLogicExtensions";
        }
    }
}
