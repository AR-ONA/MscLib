namespace MscLib.Types {
    public enum Arch {
        // x86
        x86,           // x86 32bit
        x64,           // x86 64bit

        // ARM
        Arm,           // arm 32bit
        Aarch64,       // arm 64bit
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

        public static string ToAdoptiumString(Arch arch) {
            return arch switch {
                Arch.x86 => "x86",
                Arch.x64 => "x64",
                Arch.Arm => "arm",
                Arch.Aarch64 => "aarch64",
                _ => throw new ArgumentOutOfRangeException(nameof(arch), arch, null)
            };
        }

        private static Arch MapStringToArch(string archString) {
            switch (archString?.ToUpperInvariant()) {
                case "AMD64":
                    return Arch.x64;
                case "X86":
                    return Arch.x86;
                case "ARM64":
                    return Arch.Aarch64;
                case "ARM":
                     return Arch.Arm;
                default:
                    throw new NotSupportedException($"The processor architecture '{archString}' is not supported.");
            }
        }
    }
}