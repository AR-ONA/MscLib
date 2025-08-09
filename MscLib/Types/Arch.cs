using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MscLib.Types {
    public enum Arch {
        // x86
        x86,           // x86 64bit and x86 32bit
        x64,           // x86 64bit
        amd64,         // x86 64bit
        i686,          // x86 32bit

        // ARM
        arm,           // arm 64bit and arm 32bit
        aarch64,       // arm 64bit
        aarch32,       // arm 32bit
    }
    public static class ArchitectureInfo {
        public static Arch GetProcessArch() {
            string archString = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return MapStringToArch(archString);
        }

        public static Arch GetOperatingSystemArch() {
            string archString = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");

            if (string.IsNullOrEmpty(archString)) {
                archString = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            }

            return MapStringToArch(archString);
        }

        private static Arch MapStringToArch(string archString) {
            switch (archString?.ToUpperInvariant()) {
                case "AMD64":
                    return Arch.amd64;
                case "X86":
                    return Arch.i686;
                case "ARM64":
                    return Arch.aarch64;
                case "ARM":
                     return Arch.aarch32;
                default:
                    throw new NotSupportedException($"The processor architecture '{archString}' is not supported.");
            }
        }
    }
}