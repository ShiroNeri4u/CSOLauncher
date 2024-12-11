using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Reloaded.Memory.Buffers.Internal.Kernel32.Kernel32;

namespace CSOClient
{
    internal class Hook
    {
        private const string IP = "127.0.0.1";
        private const int Port = 30002;

    }

    public static class Utils
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        [LibraryImport("kernel32.dll", SetLastError = true)]
        private static extern int VirtualQuery(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        public static uint GetModuleBase(IntPtr hModule)
        {

            if (VirtualQuery(hModule, out MEMORY_BASIC_INFORMATION mem, (uint)Marshal.SizeOf<MEMORY_BASIC_INFORMATION>()) == 0)
            {
                return 0;
            }
            else return (uint)mem.AllocationBase;
        }


        public static uint FindPattern(byte[] pattern, string mask, uint start, uint end, uint offset)
        {
            int patternLength = mask.Length;
            for (uint i = start; i < end - (uint)patternLength; i++)
            {
                bool found = true;
                for (int idx = 0; idx < patternLength; idx++)
                {
                    if (mask[idx] == 'x' && pattern[idx] != Marshal.ReadByte((IntPtr)i + idx))
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i + offset;
                }
            }
            return 0;
        }
    }
}
