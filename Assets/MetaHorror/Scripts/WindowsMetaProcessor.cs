using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace MetaHorror.Runtime
{
    public static class WindowsMetaProcessor
    {
#if UNITY_STANDALONE_WIN

        private const int SRCCOPY = 0x00CC0020;

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc,
            int nWidth,
            int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(
            IntPtr hdc,
            IntPtr obj);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest,
            int xDest,
            int yDest,
            int width,
            int height,
            IntPtr hdcSrc,
            int xSrc,
            int ySrc,
            int rop);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(
            IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(
            IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern int GetDIBits(
            IntPtr hdc,
            IntPtr hbmp,
            uint uStartScan,
            uint cScanLines,
            byte[] lpvBits,
            ref BITMAPINFO lpbi,
            uint uUsage);


        private const int SPI_GETDESKWALLPAPER = 0x0073;
        private const int MAX_PATH = 260;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(
            int uiAction, int uiParam, StringBuilder pvParam, int fWinIni);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;

        private const uint DIB_RGB_COLORS = 0;

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
        }

        public static Texture2D GetWallpaper()
        {
            StringBuilder path = new(MAX_PATH);

            bool success = SystemParametersInfo(
                SPI_GETDESKWALLPAPER,
                path.Capacity, path, 0);

            if (!success) { return null; }

            string filePath = path.ToString();

            if (!File.Exists(filePath))
                return null;

            byte[] imageData = File.ReadAllBytes(filePath);

            Texture2D texture = new(2, 2);
            texture.LoadImage(imageData);

            return texture;
        }

        public static Texture2D Capture()
        {
            int width = GetSystemMetrics(SM_CXSCREEN);
            int height = GetSystemMetrics(SM_CYSCREEN);

            IntPtr desktopWnd = GetDesktopWindow();
            IntPtr desktopDC = GetWindowDC(desktopWnd);

            IntPtr memoryDC = CreateCompatibleDC(desktopDC);

            IntPtr bitmap =
                CreateCompatibleBitmap(
                    desktopDC,
                    width,
                    height);

            IntPtr oldBitmap =
                SelectObject(
                    memoryDC,
                    bitmap);

            BitBlt(
                memoryDC,
                0,
                0,
                width,
                height,
                desktopDC,
                0,
                0,
                SRCCOPY);

            BITMAPINFO info = new BITMAPINFO();

            info.bmiHeader.biSize =
                (uint)Marshal.SizeOf<BITMAPINFOHEADER>();

            info.bmiHeader.biWidth = width;

            // 上下反転防止
            info.bmiHeader.biHeight = height;

            info.bmiHeader.biPlanes = 1;
            info.bmiHeader.biBitCount = 32;
            info.bmiHeader.biCompression = 0;

            byte[] pixels =
                new byte[width * height * 4];

            GetDIBits(
                memoryDC,
                bitmap,
                0,
                (uint)height,
                pixels,
                ref info,
                DIB_RGB_COLORS);

            Texture2D texture =
                new Texture2D(
                    width,
                    height,
                    TextureFormat.BGRA32,
                    false);

            texture.LoadRawTextureData(pixels);
            texture.Apply();

            SelectObject(memoryDC, oldBitmap);

            DeleteObject(bitmap);
            DeleteDC(memoryDC);

            ReleaseDC(
                desktopWnd,
                desktopDC);

            return texture;
        }

#else

        public static Texture2D Capture()
        {
            Debug.LogWarning(
                "DesktopCapture is only supported on Windows.");

            return null;
        }

        public static Texture2D GetWallpaper()
        {
            Debug.LogWarning(
                "Wallpaper retrieval is only supported on Windows.");

            return null;

        }
#endif
    }
}