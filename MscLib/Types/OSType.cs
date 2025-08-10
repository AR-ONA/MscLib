using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib.Types {
    public enum OSType {
        Windows,
        Linux,
        Mac
    }

    public static class OSTypeInfo {
        public static string ToAdoptiumString(this OSType os) {
            return os switch {
                OSType.Windows => "windows",
                OSType.Linux => "linux",
                OSType.Mac => "mac",
                _ => throw new ArgumentOutOfRangeException(nameof(os), os, null)
            };
        }
        public static OSType GetCurrentOs() {
            if (OperatingSystem.IsWindows()) return OSType.Windows;
            if (OperatingSystem.IsLinux()) return OSType.Linux;
            if (OperatingSystem.IsMacOS()) return OSType.Mac;
            throw new NotSupportedException("OS not supported");
        }
    }
}
