using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace HomeOfPandaEyes.Infrastructure
{
    public static class ReflectionService
    {
        public static bool TryGetInstance<T>(this Assembly assembly, string typeName, out T instance,
                                             params object[] args)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException("typeName");
            }

            bool returnValue = false;
            instance = default(T);

            try
            {
                Type type = assembly.GetType(typeName);
                if (type != null)
                {
                    instance = (T) Activator.CreateInstance(type, args);
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch
            {
                returnValue = false;
            }
            return returnValue;
        }

        public static Assembly Compile(string name, CodeCompileUnit unit, bool debug, params string[] imports)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (unit == null)
            {
                throw new ArgumentNullException("unit");
            }

            Assembly returnAssembly = null;

            string compilingPath = GetCompilePath();

            if (debug)
            {
                name =
                    new StringBuilder().Append(name).Append("_").Append(Math.Abs(Guid.NewGuid().GetHashCode())).ToString
                        ();
            }

            string assemblyFileName = name + ".dll";

            var assemblyFile = new FileInfo(Path.Combine(compilingPath, assemblyFileName));

            if (assemblyFile.Exists)
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFile.FullName);
                if (assembly != null)
                {
                    returnAssembly = assembly;

                    return returnAssembly;
                }

             //   throw new ApplicationException(string.Format("cannot load assembly {0}", assemblyFileName));
            }

            var importAssemblies = new List<string>();
            importAssemblies.AddRange(new[]
                                          {
                                              "System.dll"
                                              , "System.Core.dll"
                                              , "System.Data.dll"
                                              , "System.Data.DataSetExtensions.dll"
                                              , "System.ServiceModel.dll"
                                              , "System.Xml.dll"
                                              , "System.Xml.Linq.dll"
                                          });

            if (imports != null && imports.Length > 0)
            {
                importAssemblies.AddRange(imports);
            }

            var options = new CompilerParameters(importAssemblies.ToArray(),
                                                 Path.Combine(compilingPath, assemblyFileName), debug)
                              {
                                  GenerateExecutable = false,
                                  GenerateInMemory = true,
                                  TempFiles = new TempFileCollection(compilingPath, debug)
                              };

            if (debug)
            {
                options.CompilerOptions += ("/define:TRACE /define:DEBUG /lib:\"" + compilingPath + "\"");
            }
            else
            {
                options.CompilerOptions += ("/define:TRACE /optimize /lib:\"" + compilingPath + "\"");
            }

            CompilerResults compilerResults = (new CSharpCodeProvider()).CompileAssemblyFromDom(options, unit);

            if (compilerResults.NativeCompilerReturnValue == 0)
            {
                Assembly comiledAssembly = compilerResults.CompiledAssembly;
                returnAssembly = comiledAssembly;
            }
            if (compilerResults.Errors != null && compilerResults.Errors.Count > 0)
            {
                var compilerInfo = new StringBuilder();
                foreach (CompilerError err in compilerResults.Errors)
                {
                    compilerInfo.Append(err.ToString());
                    compilerInfo.Append("\r\n");
                }
                throw new ApplicationException(compilerInfo.ToString());
            }
            else
            {
                return returnAssembly;
            }
        }

        public static string GetCompilePath()
        {
            string path = AppDomain.CurrentDomain.RelativeSearchPath;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Environment.CurrentDirectory;
            }
            if (path.Trim().EndsWith("\\"))
            {
                path = Path.GetDirectoryName(path);
            }
            return path;
        }
    }
}