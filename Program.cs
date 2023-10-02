using System;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;

namespace EzFlip
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        static void Main(string[] args)
        {
            // Register Alt + O as global hotkey (Alt = 1, O = 0x4F)
            RegisterHotKey(IntPtr.Zero, 1, 1, 0x4F);

            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            int currentDeviceIndex = 0;

            while (true)
            {
                var msg = new NativeMessage();
                if (PeekMessage(out msg, IntPtr.Zero, 0, 0, 1))
                {
                    if (msg.message == 0x0312)
                    {
                        currentDeviceIndex = (currentDeviceIndex + 1) % devices.Count;
                        var newDevice = devices[currentDeviceIndex];
                        SwitchAudioDevice(newDevice);
                    }
                }
            }
        }

        static void SwitchAudioDevice(MMDevice newDevice)
        {
            var policyConfig = new PolicyConfigClient();
            policyConfig.SetDefaultEndpoint(newDevice.ID, (int)Role.Multimedia);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr hWnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out NativeMessage message, IntPtr hWnd, uint filterMin, uint filterMax, uint flags);
    }
}
