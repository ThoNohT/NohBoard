/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace ThoNohT.NohBoard.Extra
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal static class SingleInstanceGuard
    {
        internal const string AppId = "bm-nohboard";

        internal const string MutexName = @"Global\bm-nohboard";

        private const string PipeFullPath = @"\\.\pipe\bm-nohboard";

        private const string MmfName = "bm-nohboard-pid";

        private const byte PipeSignalByte = 0x7E;

        private const uint ErrorAlreadyExists = 183;

        private const uint ErrorPipeConnected = 535;

        private const uint WaitObject0 = 0;

        private const uint WaitAbandoned = 0x00000080;

        private const uint GenericRead = 0x80000000;

        private const uint GenericWrite = 0x40000000;

        private const uint OpenExisting = 3;

        private const uint PipeAccessDuplex = 0x00000003;

        private const uint PipeTypeByte = 0x00000000;

        private const int MutexWaitMs = 120_000;

        private const int NotifyRetries = 100;

        private const int NotifyDelayMs = 50;

        private static IntPtr heldMutexHandle = IntPtr.Zero;

        internal static bool TryAcquireOrHandshake()
        {
            var handle = CreateMutexW(IntPtr.Zero, true, MutexName);
            if (handle == IntPtr.Zero)
                return false;

            var lastError = Marshal.GetLastWin32Error();
            if (lastError != ErrorAlreadyExists)
            {
                WritePid();
                heldMutexHandle = handle;
                return true;
            }

            NotifyPeerToQuit();
            TryKillPeers();
            WaitForPeerProcessesGone();

            var waitResult = WaitForSingleObject(handle, MutexWaitMs);
            if (waitResult != WaitObject0 && waitResult != WaitAbandoned)
            {
                CloseHandle(handle);
                return false;
            }

            WritePid();
            heldMutexHandle = handle;
            return true;
        }

        internal static void Release()
        {
            if (heldMutexHandle == IntPtr.Zero)
                return;

            try
            {
                ReleaseMutex(heldMutexHandle);
            }
            catch
            {
            }

            try
            {
                CloseHandle(heldMutexHandle);
            }
            catch
            {
            }

            heldMutexHandle = IntPtr.Zero;
        }

        internal static void StartPipeServer(Action onQuitRequested)
        {
            var thread = new Thread(() => PipeServerWorker(onQuitRequested))
            {
                IsBackground = true,
                Name = "bm-nohboard-pipe",
            };
            thread.Start();
        }

        private static void PipeServerWorker(Action onQuitRequested)
        {
            while (true)
            {
                var pipe = CreateNamedPipeW(
                    PipeFullPath,
                    PipeAccessDuplex,
                    PipeTypeByte,
                    255,
                    1024,
                    1024,
                    0,
                    IntPtr.Zero);

                if (pipe == IntPtr.Zero || pipe == new IntPtr(-1))
                {
                    Thread.Sleep(200);
                    continue;
                }

                var connected = ConnectNamedPipe(pipe, IntPtr.Zero);
                var connectError = Marshal.GetLastWin32Error();
                if (!connected && connectError != ErrorPipeConnected)
                {
                    CloseHandle(pipe);
                    Thread.Sleep(50);
                    continue;
                }

                var buffer = new byte[4];
                uint read;
                ReadFile(pipe, buffer, 1, out read, IntPtr.Zero);

                try
                {
                    onQuitRequested?.Invoke();
                }
                catch
                {
                }

                try
                {
                    DisconnectNamedPipe(pipe);
                }
                catch
                {
                }

                CloseHandle(pipe);
                continue;
            }
        }

        private static void NotifyPeerToQuit()
        {
            for (var i = 0; i < NotifyRetries; i++)
            {
                var file = CreateFileW(
                    PipeFullPath,
                    GenericRead | GenericWrite,
                    0,
                    IntPtr.Zero,
                    OpenExisting,
                    0,
                    IntPtr.Zero);

                if (file != IntPtr.Zero && file != new IntPtr(-1))
                {
                    try
                    {
                        var data = new[] { PipeSignalByte };
                        uint written;
                        WriteFile(file, data, 1, out written, IntPtr.Zero);
                    }
                    finally
                    {
                        CloseHandle(file);
                    }

                    return;
                }

                Thread.Sleep(NotifyDelayMs);
            }
        }

        private static void WritePid()
        {
            try
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen(MmfName, 4))
                using (var acc = mmf.CreateViewAccessor())
                {
                    acc.Write(0, Environment.ProcessId);
                }
            }
            catch
            {
            }
        }

        private static void TryKillPeers()
        {
            TryKillOtherFromMmf();
            TryKillOtherByProcessName();
        }

        private static void WaitForPeerProcessesGone()
        {
            using (var current = Process.GetCurrentProcess())
            {
                for (var i = 0; i < 60; i++)
                {
                    if (CountOtherInstances(current) == 0)
                        return;

                    Thread.Sleep(100);
                }
            }
        }

        private static int CountOtherInstances(Process current)
        {
            var count = 0;
            foreach (var p in Process.GetProcessesByName("NohBoard"))
            {
                try
                {
                    if (IsSameAppProcess(p, current))
                        count++;
                }
                catch
                {
                }
                finally
                {
                    p.Dispose();
                }
            }

            return count;
        }

        private static void TryKillOtherFromMmf()
        {
            try
            {
                using (var mmf = MemoryMappedFile.OpenExisting(MmfName))
                using (var acc = mmf.CreateViewAccessor())
                {
                    var pid = acc.ReadInt32(0);
                    if (pid <= 0 || pid == Environment.ProcessId)
                        return;

                    using (var p = Process.GetProcessById(pid))
                    {
                        if (!IsSameAppProcess(p))
                            return;

                        try
                        {
                            p.Kill(entireProcessTree: true);
                            p.WaitForExit(5000);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch
            {
            }
        }

        private static void TryKillOtherByProcessName()
        {
            try
            {
                using (var current = Process.GetCurrentProcess())
                {
                    foreach (var p in Process.GetProcessesByName("NohBoard"))
                    {
                        try
                        {
                            if (!IsSameAppProcess(p, current))
                                continue;

                            p.Kill(entireProcessTree: true);
                            p.WaitForExit(5000);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            p.Dispose();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static bool IsSameAppProcess(Process p, Process current = null)
        {
            if (p == null)
                return false;

            current = current ?? Process.GetCurrentProcess();
            if (p.Id == current.Id)
                return false;

            if (p.SessionId != current.SessionId)
                return false;

            try
            {
                var path = p.MainModule?.FileName ?? string.Empty;
                if (path.IndexOf("nohboard", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            catch
            {
            }

            return (p.ProcessName ?? string.Empty).Equals("NohBoard", StringComparison.OrdinalIgnoreCase);
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateMutexW(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReleaseMutex(IntPtr hMutex);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFileW(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateNamedPipeW(
            string lpName,
            uint dwOpenMode,
            uint dwPipeMode,
            uint nMaxInstances,
            uint nOutBufferSize,
            uint nInBufferSize,
            uint nDefaultTimeOut,
            IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ConnectNamedPipe(IntPtr hNamedPipe, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DisconnectNamedPipe(IntPtr hNamedPipe);
    }
}
