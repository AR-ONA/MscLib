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
        aarch32sf,     // arm 32bit soft float
        aarch32hf,     // arm 32bit hard float

        // PPC
        ppc,           // ppc 64bit and ppc 32bit
        ppc64,         // ppc 64bit
        ppc64hf,       // ppc 64bit
        ppc32,         // ppc 32bit
        ppc32spe,      // ppc 32bit spe
        ppc32hf,       // ppc 32bit hard float

        // SPARC
        sparc,         // sparc 64bit and sparc 32bit
        sparc32,       // sparc 32bit
        sparcv9,       // sparcv9 64bit and sparcv9 32bit
        [EnumMember(Value = "sparcv9-64")]
        sparcv9_64     // sparcv9 64bit
    }
}
