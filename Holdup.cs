using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using System.Diagnostics;
using System.Collections;

namespace p5rpc.noholdupmusic
{
    public unsafe class Holdup
    {
        public Holdup(IReloadedHooks hooks, ILogger logger, IModLoader modLoader)
        {
            var memory = Memory.Instance;

            using var thisProcess = Process.GetCurrentProcess();
            long baseAddress = thisProcess.MainModule.BaseAddress.ToInt64();

            modLoader.GetController<IStartupScanner>().TryGetTarget(out var startupScanner);

            startupScanner.AddMainModuleScan("74 ?? 8B 05 ?? ?? ?? ?? 3D 55 01 00 00", (result) =>
            {
                long holdupMusicStart = result.Offset + baseAddress;
                if (result.Found)
                {
                    memory.SafeWrite(holdupMusicStart, (byte)0xeb);
                }
            });

            startupScanner.AddMainModuleScan("74 ?? 33 D2 41 89 86 ?? ?? ?? ?? 33 C9 E8 ?? ?? ?? ?? B9 55 01 00 00 49 89 86 ?? ?? ?? ?? E8 ?? ?? ?? ?? 0F 57 C0", (result) =>
            {
                long begMusicStart = result.Offset + baseAddress;
                if (result.Found)
                {
                    memory.SafeWrite(begMusicStart, (byte)0xeb);
                }
            });
        }
    }
}
